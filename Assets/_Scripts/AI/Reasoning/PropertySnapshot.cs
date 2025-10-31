namespace MagmaHeart.AI.Reasoning
{
    public abstract record PropertySnapshot(float Value, float Weight)
    {
        public virtual PropertySnapshot Merge(PropertySnapshot other)
            => GetType() == other.GetType()
                ? this with { Value = Value + other.Value }
                : this;

        public float Evaluation => Value * Weight;
    }
}
