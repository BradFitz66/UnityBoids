using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Flock : MonoBehaviour
{
    public List<Boids> boids;
    public Transform goal;
    public PointOctree<GameObject> tree;


    private void Start()
    {
        tree = new PointOctree<GameObject>(25, transform.position,.25f);
        foreach (Boids boid in boids)
        {
            boid.goal = goal;
            boid.transform.parent = transform;
            boid.flock = this;
            tree.Add(boid.gameObject,boid.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (tree!=null)
        {
            tree.DrawAllBounds(); // Draw node boundaries
            tree.DrawAllObjects(); // Draw object boundaries
        }
    }

    private void Update()
    {
        //Use a for loop instead of a foreach to save on performance
        for (int i = 0; i < boids.Count; i++)
        {
            //No way to just update the tree without removing and adding back each boid.
            tree.Remove(boids[i].gameObject);
            tree.Add(boids[i].gameObject, boids[i].transform.position);
        }
    }
}
