using UnityEngine;

public enum HitboxType
{
    Body,
    Head,
    Limb
}

public class Hitbox : MonoBehaviour
{
    public HitboxType hitboxType = HitboxType.Body;
}
