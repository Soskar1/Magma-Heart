using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines.States
{
    public class GameOverState : IState
    {
        public Task EnterAsync()
        {
            Debug.Log("Enter Game Over state not implemented");
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            Debug.Log("Exit Game Over state ot implemented");
            return Task.CompletedTask;
        }
    }
}
