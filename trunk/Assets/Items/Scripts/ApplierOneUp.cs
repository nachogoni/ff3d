using UnityEngine;
using System.Collections;

public class ApplierOneUp : Applier {

    void Start()
    {
        actor.lifes++;
        showText("1 UP!");
    }
}
