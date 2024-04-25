using UnityEngine;

public class ObjectCounter : MonoBehaviour
{
    [SerializeField]
    private int pickedObjects = 0;
    [SerializeField]
    private int totalMoney = 0;
    [SerializeField]
    private float mechanicalBeltSpeed = 1.0f;
    
    [SerializeField]
    private ObjectSpawner objectSpawner;
    
    [Header("Sensors")]
    [SerializeField] private ObjectSensor objectSensor;
    
    void Start()
    {
        objectSensor.OnObjectEnter += Pick;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.name);
        if (other.CompareTag("PickeableObject"))
        {
            SellableObjects instance = other.GetComponent<SellableObjects>();
            if (instance)
            {
                instance.bShouldMove = true;
            }
        }
    }
    public void Pick(GameObject obj)
    {
        totalMoney += obj.GetComponent<SellableObjects>().sellValue;
        Destroy(obj);
        pickedObjects += 1;
        objectSpawner.SpawnObject();
    }
}