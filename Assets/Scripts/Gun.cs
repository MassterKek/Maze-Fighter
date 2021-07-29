using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject stone;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            shoot();
        }
    }   

    public void shoot() {
        Instantiate(stone, firePoint.position, firePoint.rotation);
        Destroy(stone, 1.0f);
    }
}
