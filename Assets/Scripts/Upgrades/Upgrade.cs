using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public SpriteRenderer Sr;
    public CircleCollider2D collider;
    static protected GameObject Player;
    static protected MainController mainController;
    static protected Health PlayerHealth;
    private void Awake()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            mainController = Player.GetComponent<MainController>();
            PlayerHealth = Player.GetComponent<Health>();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
