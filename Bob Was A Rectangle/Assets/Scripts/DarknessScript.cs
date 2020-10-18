using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessScript : MonoBehaviour
{
    Color lightColor = new Color(RenderSettings.ambientLight.r, RenderSettings.ambientLight.g, RenderSettings.ambientLight.b, RenderSettings.ambientLight.a);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Darkness()
    {
        RenderSettings.ambientLight = Color.black;
    }

    public void Light()
    {
        RenderSettings.ambientLight = lightColor;
    }
}
