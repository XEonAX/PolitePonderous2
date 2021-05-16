using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffectedBody : MonoBehaviour
{
    public Rigidbody rb;

    public bool InitialOrbit;
    public Rigidbody InitialOrbitRb;

    // Start is called before the first frame update
    void Start()
    {
        GravitationalSystem.Instance.AffectedBodies.Add(this);
        rb = GetComponent<Rigidbody>();
        if (InitialOrbit)
        {
            Vector3 pos = transform.position - InitialOrbitRb.transform.position;
            //pos.z = pos.y;
            pos.y = 0;
            var npos = pos.normalized;
            rb.AddForce(Vector3.Cross(npos, InitialOrbitRb.transform.up) * Mathf.Sqrt(((GravitationalSystem.GravitationalConstant / rb.mass) * InitialOrbitRb.mass * rb.mass) / pos.magnitude), ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
