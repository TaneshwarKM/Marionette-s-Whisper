using UnityEngine;

public class CameraLookAtPlayer : MonoBehaviour
{
    public Transform player; // Drag and drop the Player GameObject into this field in the Inspector

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction from the camera to the player
            Vector3 directionToPlayer = player.position - transform.position;

            // Calculate the rotation needed to face the player
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Apply the rotation to the camera
            transform.rotation = lookRotation;
        }
    }
}
