using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int items_picked = 0;

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "HealthSprite") {
            //Destroy(this.transform.gameObject);
            Debug.Log("\n\nPickup Item collected!");
            items_picked++;
        } 
    }

    // public void OntriggerEnter2D(Collision2D collision) {
    //     if (collision.gameObject.tag == "Finish") {
    //         //Destroy(this.transform.gameObject);
    //         Debug.Log("\n\nPickup Item collected!");
    //         items_picked++;
    //     } 
    // }
}
