using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : BaseController
{
    private Transform nearestPickup;

    public Transform NearestPickup { get { return nearestPickup; } }

    private float nearestPickupSqrDistance;

    public float NearestPickupSqrDistance { get { return nearestPickupSqrDistance; } }

    protected override void Update()
    {
        base.Update();

        FindNearestPickup();
    }

    // Finds the nearest pickup to the character and the square distance between
    private void FindNearestPickup()
    {
        Transform closestPickup = null;
        float closestSqrDistance = Mathf.Infinity;

        foreach (Transform pickup in GameManager.Instance.ActivePickups)
        {
            float sqrDistance = Vector2.SqrMagnitude(pickup.position.ConvertTo<Vector2>() - rb.position);

            if(sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closestPickup = pickup;
            }
        }

        nearestPickup = closestPickup;
        nearestPickupSqrDistance = closestSqrDistance;
    } 
}
