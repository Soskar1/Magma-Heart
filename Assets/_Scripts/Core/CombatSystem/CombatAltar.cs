using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatAltar : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Destroy(gameObject);
        }
    }
}

