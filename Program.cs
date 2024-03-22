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

                switch (commandName.ToUpperInvariant())
                {
                    case "DEFINE":
                        Console.WriteLine("bla bla define");
                        break;
                    case "SOLVE":
                        Console.WriteLine("bla bla solve");
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
