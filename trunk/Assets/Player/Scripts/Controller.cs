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
            Debug.Log("toco un item");
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
}
