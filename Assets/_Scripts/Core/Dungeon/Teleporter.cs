using MagmaHeart.Core.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagmaHeart.Core.Dungeon
{
    public class Teleporter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerBehaviour>() != null)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}