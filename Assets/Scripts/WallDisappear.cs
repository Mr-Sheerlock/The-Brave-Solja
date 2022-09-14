using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisappear : MonoBehaviour
{
    [SerializeField] bool Condition1=false,Condition2=false;

    public void SetTCondition1()
    {
        Condition1 = true;
        Disappear();
    }

    public void SetTCondition2()
    {
        Condition2 = true;
        Disappear();
    }
    public void Disappear()
    {
        if (Condition1 && Condition2)
            Destroy(gameObject);
    }

}
