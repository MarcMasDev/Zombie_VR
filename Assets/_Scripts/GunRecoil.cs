using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float kickBack = 0.05f;
    public float rotationKick = 6f;
    public float returnSpeed = 25f;

    Vector3 currentPos;
    Quaternion currentRot;

    void Update()
    {
        currentPos = Vector3.Lerp(currentPos,Vector3.zero,Time.deltaTime * returnSpeed);

        currentRot = Quaternion.Slerp(currentRot,Quaternion.identity,Time.deltaTime * returnSpeed);

        transform.localPosition = currentPos;
        transform.localRotation = currentRot;
    }

    public void Fire()
    {
        currentPos += Vector3.back * kickBack;
        currentRot *= Quaternion.Euler(-rotationKick, 0f, 0f);
    }
}
