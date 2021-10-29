using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingAnimation : MonoBehaviour
{

    private void OnEnable()
    {
        LeanTween.scale(gameObject, (transform.localScale) * 1.6f, 0.5f);
        LeanTween.alpha(gameObject,0,0.5f).setOnComplete(() => { gameObject.SetActive(false); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
