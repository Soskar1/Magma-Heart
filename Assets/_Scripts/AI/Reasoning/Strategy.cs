using UnityEngine;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy : ScriptableObject
    {
        public abstract float EvaluateState(IBoardGameWorld world);
    }
}
