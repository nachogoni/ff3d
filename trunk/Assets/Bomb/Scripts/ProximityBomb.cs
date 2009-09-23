using UnityEngine;
using System.Collections;

public class ProximityBomb : Bomb {

    bool active = false;

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(lastX, transform.position.y, lastZ);

        time -= Time.deltaTime;
        if ((time < 0) && !active)
        {
            showText("Activated!");
            active=true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            ((Bomb)transform.parent.GetComponent(typeof(Bomb))).explote();
            
        }
    }
}
