using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicalSolver.Define
{
    using Common;
    public class Define
    {

        public static string Parse(string command, int currentIndex)
        {
            string funcBody = string.Empty;
            for (int i = currentIndex; i < command.Length; i++)
            {
                if (command[i] == '"')
                {
                    for (int j = i + 1; j < command.Length - 1; j++)
                    {
                        funcBody += command[j];
                    }
                }
            }
            return (funcBody);
        }

        public static void ValidateDefinitionCandidate(TreeNode root, List<string> parameters,
            Dictionary<string, TreeNode> rootByFuncNames)
        {
            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (root.ParameterName is not null)
            {
                if (!parameters.Contains(root.ParameterName))
                {
                    string nestedFuncName = string.Empty;
                    try
                    {
                        (nestedFuncName, _) = Common.ParseFuncName(root.ParameterName);
                    }
                    catch
                    {
                        // ignore
                    }

                    if (!rootByFuncNames.ContainsKey(nestedFuncName))
                    {
                        throw new Exception($"Параметърът {root.ParameterName} е невалиден или е недифинирана вмъкната функция");
                    }
                }
            }

            if (root.Left is not null)
            {
                ValidateDefinitionCandidate(root.Left, parameters, rootByFuncNames);
            }

            if (root.Right is not null)
            {
                ValidateDefinitionCandidate(root.Right, parameters, rootByFuncNames);
            }
        }
    }
}

