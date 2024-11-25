using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedState
{
    public EnemyState State { get; set; }
    public float OriginalWeight { get; set; }
    public float Weight { get; set; }

    public WeightedState(EnemyState state, float weight)
    {
        State = state;
        OriginalWeight = weight;
        Weight = weight;
    }

    public void ResetWeight()
    {
        Weight = OriginalWeight;
    }
}
