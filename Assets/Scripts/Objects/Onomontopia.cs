using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onomontopia : MonoBehaviour
{
    float timeStamp;
    TimeManager timeManager;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    public void Init()
    {
        if (timeManager == null)
        {
            timeManager = GameManager.ActiveGameManager.TimeManager;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        timeStamp = timeManager.GameTime;
        spriteRenderer.enabled = true;
    }

    void Update()
    {
        float t = timeManager.GameTime - timeStamp;
        const float maxTime = 0.5f;

        if (t > maxTime)
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (t < maxTime/4)
                spriteRenderer.color = new Color(1, 1, 1, t * 4);
            else if (t > 3*maxTime/4)
                spriteRenderer.color = new Color(1, 1, 1,(maxTime-t) * 4);


        }
    }
}
