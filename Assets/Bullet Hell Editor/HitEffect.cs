using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private float destroyTime = 0.25f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
