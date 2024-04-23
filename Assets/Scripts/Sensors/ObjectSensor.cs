using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSensor : MonoBehaviour
{
    public delegate void ObjectEnterEvent(GameObject sellableObject);
    public event ObjectEnterEvent OnObjectEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SellableObjects sellableObject))
        {
            OnObjectEnter?.Invoke(other.gameObject);
        }
    }
}
