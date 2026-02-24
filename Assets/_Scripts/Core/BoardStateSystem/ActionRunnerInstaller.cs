using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Commands;
using MagmaHeart.Core.BoardStateSystem.Actions.Presenters;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionRunnerInstaller : IInstaller
    {
        public ActionExecutor Install(SpawnService spawnService)
        {
            CommandRunner commandRunner = new CommandRunner(useMemory: false);
            IBoardCommandPresenter defaultPresenter = new DefaultCommandPresenter(commandRunner);

            ActionExecutor executor = new ActionExecutor(defaultPresenter);
            executor.RegisterPresenter<MoveCommand>(new MoveCommandPresenter(commandRunner));
            executor.RegisterPresenter<ApplyDamageCommand>(new ApplyDamageCommandPresenter(commandRunner, spawnService));

            return executor;
        }

        public void Dispose()
        {
            
        }
    }
}
