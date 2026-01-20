namespace MagmaHeart.Collections
{
    public class ChainNode<T>
    {
        public T Value { get; set; }

        public ChainNode<T> Next { get; set; }

        public ChainNode() { }

        public ChainNode(T value)
        {
            Value = value;
        }

        public static implicit operator ChainNode<T>(CircularList<T> circularList)
        {
            ChainNode<T> head = new ChainNode<T>(circularList.Head);
            ChainNode<T> current = head.Next;
            ChainNode<T> previous = head;

            circularList.Next();
            while (!circularList.Head.Equals(head.Value))
            {
                current = new ChainNode<T>(circularList.Head);
                previous.Next = current;

                previous = current;
                current = current.Next;

                circularList.Next();
            }

            previous.Next = head;

            return head;
        }
    }
}