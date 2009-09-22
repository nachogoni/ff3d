using UnityEngine;
using System.Collections;

public class BricksWall : Wall {

    public int aa = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override GameObject Destroy()
    {
        Object.Destroy(this.gameObject);
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        // Si me choca una explosion -> Exploto
        ExplosionParticles explosionParticles = other.gameObject.GetComponent(typeof(ExplosionParticles)) as ExplosionParticles;        
        if (explosionParticles != null)
        {
            Object.Destroy(this);
        }
    }
}