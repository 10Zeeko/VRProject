using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace VRProject.Enemy
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class SneakEnemy : MonoBehaviour
    {
        private StateMachine<SneakEnemyState, StateEvent> _enemyFsm;
        private Animator _animator;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyFsm = new ();
            
            _enemyFsm.AddState(SneakEnemyState.Idle, new IdleState(true, this));
            _enemyFsm.AddState(SneakEnemyState.FollowPlayer, new FollowPlayerState(true, this));
            _enemyFsm.AddState(SneakEnemyState.AttackPlayer, new AttackPlayerState(true, this));
            _enemyFsm.AddState(SneakEnemyState.RunAway, new RunAwayState(true, this));
            
            _enemyFsm.SetStartState(SneakEnemyState.Idle);
            
            _enemyFsm.Init();
        }

        private void Update()
        {
            _enemyFsm.OnLogic();
        }
    }
}