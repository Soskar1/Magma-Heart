using MagmaHeart.Core.Abilities.Presentation.Execution;
using MagmaHeart.Core.Abilities.Selection;
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
        private PlayerTurnController m_turnController;

        public PlayerContext Install(EntitySpawner spawner, EntityData playerData, InputContext inputContext, AbilityExecutionRunner abilityExecutionRunner, GameWorld world, GraphicRaycaster raycaster)
        {
            Entity player = spawner.Spawn(playerData, true);
            // player.gameObject.SetActive(false);

            MouseHover mouseHover = new MouseHover(inputContext.MouseListener, world, raycaster);
            AbilitySelector abilitySelector = new AbilitySelector(world);
            m_turnController = new PlayerTurnController(inputContext.MouseListener, mouseHover, abilityExecutionRunner, abilitySelector, inputContext.UserInput);

            return new PlayerContext(player, m_turnController);
        }

        public void Dispose()
        {
            m_turnController.Dispose();
        }
    }
}
