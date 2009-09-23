using UnityEngine;
using System.Collections;

public class ApplierProximityBomb : Applier {
    float time;

    // Use this for initialization
    void Start()
    {
        actor.bombCount[(int)BombType.ProximityBomb] += (int)value;
        showText("+ " + (int)value + " " + BombType.ProximityBomb + "'s");
    }

    void Update()
    {
        Object.Destroy(this);
    }
}