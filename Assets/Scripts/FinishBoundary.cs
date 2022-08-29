using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishBoundary : MonoBehaviour
{



    [Header("Events")]
    public UnityEvent LevelFinish;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelFinish.Invoke();
    }
}
