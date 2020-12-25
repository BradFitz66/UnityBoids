using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Boids> boidsInRange;

    //Don't want any rigidbody overhead
    public Vector3 velocity;

    public Transform goal;


    void Start()
    {
        velocity = Random.insideUnitSphere * 100;
    }

    Vector3 Alignment()
    {
        Vector3 vec=Vector3.zero;
        int total = 0;
        float maxAlignmentRange = 1;
        //Get average heading
        foreach (Boids boid in boidsInRange)
        {
            if (boid == this)
                continue;
            float dist = Vector3.Distance(transform.position, boid.transform.position);
            if (dist < maxAlignmentRange)
            {
                vec += (boid.velocity);
                total++;
            }
        }
        if (total > 0)
        {
            vec /= total;
            vec = Vector3.ClampMagnitude(vec, 3);
            vec -= this.velocity;

        }
        return vec;
    }
    Vector3 Seperation()
    {
        Vector3 vec=Vector3.zero;
        int maxSeperationRange=50;
        int total=0;
        foreach(Boids boid in boidsInRange)
        {
            if (boid == this)
                continue;
            float d = Vector3.Distance(transform.position, boid.transform.position);
            if (d < maxSeperationRange)
            {
                Vector3 diff = (transform.position - boid.transform.position).normalized * 100;
                diff /= Mathf.Pow(d, 2);
                vec += diff;
                total++;
            }

          
        }
        if (total > 0)
        {
            vec /= total;
            vec = Vector3.ClampMagnitude(vec, 20);
            vec -= this.velocity;
        }
        return vec;
    }
    Vector3 Cohesion()
    {
        Vector3 vec=Vector3.zero;
        float maxCohesionRange = 1;
        int total = 0;
        foreach(Boids boid in boidsInRange)
        {
            if (boid == this)
                continue;
            float dist = Vector3.Distance(transform.position, boid.transform.position);
            if (dist < maxCohesionRange)
            {
                vec += (boid.transform.position);
                total++;
            }
        }
        if (total > 0)
        {
            vec /= total;
            vec = Vector3.ClampMagnitude(vec, 3);
            vec -= transform.position;
        }
        return vec;
    }

    Vector3 Goal()
    {
        Vector3 vec;
        vec = -(transform.position - goal.position).normalized * 50; 
        vec = Vector3.ClampMagnitude(vec, 20);
        return vec;
    }

    void Simulate()
    {

        velocity += Alignment();
        velocity += Cohesion();
        velocity += Seperation();
        velocity += Goal();

        velocity = Vector3.ClampMagnitude(velocity, 50);
    }

    


    void Update()
    {
        transform.Translate(velocity*Time.deltaTime);
        Simulate();
    }
}
