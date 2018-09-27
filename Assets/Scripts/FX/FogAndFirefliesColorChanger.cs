using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogAndFirefliesColorChanger : MonoBehaviour {

    public ParticleSystem p_fog;
    public ParticleSystem p_fireflies;

    public void ChangeColor(Color color)
    {
        ParticleSystem.MainModule fogMain = p_fog.main;
        ParticleSystem.MainModule firefliesMain = p_fireflies.main;
        fogMain.startColor = new Color(color.r,color.g, color.b, fogMain.startColor.color.a);
        firefliesMain.startColor = new Color(color.r, color.g, color.b, firefliesMain.startColor.color.a);
    }
}
