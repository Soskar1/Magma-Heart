using MagmaHeart.UIWindowPopupSystem.Definitions;
using UnityEngine;

namespace MagmaHeart.Core.TutorialSystem
{
    [CreateAssetMenu(fileName = "new window trigger definition", menuName = "Magma Heart Data/UI/TutorialModel Trigger")]
    public class TutorialTriggerDefinition : WindowTriggerDefinition
    {
        [SerializeField] private TutorialFlags m_flag;

        public TutorialFlags Flag => m_flag;
    }
}

