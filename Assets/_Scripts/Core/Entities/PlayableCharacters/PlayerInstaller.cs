using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.SceneLoading;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerInstaller : IInstaller
    {
        public Entity Install(Entity playerPrefab, InputContext inputContext, RoomGrid roomGrid, IActionPreviewProvider actionPreviewProvider)
        {
            Entity spawnedPlayer = GameObject.Instantiate(playerPrefab);
            PlayerTurnController turnController = new PlayerTurnController(inputContext.MouseListener, actionPreviewProvider);
            spawnedPlayer.Initialize(roomGrid, true, turnController);

            return spawnedPlayer;
        }

        public void Dispose() { }
    }
}
