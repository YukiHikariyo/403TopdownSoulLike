using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public GameObject[] vfxPrefabs;

    public void PlayVFX(int index, Transform parent, Vector3 position, float duration)
    {
        GameObject vfx = Instantiate(vfxPrefabs[index], parent);
        vfx.GetComponent<VFX>().Initialize(duration, position);
    }
}
