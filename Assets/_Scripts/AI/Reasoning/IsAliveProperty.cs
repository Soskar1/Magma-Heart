namespace MagmaHeart.AI.Reasoning
{
    public record IsAliveProperty(bool IsAlive) : PropertySnapshot()
    {
        public static implicit operator bool(IsAliveProperty property) => property.IsAlive;
    }
}
