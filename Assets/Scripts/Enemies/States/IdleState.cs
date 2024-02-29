using UnityEngine;

namespace VRProject.Enemy
{
    public class IdleState : SneakEnemyStateBase
    {
        public IdleState(bool needsExitTime, SneakEnemy sneakEnemy) : base(needsExitTime, sneakEnemy)
        {
            base.OnEnter();
            _agent.isStopped = true;
            _animator.Play("Crouching Idle");
        }
    }
}