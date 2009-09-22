using UnityEngine;
using System.Collections;

public class ExplosionParticles : Damageable
{
    float time = .5f;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            Object.Destroy(gameObject);
        }

    }
}