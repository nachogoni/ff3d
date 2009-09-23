using UnityEngine;
using System.Collections;

public class ApplierRemoteBomb : Applier
{
    float time;

    // Use this for initialization
    void Start()
    {
        actor.bombCount[(int)BombType.RemoteBomb] += (int)value;
        showText("+ " + (int)value + " " + BombType.RemoteBomb + "'s");
    }

    void Update()
    {
        Object.Destroy(this);
    }
}