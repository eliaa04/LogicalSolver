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
            CustomDictionary<TreeNode> rootByFuncNames, CustomDictionary<List<string>> parametersByFuncNames)
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
                    int currentIndex = 0;
                    List<string> nestedParameters = new List<string>();
                    try
                    {
                        (nestedFuncName, currentIndex) = Common.ParseFuncName(root.ParameterName);
                        (nestedParameters, _) = Common.ParseParameters(root.ParameterName, currentIndex);
                    }
                    catch
                    {
                        // ignore
                    }

                    if (!rootByFuncNames.ContainsKey(nestedFuncName))
                    {
                        throw new Exception($"Параметърът {root.ParameterName} е невалиден или е недифинирана вмъкната функция");
                    }

                    if (nestedParameters.Count != parametersByFuncNames.Get(nestedFuncName).Count)
                    {
                        throw new Exception($"Вложената функция {nestedFuncName} е извикана с неправилен брой параметри");
                    }
                }
            }

            if (root.Left is not null)
            {
                ValidateDefinitionCandidate(root.Left, parameters, rootByFuncNames, parametersByFuncNames);
            }

            if (root.Right is not null)
            {
                ValidateDefinitionCandidate(root.Right, parameters, rootByFuncNames, parametersByFuncNames);
            }
        }
    }
}

