using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    // Caracteristicas del Actor al que se le aplica el controlador
    public Actor actor;

    //unicamente en el player ya que el enemy es maneja solo
    void OnTriggerEnter(Collider other)
    {
        Damageable d = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;
        if (d != null)
        {
            actor.health[0] -= d.damage;
            actor.health[0] = Mathf.Max(0, actor.health[0]);

            if (actor.health[0] == 0)
            {
                GameController.gameOver();
            }
        }

        EnemyController e = other.gameObject.GetComponent(typeof(EnemyController)) as EnemyController;
        if (e != null)
            GameController.gameOver();
    }
}
