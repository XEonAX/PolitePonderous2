using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GravitationalAttractor : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        GravitationalSystem.Instance.Attractors.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
