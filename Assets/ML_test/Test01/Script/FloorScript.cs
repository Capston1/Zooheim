using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour {

    public GameObject obj;
    public int numFood;
    public int range;
    
    // Start is called before the first frame update
    void Start() {
        for(int i = 0; i < numFood; i++) CreateFood(Random.Range(-range, range), Random.Range(-range, range));
    }

    // Update is called once per frame
    void Update() {
        
    }

    void CreateFood(int x, int z) {
        Instantiate(obj, new Vector3(x, 0.5f, z), Quaternion.identity);
    }
}
