using UnityEngine;

public class CurveInputMgr : IInputMgr
{
    public static IInputMgr Instance { get; set; }
    public bool _disableStabilizer;
    public bool _PrimaryFire;
    public bool _SecondaryFire;

    public AnimationCurve LinearForward;
    public AnimationCurve LinearRight;
    public AnimationCurve LinearUp;
    public AnimationCurve AngularUp;
    public AnimationCurve AngularRight;
    public AnimationCurve AngularTwist;
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        disableStabilizer = _disableStabilizer;
        vAim = new Vector2(AngularRight.Evaluate(Time.time), AngularUp.Evaluate(Time.time));
        vRoll = AngularTwist.Evaluate(Time.time);
        vUpDown = LinearUp.Evaluate(Time.time);
        vLeftRight = LinearRight.Evaluate(Time.time);
        vForwardBack = LinearForward.Evaluate(Time.time);
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