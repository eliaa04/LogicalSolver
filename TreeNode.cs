using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver
{
    public class TreeNode
    {
        public string Operator { get; set; }
        public string ParameterName { get; set; }
        public bool Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
    }

}
