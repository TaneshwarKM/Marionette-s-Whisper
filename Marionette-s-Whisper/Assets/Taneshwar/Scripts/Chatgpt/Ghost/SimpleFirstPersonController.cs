using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    public float rotationSpeed = 2f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, mouseX);
        Camera.main.transform.Rotate(Vector3.left, mouseY);
    }
}
