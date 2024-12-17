using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Damage : MonoBehaviour
{
    public Vector3 boxCastSize = new Vector3(1, 1, 1); // Default size of the box
    public Vector3 boxCastOffset = new Vector3(0, 1, 1); // Offset from the character position
    public Vector3 boxCastCenter = new Vector3(0, 0, 1); // Center of the BoxCast relative to the offset
    public float boxCastDistance = 1.0f; // Distance the box cast should extend

    public float m_MaxDistance = 1.0f;
    public float m_Speed = 20.0f;
    private bool m_HitDetect;
    private Collider m_Collider;
    private RaycastHit m_Hit;

    void Start()
    {
        m_Collider = GetComponent<Collider>();
        
    }

    void Update()
    {
        // Simple movement in x and z axes
        float xAxis = Input.GetAxis("Horizontal") * m_Speed;
        float zAxis = Input.GetAxis("Vertical") * m_Speed;
        transform.Translate(new Vector3(xAxis, 5, zAxis));
    }

    void FixedUpdate()
    {
        // Test to see if there is a hit using a BoxCast
        Vector3 boxCastOrigin = transform.position + boxCastOffset + boxCastCenter;
        m_HitDetect = Physics.BoxCast(boxCastOrigin, boxCastSize * 0.5f, transform.forward, out m_Hit, transform.rotation, boxCastDistance);
        if (m_HitDetect)
        {
            if(m_Hit.collider.CompareTag("Enemy"))
            {
                // Output the name of the Collider your Box hit
             
                
            }
            
        }
    }

    public void ApplyDamage(GameObject enemy)
    {
        // Assuming the enemy has a component that handles damage (e.g., an EnemyHealth script)
        EnnemyAI enemyHealth = enemy.GetComponent<EnnemyAI>();
        if (enemyHealth != null)
        {
            // Apply damage to the enemy
            enemyHealth.TakeDamage(10); // Damage amount (adjust as needed)
        }
        else
        {
            Debug.LogWarning("No EnemyHealth component found on " + enemy.name);
        }
    }

    // Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Calculate the box cast origin
        Vector3 boxCastOrigin = transform.position + boxCastOffset + boxCastCenter;

        // Check if there has been a hit yet
        if (m_HitDetect)
        {
            // Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(boxCastOrigin, transform.forward * m_Hit.distance);
            // Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(boxCastOrigin + transform.forward * m_Hit.distance, boxCastSize);
        }
        // If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            // Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(boxCastOrigin, transform.forward * boxCastDistance);
            // Draw a cube at the maximum distance
            Gizmos.DrawWireCube(boxCastOrigin + transform.forward * boxCastDistance, boxCastSize);
        }
    }
}
