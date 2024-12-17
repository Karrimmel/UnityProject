using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10; // Dégâts infligés

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet touché a un script de type "Enemy"
        EnnemyAI enemy = other.GetComponent<EnnemyAI>();
        if (enemy != null)
        {
            Debug.Log(enemy.name + " has " + enemy.hp);
            enemy.TakeDamage(damageAmount); // Inflige les dégâts
        }
    }
}
