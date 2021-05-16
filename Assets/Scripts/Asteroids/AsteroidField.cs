using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AsteroidField : MonoBehaviour
{
    public List<Asteroid> OriginalAsteroids;
    public List<Asteroid> Asteroids = new List<Asteroid>();
    public int Count;
    public float MinRadius;
    public float MaxRadius;
    public float Velocity;
    public Rigidbody rb;
    public ParticleSystem ps;
    public static AsteroidField Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        rb = GetComponentInParent<Rigidbody>();
        if (rb == null) throw new UnityEngine.UnityException("Need to be a RB's child");
        ps = GetComponent<ParticleSystem>();
        var emissionshape = ps.shape;
        emissionshape.radius = MaxRadius;
        emissionshape.radiusThickness = (MaxRadius - MinRadius) / MaxRadius;
        OriginalAsteroids.AddRange(GetComponentsInChildren<Asteroid>());
        Velocity = GravitationalSystem.GravitationalConstant / 100f;
        MinRadius = MinRadius + transform.localEulerAngles.x;
        MaxRadius = MaxRadius + transform.localEulerAngles.x;
        var incrementorAngle = Mathf.PI * 2f / Count;
        for (int i = 0; i < Count; i++)
        {
            float angle = i * incrementorAngle;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * Random.Range(MinRadius, MaxRadius), 0, Mathf.Sin(angle) * Random.Range(MinRadius, MaxRadius));
            // Vector3 pos = Random.insideUnitCircle.normalized;
            // pos.z = pos.y;
            // pos.y = 0;
            // pos.Normalize();
            // pos = pos * Random.Range(MinRadius, MaxRadius);
            pos = transform.rotation * pos;
            var worldpos = pos + transform.position;
            var asteroid = Instantiate(OriginalAsteroids[Random.Range(0, OriginalAsteroids.Count)], worldpos, Random.rotationUniform);
            asteroid.rb = asteroid.GetComponent<Rigidbody>();
            asteroid.rb.AddForce(Vector3.Cross(pos.normalized, transform.up) * Mathf.Sqrt(((GravitationalSystem.GravitationalConstant / asteroid.rb.mass) * rb.mass * asteroid.rb.mass) / pos.magnitude), ForceMode.VelocityChange);
            Asteroids.Add(asteroid);
        }
    }

}
