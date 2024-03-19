using System;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace VRProject.Enemy
{
    public class SneakEnemyStateBase : State<SneakEnemyState, StateEvent>
    {
        protected readonly SneakEnemy _sneakEnemy;
        protected readonly NavMeshAgent _agent;
        protected readonly Animator _animator;
        protected bool _requestedExit;
        protected float _exitTime;

        protected readonly Action<State<SneakEnemyState, StateEvent>> onEnter;
        protected readonly Action<State<SneakEnemyState, StateEvent>> onLogic;
        protected readonly Action<State<SneakEnemyState, StateEvent>> onExit;
        protected readonly Func<State<SneakEnemyState, StateEvent>, bool> canExit;

        public SneakEnemyStateBase(bool needsExitTime,
            SneakEnemy sneakEnemy,
            float exitTime = 0.1f,
            Action<State<SneakEnemyState, StateEvent>> onEnter = null,
            Action<State<SneakEnemyState, StateEvent>> onLogic = null,
            Action<State<SneakEnemyState, StateEvent>> onExit = null,
            Func<State<SneakEnemyState, StateEvent>, bool> canExit = null)
        {
            this._sneakEnemy = sneakEnemy;
            this.onEnter = onEnter;
            this.onLogic = onLogic;
            this.onExit = onExit;
            this.canExit = canExit;
            this._exitTime = exitTime;
            this.needsExitTime = needsExitTime;
            
            _agent = sneakEnemy._agent;
            _animator = sneakEnemy._animator;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _requestedExit = false;
            onEnter?.Invoke(this);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (_requestedExit && timer.Elapsed >= _exitTime)
            {
                fsm.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if (!needsExitTime || canExit != null && canExit(this))
            {
                fsm.StateCanExit();
            }
            else
            {
                _requestedExit = true;
            }
        }
    }
}