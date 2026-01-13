using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Statistics;
using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_completedRoomsTextField;
        [SerializeField] private TextMeshProUGUI m_currentRecordTextField;
        [SerializeField] private TextMeshProUGUI m_newRecordTextField;

        private Battle m_battle;
        private CompletedRoomsCounter m_completedRoomsCounter;

        // TODO: Remove this after the save system implementation
        private const string m_saveKey = "CompletedRoomsRecord";

        public void Initialize(Battle battle, CompletedRoomsCounter counter)
        {
            m_battle = battle;
            m_completedRoomsCounter = counter;
            m_battle.OnBattleEnded += HandleOnBattleEnded;
        }

        private void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            if (!args.IsPlayerVictory)
            {
                DisplayStatistics();
                gameObject.SetActive(true);
                m_battle.OnBattleEnded -= HandleOnBattleEnded;
            }
        }

        private void DisplayStatistics()
        {
            int completedRooms = m_completedRoomsCounter.CompletedRooms;
            m_completedRoomsTextField.text = $"{m_completedRoomsTextField.text} {completedRooms}";

            int currentRecord = PlayerPrefs.GetInt(m_saveKey, 0);
            
            if (completedRooms > currentRecord)
            {
                currentRecord = completedRooms;
                m_newRecordTextField.gameObject.SetActive(true);

                // TODO: Remove this after the save system implementation
                PlayerPrefs.SetInt(m_saveKey, completedRooms);
                PlayerPrefs.Save();
            }

            m_currentRecordTextField.text = $"{m_currentRecordTextField.text} {currentRecord}";
        }

        public void Restart() => MagmaHeart.Restart();
        public void ExitGame() => MagmaHeart.ExitGame();
    }
}