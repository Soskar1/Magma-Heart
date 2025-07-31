namespace MagmaHeart.Core
{
    public class UserInput
    {
        public Controls Controls { get; private set;}

        public UserInput() => Controls = new Controls();

        public void Enable() => Controls.Enable();
        public void Disable() => Controls.Disable();
    }
}