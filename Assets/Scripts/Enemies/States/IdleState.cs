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
            _animator.SetBool("playerOutOfRange", false);
        }

        public override void OnLogic()
        {
            _agent.SetDestination(_agent.transform.position);
        }
    }
}