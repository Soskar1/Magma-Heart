using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionRunnerInstaller : IInstaller
    {
        public ActionRunner Install(int moveCommandSpeed)
        {
            ActionRunner runner = new ActionRunner();
            runner.RegisterPresenter<MoveCommand>(new MoveCommandPresenter(moveCommandSpeed));
            runner.RegisterPresenter<ApplyDamageCommand>(new ApplyDamageCommandPresenter());

            return runner;
        }

        public void Dispose()
        {
            
        }
    }
}
