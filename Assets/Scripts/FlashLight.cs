using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;
using VRProject.Enemy;
using Random = UnityEngine.Random;

public class FlashLight : MonoBehaviour
{
    public XRNode handNode;
    private Vector3 _lastHandPosition;
    [SerializeField]
    private float shakeThreshold = 0.04f;
    [SerializeField]
    private int battery = 100;
    private int _maxBattery = 100;
    private int _recharge = 3;
    private bool _isOn = false;
    [SerializeField]
    private Light light;
    private float _batteryDrainTimer = 0f;
    
    [SerializeField]
    private Slider batterySlider;
    
    [SerializeField]
    private LightDetection lightDetection; 
    [SerializeField]
    private SneakEnemy sneakEnemy;
    
    public delegate void FlashLightEvent(bool isOn);
    public delegate void EnemySpottedEvent(bool isSpotted);
    public event FlashLightEvent OnFlashLightEvent;
    public event EnemySpottedEvent OnEnemySpottedEvent;

    private void Start()
    {
        TurnOn();
    }

    public void TurnOff()
    {
        if(_isOn)
        {
            _isOn = false;
            OnFlashLightEvent?.Invoke(_isOn);
            light.enabled = false;
        }
    }

    public void TurnOn()
    {
        if(!_isOn)
        {
            _isOn = true;
            OnFlashLightEvent?.Invoke(_isOn);
            light.enabled = true;
        }
    }
    
    public int GetBattery()
    {
        return battery;
    }

    void Update()
    {
        Vector3 currentHandPosition;
        if (TryGetHandPosition(handNode, out currentHandPosition))
        {
            if (Vector3.Distance(_lastHandPosition, currentHandPosition) > shakeThreshold)
            {
                battery += _recharge;
                if (battery > _maxBattery)
                {
                    battery = _maxBattery;
                }
            }
            _lastHandPosition = currentHandPosition;
        }

        if(_isOn)
        {
            _batteryDrainTimer += Time.deltaTime;
            if (_batteryDrainTimer >= 1f)
            {
                battery--;
                _batteryDrainTimer = 0f;
                UpdateSlider(battery);
                if(battery <= 0 || (battery < 20 && Random.value < 0.075f))
                {
                    TurnOff();
                }
            }
            light.intensity = (float)battery / _maxBattery;

            if (lightDetection.IsDetectedByLight(sneakEnemy.transform))
            {
                OnEnemySpottedEvent?.Invoke(true);
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (_isOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    bool TryGetHandPosition(XRNode node, out Vector3 position)
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (XRNodeState nodeState in nodeStates)
        {
            if (nodeState.nodeType == node)
            {
                if (nodeState.TryGetPosition(out position))
                {
                    return true;
                }
            }
        }

        position = Vector3.zero;
        return false;
    }

    void UpdateSlider(float aBattery)
    {
        batterySlider.value = aBattery / _maxBattery;
    }
}
