using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject target;
    public float radius = 1f;

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = (target.transform.position - transform.position).normalized;

        Quaternion newRotation = Quaternion.AngleAxis(radius, Vector3.forward) * Quaternion.LookRotation(direction);

        transform.position = direction * 100;

        if (Vector3.Distance(transform.parent.position, transform.position) > radius)
        {
            transform.localPosition = transform.localPosition.normalized * radius;
        }

        transform.rotation = newRotation;
        transform.Rotate(90, 0, 0);
    }
}
