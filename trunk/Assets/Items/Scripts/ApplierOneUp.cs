using UnityEngine;
using System.Collections;

public class ApplierOneUp : Applier {

    void Start()
    {
        actor.lifes++;
        showText("1 UP!");
    }

    // Update is called once per frame
    //Descomentar para que el size vuelva a su estado normal dsp de un tiempo

    void Update()
    {
        time += Time.deltaTime;
        if (time > maxTime)
        {
            time = 0;
            End();
            Object.Destroy(this);
        }
    }

    void End()
    {
        
    }
}
