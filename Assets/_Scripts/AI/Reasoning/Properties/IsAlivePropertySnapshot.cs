namespace MagmaHeart.AI
{
    public record IsAlivePropertySnapshot(bool IsAlive) : PropertySnapshot()
    {
        public static implicit operator bool(IsAlivePropertySnapshot property) => property.IsAlive;
    }
}
