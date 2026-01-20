using NUnit.Framework;

namespace MagmaHeart.Collections.Tests
{
    internal class TreeNodeTests
    {
        [Test]
        public void TreeNode_Constructor_CreatesNodeWithoutParentAndChildren()
        {
            TreeNode<int> treeNode = new TreeNode<int>(2);

            Assert.That(treeNode.Parent, Is.Null);
            Assert.That(treeNode.Value, Is.EqualTo(2));
            Assert.That(treeNode.ChildrenNodes, Is.Empty);
        }

        [Test]
        public void TreeNode_AddChild_AddsChildNode()
        {
            TreeNode<int> treeNode = new TreeNode<int>(2);

            treeNode.AddChild(5);

            TreeNode<int> child = treeNode.ChildrenNodes[0];
            Assert.That(child.Parent, Is.EqualTo(treeNode));
            Assert.That(child.Value, Is.EqualTo(5));
        }
    }
}
