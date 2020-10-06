using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlash : MonoBehaviour
{
    float nextFlash;
    const float flashRate = 2f;
    const float stepTime = 0.06f;
    TimeManager timeManager;
    SpriteRenderer spriteRenderer;
    float scale;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        scale = transform.localScale.x;
    }
    private void Update()
    {
        float t = timeManager.WorldTime - nextFlash;
        
        if(t > 0)
        {
            spriteRenderer.enabled = true;
            if (t < stepTime)
                transform.localScale = Vector3.one * scale * t * (1f/stepTime);
            else if (t < 2 * stepTime)
                transform.localScale = Vector3.one * scale;
            else
                transform.localScale = Vector3.one * (scale * ((stepTime * 3) - t) * (1f/stepTime));

            if(t > stepTime * 3)
            {
                nextFlash = timeManager.WorldTime + flashRate + (Random.value * flashRate);
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
    
}
