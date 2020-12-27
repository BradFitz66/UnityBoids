using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> boidsInRange=new List<GameObject>(216);

    //Use a simple velocity based movement that just translates the transform to avoid any overhead from rigidbodies
    public Vector3 velocity;

    public Transform goal;
    float speed = 50;
    public Flock flock;

    void Start()
    {
        velocity = UnityEngine.Random.insideUnitSphere * 100;
    }

    Vector3 Alignment()
    {
        Vector3 vec = Vector3.zero;
        float maxAlignmentDistance = 2;
        int total = 0;
        //Get average heading
        //Like in flock.cs, I've decided to use a for loop instead of a foreach for performance reasons
        for (int i=0; i<boidsInRange.Count; i++)
        {
            Boids boid = boidsInRange[i].GetComponent<Boids>();
            float d = Vector3.Distance(boid.transform.position, transform.position);
            if (boid == this || d > maxAlignmentDistance)
                continue;
           
                vec += (boid.transform.position+boid.velocity).normalized * speed;
                total++;
            
        }
        if (total > 0)
        {
            vec /= total;
            vec -= this.velocity;

        }
        return vec;
    }
    Vector3 Seperation()
    {
        Vector3 vec = Vector3.zero;
        int total = 0;
        float maxSeparationDistance = 3;
        for (int i = 0; i < boidsInRange.Count; i++)
        {
            Boids boid = boidsInRange[i].GetComponent<Boids>();
            float d = Vector3.Distance(transform.position, boid.transform.position);
            if (boid == this || d > maxSeparationDistance)
                continue;
            Vector3 diff = (transform.position - boid.transform.position).normalized * 10;
            diff /= (float)BetterFastPow(d, 2);
            vec += diff;
            total++;
            
        }
        if (total > 0)
        {
            vec /= total;
            vec = Vector3.ClampMagnitude(vec, 10);
            vec -= this.velocity;
        }
        return vec;
    }

    //https://nic.schraudolph.org/pubs/Schraudolph99.pdf
    static public double BetterFastPow(double a, double b)
    {
        b *= 0.5f;
        double tmpF = 9076650 * (a - 1) / (a + 1 + 4 * (Math.Sqrt(a)));
        long tmp1 = (long)(1072632447 + tmpF * b);
        long tmp2 = (long)(1072632447 - tmpF * b);
        return BitConverter.Int64BitsToDouble(tmp1 << 32) / BitConverter.Int64BitsToDouble(tmp2 << 32);
    }

    Vector3 Cohesion()
    {
        Vector3 vec = Vector3.zero;
        float maxCohesionDistance = 1;
        int total = 0;
        for (int i = 0; i < boidsInRange.Count; i++)
        {
            Boids boid = boidsInRange[i].GetComponent<Boids>();
            float d = Vector3.Distance(boid.transform.position, transform.position);
            if (boid == this || d>maxCohesionDistance)
                continue;
            vec += (boid.transform.position);
            total++;
            
        }
        if (total > 0)
        {
            vec /= total;
            vec -= transform.position;
        }
        return vec;
    }

    public Vector3 Goal()
    {
        Vector3 vec=Vector3.zero;
        vec = (goal.position - transform.position).normalized * speed;
        return vec;
    }

    void Simulate()
    {
        flock.tree.GetNearbyNonAlloc(transform.position, 1, boidsInRange);
        //Clamp magnitudes to limit the amount of force each part can add to stop them cancelling out eachother by being too strong
        //I make alignment and cohesion not add a lot of force while allowing goal and separation to add a lot.
        velocity += Vector3.ClampMagnitude(Alignment(), 10);
        velocity += Vector3.ClampMagnitude(Cohesion(), 5);
        velocity += Vector3.ClampMagnitude(Seperation(), 30);
        velocity += Vector3.ClampMagnitude(Goal(), 20);

        velocity = Vector3.ClampMagnitude(velocity, speed);
    }




    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        Simulate();
    }
}
