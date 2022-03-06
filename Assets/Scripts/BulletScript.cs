using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 velocity = new Vector3(0,0,0);
    public float bulletSpeed = 0.1f;

    private void FixedUpdate()
    {
        transform.position = transform.position + (velocity * bulletSpeed);
    }
}
