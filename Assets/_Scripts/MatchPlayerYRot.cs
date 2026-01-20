using UnityEngine;

public class MatchPlayerYRot : MonoBehaviour
{
    [Header("References")]
    public Transform hmdTransform;
    public Transform moveTransform;

    [Header("Settings")]
    public float verticalOffset = -0.5f;
    public float rotationSpeed = 10f;
    public float moveThreshold = 0.01f;

    private Vector3 previousPos;

    void Start()
    {
        if (moveTransform != null) previousPos = moveTransform.position;
    }

    void LateUpdate()
    {
        if (hmdTransform == null || moveTransform == null) return;

        Vector3 targetPosition = hmdTransform.position;
        targetPosition.y += verticalOffset;
        transform.position = targetPosition;

        Vector3 currentPosFlat = new Vector3(moveTransform.position.x, 0, moveTransform.position.z);
        Vector3 prevPosFlat = new Vector3(previousPos.x, 0, previousPos.z);
        float distanceMoved = Vector3.Distance(currentPosFlat, prevPosFlat);

        if (distanceMoved > moveThreshold)
        {
            Vector3 lookDir = hmdTransform.forward;
            lookDir.y = 0;

            if (lookDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        previousPos = moveTransform.position;
    }
}