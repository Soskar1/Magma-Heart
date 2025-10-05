using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class CombatUserInput
    {
        private UserInput m_userInput;
        public UserInput UserInput => m_userInput;
        
        public TurnBasedMouseControl MouseControl { get; init; }

        public Controls.TurnBasedPlayerActions TurnBasedPlayer => m_userInput.Controls.TurnBasedPlayer;
        public Vector2 CameraMovement => TurnBasedPlayer.CameraMovement.ReadValue<Vector2>();
        public float MouseScroll => TurnBasedPlayer.MouseScroll.ReadValue<float>();

        public CombatUserInput(UserInput userInput, DungeonGrid grid)
        {
            m_userInput = userInput;
            MouseControl = new TurnBasedMouseControl(this, grid);
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