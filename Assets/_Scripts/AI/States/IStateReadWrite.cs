namespace MagmaHeart.AI.States
{
    public interface IStateReadWrite
    {
        public Board Board { get; }

        public T GetProperty<T>(AIUnit unit) where T : PropertySnapshot;
        public void Write<T> (T property) where T : PropertySnapshot;
    }
}