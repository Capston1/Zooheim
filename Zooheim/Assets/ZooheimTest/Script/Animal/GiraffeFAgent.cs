using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GiraffeFAgent : GiraffeAgent {
    public GiraffeAgent ChildPrefab;
    protected bool AnimalChildbirthFlag = false;

    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);
        sensor.AddObservation(AnimalChildbirthFlag);
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        if((AnimalTimer > AnimalChildbirthPeriod) && !AnimalChildbirthFlag) {
            AnimalChildbirthFlag = true;
        }
    }

    public override void Childbirth() {
        Debug.Log("Giraffe Child birth");
        AnimalTimer = 0f;
        AnimalChildbirthFlag = false;
        float RandNum = Random.Range(0f, 1f);
        float RandomOffset = Random.Range(0.2f, 0.5f);
        Vector3 BirthPosition = new Vector3(RandomOffset, 0f, RandomOffset);
        BirthPosition += transform.localPosition;
        if(BirthPosition.x < -range) BirthPosition.x += 0.1f;
        if(BirthPosition.z < -range) BirthPosition.z += 0.1f;
        if(BirthPosition.x > range) BirthPosition.x -= 0.1f;
        if(BirthPosition.z > range) BirthPosition.z -= 0.1f;

        if(RandNum > 0.5f) {
            var Child = Instantiate(ChildPrefab, BirthPosition, Quaternion.identity);
            Child.GetComponent<GiraffeAgent>().AnimalInit();
            Child.transform.parent = transform.parent.transform;
        }
        else {
            var Child = Instantiate(gameObject, BirthPosition, Quaternion.identity);
            Child.GetComponent<GiraffeFAgent>().AnimalInit();
            Child.transform.parent = transform.parent.transform;
        }
        
            
    }

    public override void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Grass") && AnimalEnergy < AnimalEnoughEnergy) {
            float AteCalorie = other.gameObject.GetComponent<Grass>().Eat();
            Eat(AteCalorie);
            Freeze(2.0f);
        }
        else if(other.gameObject.CompareTag("Tree")) {
            float AteCalorie = other.gameObject.GetComponent<Wood>().Eat();
            Eat(AteCalorie);
            Debug.Log("Tree collison");
            Freeze(2.0f);
        }
        else if(AnimalChildbirthFlag && other.gameObject.CompareTag("Giraffe")) {
            Childbirth();
            Freeze(2f);
            AddReward(AnimalChildbirthReward);
        }
    }
}
