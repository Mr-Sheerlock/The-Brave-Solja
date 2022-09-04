using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{
    SpriteRenderer Sr;

    bool colided=false;
    // Start is called before the first frame update
    void Start()
    {
        Sr=GetComponent<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (colided)
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        colided=false;
        Color c = Sr.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.05f)
        {
            c.a = alpha;
            Sr.color = c;
            yield return new WaitForSeconds(.05f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colided=true;
    }
}
