using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isDead = false;
    public float playerHealth = 10;
    CapsuleCollider playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    public void ApplyDammage(float EnnemyDammage){
        playerHealth -= EnnemyDammage;
        if (playerHealth <= 0){
            Dead();
        }
    }

    public void Dead(){
        isDead = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            ApplyDammage(10);
        }
        if (isDead == true){
            Destroy(gameObject);
        }
        
    }
}