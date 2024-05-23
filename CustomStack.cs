namespace LogicalSolver
{
    public class CustomStack
    {
        private List<TreeNode> _nodes = new List<TreeNode>();

        public void Push(TreeNode node) => _nodes.Add(node);

        public TreeNode Peek() => _nodes[_nodes.Count - 1];

        public TreeNode Pop()
        {
            var node = Peek();
            _nodes.RemoveAt(_nodes.Count - 1);
            return node;
        }
    }
}
