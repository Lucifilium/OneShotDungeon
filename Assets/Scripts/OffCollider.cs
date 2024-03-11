using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCollider : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider;
    Damageable damageable;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        damageable = GetComponentInParent<Damageable>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!damageable.IsAlive)
        {
            capsuleCollider.enabled = false;
        }
    }
}
