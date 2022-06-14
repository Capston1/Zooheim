using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent {
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public Transform BadTarget;
    public override void OnEpisodeBegin() {
       // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }

        Vector3 TargetLoc;
        Vector3 BadTargetLoc;
        // Move the target to a new spot
        while(true) {
            TargetLoc = new Vector3(Random.value * 16 - 8,
                                           0.5f,
                                           Random.value * 16 - 8);
            BadTargetLoc = new Vector3(Random.value * 16 - 8,
                                           0.5f,
                                           Random.value * 16 - 8);
            if(Vector3.Distance(TargetLoc, BadTargetLoc) >= 3.0f) break; 
        }
        Target.localPosition = TargetLoc;
        BadTarget.localPosition = BadTargetLoc;
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 30;
    public float moveSpeed = 2;
    public float turnSpeed = 300;
    public override void OnActionReceived(ActionBuffers actionBuffers) {
        // Actions, size = 2
        // Vector3 controlSignal = Vector3.zero;
        // controlSignal.x = actionBuffers.ContinuousActions[0];
        // controlSignal.z = actionBuffers.ContinuousActions[1];
        // rBody.AddForce(controlSignal * forceMultiplier);
        // // Rewards
        // float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        // // Reached target
        // if (distanceToTarget < 1.42f) {
        //    SetReward(1.0f);
        //    EndEpisode();
        // }

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var continuousActions = actionBuffers.ContinuousActions;
        var discreteActions = actionBuffers.DiscreteActions;

        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var right = Mathf.Clamp(continuousActions[1], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[2], -1f, 1f);

        dirToGo = transform.forward * forward;
        dirToGo += transform.right * right;
        rotateDir = -transform.up * rotate;

        rBody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (rBody.velocity.sqrMagnitude > 15f) // slow it down
            rBody.velocity *= 0.95f;


        // Fell off platform
        if (this.transform.localPosition.y < 0) {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Target")) {
            AddReward(1.0f);
            EndEpisode();
        }
        else if(collider.gameObject.CompareTag("BadTarget")) {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D)) continuousActionsOut[2] = 1;
        if (Input.GetKey(KeyCode.W)) continuousActionsOut[0] = 1;
        if (Input.GetKey(KeyCode.A)) continuousActionsOut[2] = -1;
        if (Input.GetKey(KeyCode.S)) continuousActionsOut[0] = -1;
    }
}
