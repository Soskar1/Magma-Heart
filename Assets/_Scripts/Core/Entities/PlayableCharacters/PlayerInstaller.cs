using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerInstaller : IInstaller
    {
        public Entity Install(EntitySpawner spawner, EntityData playerData, InputContext inputContext, IActionPreviewProvider actionPreviewProvider, ActionRunner actionRunner)
        {
            PlayerTurnController turnController = new PlayerTurnController(inputContext.MouseListener, actionPreviewProvider, actionRunner);
            Entity player = spawner.Spawn(playerData, true, turnController);
            player.gameObject.SetActive(false);

            return player;
        }

        public void Dispose() { }
    }
}
