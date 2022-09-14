using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBoundary : MonoBehaviour
{



    [Header("Events")]
    public UnityEvent LevelFinishEnter;
    public UnityEvent LevelFinishExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelFinishEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LevelFinishExit.Invoke();
    }
}
