using UnityEngine;

namespace VRProject.Enemy
{
    public class RunAwayState : SneakEnemyStateBase
    {
        private Transform Target;
        private float _timeRunning = 0.0f;
        private Vector3[] _safeSpots;
        private float _safespotInterval = 15.0f;

        public RunAwayState(bool needsExitTime, SneakEnemy sneakEnemy, Transform Target, Vector3[] safeSpots) : base(needsExitTime, sneakEnemy)
        {
            this.Target = Target;
            _safeSpots = safeSpots;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _timeRunning = 0.0f;
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
                
                _timeRunning += Time.deltaTime;
                
                if (_timeRunning >= _safespotInterval)
                {
                    GoToRandomSafespot();
                }
            }
        }

        private void GoToRandomSafespot()
        {
            if (_safeSpots.Length > 0)
            {
                _timeRunning = 0.0f;
                Vector3 randomSafespot = _safeSpots[Random.Range(0, _safeSpots.Length)];
                _agent.SetDestination(randomSafespot);
            }
        }
    }
}