using UnityEngine;
using System.Collections;

public class ApplierSize : Applier
{
    float time;
    // Use this for initialization
    void Start()
    {
        actor.bombSize += (int)value;
        showText("More Size!");
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
        actor.bombSize -= (int)value;
    }
}
