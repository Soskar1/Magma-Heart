using MagmaHeart.Core.CombatSystem;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class GameOverUI : MonoBehaviour
    {
        private Battle m_battle;

        public void Initialize(Battle battle)
        {
            m_battle = battle;
            m_battle.OnBattleEnded += HandleOnBattleEnded;
        }

        private void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            if (!args.IsPlayerVictory)
            {
                gameObject.SetActive(true);
                m_battle.OnBattleEnded -= HandleOnBattleEnded;
            }
        }

        public void Restart() => MagmaHeart.Restart();
        public void ExitGame() => MagmaHeart.ExitGame();
    }
}