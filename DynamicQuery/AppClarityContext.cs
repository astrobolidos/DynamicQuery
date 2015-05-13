using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace DynamicQuery
{
    public class AppClarityContext : DbContext
    {
        public AppClarityContext() :
            base("CustomerDB")
        {
        }

        public void GetRecordType(Guid RecordTypeID, int AttachmentID)
        {
            var command = String.Format("[dbo].[Usp_StorageEx_Se] @RecordTypeID='{0}', @AttachmentID = {1}", RecordTypeID, AttachmentID);

            using (var ctx = new AppClarityContext())
            using (var cmd = ctx.Database.Connection.CreateCommand())
            {
                ctx.Database.Connection.Open();
                cmd.CommandText = command;
                using (var reader = cmd.ExecuteReader())
                {
                    //var model = Read(reader).ToList();
                    var rows = ReadAsDynamic(reader);
                }
            }
        }

        private static IEnumerable<object[]> Read(DbDataReader reader)
        {
            while (reader.Read())
            {
                var values = new List<object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    values.Add(reader.GetValue(i));
                }
                yield return values.ToArray();
            }
        }

        private List<IDictionary<string, object>> ReadAsDynamic(DbDataReader reader)
        {
            var results = new List<IDictionary<string,object>>();

            while (reader.Read())
            {
                var expandoObject = new ExpandoObject() as IDictionary<string, object>;

                for (var i = 0; i < reader.FieldCount; i++)
                    expandoObject.Add(reader.GetName(i), reader[i]);

                results.Add(expandoObject);  
            }

            return results;


        }
    }
}