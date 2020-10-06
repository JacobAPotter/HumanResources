using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    float collectedTimeStamp;
    TimeManager timeManager;
    Player player;
    int collectableIndex;
    Vector3 defaultScale;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        player = GameManager.ActiveGameManager.Player;
        collectedTimeStamp = float.MinValue;

        if (name.Contains("1"))
            collectableIndex = 1;
        else if (name.Contains("2"))
            collectableIndex = 2;
        if (name.Contains("3"))
            collectableIndex = 3;

        defaultScale = transform.localScale;
    }

    private void Update()
    {
        float timeDiff = timeManager.GameTime - collectedTimeStamp;

        if (timeDiff < 6f)
        {
            if (timeDiff < 3f)
            {
                transform.Translate(Vector3.up * timeManager.GameDeltaTime, Space.World);
                
            }
            else if (timeDiff < 5.9f)
            {
                transform.position = GameManager.ActiveGameManager.MainCamera.transform.position + Vector3.forward * 10.5f;
                transform.localScale = defaultScale * 5f;
                GetComponent<ConstantRotation>().enabled = false;
                transform.localRotation = Quaternion.Euler(0, (timeDiff-4.5f) * 12, 0);
            }
            else
                gameObject.SetActive(false);
        }
        else if(Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            PlayerHasCollected();
        }
    }

    public void PlayerHasCollected()
    {
        collectedTimeStamp = timeManager.GameTime;
        GameManager.ActiveGameManager.FXManager.GetSparkles(transform.position);
        GameManager.ActiveGameManager.LevelManager.CollectableCollected(GameManager.ActiveGameManager.SettingsManager.GameSlot,
                                                                        GameManager.ActiveGameManager.LevelManager.CurrentLevelName,
                                                                        collectableIndex);
    }
}
