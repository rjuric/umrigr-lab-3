using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Startup
{
    [RuntimeInitializeOnLoadMethod]
    static void Start()
    {
        Screen.fullScreen = true;
    }

}
