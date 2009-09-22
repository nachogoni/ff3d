using UnityEngine;
using System.Collections;

public class PlayerController : Controller
{
    // Maxima velocidad inicial en alguno de los ejes
    const float BOMB_INIT_VEL = 5;
    // Maxima distancia inicial al Player hacia arriba
    const float BOMB_INIT_POS = 1;
    // Minimo tiempo que tiene que haber pasado para poder poner otra bomba
    const float BOMB_MIN_ELPSED_TIME = 0.3f;

    GameObject floor;

    [HideInInspector]
    public float elapsed = 0;
    [HideInInspector]
    public float lastX, lastZ;
    [HideInInspector]
    public float h, v;

    // Use this for initialization
	void Start () {
        floor = UnityEngine.GameObject.Find("Floor");
        h = 0;
        v = 0;
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0f, 0f, 300f, 300f), " Health: " + actor.health[0] + "/" + actor.health[1]);
        //GUI.Label(new Rect(0f, 10f, 300f, 300f), " Score: " + score);
    }

    void Update()
    {
        float x = transform.position.x % 1 + 0.02f, z = transform.position.z % 1 + 0.02f;
        GameObject[] walls = ((Maps)floor.GetComponent(typeof(Maps))).walls;
        int index;

        // Para que solo se mueva en una direccion...
        float va, ha, actualX, actualZ;

        ha = Mathf.Abs(h = Input.GetAxis("Horizontal")) - Random.RandomRange(0f, 0.01f);
        va = Mathf.Abs(v = Input.GetAxis("Vertical")) - Random.RandomRange(0f, 0.01f);

        if (va > ha)
            h = 0;
        else if (ha > va)
            v = 0;

        // Calculo la posicion actual
        if (Mathf.Abs(x) < 0.5f) x = 0f; else x = 1f * Mathf.Sign(x);
        if (Mathf.Abs(z) < 0.5f) z = 0f; else z = 1f * Mathf.Sign(z);

        actualX = transform.position.x - transform.position.x % 1 + x;
        actualZ = transform.position.z - transform.position.z % 1 + z;

        // Tiempo pasado desde que se coloco la ultima bomba
        elapsed += Time.deltaTime;

        index = ((int)actualZ + 14) * 30 + ((int)actualX + 14);

        // Si hay bombas disponibles, chequeo
        if (actor.bombCount > 0 && elapsed > BOMB_MIN_ELPSED_TIME && Input.GetKey(KeyCode.Space) && walls[index] == null)
        {
            // Creo una instancia del prefab
            GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Bombs/" + ((BombType)GameController.getBombType()).ToString(), typeof(GameObject)));
            Bomb bomb = go.GetComponent(typeof(Bomb)) as Bomb;

            // Actualizo el mapa con la bomba actual y le paso el indice de su posicion a la bomba
            walls[index] = bomb.gameObject;
            bomb.index = index;
          
            bomb.transform.position = new Vector3(actualX, 1.6f, actualZ);
            bomb.transform.parent = floor.transform;

            // Actualizo el contador
            actor.bombCount--;
            
            // Seteo quien coloco la bomba para restaurarle el contador
            bomb.deliverer = this.actor;
            // Seteo la referencia al mapa
            bomb.walls = walls;

            // Seteo la cantidad de celdas de la explosion de la bomba
            bomb.size = actor.bombSize;

            elapsed = 0;
        }

    }

	// Update is called once per frame
    void FixedUpdate()
    {
        // Posicion del Player
        rigidbody.velocity = Vector3.right * h * Time.deltaTime * actor.speed +
                             Vector3.forward * v * Time.deltaTime * actor.speed;
    }
}