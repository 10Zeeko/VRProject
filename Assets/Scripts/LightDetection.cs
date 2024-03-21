using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public Light flashlight;
    public LayerMask obstacleMask;

    public bool IsDetectedByLight(Transform enemy)
    {
        Vector3 directionToEnemy = (enemy.position - flashlight.transform.position).normalized;
        float distanceToEnemy = Vector3.Distance(flashlight.transform.position, enemy.position);

        // Check if the enemy is within the light's range and angle
        if (distanceToEnemy <= flashlight.range && Vector3.Angle(flashlight.transform.forward, directionToEnemy) <= flashlight.spotAngle / 2)
        {
            // Cast a ray towards the enemy
            if (!Physics.Raycast(flashlight.transform.position, directionToEnemy, distanceToEnemy, obstacleMask))
            {
                // If the raycast doesn't hit any obstacles, the enemy is detected by the light
                return true;
            }
        }

        // If the enemy is out of range or blocked by an obstacle, it's not detected by the light
        return false;
    }
}
