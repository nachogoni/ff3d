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

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

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
