using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class GravitationalSystem : MonoBehaviour
{
    public static GravitationalSystem Instance;
    public List<GravitationalAttractor> Attractors;
    public int AttractorsCount;
    public static float GravitationalConstant=0.01f;
    public List<GravityAffectedBody> AffectedBodies;






    private NativeArray<Vector4> AttractorPositions;
    private NativeArray<Vector4> AffectedBodyPositions;
    private NativeArray<Vector3> GravityForces;
    bool JobVarsCreated = false;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!JobVarsCreated)
        {
            AttractorPositions = new NativeArray<Vector4>(Attractors.Count, Allocator.Persistent);
            AffectedBodyPositions = new NativeArray<Vector4>(AffectedBodies.Count, Allocator.Persistent);

            GravityForces = new NativeArray<Vector3>(AffectedBodies.Count, Allocator.Persistent);
            AttractorsCount=Attractors.Count;
            JobVarsCreated = true;
        }

        for (var i = 0; i < AttractorPositions.Length; i++)
        {
            var pos = Attractors[i].transform.position;
            AttractorPositions[i] = new Vector4(pos.x, pos.y, pos.z, Attractors[i].rb.mass);
        }
        for (var i = 0; i < AffectedBodyPositions.Length; i++)
        {
            var pos = AffectedBodies[i].transform.position;
            AffectedBodyPositions[i] = new Vector4(pos.x, pos.y, pos.z, AffectedBodies[i].rb.mass);
        }


        var job = new GravitationalJob()
        {
            Attractors = AttractorsCount,
            AttractorPositions = AttractorPositions,
            AffectedBodyPositions = AffectedBodyPositions,
            deltaTime = Time.fixedDeltaTime,
            GravityForces = GravityForces,
            GravitationalConstant = GravitationalConstant
        };

        // Schedule a parallel-for job. First parameter is how many for-each iterations to perform.
        // The second parameter is the batch size,
        // essentially the no-overhead innerloop that just invokes Execute(i) in a loop.
        // When there is a lot of work in each iteration then a value of 1 can be sensible.
        // When there is very little work values of 32 or 64 can make sense.
        JobHandle jobHandle = job.Schedule(AffectedBodyPositions.Length, 32);
        jobHandle.Complete();
        // Debug.Log(">>>>>>");
        for (var i = 0; i < AffectedBodyPositions.Length; i++)
        {
            // Debug.Log(job.GravityForces[i]);
            AffectedBodies[i].rb.AddForce(job.GravityForces[i]);
        }
        // Debug.Log("<<<<<<");
    }




    private void OnDestroy()
    {
        // Native arrays must be disposed manually.
        AttractorPositions.Dispose();
        AffectedBodyPositions.Dispose();
        GravityForces.Dispose();
    }









    [BurstCompile(CompileSynchronously = true)]


    struct GravitationalJob : IJobParallelFor
    {
        // Jobs declare all data that will be accessed in the job
        // By declaring it as read only, multiple jobs are allowed to access the data in parallel
        [ReadOnly]
        public NativeArray<Vector4> AttractorPositions;
        [ReadOnly]
        public NativeArray<Vector4> AffectedBodyPositions;

        // By default containers are assumed to be read & write
        public NativeArray<Vector3> GravityForces;

        // Delta time must be copied to the job since jobs generally don't have concept of a frame.
        // The main thread waits for the job same frame or next frame, but the job should do work deterministically
        // independent on when the job happens to run on the worker threads.
        public float deltaTime;
        [ReadOnly]
        public float GravitationalConstant;
        [ReadOnly]
        public int Attractors;



        // The code actually running on the job
        public void Execute(int i)
        {
            var affectedBody = AffectedBodyPositions[i];
            GravityForces[i] = Vector3.zero;
            for (int j = 0; j < Attractors; j++)
            {
                Vector3 attractorPosition = AttractorPositions[j];
                Vector3 attractedPosition = AffectedBodyPositions[i];
                if (attractorPosition == attractedPosition)
                {
                    GravityForces[i] = Vector3.zero;
                    continue;
                }
                float attractorMass = AttractorPositions[j].w;
                float affectedMass = AffectedBodyPositions[i].w;

                Vector3 dir = (attractorPosition - attractedPosition);
                float sqrDist = dir.sqrMagnitude;
                float invSqrDist = 1.0f / sqrDist;
                dir = dir.normalized;
                Vector3 xtraGravity = (GravitationalConstant * (attractorMass * affectedMass) * dir) * invSqrDist;
                //bodyVelocity.Linear += xtraGravity * deltaTime;
                GravityForces[i] += xtraGravity;
            }
        }
    }

}
