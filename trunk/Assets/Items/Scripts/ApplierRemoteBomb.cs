using UnityEngine;
using System.Collections;

public class ApplierRemoteBomb : Applier
{
    float time;

    // Use this for initialization
    void Start()
    {
        GameController.setBombType(BombType.RemoteBomb);
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
        GameController.setBombType(BombType.DefaultBomb);
    }
}