using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private int battery = 100;
    private int maxBattery = 100;
    private bool isOn = false;

    public void TurnOff()
    {
        if(isOn)
        {
            isOn = false;
        }
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            isOn = true;
        }
    }
    
    public int GetBattery()
    {
        return battery;
    }

    private void Update()
    {
        if(isOn)
        {
            battery--;
            if(battery <= 0)
            {
                TurnOff();
            }
        }
    }
}
