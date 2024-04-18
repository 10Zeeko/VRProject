using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectCounter : MonoBehaviour
{
    [SerializeField]
    private int pickedObjects = 0;
    [SerializeField]
    private int totalMoney = 0;
    [SerializeField]
    private float mechanicalBeltSpeed = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.name);
        if (other.CompareTag("PickeableObject"))
        {
            Debug.Log("Picked up: " + other.name);
        }
    }

    public void Pick(GameObject obj)
    {
        totalMoney += obj.GetComponent<SellableObjects>().sellValue;
        Destroy(obj);
        pickedObjects += 1;
    }
}