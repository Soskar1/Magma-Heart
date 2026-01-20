using System;

namespace MagmaHeart.Core.TutorialSystem
{
    public class OnTutorialFlagSetEventArgs : EventArgs
    {
        public TutorialFlags NewFlag { get; init; }

        public OnTutorialFlagSetEventArgs(TutorialFlags flag) => NewFlag = flag;
    }
}