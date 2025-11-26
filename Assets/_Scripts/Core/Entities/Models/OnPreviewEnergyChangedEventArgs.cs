using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class OnPreviewEnergyChangedEventArgs : EventArgs
    {
        public int CurrentEnergy { get; init; }
        public int PreviewCost { get; init; }

        public OnPreviewEnergyChangedEventArgs(int previewCost, int currentEnergy)
        {
            PreviewCost = previewCost;
            CurrentEnergy = currentEnergy;
        }
    }
}
