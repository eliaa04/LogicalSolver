using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver.All
{
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

            for (int i = 0; i < arguments.Count; i++)
            {
                funcNameWithArguments += ($"{arguments[i]}");
                if (i != arguments.Count - 1)
                {
                    funcNameWithArguments += ",";
                }
            }

            funcNameWithArguments += ')';
            return funcNameWithArguments;
        }
    }
}
