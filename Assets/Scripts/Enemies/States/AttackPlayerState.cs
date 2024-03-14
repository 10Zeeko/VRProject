using System;
using UnityEngine;
using UnityHFSM;

namespace VRProject.Enemy
{
    public class AttackPlayerState : SneakEnemyStateBase
    {
        public AttackPlayerState(
            bool needsExitTime,
            SneakEnemy Enemy,
            Action<State<SneakEnemyState, StateEvent>> onEnter,
            float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter) {}

        public override void OnEnter()
        {
            _agent.isStopped = true;
            base.OnEnter();
            _animator.Play("AttackPlayer");
        }
    }
}