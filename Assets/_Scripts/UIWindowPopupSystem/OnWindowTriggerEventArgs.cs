using System;
using MagmaHeart.UIWindowPopupSystem.Definitions;

namespace MagmaHeart.UIWindowPopupSystem
{
    public class OnWindowTriggerEventArgs : EventArgs
    {
        public WindowTriggerDefinition Trigger { get; init; }

        public OnWindowTriggerEventArgs(WindowTriggerDefinition trigger) => Trigger = trigger;
    }
}