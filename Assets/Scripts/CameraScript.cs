using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Transform player;
    MeshRenderer mat;
    bool isRayHitted;

    private void Start()
    {
        isRayHitted = false;
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        float distance = Vector3.Distance(transform.position, player.position);

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.GetComponent<MeshRenderer>() && hit.collider.gameObject.layer == 8)
            {
                mat = hit.collider.GetComponent<MeshRenderer>();
                float alpha = mat.material.color.a;
                alpha -= -(Mathf.Pow(mat.material.color.a - 0.75f, 2) - 0.07f) * Time.deltaTime * 50;
                alpha = Mathf.Clamp(alpha, 0.5f, 1);
                mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, alpha);
                isRayHitted = true;
            }
            if (!isRayHitted && mat != null)
            {
                float alpha = mat.material.color.a;
                alpha += -(Mathf.Pow(mat.material.color.a - 0.75f, 2) - 0.07f) * Time.deltaTime * 50;
                alpha = Mathf.Clamp(alpha, 0.5f, 1);
                mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, alpha);
            }
            else isRayHitted = false;
        }
    }
}
