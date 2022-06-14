using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public enum Type {
    Herbivore = 0,
    Carnivore = 1
}

public class AnimalAgent : Agent {
    Rigidbody rBody;
    public Type AnimalType;
    public FoodProcessorScript FoodControl;

    public int range = 40;
    int TimeCount;
    int TakenGrassNum = 0;

    void Start() {
        rBody = GetComponent<Rigidbody>();
        FoodControl = FindObjectOfType<FoodProcessorScript>();
    }

    public override void OnEpisodeBegin() {
        TakenGrassNum = 0;
        transform.localPosition = new Vector3(Random.Range(-range, range), 0.5f, Random.Range(-range, range));
        FoodControl.ResetFood();
    }
    
    private float HerbiMoveSpeed = 0.4f;
    private float CarniMoveSpeed = 0.5f;

    private float HerbiTurnSpeed = 150.0f;
    private float CarniTurnSpeed = 100.0f;

    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {
        MoveAgent(actionBuffers);

        /*
        var forward = Mathf.Clamp(continuousActions[0], 0f, 1f);
        var rotate = Mathf.Clamp(continuousActions[1], -1f, 1f);

        dirToGo = transform.forward * forward;
        rotateDir = -transform.up * rotate;
        
        rBody.AddForce(dirToGo * MoveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * TurnSpeed);

        if (rBody.velocity.sqrMagnitude > SpeedLimit) // slow it down
            rBody.velocity *= 0.95f;
        */
    }

    public void MoveAgent(ActionBuffers actionBuffers) {
        var continuousActions = actionBuffers.ContinuousActions;
        var discreteActions = actionBuffers.DiscreteActions;
        
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        float MoveSpeed = 0;
        float TurnSpeed = 0;
        float SpeedLimit = 0;

        if(this.AnimalType == Type.Herbivore) {
            MoveSpeed = HerbiMoveSpeed;
            TurnSpeed = HerbiTurnSpeed;
            SpeedLimit = 10.0f;
        }
        else if(this.AnimalType == Type.Carnivore) {
            MoveSpeed = CarniMoveSpeed;
            TurnSpeed = CarniTurnSpeed;
            SpeedLimit = 8.0f;
        }

        var forwardAxis = discreteActions[0];
        var rotateAxis = discreteActions[1];

        switch(forwardAxis) {
            case 1:
                dirToGo = transform.forward * MoveSpeed;
                break;
        }

        switch(rotateAxis) {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * TurnSpeed);
        rBody.AddForce(dirToGo, ForceMode.VelocityChange);

        if (rBody.velocity.sqrMagnitude > SpeedLimit) // slow it down
            rBody.velocity *= 0.95f;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 1;
        if (Input.GetKey(KeyCode.D)) discreteActionsOut[1] = 1;
        if (Input.GetKey(KeyCode.A)) discreteActionsOut[1] = 2;
    }

    public void OnTriggerEnter(Collider other) {
        if(AnimalType == Type.Herbivore) {
            if(other.gameObject.CompareTag("Grass")) {
                TakenGrassNum++;
                AddReward(2.0f);
                Debug.Log("Herbivore ate grass!!");
                if(TakenGrassNum > 10) EndEpisode();
            }
        }
    }

    public void OnCollisionEnter(Collision other) {
        if(AnimalType == Type.Carnivore) {
            if(other.gameObject.CompareTag("Herbivore")) {
                AddReward(5.0f);
                other.gameObject.GetComponent<AnimalAgent>().AddReward(-3.0f);
                Debug.Log("Herbivore caught by Carnivore!!");
                EndEpisode();
            }
        }
    }

    public void OnCollisionStay(Collision other) {
        if(other.gameObject.CompareTag("Wall")) {
            AddReward(-0.1f);
            //Debug.Log(gameObject.name + " stuck at wall!!");
    }
}

//    public void AnimalAddReward(float reward) {
//       AddReward(reward);
//    }
}
