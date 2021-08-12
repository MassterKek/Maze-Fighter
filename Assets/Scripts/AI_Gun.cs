using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lazer;

    public void shoot() {
        GameObject bullet = Instantiate(lazer, firePoint.position, firePoint.rotation);
        Destroy(bullet, 1.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            shoot();
        }
    }
}
