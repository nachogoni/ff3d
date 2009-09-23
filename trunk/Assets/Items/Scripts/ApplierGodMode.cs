using UnityEngine;
using System.Collections;

public class ApplierGodMode : Applier
{
    float auxDamage;
    float time;

    // Use this for initialization
    void Start()
    {
        auxDamage = actor.health[1];
        actor.health[1] = 0;
        showText("GodMode!");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > maxTime)
        {
            time = 0;
            End();
            Object.Destroy(this);
        }
    }

    void End()
    {
        actor.health[1] = auxDamage;
    }
}
