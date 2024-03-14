using UnityEngine;

namespace VRProject.Enemy
{
    public class RunAwayState : SneakEnemyStateBase
    {
        private Transform Target;
        public RunAwayState(bool needsExitTime, SneakEnemy sneakEnemy) : base(needsExitTime, sneakEnemy)
        {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _animator.Play("Stand Up");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            // Run away from player using the target (player) position
            // In progress
            _agent.SetDestination(Target.position);
            
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}