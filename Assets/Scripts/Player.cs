using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 5f)]
    private float HistoricalPositionDuration = 1f;
    [SerializeField]
    [Range(0.001f, 1f)]
    private float HistoricalPositionInterval = 0.1f;

    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach (Vector3 velocity in HistoricalVelocities)
            {
                average += velocity;
            }
            average.y = 0;

            return average / HistoricalVelocities.Count;
        }
    }

    private Queue<Vector3> HistoricalVelocities;
    private float LastPositionTime;
    private int MaxQueueSize;

    private void Awake()
    {
        MaxQueueSize = Mathf.CeilToInt(1f / HistoricalPositionInterval * HistoricalPositionDuration);
        HistoricalVelocities = new Queue<Vector3>(MaxQueueSize);
    }

    private void Update()
    {
        if (LastPositionTime + HistoricalPositionInterval <= Time.time)
        {
            if (HistoricalVelocities.Count == MaxQueueSize)
            {
                HistoricalVelocities.Dequeue();
            }

            HistoricalVelocities.Enqueue(GetComponent<Rigidbody>().velocity);
            LastPositionTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("You_lost");
        }
    }
}
