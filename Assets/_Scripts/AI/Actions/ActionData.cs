using UnityEngine;

namespace MagmaHeart.AI.Actions
{
    public abstract class ActionData : ScriptableObject
    {
        public abstract ActionDefinition GetDefinition();
    }
}
