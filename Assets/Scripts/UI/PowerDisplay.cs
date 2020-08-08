using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    Image[] powerUnits;
    RectTransform rectTransform;
    PowerManager powerManager;
    Image tempUnit;

    void Start()
    {
        powerManager = GameManager.ActiveGameManager.PowerManager;
        rectTransform = GetComponent<RectTransform>();
        tempUnit = transform.Find("TempUnit").GetComponent<Image>();
        
        Init();
        tempUnit.gameObject.SetActive(false);
    }

    void Update()
    {
        //the number of power units has changed
        if(powerUnits.Length != powerManager.MaxPowerUnits)
            Init();

        for(int i =0; i < powerUnits.Length; i++)
            powerUnits[i].enabled = i < powerManager.PowerUnits;
    }

    void Init()
    {
        if(powerUnits != null)
        {
            for(int i =0; i < powerUnits.Length; i++)
                GameObject.Destroy(powerUnits[i].gameObject);
        }

        if (powerManager.MaxPowerUnits <= 0)
        {
            powerUnits = new Image[0];
            return;
        }

        powerUnits = new Image[powerManager.MaxPowerUnits];

        float height =   rectTransform.rect.height / powerUnits.Length;

        tempUnit.gameObject.SetActive(true);

        const float shrink = 0.9f;

        for(int i =0; i < powerUnits.Length; i++)
        {
            powerUnits[i] = GameObject.Instantiate(tempUnit.gameObject).GetComponent<Image>();
            powerUnits[i].transform.parent = transform;

            RectTransform r = powerUnits[i].GetComponent<RectTransform>();

            r.anchoredPosition = new Vector2(0,  height * (powerUnits.Length - i) - 100 + (50 - 50/(Mathf.Max( powerUnits.Length, 1)))) * shrink;
            r.sizeDelta = new Vector2(rectTransform.rect.width, height * shrink);
        }

        tempUnit.gameObject.SetActive(false);

    }

}

