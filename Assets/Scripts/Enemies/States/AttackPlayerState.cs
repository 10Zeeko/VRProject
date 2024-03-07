using System;
using UnityEngine;
using UnityHFSM;

namespace VRProject.Enemy
{
    public class AttackPlayerState : SneakEnemyStateBase
    {
        public AttackPlayerState(
            bool needsExitTime,
            SneakEnemy sneakEnemy,
            Action<State<SneakEnemyState, StateEvent>> onEnter,
            float exitTime = 0.1f) : base(needsExitTime, sneakEnemy, exitTime, onEnter)
        {
            
        }

        public override void OnEnter()
        {
            _agent.isStopped = true;
            base.OnEnter();
            _animator.Play("AttackPlayer");
        }
        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                _agent.SetDestination(new Vector3(10, 0, 10));
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}