namespace MagmaHeart.Abilities.Effects
{
    public sealed record SpendResourceEffect(int ExecutorId, ParameterId Parameter, int Amount) : AbilityEffect(ExecutorId);
}
