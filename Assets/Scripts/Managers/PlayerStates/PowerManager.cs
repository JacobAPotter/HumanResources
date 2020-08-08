using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public float Power { get; private set; }

    public int MaxPowerUnits { get; private set; } = 5;

    public int PowerUnits { get { return (int)Mathf.Floor(Power + 0.01f); } }

    private void Start()
    {
        ResetPower();
    }

    public void ResetPower()
    {
        Power = MaxPowerUnits;
    }

    public void UsePower()
    {
        UsePower(1);
    }

    public void UsePower(int numberUnits)
    {
        Power -= numberUnits;

        if (Power < 0)
            Power = 0;
    }

    public void UsePower(float percentage)
    {
        int u = PowerUnits;

        Power -= percentage * MaxPowerUnits;

        if (Power < 0)
            Power = 0;

        //make sure were losing at least one unit 
        //for every power use 
        if (PowerUnits == u)
            Debug.Log("No power lost");
    }

   
}
