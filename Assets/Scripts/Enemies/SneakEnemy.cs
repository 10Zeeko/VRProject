using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityHFSM;
using VRProject.Sensors;

namespace VRProject.Enemy
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class SneakEnemy : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GameObject player;
        
        [FormerlySerializedAs("AttackCooldown")]
        [Header("Attack Config")]
        [SerializeField]
        [Range(0.1f, 5f)]
        private float attackCooldown = 2;
        
        [Header("Sensors")]
        [SerializeField] private PlayerSensor followPlayerSensor;
        [SerializeField] private PlayerSensor rangeAttackPlayerSensor;
        [SerializeField] private PlayerSensor runAwayPlayerSensor;
        
        [Header("Debug info")]
        [SerializeField] private bool isPlayerInFollowRange;
        [SerializeField] private bool isPlayerInAttackRange;
        [SerializeField] private bool isPlayerInRunAwayRange;
        [SerializeField]
        private float LastAttackTime;
        
        private StateMachine<SneakEnemyState, StateEvent> _enemyFsm;
        [SerializeField]
        public Animator _animator;
        [SerializeField]
        public NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyFsm = new ();
            
            // Add states
            _enemyFsm.AddState(SneakEnemyState.Idle, new IdleState(true, this));
            _enemyFsm.AddState(SneakEnemyState.FollowPlayer, new FollowPlayerState(true, this, player.transform));
            _enemyFsm.AddState(SneakEnemyState.AttackPlayer, new AttackPlayerState(true, this, OnAttack));
            _enemyFsm.AddState(SneakEnemyState.RunAway, new RunAwayState(true, this));
            //_enemyFsm.SetStartState(SneakEnemyState.Idle);
            
            // Add transitions
            _enemyFsm.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<SneakEnemyState>(SneakEnemyState.Idle, SneakEnemyState.FollowPlayer));
            _enemyFsm.AddTriggerTransition(StateEvent.LostPlayer, new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.Idle));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.Idle, SneakEnemyState.FollowPlayer, 
                (transition) => isPlayerInFollowRange 
                                && Vector3.Distance(player.transform.position, transform.position) > _agent.stoppingDistance));
            
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.Idle, 
                (transition) => !isPlayerInFollowRange
                                || Vector3.Distance(player.transform.position, transform.position) <= _agent.stoppingDistance)
            );
            
            // Attack transitions
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.Idle, SneakEnemyState.AttackPlayer));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.AttackPlayer));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.AttackPlayer, SneakEnemyState.Idle));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.AttackPlayer, SneakEnemyState.RunAway));
            
            // Run away transitions
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.RunAway));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.RunAway, SneakEnemyState.Idle));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.RunAway, SneakEnemyState.FollowPlayer));
            
            _enemyFsm.Init();
        }
        
        private void Start()
        {
            followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            rangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
            rangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
            runAwayPlayerSensor.OnPlayerEnter += RunAwayPlayerSensor_OnPlayerEnter;
            runAwayPlayerSensor.OnPlayerExit += RunAwayPlayerSensor_OnPlayerExit;
        }

        private void FollowPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition)
        {
            _enemyFsm.Trigger(StateEvent.LostPlayer);
            isPlayerInFollowRange = false;
        }

        private void FollowPlayerSensor_OnPlayerEnter(Transform obj)
        {
            _enemyFsm.Trigger(StateEvent.DetectPlayer);
            isPlayerInFollowRange = true;
        }
        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 obj) => isPlayerInAttackRange = false;

        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform obj) => isPlayerInAttackRange = true;
        private void RunAwayPlayerSensor_OnPlayerEnter(Transform obj) => isPlayerInRunAwayRange = true;
        private void RunAwayPlayerSensor_OnPlayerExit(Vector3 obj) => isPlayerInRunAwayRange = false;

        private void OnAttack(State<SneakEnemyState, StateEvent> state)
        {
            transform.LookAt(player.transform.position);
            LastAttackTime = Time.time;
        }

        private void Update()
        {
            _enemyFsm.OnLogic();
            Debug.Log(_enemyFsm.ActiveStateName);
        }
    }
}