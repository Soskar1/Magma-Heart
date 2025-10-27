using System;

namespace MagmaHeart.Collections
{
    public class ChainNode<T> where T : IEquatable<T>
    {
        public T Value { get; set; }

        public ChainNode<T> Next { get; set; }

        public ChainNode(T value)
        {
            Value = value;
        }

        public static implicit operator ChainNode<T>(CircularList<T> circularList)
        {
            ChainNode<T> head = null;
            ChainNode<T> current = head;
            ChainNode<T> previous = null;

            foreach (T value in circularList)
            {
                if (head == null)
                {
                    head = new ChainNode<T>(value);
                    current = head.Next;
                    previous = head;
                }
                else
                {
                    current = new ChainNode<T>(value);
                    previous.Next = current;

                    previous = current;
                    current = current.Next;
                }
            }

            previous.Next = head;

            return head;
        }
    }
}