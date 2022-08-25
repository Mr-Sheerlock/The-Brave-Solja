using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    [SerializeField] Transform[] Positions;

    #region Control
    [SerializeField] int increment = 1;
    [SerializeField] int i = 0;
    [SerializeField] int speed = 5;
    Vector2 MovingDirection;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position- Positions[i].position).magnitude<1.1)
        {
            if (i == Positions.Length - 1)
            {
                i = -1;
            }

            i += increment;
        }
        else
        {
            //Vector3.Lerp(transform.position, Positions[i].position, AnimationTime_s * Time.deltaTime);
            Move(Positions[i].position);
        }
    }

    void Move(Vector2 newPosition)
    {
        MovingDirection = newPosition - (Vector2)transform.position;
        transform.position += (Vector3)MovingDirection.normalized * speed*Time.deltaTime;
    }
}
