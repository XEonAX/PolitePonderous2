

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyInputMgr : IInputMgr
{
    public static JoyInputMgr Instance;

    public Joystick LinearJoy;
    public Joystick AngularJoy;
    public Joystick VJoy;
    public Joystick RollJoy;
    public Button BtnShoot1;
    public Button BtnDisableFlightAssist;
    public Button BtnShoot2;

    private void Start()
    {
        Instance = this;
        disableStabilizer = false;
    }

    private void Update()
    {
        //vAim = new Vector2(YawH1Joy.Horizontal + YawH2Joy.Horizontal, VerticalV1Joy.Vertical + VerticalV2Joy.Vertical);
        vAim = new Vector2(AngularJoy.Horizontal, AngularJoy.Vertical);
        vForwardBack = LinearJoy.Vertical;
        vLeftRight = LinearJoy.Horizontal;
        vRoll = -RollJoy.Horizontal;
        vUpDown = VJoy.Vertical;
    }
}