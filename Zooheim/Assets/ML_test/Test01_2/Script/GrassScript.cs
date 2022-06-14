using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Herbivore"))
            Destroy(gameObject);
    }
}
