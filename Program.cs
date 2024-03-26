using LogicalSolver;
using LogicalSolver.Common;
using LogicalSolver.Define;
using System.Text;

namespace CourseProject
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Въведете команда:");
                string command = Console.ReadLine();

                string commandName = string.Empty;

                foreach (var symbol in command)
                {
                    if (Char.IsWhiteSpace(symbol))
                    {
                        break;
                    }

                    commandName += symbol;
                }

                var commandRemainder = command.Substring((commandName.Length + 1));


                switch (commandName.ToUpperInvariant())
                {
                    case "DEFINE":
                        (string defineFuncName, int defineCurrentIndex) = Common.ParseFuncName(commandRemainder);
                        (List<string> parametersList, defineCurrentIndex) = Common.ParseParameters(commandRemainder, defineCurrentIndex);
                        string funcBody = Define.Parse(commandRemainder, defineCurrentIndex);
                        Tree tree = new Tree();
                        TreeNode root = tree.BuildTree(funcBody);
                        Dictionary<string, TreeNode> rootByFuncNames = new Dictionary<string, TreeNode>();
                        rootByFuncNames.Add(defineFuncName, root);
                        break;
                    case "SOLVE":
                        (string solveFuncName, int solveCurrentIndex) = Common.ParseFuncName(commandRemainder);
                        (List<string> argumentsList, solveCurrentIndex) = Common.ParseParameters(commandRemainder, solveCurrentIndex);
                        
                        break;
                    case "ALL":
                        Console.WriteLine("bla bla all");
                        break;
                    default:
                        Console.WriteLine("Командата не е разпозната. Въведете валидна команда!");
                        break;
                }
            }
        }
    }
}
