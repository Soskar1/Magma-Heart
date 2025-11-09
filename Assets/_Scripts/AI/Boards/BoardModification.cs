namespace MagmaHeart.AI.Boards
{
    public abstract record BoardModification
    {
        public abstract void Apply(SimulatedBoard board);
        public abstract void Undo(SimulatedBoard board);
    }
}
