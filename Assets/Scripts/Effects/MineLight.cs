using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class MineLight : MonoBehaviour
{
    [SerializeField] float FadingRate=0.1f;
    [SerializeField] float FadingAmount=0.4f;
    [SerializeField] Light2D light;
    bool fading = true;
    float alpha = 1;
    
    

    public void SetRadius(float newradius)
    {
        light.pointLightInnerRadius = newradius;
        light.pointLightOuterRadius = newradius;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        //Color c = renderer.material.color;
        if (fading)
        {
            while(alpha > 0)
            {
                alpha -= FadingAmount * Time.deltaTime;
                light.intensity = alpha;
                yield return new WaitForSeconds(FadingRate);
            }
            
            fading = false;
        }
        else
        {
            while (alpha < 1)
            {
                alpha += FadingAmount * Time.deltaTime;
                light.intensity = alpha;
                yield return new WaitForSeconds(FadingRate);
            }
            
            fading =true;
        }

    }
}
