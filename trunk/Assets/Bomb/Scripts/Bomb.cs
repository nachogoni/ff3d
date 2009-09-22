using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    [HideInInspector]
    public Actor deliverer;
    [HideInInspector]
    public int size = 1;
    [HideInInspector]
    public GameObject[] walls;

    public float time = 10;
    public float damage = 5;

    float lastX, lastZ;

	// Use this for initialization
	void Start () {
        lastX = transform.position.x;
        lastZ = transform.position.z;
    }
	
	// Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(lastX, transform.position.y, lastZ);

        time -= Time.deltaTime;
        if (time < 0)
        {
            // Creo una instancia de una Explosion
            GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Explosion", typeof(GameObject)));
            Explosion explosion = go.GetComponent(typeof(Explosion)) as Explosion;
            explosion.transform.position = transform.position;
            //explosion.transform.parent = transform.parent;
            explosion.size = size;

            // Actualizo la cantidad de bombas del Actor
            if (deliverer != null)
                deliverer.bombCount++;

            explosion.walls = walls;

            Object.Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si me choca una explosion -> Exploto
        ExplosionParticles explosionParticles = other.gameObject.GetComponent(typeof(ExplosionParticles)) as ExplosionParticles;
        if (explosionParticles != null)
        {
            time = -1f;
        }
    }
}
