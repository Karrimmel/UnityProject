using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{

    public Transform Target;
    public float Distance;
    public Animator animator;
    public float attackRange = 2.2f;
 
    // Cooldown des attaques
    public float attackRepeatTime = 1;
    private float attackTime;
 
    // Montant des dégâts infligés
    public int enemyAttackDamage;
    public UnityEngine.AI.NavMeshAgent agent;

    public float enemyHealth;
    private bool isDead = false;
    public int isAttackingHash;
    public Damage damageScript;

    public int hp = 3;
    // Start is called before the first frame update
    void Start()
    {
        damageScript = GetComponent<Damage>();
        animator = GetComponentInChildren<Animator>();
       
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        attackTime = Time.time;
        isAttackingHash = Animator.StringToHash("isAttacking");
    }

    // Update is called once per frame
    void Update()
    {   
      
        bool isAttacking = animator.GetBool(isAttackingHash);
        if(!isDead){
            if (Target != null)
            {
                agent.destination = Target.position;
                if(damageScript.playerCollision){
                    if(!isAttacking){
                        animator.SetBool(isAttackingHash, true);
                        damageScript.TriggerDamageOnPlayer();
                    }
                }

                if(!damageScript.playerCollision){
                    animator.SetBool(isAttackingHash, false);
                }
                //agent.destination = Target.position;
                // if(!isAttacking && damageScript.playerCollision)
                // {
                //     animator.SetBool(isAttackingHash, true);
                //     attack();
                // }
                
                // if(isAttacking && !damageScript.playerCollision){
                //     animator.SetBool(isAttackingHash, false);
                // }
                // Distance = Vector3.Distance(Target.position, transform.position);
                // if (Distance < attackRange)
                // {
                //     attack();
                // }
            }
        }
        

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
        Target.GetComponent<Player>().ApplyDammage(enemyAttackDamage);
        //  agent.destination = transform.position;
        //  if (Time.time > attackTime) {
        //     //animations.Play("hit");
        //     Target.GetComponent<Player>().ApplyDammage(enemyAttackDamage);
        //     Debug.Log("L'ennemi a envoyé " + enemyAttackDamage + " points de dégâts");
        //     attackTime = Time.time + attackRepeatTime;
        // }
    }
    private void OnCollisionEnter(Collision collision){
            hp--;
            if (hp <=0)
                Destroy(this.gameObject);
        }
}