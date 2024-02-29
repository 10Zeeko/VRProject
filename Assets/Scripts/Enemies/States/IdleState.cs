using UnityEngine;

namespace VRProject.Enemy
{
    public class IdleState : SneakEnemyStateBase
    {
        public IdleState(bool needsExitTime, SneakEnemy sneakEnemy) : base(needsExitTime, sneakEnemy)
        {
            base.onEnter();
            _agent.isStopped = true;
            Animator.Play("Idle");
        }
    }
}