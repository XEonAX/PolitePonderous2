using UnityEngine;

public class RandomInputMgr : IInputMgr
{
    public static IInputMgr Instance { get; set; }
    public bool _disableStabilizer;
    public Vector2 _vAim;
    public float _vRoll;
    public float _vUpDown;
    public float _vLeftRight;
    public float _vForwardBack;
    public bool _PrimaryFire;
    public bool _SecondaryFire;

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        disableStabilizer = _disableStabilizer;

        //Get the vector
        Vector3 linearvector = RandomSmoothPointOnUnitSphere(Time.time / 10, false);
        Vector3 angularvector = RandomSmoothPointOnUnitSphere(Time.time / 50, true);
        vAim = angularvector;
        vRoll = angularvector.z;
        vUpDown = linearvector.y;
        vLeftRight = linearvector.x;
        vForwardBack = linearvector.z;
        PrimaryFire = _PrimaryFire;
        SecondaryFire = _SecondaryFire;
    }

    //There are probably better ways to do this.
    Vector3 RandomSmoothPointOnUnitSphere(float time, bool angular)
    {

        //Get the x of the vector
        float x = Mathf.Lerp(-1, 1, Mathf.PerlinNoise(time, angular ? 100 : 200));

        //Get the y of the vector
        float y = Mathf.Lerp(-1, 1, Mathf.PerlinNoise(time, angular ? 300 : 400));

        //Get the z of the vector
        float z = Mathf.Lerp(-1, 1, Mathf.PerlinNoise(time, angular ? 500 : 600));
        //Create a vector3
        Vector3 vector = new Vector3(x, y, z);

        //Normalize the vector and return it
        return Vector3.Normalize(vector);

    }
}