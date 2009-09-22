using UnityEngine;
using System.Collections;

enum crash_enum { NOT_CRASHED = 0, BEING_CRASHED, CRASHED };

public class EnemyController : Controller
{
    GameObject floor;
    float elapsed = 0;
    float h, v;

    //tiempo en tomar una descision
    public float decision_time = 1;
    //probabilidad de cambiar de lado
    const float change_side = 0.8f;
    //enum de estado de choque
    crash_enum crash;
    //score que da al matarlo
    public int score = 10;

    // Use this for initialization
    void Start()
    {
        floor = UnityEngine.GameObject.Find("Floor");
        h = 0;
        v = 1;
    }

    void change_movement(bool crashed)
    {
        float rand = Random.RandomRange(-1f, 1f);
        //si elijo cambiar de posicion
        if ((Mathf.Abs(rand) > change_side) || crashed)
        {
            if (h != 1)
            {
                v = 0;
                h = 1 * Mathf.Sign(rand);
            }
            else
            {
                v = 1 * Mathf.Sign(rand);
                h = 0;
            }
        }
    }

    void Update()
    {
        // tiempo desde el ultimo update
        elapsed += Time.deltaTime;

        //si estoy chocando no hago nada y cambio la direccion
        if (crash == crash_enum.CRASHED)
        {
            change_movement(true);
            crash = crash_enum.BEING_CRASHED;
            return;
        }

        // si es hora de tomar una desicion
        if ((elapsed >= decision_time) && (crash == crash_enum.NOT_CRASHED))
        {
            elapsed -= decision_time;

            change_movement(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Posicion del Player
        Vector3 movement = Vector3.zero;

        movement = Vector3.right * h * Time.deltaTime +
                   Vector3.forward * v * Time.deltaTime;

        rigidbody.transform.position += movement * 2;
    }

    void OnTriggerEnter(Collider other)
    {
        
        Wall wall = other.gameObject.GetComponent(typeof(Wall)) as Wall;
        Controller c = other.gameObject.GetComponent(typeof(Controller)) as Controller;
        Bomb b = other.gameObject.GetComponent(typeof(Bomb)) as Bomb;

        if ((wall != null) || (c != null) || (b != null))
        {
            crash = crash_enum.CRASHED;
            Vector3 reposition = Vector3.zero;

            if (v != 0)
                reposition = Vector3.forward * Mathf.Sign(v);
            else
                reposition = Vector3.right * Mathf.Sign(h);

            rigidbody.transform.position -= reposition * 0.08f;

            return;
        }

        Damageable d = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;
        if (d != null)
        {
            actor.health[0] -= d.damage;
            actor.health[0] = Mathf.Max(0, actor.health[0]);

            if (actor.health[0] == 0)
            {
                die();
            }
        }
    }

    void die()
    {
        //cargar el score
        GameController.enemyDie(score);
        //mato el gameobject
        Object.Destroy(actor.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        crash = crash_enum.NOT_CRASHED;
    }
}
