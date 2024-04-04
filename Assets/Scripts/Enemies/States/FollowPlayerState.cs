using UnityEngine;

namespace VRProject.Enemy
{
    public class FollowPlayerState : SneakEnemyStateBase
    {
        private Transform Target;
        [SerializeField] private float _speed = 1.0f;

        public FollowPlayerState(bool needsExitTime, SneakEnemy sneakEnemy, Transform Target) : base(needsExitTime, sneakEnemy)
        {
            this.Target = Target;
            _agent.speed = _speed;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _animator.SetBool("playerOutOfRange", true);
        }
        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                // you can add a more complex movement prediction algorithm like what 
                // we did in AI Series 44: https://youtu.be/1Jkg8cKLsC0
                _agent.SetDestination(Target.position);
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("playerOutOfRange", false);
                fsm.StateCanExit();
            }
        }
    }
}