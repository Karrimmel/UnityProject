using UnityEngine;

public class Damage : MonoBehaviour
{
    public Vector3 boxCastSize = new Vector3(1, 1, 1);
    public Vector3 boxCastOffset = new Vector3(0, 1, 1);
    public Vector3 boxCastCenter = new Vector3(0, 0, 1);
    public float boxCastDistance = 1.0f;
    public bool enemyCollision = false;
    private Player playerScript;

    private Collider detectedEnemy; // Store the detected enemy

    void FixedUpdate()
    {
        // Calculate the box cast origin
        Vector3 boxCastOrigin = transform.position + boxCastOffset + boxCastCenter;

        // Perform the BoxCast
        if (Physics.BoxCast(boxCastOrigin, boxCastSize * 0.5f, transform.forward, out RaycastHit hit, transform.rotation, boxCastDistance))
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
        }
        else
        {
            detectedEnemy = null; // Clear if no hit
        }
    }

    public void TriggerDamage()
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Calculate the box cast origin
        Vector3 boxCastOrigin = transform.position + boxCastOffset + boxCastCenter;

        // Draw the BoxCast visualization
        if (Physics.BoxCast(boxCastOrigin, boxCastSize * 0.5f, transform.forward, out RaycastHit hit, transform.rotation, boxCastDistance))
        {
            Gizmos.DrawRay(boxCastOrigin, transform.forward * hit.distance);
            Gizmos.DrawWireCube(boxCastOrigin + transform.forward * hit.distance, boxCastSize);
        }
        else
        {
            Gizmos.DrawRay(boxCastOrigin, transform.forward * boxCastDistance);
            Gizmos.DrawWireCube(boxCastOrigin + transform.forward * boxCastDistance, boxCastSize);
        }
    }
}
