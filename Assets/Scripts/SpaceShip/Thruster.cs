using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float maxForce;

    public float power;
    public float smoothPower;

    public float ThrottleSpeed;

    private Rigidbody spaceship;
    private float scale;

    public Transform exhaust;
    private TrailRenderer trailRenderer;
    private ParticleSystem particleSystem;
    public Vector3 ForceVector;
    public Vector3 TorqueVector;

    private void Awake()
    {
        spaceship = GetComponentInParent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.magnitude;
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        // if (trailRenderer != null)
        //     trailRenderer.startWidth = 0.2f * transform.localScale.sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        exhaust.localScale = new Vector3(exhaust.localScale.x, exhaust.localScale.y, smoothPower * (maxForce / .2f));
    }

    void FixedUpdate()
    {
        power = Mathf.Clamp(power, 0, 1);
        float smoothingVelocity = 0;
        smoothPower = Mathf.SmoothDamp(smoothPower, power, ref smoothingVelocity, Time.fixedDeltaTime, ThrottleSpeed);
        spaceship.AddForceAtPosition(transform.forward * Mathf.Clamp(maxForce * power, 0, maxForce), transform.position, ForceMode.Force);

        if (trailRenderer != null)
        {
            if (smoothPower > 0.3f)
            {
                trailRenderer.emitting = true;
            }
            else
                trailRenderer.emitting = false;
        }

        if (particleSystem != null)
        {
            var emission = particleSystem.emission;
            var main = particleSystem.main;
            if (smoothPower > 0.3f)
            {
                main.startSpeed = smoothPower * 5;
                emission.rateOverTimeMultiplier = smoothPower * 100;
            }
            else
            {
                emission.rateOverTimeMultiplier = 0;
                main.startSpeed = 0;
            }
        }
        //InitializeThrustVector();
    }

    internal void InitializeThrustVector()
    {
        float FForward = Vector3.Dot(transform.forward, spaceship.transform.forward);

        float FRight = Vector3.Dot(transform.forward, spaceship.transform.right);
        float FUp = Vector3.Dot(transform.forward, spaceship.transform.up);

        var torqueCross = Vector3.Cross(transform.position - spaceship.worldCenterOfMass, transform.forward);
        float TForward = Vector3.Dot(torqueCross, spaceship.transform.forward);
        float TRight = Vector3.Dot(torqueCross, spaceship.transform.right);
        float TUp = Vector3.Dot(torqueCross, spaceship.transform.up);

        ForceVector = new Vector3(FRight, FUp, FForward) * (maxForce);
        TorqueVector = new Vector3(TRight, TUp, TForward) * (maxForce);
    }
}
