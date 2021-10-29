using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image[] dot = new Image[3];
    int i=-1;
    void Start()
    {
        StartCoroutine(DotsAnimation());
    }

    IEnumerator DotsAnimation()
    {
        
        i++;
        if (i > dot.Length - 1)
        {
            i = 0;
        }
        yield return new WaitForSeconds(0.3f);        
        dot[i].color = Color.HSVToRGB(0, 0, 100/100f);
        if (i == 0)
        {
            dot[dot.Length - 1].color = Color.HSVToRGB(0, 0, 80/100f);            
        }
        else
        {
            dot[i - 1].color = Color.HSVToRGB(0, 0, 80/100f);
        }
       
        StartCoroutine(DotsAnimation());
    }
}
