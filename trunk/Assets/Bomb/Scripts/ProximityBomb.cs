using UnityEngine;
using System.Collections;

public class ProximityBomb : Bomb {

    bool active = false;

    // Update is called once per frame
    void Update () {
        /*((Bomb)GetComponentInChildren(typeof(Bomb))).*/transform.position = new Vector3(lastX, transform.position.y, lastZ);

        Debug.Log(lastX + " - " + lastZ);

        time -= Time.deltaTime;
        if ((time < 0) && !active)
        {
            Debug.Log("Bomba activada");
            active=true;
        }

        if ((Input.GetKey(KeyCode.K)) && active)
        {
            ((Bomb)GetComponentInChildren(typeof(Bomb))).explote();
        }
    }

        void OnTriggerEnter(Collider other)
    {
        if (active)
            explote();
    }
}
