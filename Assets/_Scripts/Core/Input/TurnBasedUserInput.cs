using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class TurnBasedUserInput
    {
        private UserInput m_userInput;
        
        public MouseControl MouseControl { get; init; }

        public Controls.TurnBasedPlayerActions TurnBasedPlayer => m_userInput.Controls.TurnBasedPlayer;
        public Vector2 CameraMovement => TurnBasedPlayer.CameraMovement.ReadValue<Vector2>();

        public TurnBasedUserInput(UserInput userInput, DungeonGrid grid)
        {
            m_userInput = userInput;
            MouseControl = new MouseControl(m_userInput, grid);
        }

        public void Enable()
        {
            TurnBasedPlayer.Enable();
            MouseControl.Enable();
        }

        public void Disable()
        {
            TurnBasedPlayer.Disable();
            MouseControl.Disable();
        }
    }
}