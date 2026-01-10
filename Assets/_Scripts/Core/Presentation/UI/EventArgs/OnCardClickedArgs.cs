using MagmaHeart.Core.Artifacts;
using System;

namespace MagmaHeart.Core.Presentation.UI
{
    public class OnCardClickedArgs : EventArgs
    {
        public RewardCard RewardCard { get; init; }

        public OnCardClickedArgs(RewardCard rewardCard) => RewardCard = rewardCard;
    }
}
