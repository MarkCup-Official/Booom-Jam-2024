using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticAdapter : MonoBehaviour
{
    private void Start()
    {
        SimpleTouchController touchController = GetComponent<SimpleTouchController>();

        InputMgr.touchController = touchController;
    }
}
