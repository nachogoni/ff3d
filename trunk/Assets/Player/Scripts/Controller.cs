using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    // Caracteristicas del Actor al que se le aplica el controlador
    public Actor actor;

    // Minimo tiempo que tiene que haber pasado para poder poner otra bomba
    public const float BOMB_MIN_ELPSED_TIME = 0.3f;

    [HideInInspector]
    public GameObject floor;
    [HideInInspector]
    public float elapsed = 0;
    [HideInInspector]
    public float lastX, lastZ;
    [HideInInspector]
    public float h, v;

    // Use this for initialization
    void Start()
    {
        floor = UnityEngine.GameObject.Find("Floor");
        h = 0;
        v = 1;
    }

    //unicamente en el player ya que el enemy es maneja solo
    void OnTriggerEnter(Collider other)
    {
        Damageable d = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;
        if (d != null)
        {
            //Si tiene godMode no aplica
            if (actor.health[1] != 0)
            {
                actor.health[0] -= d.damage;
                actor.health[0] = Mathf.Max(0, actor.health[0]);

                if (actor.health[0] == 0)
                {
                    GameController.gameOver();
                }
            }
        }

        EnemyController e = other.gameObject.GetComponent(typeof(EnemyController)) as EnemyController;
        //Si tiene godMode no aplica
        if ((e != null) && (actor.health[1] != 0))
            GameController.gameOver();

        Item item = other.gameObject.GetComponent(typeof(Item)) as Item;
        if (item != null)
        {
            Applier app = gameObject.AddComponent(item.itemType.ToString()) as Applier;
            //actor
            app.actor = actor;
            //valor
            app.value = item.value;
            //tiempo
            app.maxTime = item.time;
            Object.Destroy(item.gameObject);
        }
    }

    public void plantBomb()
    {
        GameObject[] walls = ((Maps)floor.GetComponent(typeof(Maps))).walls;
        int index;
        float x = transform.position.x % 1 + 0.02f, z = transform.position.z % 1 + 0.02f;
        float actualX, actualZ;

        // Calculo la posicion actual
        if (Mathf.Abs(x) < 0.5f) x = 0f; else x = 1f * Mathf.Sign(x);
        if (Mathf.Abs(z) < 0.5f) z = 0f; else z = 1f * Mathf.Sign(z);

        actualX = transform.position.x - transform.position.x % 1 + x;
        actualZ = transform.position.z - transform.position.z % 1 + z;

        // Creo una instancia del prefab
        index = ((int)actualZ + 14) * 30 + ((int)actualX + 14);

        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Bombs/" + (actor.bomb).ToString(), typeof(GameObject)));
        Bomb bomb = go.GetComponent(typeof(Bomb)) as Bomb;

        // Actualizo el mapa con la bomba actual y le paso el indice de su posicion a la bomba
        walls[index] = bomb.gameObject;
        bomb.index = index;

        bomb.transform.position = new Vector3(actualX, 1.6f, actualZ);
        bomb.transform.parent = floor.transform;

        // Actualizo el contador
        //actor.bombCount--;

        // Seteo quien coloco la bomba para restaurarle el contador
        bomb.deliverer = this.actor;
        // Seteo la referencia al mapa
        bomb.walls = walls;

        // Seteo la cantidad de celdas de la explosion de la bomba
        bomb.size = actor.bombSize;
    }
}
