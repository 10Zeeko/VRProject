using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    // Array of objects
    public GameObject[] pickedObjects;

    public void Pick(GameObject obj)
    {
        obj.SetActive(false);
        // Add object to array of picked objects
        pickedObjects = pickedObjects.Append(obj).ToArray();
    }
}