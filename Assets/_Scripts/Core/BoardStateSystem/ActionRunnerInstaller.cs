using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class ActionRunnerInstaller : IInstaller
    {
        public ActionRunner Install(int moveCommandSpeed)
        {
            MoveCommandPresenter moveCommandPresenter = new MoveCommandPresenter(moveCommandSpeed);
            ApplyDamageCommandPresenter applyDamagePresenter = new ApplyDamageCommandPresenter();
            return new ActionRunner(moveCommandPresenter, applyDamagePresenter);
        }

        public void Dispose()
        {
            
        }
    }
}
