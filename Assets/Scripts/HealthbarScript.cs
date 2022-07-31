using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour
{
    public Transform bar;

    Quaternion initRot;

    void Start()
    {
        initRot = transform.rotation;
    }


    public void SetSize(float SizeNormalized)
    {
        bar.localScale = new Vector3(SizeNormalized, 1);
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = initRot;

    }
}
