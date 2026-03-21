using MagmaHeart.Abilities;
using MagmaHeart.Core.Entities.Models;
using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    public class MagmaHeartPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        private MagmaHeartModel m_model;

        public void Initialize(MagmaHeartModel magmaHeartModel)
        {
            m_model = magmaHeartModel;
            m_model.OnParameterValueChanged += HandleOnParameterValueChanged;
        }

        public void OnDisable() => m_model.OnParameterValueChanged -= HandleOnParameterValueChanged;

        private void HandleOnParameterValueChanged(object sender, OnParameterValueChangedEventArgs e)
        {
            m_text.text = e.NewValue.ToString();
        }
    }
}
