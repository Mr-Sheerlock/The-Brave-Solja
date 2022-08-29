using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossMusic : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnEnter;
    bool done=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!done)
        {
            OnEnter.Invoke();
            done = true;
        }
    }

}
