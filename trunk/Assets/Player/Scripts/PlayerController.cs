using UnityEngine;
using System.Collections;

public class PlayerController : Controller
{
    // Maxima velocidad inicial en alguno de los ejes
    const float BOMB_INIT_VEL = 5;
    // Maxima distancia inicial al Player hacia arriba
    const float BOMB_INIT_POS = 1;

    //Tiempo minimo de keypress
    public const float KEYPRESS_MIN_ELPSED_TIME = 0.15f;
    //Tiempo minimo de bomba
    public const float BOMB_MIN_ELPSED_TIME = 0.3f;

    float keyPressElapsed = 0;

    void OnGUI()
    {
        GUI.Label(new Rect(0f, 30f, 300f, 300f), " Weapon: " + actor.bomb.ToString() + " (" + actor.bombCount[(int)actor.bomb] + ")" );
    }

    void Update()
    {
        GameObject[] walls = ((Maps)floor.GetComponent(typeof(Maps))).walls;
        int index;
        float x = transform.position.x % 1 + 0.02f, z = transform.position.z % 1 + 0.02f;

        // Para que solo se mueva en una direccion...
        float va, ha, actualX, actualZ;

        ha = Mathf.Abs(h = Input.GetAxis("Horizontal")) - Random.RandomRange(0f, 0.01f);
        va = Mathf.Abs(v = Input.GetAxis("Vertical")) - Random.RandomRange(0f, 0.01f);

        if (va > ha)
            h = 0;
        else if (ha > va)
            v = 0;

        actualX = transform.position.x - transform.position.x % 1 + x;
        actualZ = transform.position.z - transform.position.z % 1 + z;

        index = ((int)actualZ + 14) * 30 + ((int)actualX + 14);

        // Tiempo pasado desde que se coloco la ultima bomba
        elapsed += Time.deltaTime;

        // tiempo que paso desde que presiono una tecla
        keyPressElapsed += Time.deltaTime;

        

        if (keyPressElapsed > KEYPRESS_MIN_ELPSED_TIME)
        {
            if (Input.GetKey(KeyCode.E))
            {
                switchWeapon(1);
            }else if (Input.GetKey(KeyCode.Q))
            {
                switchWeapon(-1);
            }
            // Si hay bombas disponibles, chequeo
            //if (actor.bombCount > 0 && elapsed > BOMB_MIN_ELPSED_TIME && Input.GetKey(KeyCode.Space) && walls[index] == null)
            else if (actor.bombCount[(int)actor.bomb] > 0 && elapsed > BOMB_MIN_ELPSED_TIME && Input.GetKey(KeyCode.Space))
            {
                //planto la bomba
                plantBomb();
                //la descuento
                actor.bombCount[(int)actor.bomb] -= 1;
                elapsed = 0;
            }
            keyPressElapsed -= KEYPRESS_MIN_ELPSED_TIME;
        }

    }

    void switchWeapon(int i)
    {
        //muevo el arma
        actor.bomb = (BombType)(((int)actor.bomb + i) % 3);
    }

	// Update is called once per frame
    void FixedUpdate()
    {
        // Posicion del Player
        rigidbody.velocity = Vector3.right * h * Time.deltaTime * actor.speed +
                             Vector3.forward * v * Time.deltaTime * actor.speed;
    }
}