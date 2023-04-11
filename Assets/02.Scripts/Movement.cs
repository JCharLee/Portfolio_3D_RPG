using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float h, v;

    void Start()
    {
        
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = (Vector3.forward * v) + (Vector3.right * h);

        transform.Translate(movedir.normalized * moveSpeed * Time.deltaTime);
    }
}