namespace LogicalSolver.All
{
    using Common;
    public class All
    {
        public static void GenerateVariationsWithRep(List<List<string>> variations,List<int> workArr,int n,int k,int index = 0)
        {
            if (index >= k)
            {
                var variation = new List<string>();
                foreach (var item in workArr)
                {
                    variation.Add(item.ToString());
                }
                variations.Add(variation);

                return;
            }

            for (int i = 0; i < n; i++)
            {
                workArr[index] = i;
                GenerateVariationsWithRep(variations,workArr,n,k,index + 1);
            }

        }

        public static string ConvertToArgumentsKey(string funcName, List<string> arguments)
        {
            string funcNameWithArguments = $"{funcName}(";
            funcNameWithArguments = Common.ListToString(funcNameWithArguments,arguments);

            funcNameWithArguments += ')';
            return funcNameWithArguments;
        }

        public static int ToNumber(bool value)
        {
            if (value == true)
            {
                return 1;
            }

            if (value == false)
            {
                return 0;
            }

            throw new ArgumentException("Boolean cannot be represented as a number", nameof(value));
        }
    }
}
