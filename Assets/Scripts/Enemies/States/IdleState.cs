using UnityEngine;

namespace VRProject.Enemy
{
    public class IdleState : SneakEnemyStateBase
    {
        private float AnimationLoopCount = 0;
        public IdleState(bool needsExitTime, SneakEnemy sneakEnemy) : base(needsExitTime, sneakEnemy) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.isStopped = true;
            _animator.Play("Crouching Idle");
        }
        public override void OnLogic()
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

            if (state.normalizedTime >= AnimationLoopCount + 1)
            {
                float value = Random.value;
                if (value < 0.95f)
                {
                    if (!state.IsName("Idle_A"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    _animator.Play("Idle_A");
                }
                else if (value < 0.975f)
                {
                    if (!state.IsName("Idle_B"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    _animator.Play("Idle_B");
                }
                else
                {
                    if (!state.IsName("Idle_C"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    _animator.Play("Idle_C");
                }
            }
            
            base.OnLogic();
        }
    }
}