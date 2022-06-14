using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class HerbivoreScript : Agent
{
    // Start is called before the first frame update
    Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin() {
    }

    private float moveSpeed = 2.0f;
    private float turnSpeed = 300.0f;

    public override void CollectObservations(VectorSensor sensor) {
    }
    public override void OnActionReceived(ActionBuffers actionBuffers) {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var continuousActions = actionBuffers.ContinuousActions;
        var discreteActions = actionBuffers.DiscreteActions;

        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[1], -1f, 1f);

        dirToGo = transform.forward * forward;
        rotateDir = -transform.up * rotate;

        rBody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (rBody.velocity.sqrMagnitude > 15f) // slow it down
            rBody.velocity *= 0.95f;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D)) continuousActionsOut[1] = -1;
        if (Input.GetKey(KeyCode.W)) continuousActionsOut[0] = 1;
        if (Input.GetKey(KeyCode.A)) continuousActionsOut[1] = 1;
        if (Input.GetKey(KeyCode.S)) continuousActionsOut[0] = -1;
    }
}
