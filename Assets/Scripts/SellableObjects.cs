using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SellableObjects : MonoBehaviour
{
    public int sellValue = 0;
    public bool bShouldMove = false;
    
    [SerializeField]
    private float speed = 2.0f;
    void Start()
    {
        sellValue = Random.Range(20, 100);
    }

    private void FixedUpdate()
    {
        if (bShouldMove)
        {
            transform.Translate(Time.deltaTime * -speed, 0, 0, Space.World);
        }
    }
}
