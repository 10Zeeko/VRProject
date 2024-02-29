using UnityEngine;

namespace VRProject.Enemy
{
    public class FollowPlayerState : SneakEnemyStateBase
    {
        private Transform Target;

        public FollowPlayerState(bool needsExitTime, SneakEnemy sneakEnemy) : base(needsExitTime, sneakEnemy)
        {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _animator.Play("FollowPlayer");
        }
        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                _agent.SetDestination(Target.position);
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}