using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public record PlayerContext(Entity Player, PlayerTurnController TurnController);

    public class PlayerInstaller : IInstaller
    {
        public PlayerContext Install(EntitySpawner spawner, EntityData playerData, InputContext inputContext, ActionExecutor actionRunner, GameWorld gameWorld, GraphicRaycaster raycaster)
        {
            Entity player = spawner.Spawn(playerData, true);
            player.gameObject.SetActive(false);

            MouseHover mouseHover = new MouseHover(inputContext.MouseListener, gameWorld, raycaster);
            PlayerTurnController turnController = new PlayerTurnController(inputContext.MouseListener, mouseHover, actionRunner, gameWorld);

            return new PlayerContext(player, turnController);
        }

        public void Dispose() { }
    }
}
