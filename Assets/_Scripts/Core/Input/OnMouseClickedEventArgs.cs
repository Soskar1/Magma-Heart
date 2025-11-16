namespace MagmaHeart.Core.Input
{
    public class OnMouseClickedEventArgs
    {
        public bool IsOverUIElement { get; init; }

        public OnMouseClickedEventArgs(bool isOverUIElement) => IsOverUIElement = isOverUIElement;
    }
}
