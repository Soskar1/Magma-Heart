using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation
{
    public enum CursorType
    {
        Default,
        Attack,
        Invalid
    }

    public class CursorPresenter : MonoBehaviour
    {
        [SerializeField] private Texture2D m_defaultCursorTexture;
        [SerializeField] private Texture2D m_attackCursorTexture;
        [SerializeField] private Texture2D m_invalidCursorTexture;

        public void SetCursor(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Default:
                    Cursor.SetCursor(m_defaultCursorTexture, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.Attack:
                    Cursor.SetCursor(m_attackCursorTexture, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.Invalid:
                    Cursor.SetCursor(m_invalidCursorTexture, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(m_defaultCursorTexture, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}
