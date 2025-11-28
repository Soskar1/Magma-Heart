using System;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class OnCanExecuteActionsChangedEventArgs : EventArgs
    {
        public bool CanExecute { get; init; }

        public OnCanExecuteActionsChangedEventArgs(bool canExecute) => CanExecute = canExecute;
    }
}
