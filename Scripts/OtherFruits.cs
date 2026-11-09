
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherFruits : MonoBehaviour
{
    public float opacity = 0.5f;
    public float darkAmount = 0.5f;

    void OnTriggerEnter(Collider collision)
    {
        Renderer rend = GetComponent<Renderer>();

        if (rend != null)
        {
            Color color = rend.material.color;

            color.a = opacity;
            color.r *= darkAmount;
            color.g *= darkAmount;
            color.b *= darkAmount;

            rend.material.color = color;
        }
    }
}
