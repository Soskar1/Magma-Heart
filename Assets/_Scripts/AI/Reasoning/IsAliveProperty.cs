namespace MagmaHeart.AI.Reasoning
{
    public record IsAliveProperty(bool IsAlive) : PropertySnapshot(0, 0)
    {
        public static implicit operator bool(IsAliveProperty property) => property.IsAlive;
    }
}
