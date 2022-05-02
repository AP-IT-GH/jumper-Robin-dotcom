using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class JumperAgent : Agent
{
    public Transform coin;
    public Transform obstacle;

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(-7f, 0.5f, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(coin.localPosition);
        sensor.AddObservation(obstacle.localPosition);

    }

    public float jumpSpeed = 2f;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Acties
        Vector3 controlSignal = Vector3.zero;

        controlSignal.y = actions.ContinuousActions[0];

        this.transform.Translate(controlSignal * jumpSpeed);

        //Beloningen
        AddReward(0.0001f);
        
    }

    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    var continuousActionsOut = actionsOut.ContinuousActions;

    //    continuousActionsOut[0] = Input.GetAxis("Vertical");
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") == true)
        {
            AddReward(-1f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Coin") == true)
        {
            AddReward(1f);
            Destroy(collision.gameObject);
        }
    }
}
