using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcessorScript : MonoBehaviour
{
    public int numFood;
    public int range;
    int TimeCount;

    public GameObject grass;

    // Start is called before the first frame update
    void Start() {
        TimeCount = 0;
        ResetFood();
    }

    // Update is called once per frame
    void Update() {
        
    }

        // Update is called once per frame
    void FixedUpdate() {
        TimeCount = (TimeCount + 1) % 120000;
        if(TimeCount % 12000 == 0) CreateFood();
    }

    public void ResetFood() {
        Transform[] grassList = GetComponentsInChildren<Transform>();
        if(grassList != null) {
            for(int i = 1; i < grassList.Length; i++)
                Destroy(grassList[i].gameObject);
        }
        for(int i = 0; i < numFood; i++)
            CreateFood();
    }
    public void CreateFood() {
        int x = Random.Range(-range, range);
        int z = Random.Range(-range, range); 
        Instantiate(grass, new Vector3(x, 0.5f, z), Quaternion.identity).transform.parent = transform;
    }
}
