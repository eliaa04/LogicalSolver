using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicalSolver.Solve
{
    using Common;
    public class Solve
    {
        public static void ReplaceParametersWithValues(TreeNode root, List<string> parameters, List<string> values,
            Dictionary<string,bool> solvedFunctions)        {
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
                        root.Value =ToBoolean( values[i]);
                        break;
                    }
                }

                if (!parameters.Contains(root.ParameterName))
                {
                    string funcNameWithArguments = ConvertToArgumentsKey(root.ParameterName, parameters, values);
                    if (solvedFunctions.ContainsKey(funcNameWithArguments))
                    {
                        root.Value = solvedFunctions[funcNameWithArguments];
                    }
                }
            }
         
            if (root.Left is not null)
            {
                ReplaceParametersWithValues(root.Left, parameters, values, solvedFunctions);
            }

            if (root.Right is not null)
            {
                ReplaceParametersWithValues(root.Right, parameters, values, solvedFunctions);
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

        public static string ConvertToArgumentsKey(string nestedFunctionParametersKey, List<string> parameters, List<string> values)
        {
            
            (string funcName, int currentIndex) = Common.ParseFuncName(nestedFunctionParametersKey);
            (List<string> nestedFunctionParameters, _) = Common.ParseParameters(nestedFunctionParametersKey, currentIndex);

            string funcNameWithArguments = $"{funcName}(";

            for (int k = 0; k < nestedFunctionParameters.Count; k++)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (nestedFunctionParameters[k] == parameters[i])
                    {
                        bool value = ToBoolean(values[i]);
                        funcNameWithArguments += ($"{value}");
                        if (k != nestedFunctionParameters.Count - 1)
                        {
                            funcNameWithArguments += ",";
                        }

                        break;
                    }
                }
            }

            funcNameWithArguments += ')';
            return funcNameWithArguments;

        }
    }
}
