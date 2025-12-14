namespace MagmaHeart.AI.Actions
{
    public record ActionArgs(AIUnitModel Executor);
    public record ActionArgs<TUnit>(TUnit TypedExecutor) : ActionArgs(TypedExecutor)
        where TUnit : AIUnitModel;
}