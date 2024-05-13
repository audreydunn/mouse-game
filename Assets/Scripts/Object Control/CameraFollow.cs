using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the character's transform
    public float zoomThreshold = 2f; // Distance threshold for zooming in
    public string wallTag = "Wall"; // Tag used to identify walls
    public float minDistance = 2f; // Minimum distance for maximum zoom
    public float maxDistance = 4f; // Maximum distance for no zoom
    public float cameraHeight = 3f; // Height of the camera from the player's base
    public float smoothSpeed = 1f; // How smoothly the camera moves

    private float previousTargetZPosition = 0;
    
    void LateUpdate()
    {
        // Find the nearest wall within the detection radius
        float distanceToWall = FindNearestWallWithTag(target.position, wallTag);
        
        // Determine the desired camera distance based on the distance to the nearest wall
        float targetDistance = maxDistance;
        if (distanceToWall < zoomThreshold)
        {
            targetDistance = Mathf.Lerp(minDistance, maxDistance, Time.deltaTime);
        }

        // Compensate the camera zoom with the change in player position to reduce camera shaking
        targetDistance -= Math.Abs(target.position.z - previousTargetZPosition);
        previousTargetZPosition = target.position.z;

        // TODO: possibly make the camera zooming still work when the player is moving instead of it just being neutral

        //Debug.Log("Distance to Wall: " + distanceToWall);
        //Debug.Log("Target Distance: " + targetDistance);

        // Calculate the new camera position as the player position plus the offset normalized and scaled by the target distance
        Vector3 zoom = target.forward * targetDistance;
        Vector3 desiredPosition = target.position - zoom + Vector3.up * cameraHeight;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // Ensure the camera is always looking at the player
        transform.LookAt(target.position);
    }

    float FindNearestWallWithTag(Vector3 playerPosition, string tag)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag(tag);
        float minDistance = Mathf.Infinity;

        foreach (GameObject wall in walls)
        {
            Collider collider = wall.GetComponent<Collider>();
            if (collider != null)
            {
                Vector3 closestPointOnCollider = collider.ClosestPoint(playerPosition);
                float distance = Vector3.Distance(playerPosition, closestPointOnCollider);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }

        return minDistance;
    }
}
