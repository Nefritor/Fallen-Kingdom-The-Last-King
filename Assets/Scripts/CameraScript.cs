using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform player;
    public LayerMask collisionMask;

	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        float distance = Vector3.Distance(transform.position, player.position);

        if (Physics.Raycast(ray, out hit, distance, collisionMask))
        {
            if (hit.collider.GetComponent<Alpha>())
            {
                hit.collider.GetComponent<Alpha>().SemiAlpha();
            }
        } 
	}
}
