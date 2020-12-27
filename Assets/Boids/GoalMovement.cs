using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    Vector3 pos;

    float movingTimer=4;
    float movingTimerCounter=2;

    private void Start()
    {
        pos = Vector3.zero + Random.insideUnitSphere * 3000;
        movingTimerCounter = movingTimer;
    }

    private void Update()
    {
        movingTimerCounter -= 1 * Time.deltaTime;
        if (movingTimerCounter <= 0 || Vector3.Distance(transform.position,pos)<1)
        {
            movingTimerCounter = movingTimer;
            pos = Vector3.zero + Random.insideUnitSphere * 3000;
        }
        transform.Translate((pos - transform.position).normalized*50*Time.deltaTime);
    }
}
