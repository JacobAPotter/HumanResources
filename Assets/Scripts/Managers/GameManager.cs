using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager ActiveGameManager;

    public MainCamera MainCamera { get; private set; }
    public Player Player { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public StateManager StateManager { get; private set; }
    public Text DebugText { get; private set; }
    public ColliderManager ColliderManager { get; private set; }
    public MaterialManager MaterialManager { get; private set; }
    public PowerManager PowerManager { get; private set; }

    void Awake()
    {
        ActiveGameManager = this;

        Transform world = GameObject.Find("World").transform;

        MainCamera = world.Find("MainCamera").GetComponent<MainCamera>();
        TimeManager = world.Find("TimeManager").GetComponent<TimeManager>();
        InputManager = world.Find("InputManager").GetComponent<InputManager>();
        DebugText = world.Find("Canvas").Find("Debug").GetComponent<Text>();
        ColliderManager = world.Find("ColliderManager").GetComponent<ColliderManager>();
        MaterialManager = world.Find("MaterialManager").GetComponent<MaterialManager>();
        Player = world.Find("Player").GetComponent<Player>();
        PowerManager = Player.GetComponent<PowerManager>();
        StateManager = Player.GetComponent<StateManager>();
    }
}
