namespace MagmaHeart.AI.Actions
{
    public record ActionArgs(AIUnitModel Executor);
    public record ActionArgs<T>(T TypedExecutor) : ActionArgs(TypedExecutor) where T : AIUnitModel;
}