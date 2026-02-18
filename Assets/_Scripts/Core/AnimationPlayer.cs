using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        private int m_currentAnimationState;
        public int CurrentAnimationState
        {
            get => m_currentAnimationState;
            set
            {
                m_currentAnimationState = value;

                // Note: Here I am using Play instead of CrossFade to ensure the animation starts from the beginning immediately.
                // With CrossFade, there could be a slight blending delay.
                Animator.Play(CurrentAnimationState, 0, 0);

                // Note: Force the animator to update immediately to apply the new state
                Animator.Update(0);
            }
        }

        public void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public async Task WaitForStateToFinish(int stateHash, CancellationToken cancellationToken = default)
        {
            // 1) Wait until we are actually in the state
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var info = Animator.GetCurrentAnimatorStateInfo(0);
                if (info.shortNameHash == stateHash) break;

                await Task.Yield();
            }

            // 2) Wait until it finishes (non-looping)
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!Animator.IsInTransition(0))
                {
                    var info = Animator.GetCurrentAnimatorStateInfo(0);
                    if (info.shortNameHash == stateHash && info.normalizedTime >= 1f)
                        return;
                }

                await Task.Yield();
            }
        }
    }
}