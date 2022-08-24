using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] float Timealive=1f;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Destroyit());

    }

    IEnumerator Destroyit()
    {
        yield return new WaitForSeconds(Timealive);
        Destroy(gameObject);
    }
}
