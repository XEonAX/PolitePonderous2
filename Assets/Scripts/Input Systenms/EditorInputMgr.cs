using UnityEngine;

public class EditorInputMgr : IInputMgr
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
        vAim = _vAim;
        vRoll = _vRoll;
        vUpDown = _vUpDown;
        vLeftRight = _vLeftRight;
        vForwardBack = _vForwardBack;
        PrimaryFire = _PrimaryFire;
        SecondaryFire = _SecondaryFire;
    }
}