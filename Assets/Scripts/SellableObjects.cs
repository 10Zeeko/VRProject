using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellableObjects : MonoBehaviour
{
    public int sellValue = 0;
    void Start()
    {
        sellValue = Random.Range(1, 100);
    }
}
