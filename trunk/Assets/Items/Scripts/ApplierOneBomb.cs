using UnityEngine;
using System.Collections;

public class ApplierOneBomb : Applier
{
    float time;

    // Use this for initialization
    void Start()
    {
        Debug.Log(actor.bombCount + " - " + (int)value + " - " + value);
        actor.bombCount += (int)value;
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
    }
}
