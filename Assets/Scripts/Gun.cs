using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject stone;
    public GameObject cube;
    public int cubes_picked = 0;
    public bool isPicked;

    public void shoot() {
        GameObject bullet = Instantiate(stone, firePoint.position, firePoint.rotation);
        Destroy(bullet, 1.5f);
    }

    public void pickup() {
        cubes_picked++;
        Debug.Log("Cubes Picked: " + cube);
        Destroy(cube);
    }

    void Update()
    {
        if (isPicked) {
            pickup();
        }
        if (Input.GetMouseButtonDown(0)) {
            shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D item) {
        if (item.gameObject.name.Equals("Player")) {
            isPicked = true;
        }
    }
}
