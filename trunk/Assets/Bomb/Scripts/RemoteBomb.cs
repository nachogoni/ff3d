using UnityEngine;
using System.Collections;

public class RemoteBomb : Bomb
{

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(lastX, transform.position.y, lastZ);

        if (Input.GetKey(KeyCode.K))
        {
            explote();
        }
    }
}
