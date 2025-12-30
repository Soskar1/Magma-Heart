using UnityEngine;

namespace MagmaHeart.Core.StateMachines.States
{
    public record TravelStatePayload(TravelReason Reason) : StatePayload;
}
