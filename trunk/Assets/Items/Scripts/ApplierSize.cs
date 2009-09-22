using UnityEngine;
using System.Collections;

public class ApplierSize : Applier
{
    // Use this for initialization
    void Start()
    {
        actor.bombSize += (int)value;
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
