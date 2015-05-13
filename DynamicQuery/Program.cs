using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new AppClarityContext();
            context.GetRecordType(new Guid("8DD39800-07D3-E411-80BD-00155D141418"), 44);
        }
    }
}
