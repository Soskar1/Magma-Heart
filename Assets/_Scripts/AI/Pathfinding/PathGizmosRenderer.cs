using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Pathifinding
{
    public class PathGizmosRenderer : MonoBehaviour
    {
        public List<Vector2> CurrentPath { get; set; } = new List<Vector2>();

        private void OnDrawGizmosSelected()
        {
            if (CurrentPath == null)
                return;

            Gizmos.color = Color.green;
            for (int i = 1; i < CurrentPath.Count; ++i)
                Gizmos.DrawLine(CurrentPath[i - 1], CurrentPath[i]);
        }
    }
}