using UnityEngine;
using System.Collections;

public class ApplierOneBomb : Applier
{

    // Use this for initialization
    void Start()
    {
        actor.bombCount += (int)value;
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
