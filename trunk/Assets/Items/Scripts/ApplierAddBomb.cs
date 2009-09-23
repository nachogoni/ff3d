using UnityEngine;
using System.Collections;

public class ApplierAddBomb : Applier
{

    // Use this for initialization
    void Start()
    {
        actor.bombCount[(int)actor.bomb] += (int)value;
        showText("+ " + (int)value + " " + actor.bomb + "'s");
    }

    // Update is called once per frame
    void Update()
    {
       Object.Destroy(this);
    }

    void End()
    {
    }
}
