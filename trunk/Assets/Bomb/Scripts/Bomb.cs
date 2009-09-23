using UnityEngine;
using System.Collections;

public enum BombType
{
    DefaultBomb = 0,
    RemoteBomb,
    ProximityBomb
}

public class Bomb : MonoBehaviour {

    [HideInInspector]
    public Actor deliverer;
    [HideInInspector]
    public int size = 1;
    [HideInInspector]
    public GameObject[] walls;
    [HideInInspector]
    public int index = 0;

    public float time = 3;
    [HideInInspector]
    public float lastX, lastZ;

	// Use this for initialization
	void Start () {
        lastX = transform.position.x;
        lastZ = transform.position.z;
    }
	
    public GameObject Destroy()
    {
        time = -1f;
        return null;
    }

    void Update()
    {
        transform.position = new Vector3(lastX, transform.position.y, lastZ);
    }

    public void showText(string value)
    {
        //Creo el texto
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Items/ItemText", typeof(GameObject)));
        go.transform.position = transform.position + Vector3.forward;
        ItemText itext = go.GetComponent(typeof(ItemText)) as ItemText;
        itext.value = value;
    }

    public void explote()
    {
        // Creo una instancia de una Explosion
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Explosion", typeof(GameObject)));
        Explosion explosion = go.GetComponent(typeof(Explosion)) as Explosion;
        explosion.transform.position = transform.position;
        explosion.size = size;

        explosion.walls = walls;


        Object.Destroy(gameObject);
        walls[index] = null;
    }

/*    void OnTriggerEnter(Collider other)
    {
        // Si me choca una explosion -> Exploto
        ExplosionParticles explosionParticles = other.gameObject.GetComponent(typeof(ExplosionParticles)) as ExplosionParticles;
        if (explosionParticles != null)
        {
            time = -1f;
        }
    }*/
}