using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    int damage;

    public abstract void Shoot(Transform shootingPoint);
    public abstract void DontShoot();


}
