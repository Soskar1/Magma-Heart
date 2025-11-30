using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class CombatUserInput
    {
        private UserInput m_userInput;
        public UserInput UserInput => m_userInput;
        
        public CombatMouseControl MouseControl { get; init; }

        public CombatUserInput(UserInput userInput, DungeonGrid grid, List<MouseOverUIElement> uiElementEvents)
        {
            m_userInput = userInput;
            MouseControl = new CombatMouseControl(this, grid, uiElementEvents);
        }

        public void Enable()
        {
            MouseControl.Enable();
        }

        public void Disable()
        {
            MouseControl.Disable();
        }
    }
}