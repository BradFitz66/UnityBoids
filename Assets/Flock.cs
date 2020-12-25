using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public List<Boids> boids;
    public Transform goal;

    private void Start()
    {
        foreach (Boids boid in boids)
        {
            boid.boidsInRange = boids;
            boid.goal = goal;
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    private void Update()
    {
        

    }
}
