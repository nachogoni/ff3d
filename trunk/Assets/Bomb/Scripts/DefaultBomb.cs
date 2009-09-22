using UnityEngine;
using System.Collections;

public class DefaultBomb : Bomb {

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(lastX, transform.position.y, lastZ);

        time -= Time.deltaTime;
        if (time < 0)
        {
            explote();
        }
	}
}
