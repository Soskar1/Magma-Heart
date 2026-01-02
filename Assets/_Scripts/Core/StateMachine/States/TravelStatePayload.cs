using MagmaHeart.StateMachine;
using UnityEngine;

namespace MagmaHeart.Core.StateMachine.States
{
    public record TravelStatePayload(TravelReason Reason) : StatePayload;
}
