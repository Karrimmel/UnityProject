using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{

    public Transform Target;
    public float Distance;
    public float attackRange = 2.2f;
 
    // Cooldown des attaques
    public float attackRepeatTime = 1;
    private float attackTime;

    //private Animation animations;
 
    // Montant des dégâts infligés
    public float TheDammage = 10;
    public UnityEngine.AI.NavMeshAgent agent;

    public float enemyHealth;
    private bool isDead = false;

    public int hp = 3;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        attackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   

        handleAttack();
        if(!isDead){
            if (Target != null)
            {
                agent.destination = Target.position;
                Distance = Vector3.Distance(Target.position, transform.position);
                if (Distance < attackRange)
                {
                    attack();
                }
            }
        }
        

    }

    public void handleAttack()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining health: " + enemyHealth);

        if (enemyHealth <= 0)
        {
            
            Die();
        }
    }

    private void Die()
    {
        isDead = true; // Empêche l'ennemi d'agir après sa mort
        Debug.Log(gameObject.name + " est mort !");
        Destroy(this.gameObject); // Détruit l'ennemi
    }


    void attack(){
         agent.destination = transform.position;
         if (Time.time > attackTime) {
            //animations.Play("hit");
            Target.GetComponent<Player>().ApplyDammage(TheDammage);
            Debug.Log("L'ennemi a envoyé " + TheDammage + " points de dégâts");
            attackTime = Time.time + attackRepeatTime;
        }
    }
    private void OnCollisionEnter(Collision collision){
            hp--;
            if (hp <=0)
                Destroy(this.gameObject);
        }
}