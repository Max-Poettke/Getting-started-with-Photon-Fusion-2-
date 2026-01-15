using UnityEngine;

public class UIFollowAnimated : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 0.07f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float distanceBeforeRealign = 10f;
    [SerializeField] private float maxAngle = 10f;

    private Vector3 direction;
    private float targetAngle;

    public void Initialize(Transform _target){
        target = _target;
    }

    private void Update()
    {
        if(target == null){
            return;
        }
        LerpPosition();
        LerpRotation();
    }

    private void LerpPosition()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position,
            followSpeed
        );
    }

    private void LerpRotation()
    {
        Vector3 difference = target.transform.position - transform.position;

        if (difference.magnitude > distanceBeforeRealign)
        {
            direction = (target.transform.position + Vector3.up * distanceBeforeRealign - transform.position).normalized;
            targetAngle = Mathf.Clamp(
                Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg / 3f,
                -maxAngle,
                maxAngle
            );
        }
        else
        {
            targetAngle = -Mathf.DeltaAngle(0f, target.transform.parent.rotation.eulerAngles.z);
        }

        float currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        float newAngle = -Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
