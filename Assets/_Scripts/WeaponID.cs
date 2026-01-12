using UnityEngine;


public class WeaponID : MonoBehaviour
{
    public int id = 0;
    private void OnDestroy()
    {
        AmmoManager.Instance.ResetParent(this);
    }
}
