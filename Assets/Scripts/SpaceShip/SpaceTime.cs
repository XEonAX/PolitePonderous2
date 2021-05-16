using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTime : MonoBehaviour
{
    Material SpaceTimeMaterial;
    public bool HeightOnly;
    public float HeightDivider = 4;
    public float PositionDivider = 10;
    // Start is called before the first frame update
    void Start()
    {
        SpaceTimeMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (HeightOnly)
            SpaceTimeMaterial.mainTextureOffset = new Vector2(0, transform.position.y) / HeightDivider;
        else
            SpaceTimeMaterial.mainTextureOffset = new Vector2(transform.position.x, transform.position.z) / PositionDivider;
    }
}
