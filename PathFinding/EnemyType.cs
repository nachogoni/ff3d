using UnityEngine;
using System;

public class EnemyType
{

    public Vector3 pos;

    float lastUpdate;
    

    public EnemyType(Vector3 position)
    {
        pos = position;

        lastUpdate = Time.realtimeSinceStartup;

    }

    public Vector3 newPos(Vector3 position)
    {
        float actual = Time.realtimeSinceStartup;

        Vector3 v = new Vector3(pos.x + ((position.x - pos.x) * 10f * (actual - lastUpdate) / (actual - lastUpdate)), pos.y, 
                                pos.z + ((position.z - pos.z) / (actual - lastUpdate)) * 4 * (actual - lastUpdate));

        /*Debug.Log(Time.realtimeSinceStartup + " estuvo aca " + pos.x + 
            " supongo que va a estar aca " + v.x + " en " + (actual + 5f * (actual - lastUpdate)));
        */
        pos = position;

        lastUpdate = Time.realtimeSinceStartup;

        return v;
    }

}