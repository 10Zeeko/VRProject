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
        [SerializeField] private FlashLight flashlightPlayerSensor;
        
        [Header("Debug info")]
        [SerializeField] private bool isPlayerInFollowRange;
        [SerializeField] private bool isPlayerInAttackRange;
        [SerializeField] private bool isPlayerInRunAwayRange;
        
        [SerializeField] private bool shouldAttack = false;
        [SerializeField] private bool shouldRunAway = false;
        
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
            _enemyFsm.AddState(SneakEnemyState.Idle, new IdleState(false, this));
            _enemyFsm.AddState(SneakEnemyState.FollowPlayer, new FollowPlayerState(true, this, player.transform, player.GetComponent<Player>()));
            _enemyFsm.AddState(SneakEnemyState.AttackPlayer, new AttackPlayerState(true, this, OnAttack, player.transform));
            _enemyFsm.AddState(SneakEnemyState.RunAway, new RunAwayState(true, this, player.transform));
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
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.Idle, SneakEnemyState.AttackPlayer, 
                (transition) => shouldAttack)
            );
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.AttackPlayer, 
                (transition) => shouldAttack)
            );
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.AttackPlayer, SneakEnemyState.RunAway, 
                (transition) => shouldRunAway)
            );
            
            // Run away transitions
            //_enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.FollowPlayer, SneakEnemyState.RunAway));
            _enemyFsm.AddTransition(new Transition<SneakEnemyState>(SneakEnemyState.RunAway, SneakEnemyState.Idle, 
                (transition) => !isPlayerInRunAwayRange)
            );
            
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
            flashlightPlayerSensor.OnFlashLightEvent += FlashlightPlayerSensor_FlashLightEvent;
            flashlightPlayerSensor.OnEnemySpottedEvent += FlashlightPlayerSensor_OnEnemySpottedEvent;
        }
        private void FlashlightPlayerSensor_OnEnemySpottedEvent(bool isSpotted)
        {
            shouldRunAway = isSpotted;
            shouldAttack = false;
        }
        private void FlashlightPlayerSensor_FlashLightEvent(bool isOn)
        {
            shouldAttack = !isOn;
            if (!isOn) shouldRunAway = false;
        }
        private void FollowPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition)
        {
            _enemyFsm.Trigger(StateEvent.LostPlayer);
            isPlayerInFollowRange = true;
        }
        private void FollowPlayerSensor_OnPlayerEnter(Transform obj)
        {
            _enemyFsm.Trigger(StateEvent.DetectPlayer);
            isPlayerInFollowRange = false;
        }
        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 obj)
        {
            isPlayerInAttackRange = false;
            shouldAttack = false;
        }
        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform obj)
        {
            isPlayerInAttackRange = true;
            shouldAttack = true;
        }
        private void RunAwayPlayerSensor_OnPlayerEnter(Transform obj)
        {
            isPlayerInRunAwayRange = true;
            _animator.SetBool("flashlightOff", false);
        }
        private void RunAwayPlayerSensor_OnPlayerExit(Vector3 obj)
        {
            isPlayerInRunAwayRange = false;
            shouldRunAway = false;
            _animator.SetBool("EnemyDetected", false);
            _animator.SetTrigger("hidded");
        }
        private void OnAttack(State<SneakEnemyState, StateEvent> state)
        {
            if (Time.time - LastAttackTime >= attackCooldown)
            {
                transform.LookAt(player.transform.position);
                LastAttackTime = Time.time;
            }
        }

        private void Update()
        {
            _enemyFsm.OnLogic();
            Debug.Log(_enemyFsm.ActiveStateName);
        }
    }
}