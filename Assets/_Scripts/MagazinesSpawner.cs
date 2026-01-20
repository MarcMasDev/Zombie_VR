using System.Collections.Generic;
using UnityEngine;

public class MagazinesSpawner : MonoBehaviour
{
    public static MagazinesSpawner Instance;
    void Awake()
    {
        Instance = this;
    }
    public void Spawn()
    {
    }
    public void Despawn()
    {

    }
}
