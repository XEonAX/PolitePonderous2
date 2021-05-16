
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
public class PersonalCamera : MonoBehaviour
{
    Spaceship spaceship;
    private Vector3 originalPositionOffset;
    private Quaternion originalRotationOffset;
    private Vector3 offsetDirection;
    public void Start()
    {
        spaceship = GetComponentInParent<Spaceship>();
        originalPositionOffset = transform.localPosition;
        originalRotationOffset = transform.localRotation;
        transform.parent = spaceship.transform;
        transform.position = spaceship.CameraPosition.position;
        transform.rotation = spaceship.CameraPosition.rotation;
        // offsetDirection = OffDirMul*(spaceship.transform.position - transform.position);
        offsetDirection = transform.localPosition;
    }

    private void Update()
    {
        if (spaceship != null)
        {
            var lagPosition = Vector3.ClampMagnitude(spaceship.transform.InverseTransformDirection(spaceship.rb.velocity) / 6, 2);
            var lagRotation = Vector3.ClampMagnitude(spaceship.transform.InverseTransformVector(spaceship.rb.angularVelocity) / 3, .3f) * Mathf.Rad2Deg;
            //transform.localPosition = -lagPosition;
            transform.localPosition = -lagPosition + ((Quaternion.Euler(-lagRotation) * (offsetDirection)));//- offsetDirection
            //transform.Position =  + Quaternion.Euler(lagRotation) * -lagPosition;
            transform.localRotation = Quaternion.Euler(-lagRotation);
            // var pitch = Quaternion.AngleAxis(Vector3.Dot(-spaceship.transform.right, spaceship.rb.angularVelocity) * 10, spaceship.transform.right);
            // transform.localPosition = pitch * (originalPositionOffset - lagPosition);
            // transform.localRotation = Quaternion.AngleAxis(-Vector3.Dot(-spaceship.transform.forward, spaceship.rb.angularVelocity) * 2, transform.localPosition) * pitch;
            // transform.Rotate(-transform.localPosition, Vector3.Dot(-transform.localPosition, spaceship.rb.angularVelocity) / 10);
            // transform.localRotation = originalRotationOffset + spaceship.rb.angularVelocity;
        }
    }
}