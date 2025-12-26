namespace MagmaHeart.AI.Actions
{
    public record ActionArgs(AIUnitModel Executor, ActionData Data);
    public record ActionArgs<TUnit>(TUnit TypedExecutor, ActionData Data) : ActionArgs(TypedExecutor, Data)
        where TUnit : AIUnitModel;
}