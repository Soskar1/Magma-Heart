namespace MagmaHeart.AI.States
{
    public record IsAlivePropertySnapshot(bool IsAlive) : PropertySnapshot()
    {
        public static implicit operator bool(IsAlivePropertySnapshot property) => property.IsAlive;
    }
}
