 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalSolver
{
    public class Tree
    {
        private Stack<TreeNode> _stack = new Stack<TreeNode>();

        public TreeNode BuildTree(string funcBody)
        {
            List<string> funcBodyElements = ParseFunctionBody(funcBody);

            foreach (var element in funcBodyElements)
            {
                if (element != "&" && element != "|" && element != "!")
                {
                    var newNode = new TreeNode(){ ParameterName = element }; 
                    _stack.Push(newNode);
                }
                else
                {
                    if (element == "!")
                    {
                        var firstNode = _stack.Pop();
                        var newNode = new TreeNode() { Operator = element, Left = firstNode};
                        _stack.Push(newNode);
                    }
                    else
                    {
                        var firstNode = _stack.Pop();
                        var secondNode = _stack.Pop();
                        var newNode = new TreeNode() { Operator = element, Left = firstNode, Right = secondNode };
                        _stack.Push(newNode);
                    }
                }
            }

            return _stack.Peek();
        }

        private List<string> ParseFunctionBody(string funcBody)
        {
            List<string> funcBodyElements = new();
            bool isFunctionParametersList = false;
            string currentElement = string.Empty;

            foreach (var ch in funcBody)
            {
                if (ch == '(')
                {
                    isFunctionParametersList = true;
                    currentElement += ch;
                    continue;
                }

                if (ch == ')')
                {
                    isFunctionParametersList = false;
                    currentElement += ch;
                    continue;
                }

                if (Char.IsWhiteSpace(ch))
                {
                    if (isFunctionParametersList)
                    {
                        currentElement += ch;
                    }
                    else
                    {
                        funcBodyElements.Add(currentElement);
                        currentElement = string.Empty;
                    }

                    continue;
                }

                currentElement += ch;
            }

            if (!string.IsNullOrEmpty(currentElement))
            {
                funcBodyElements.Add(currentElement);
            }

            return funcBodyElements;
        }
    }
}
