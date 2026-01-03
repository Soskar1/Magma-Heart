using MagmaHeart.Core.Input.Mouse;

namespace MagmaHeart.Core.Input
{
    public record InputContext(UserInput UserInput, MouseListener MouseListener, MouseHoverEngine MouseHoverEngine);
}
