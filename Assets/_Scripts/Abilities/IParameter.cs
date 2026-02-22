namespace MagmaHeart.Abilities
{
    public interface IParameter
    {
        public ParameterId Id { get; }
        public float CurrentValue { get; set; }
    }
}
