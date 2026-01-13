using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class EscapeScreen : MonoBehaviour
    {
        public void Restart() => MagmaHeart.Restart();
        public void ExitGame() => MagmaHeart.ExitGame();
    }
}