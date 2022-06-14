using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class EnvController : MonoBehaviour {
    public GameObject grass;
    public FoodProcessorScript FoodControl;

    public int range;

    int TimeCount;
    
    // Start is called before the first frame update
    void Start() {
        FoodControl = FindObjectOfType<FoodProcessorScript>();
    }
}
