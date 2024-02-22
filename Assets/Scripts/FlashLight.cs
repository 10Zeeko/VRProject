using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class FlashLight : MonoBehaviour
{
    public XRNode handNode;
    private Vector3 lastHandPosition;
    [SerializeField]
    private float shakeThreshold = 0.04f;
    [SerializeField]
    private int battery = 100;
    private int maxBattery = 100;
    private bool isOn = false;
    [SerializeField]
    private Light light;
    private float batteryDrainTimer = 0f; // Add a timer for battery drain

    private void Start()
    {
        TurnOn();
    }

    public void TurnOff()
    {
        if(isOn)
        {
            isOn = false;
            light.enabled = false;
        }
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            isOn = true;
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
            if (Vector3.Distance(lastHandPosition, currentHandPosition) > shakeThreshold)
            {
                battery = Mathf.Min(maxBattery, battery + 1);
            }
            lastHandPosition = currentHandPosition;
        }

        if(isOn)
        {
            batteryDrainTimer += Time.deltaTime;
            if (batteryDrainTimer >= 1f)
            {
                battery--;
                batteryDrainTimer = 0f;
            }
            light.intensity = (float)battery / maxBattery;

            if(battery <= 0 || (battery < 20 && Random.value < 0.01f))
            {
                TurnOff();
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
}
