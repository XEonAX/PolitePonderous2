using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    public GameObject TraceThis;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lr.positionCount += 1;
        lr.SetPosition(lr.positionCount - 1, TraceThis.transform.position - transform.position);
    }
}
