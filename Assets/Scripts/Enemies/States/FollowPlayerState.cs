using UnityEngine;

namespace VRProject.Enemy
{
    public class FollowPlayerState : SneakEnemyStateBase
    {
        private Transform _target;
        private Player _player;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField]
        [Range(-1, 1)]
        public float MovementPredictionThreshold = 0;
        [SerializeField]
        [Range(0.25f, 2f)]
        public float MovementPredictionTime = 1f;

        public FollowPlayerState(bool needsExitTime, SneakEnemy sneakEnemy, Transform target, Player player) : base(needsExitTime, sneakEnemy)
        {
            this._target = target;
            _agent.speed = _speed;
            _player = player;
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
                float timeToPlayer = Vector3.Distance(_target.transform.position, _agent.transform.position) / _agent.speed;
                if (timeToPlayer > MovementPredictionTime)
                {
                    timeToPlayer = MovementPredictionTime;
                }
                
                Vector3 targetPosition = _target.transform.position + _player.AverageVelocity * timeToPlayer;
                Vector3 directionToTarget = (targetPosition - _agent.transform.position).normalized;
                Vector3 directionToPlayer = (_target.transform.position - _agent.transform.position).normalized;

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);

                if (dot < MovementPredictionThreshold)
                {
                    targetPosition = _target.transform.position;
                }

                _agent.SetDestination(targetPosition);
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("playerOutOfRange", false);
                fsm.StateCanExit();
            }
        }
    }
}