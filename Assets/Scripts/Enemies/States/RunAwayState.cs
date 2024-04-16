using UnityEngine;

namespace VRProject.Enemy
{
    public class RunAwayState : SneakEnemyStateBase
    {
        private Transform Target;
        public RunAwayState(bool needsExitTime, SneakEnemy sneakEnemy, Transform Target) : base(needsExitTime, sneakEnemy)
        {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _animator.SetBool("EnemyDetected", true);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                Vector3 runTo = _agent.transform.position + ((_agent.transform.position - Target.position) * 5);
                float distance = Vector3.Distance(_agent.transform.position, Target.position);
                _agent.SetDestination(runTo);
            }

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}