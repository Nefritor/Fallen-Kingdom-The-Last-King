using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour {

    MeshRenderer material;
    bool isRayHitted;

    void Start()
    {
        material = GetComponent<MeshRenderer>();
        isRayHitted = false;
    }

    void Update()
    {
        if (!isRayHitted)
        {
            FullAlpha();
        }
        else isRayHitted = false;
    }
    public virtual void SemiAlpha()
    {
        isRayHitted = true;
        material.material.color = new Color(material.material.color.r, material.material.color.g, material.material.color.b, 0.5f);
    }

    public virtual void FullAlpha()
    {
        material.material.color = new Color(material.material.color.r, material.material.color.g, material.material.color.b, 1);
    }
}
