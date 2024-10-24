using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SFXPool : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    public int maxSize = 10000;
    public GameObject sfxAudioSourcePrefab;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 10, maxSize);
    }

    public GameObject CreateFunc()
    {
        GameObject obj = Instantiate(sfxAudioSourcePrefab, transform);
        obj.GetComponent<SFXAudioSource>().pool = pool;
        return obj;
    }

    public void ActionOnGet(GameObject obj) => obj.SetActive(true);

    public void ActionOnRelease(GameObject obj) => obj.SetActive(false);

    public void ActionOnDestroy(GameObject obj) => Destroy(obj);
}
