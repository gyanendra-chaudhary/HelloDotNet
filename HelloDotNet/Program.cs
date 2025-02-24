namespace HelloDotNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        // Construct from per post
        public TreeNode ConstructFromPerPost(int[] preorder, int[] postorder)
        {
            if ((preorder == null || postorder == null) || (preorder.Length != postorder.Length) ||
                (preorder[0] != postorder[preorder.Length - 1]))
                return null;
            TreeNode result = new TreeNode();
            
            return result;
        }
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
}