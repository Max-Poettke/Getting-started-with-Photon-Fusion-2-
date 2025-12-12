using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;
    
    private void LateUpdate() {
        if(Target == null) return;

        transform.position = Target.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        horizontalRotation += mouseX * MouseSensitivity;     

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);

    }
}
