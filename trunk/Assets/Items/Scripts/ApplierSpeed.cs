using UnityEngine;
using System.Collections;

public class ApplierSpeed : Applier
{
    float time;

    // Use this for initialization
    void Start()
    {
        actor.speed += value;
        showText("Run Forest, Run!");
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
        actor.speed -= value;
    }
}
