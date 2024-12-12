using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srinivas.UseFulExtenstions
{
    public class RelationTable
    {
        public RelationTable() { }
        public string RelationName { get; set; }
        public string ParentTableName { get; set; }
        public string ParentTableKeycolumn { get; set; }
        public string ChaildTableName { get; set; }
        public string ChaildTableKeycolumn { get; set; }
    }
}
