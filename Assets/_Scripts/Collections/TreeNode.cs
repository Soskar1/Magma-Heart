using System.Collections.Generic;

namespace MagmaHeart.Collections
{
    public class TreeNode<T>
    {
        public T Value { get; set; }

        public List<TreeNode<T>> ChildrenNodes { get; init; }
        public TreeNode<T> Parent { get; init; }

        public TreeNode(T value, TreeNode<T> parent = null)
        {
            Value = value;

            ChildrenNodes = new List<TreeNode<T>>();
            Parent = parent;
        }

        public void AddChild(T value)
        {
            TreeNode<T> node = new TreeNode<T>(value, this);
            ChildrenNodes.Add(node);
        }
    }
}
