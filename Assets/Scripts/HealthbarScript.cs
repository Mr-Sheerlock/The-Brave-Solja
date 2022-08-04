using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour
{
    public Transform bar;

    Quaternion initRot;
    Vector2 initPositionDiff;
    Vector2 ParentPos;
    void Start()
    {
        initRot = transform.rotation;
        //get position relative to player
        Transform Parenttrans = transform.parent.transform;
        initPositionDiff = Parenttrans.position - transform.position;
    }


    public void SetSize(float SizeNormalized)
    {
        bar.localScale = new Vector3(SizeNormalized, 1);
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = initRot;
        ParentPos = transform.parent.transform.position;
        transform.position = ParentPos - initPositionDiff;
    }
}
