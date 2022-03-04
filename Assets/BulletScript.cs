using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 velocity = new Vector3(0,0,0);
    public int bulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        transform.position = transform.position + (velocity * bulletSpeed);
    }
}
