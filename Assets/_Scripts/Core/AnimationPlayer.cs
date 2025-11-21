using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        private TaskCompletionSource<bool> m_animationEnded;

        private int m_currentAnimationState;
        public int CurrentAnimationState
        {
            get => m_currentAnimationState;
            set
            {
                m_currentAnimationState = value;
                Animator.CrossFade(CurrentAnimationState, 0);
            }
        }

        public void Awake() => Animator = GetComponent<Animator>();

        public void PlayAnimations()
        {
            if (DoesCurrentAnimationEnded())
            {
                Animator.CrossFade(CurrentAnimationState, 0, 0, 0);

                if (m_animationEnded != null)
                { 
                    m_animationEnded.SetResult(true);
                    m_animationEnded = null;
                }
            }
        }

        public bool DoesCurrentAnimationEnded() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;

        public Task WaitForAnimationEnd()
        {
            m_animationEnded = new TaskCompletionSource<bool>();
            return m_animationEnded.Task;
        }
    }
}