using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnPlayerStatus : MonoBehaviour
{
    [SerializeField]
    bool hasPowers;
    [SerializeField]
    bool hasGirl;

    bool initialized;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        bool activate = true;

        GameSettings settings = LevelManager.GetSettings(SettingsManager.ActiveSettingsManager.GameSlot);

        if (hasPowers)
            if (!settings.hasPowers)
                activate = false;

        if (hasGirl)
            if (!settings.hasGirl)
                activate = false;

        gameObject.SetActive(activate);
        initialized = true;
    }

     public bool IsActive
    {
        get
        {
            if (!initialized)
                Initialize();
               
            return gameObject.activeSelf;

        }
    }
}
