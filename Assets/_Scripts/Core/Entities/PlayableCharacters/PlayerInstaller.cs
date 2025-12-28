using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.SceneLoading;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerInstaller : IInstaller
    {
        public Player Install(Player playerPrefab, InputContext inputContext, ActionDatabase database, RoomGrid roomGrid)
        {
            ActionSelector actionSelectorChain = new AttackActionSelector(database.Get<AttackAction>());
            actionSelectorChain.Next = new MovementActionSelector(database.Get<MovementAction>());
            ActionPreviewService previewService = new ActionPreviewService(actionSelectorChain);

            Player spawnedPlayer = GameObject.Instantiate(playerPrefab);
            spawnedPlayer.Initialize(inputContext.mouseListener, roomGrid, previewService);

            return spawnedPlayer;
        }

        public void Dispose() { }
    }
}
