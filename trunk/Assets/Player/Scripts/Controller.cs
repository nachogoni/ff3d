using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    // Caracteristicas del Actor al que se le aplica el controlador
    public Actor actor;

    void OnTriggerEnter(Collider other)
    {
        Damageable d = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;
        if (d != null)
        {
            actor.health[0] -= d.damage;
            actor.health[0] = Mathf.Max(0, actor.health[0]);

            if (actor.health[0] == 0)
            {
                gameover();
            }
        }

        EnemyController e = other.gameObject.GetComponent(typeof(EnemyController)) as EnemyController;
        if (e != null)
            gameover();
    }

    void gameover()
    {
        Application.LoadLevel(1);
    }
}
