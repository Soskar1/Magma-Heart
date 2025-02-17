using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private UserInput m_userInput;
        private Rigidbody2D m_rb2d;
        [SerializeField] private float m_speed;

        private void Awake() 
        {
            m_userInput = new UserInput();
            m_rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start() => m_userInput.Enable();
        
        public void FixedUpdate()
        {
            m_rb2d.AddForce(m_userInput.Movement * m_speed);
        }                
    }
}
