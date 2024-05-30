using System;
using UnityEngine;
using UnityHFSM;

namespace VRProject.Enemy
{
    public class AttackPlayerState : SneakEnemyStateBase
    {
        private Transform Target;
        [SerializeField] private float _speed = 2.0f;
        [SerializeField] private bool _shouldAttack = false;
        private float _attackCheckTimer = 0.0f;
        private AudioSource _audioSource;

        public AttackPlayerState(
            bool needsExitTime,
            SneakEnemy Enemy,
            Action<State<SneakEnemyState, StateEvent>> onEnter,
            Transform Target,
            AudioSource audioSource,
            float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter)
        {
            this.Target = Target;
            _agent.speed = _speed;
            _audioSource = audioSource;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _shouldAttack = false;
        }
        
        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit && _shouldAttack)
            {
                _animator.SetBool("flashlightOff", true);
                _agent.SetDestination(Target.position);
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("flashlightOff", false);
                _shouldAttack = false;
                fsm.StateCanExit();
            }

            _attackCheckTimer += Time.deltaTime;
            if (_attackCheckTimer >= 0.5f)
            {
                _attackCheckTimer = 0.0f;
                if (!_shouldAttack)
                {
                    _shouldAttack = UnityEngine.Random.value > 0.95f;
                    if (_shouldAttack)
                    {
                        _audioSource.Play();
                    }
                }
            }
        }
    }
}