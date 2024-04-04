using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver.Solve
{
    public class Solve
    {
        public static void ReplaceParametersWithValues(TreeNode root, List<string> parameters, List<string> values)
        {
            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (root.ParameterName is not null)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i] == root.ParameterName)
                    {
                        root.Value = ToBoolean(values[i]); 
                        break;
                    }
                }
            }
            //TODO: handle case when the parameter is not found in the parameters list => already solved function
            if (root.Left is not null)
            {
                ReplaceParametersWithValues(root.Left, parameters, values);
            }

            if (root.Right is not null)
            {
                ReplaceParametersWithValues(root.Right, parameters, values);
            }
        }

        public static bool SolveNode(TreeNode currentNode)
        {
            if (!String.IsNullOrEmpty(currentNode.Operator))
            {
                bool leftResult = default;
                if (currentNode.Left is not null)
                { 
                    leftResult = SolveNode(currentNode.Left);
                }

                bool rightResult = default;
                if (currentNode.Right is not null)
                {
                    rightResult = SolveNode(currentNode.Right);
                }

                if (currentNode.Operator == "&")
                {
                    currentNode.Value = leftResult && rightResult;
                }
                else if (currentNode.Operator == "|")
                {
                    currentNode.Value = leftResult || rightResult;
                }
                else if (currentNode.Operator == "!")
                {
                    currentNode.Value = !leftResult;
                }

               return currentNode.Value;
            }

            return currentNode.Value;
        }

        public static bool ToBoolean(string number)
        {
            if (number == "1")
            {
                return true;
            }

            if (number == "0")
            {
                return false;
            }

            throw new ArgumentException("Number cannot be represented as a boolean", nameof(number));
        }
    }
}
