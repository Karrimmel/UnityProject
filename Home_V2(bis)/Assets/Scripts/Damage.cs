using UnityEngine;

public class Damage : MonoBehaviour
{
    public Vector3 boxCastSize = new Vector3(1, 1, 1);
    public Vector3 boxCastOffset = new Vector3(0, 1, 1);
    public Vector3 boxCastCenter = new Vector3(0, 0, 1);
     public Vector3 boxCastRotation = Vector3.zero;
    
    public float boxCastDistance = 1.0f;
    public bool enemyCollision = false;
    public bool playerCollision = false;
    public Player playerScript;
    public EnnemyAI enemyScript;

    public Collider detectedEnemy; // Store the detected enemy
    public Collider detectedPlayer;
    void FixedUpdate()
    {
        // Calculate the box cast origin
        Vector3 boxCastOrigin = transform.position + transform.rotation * (boxCastOffset + boxCastCenter);

        // Calculate the box rotation using the boxCastRotation in local space
        Quaternion boxRotation = transform.rotation * Quaternion.Euler(boxCastRotation);

        // Perform the BoxCast
        if (Physics.BoxCast(boxCastOrigin, boxCastSize * 0.5f, transform.forward, out RaycastHit hit, boxRotation, boxCastDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Store the detected enemy
                detectedEnemy = hit.collider;
                enemyCollision = true;
            }
            else
            {
                enemyCollision = false;
                detectedEnemy = null; // Clear if no valid enemy detected
            }

            if (hit.collider.CompareTag("Player"))
            {
                // Store the detected player
                detectedPlayer = hit.collider;
                playerCollision = true;
            }
            else
            {
                playerCollision = false;
                detectedPlayer = null; // Clear if no valid player detected
            }
        }
        else
        {
            enemyCollision = false;
            detectedEnemy = null;

            playerCollision = false;
            detectedPlayer = null;
        }
    }

    public void TriggerDamageOnEnemy()
    {
        playerScript = GetComponent<Player>();
        // Apply damage only if an enemy is detected
        if (detectedEnemy != null)
        {
            EnnemyAI enemyAI = detectedEnemy.GetComponent<EnnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(playerScript.playerAttackDamage); // Adjust damage amount as needed
                Debug.Log($"Enemy {detectedEnemy.name} took damage!");
            }
            else
            {
                Debug.LogWarning("Detected enemy does not have an EnnemyAI component.");
            }
        }
        else
        {
            Debug.Log("No enemy detected to apply damage.");
        }
        }

        public void TriggerDamageOnPlayer()
        {
        enemyScript= GetComponent<EnnemyAI>();
        Debug.Log("player",detectedPlayer);
        // Apply damage only if an enemy is detected
        if (detectedPlayer != null)
        {
            Player player = detectedPlayer.GetComponent<Player>();
            if (player != null)
            {
                player.ApplyDammage(enemyScript.enemyAttackDamage); // Adjust damage amount as needed
                Debug.Log($"Enemy {detectedPlayer.name} took damage!");
            }
            else
            {
                Debug.LogWarning("Detected enemy does not have an EnnemyAI component.");
            }
        }
        else
        {
            Debug.Log("No enemy detected to apply damage.");
        }
        }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Calculate the box cast origin
        Vector3 boxCastOrigin = transform.position + (boxCastOffset + boxCastCenter);

        // Calculate the box rotation using the boxCastRotation in local space
        Quaternion boxRotation = transform.rotation * Quaternion.Euler(boxCastRotation);

        // Draw the BoxCast visualization
        if (Physics.BoxCast(boxCastOrigin, boxCastSize * 0.5f, transform.forward, out RaycastHit hit, boxRotation, boxCastDistance))
        {
            Gizmos.DrawRay(boxCastOrigin, transform.forward * hit.distance);
            Gizmos.matrix = Matrix4x4.TRS(boxCastOrigin + transform.forward, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxCastSize);
        }
        else
        {
            Gizmos.DrawRay(boxCastOrigin, transform.forward * boxCastDistance);
            Gizmos.matrix = Matrix4x4.TRS(boxCastOrigin + transform.forward * boxCastDistance, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxCastSize);
        }
    }
}