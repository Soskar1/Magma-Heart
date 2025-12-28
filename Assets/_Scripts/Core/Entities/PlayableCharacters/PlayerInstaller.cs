using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.SceneLoading;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerInstaller : IInstaller
    {
        public Player Install(Player playerPrefab, InputContext inputContext, RoomGrid roomGrid, IActionPreviewProvider actionPreviewProvider)
        {
            Player spawnedPlayer = GameObject.Instantiate(playerPrefab);
            spawnedPlayer.Initialize(inputContext.mouseListener, roomGrid, actionPreviewProvider);

            return spawnedPlayer;
        }

        public void Dispose() { }
    }
}
