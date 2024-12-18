using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    private float timeSinceFirstClick = 0f;
    private float attackComboDelay = 0.5f;
    public float PlayerHealth;
    private int isWalkingHash;
    private int isRunningHash;
    private int is1stAttackHash;
    private int is2ndAttackHash;
    private Damage damageScript;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        damageScript = GetComponent<Damage>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }


        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        is1stAttackHash = Animator.StringToHash("is1stAttack");
        is2ndAttackHash = Animator.StringToHash("is2ndAttack");

    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        // Movement input
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Rotate to face movement direction
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Walking and running animations
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool isRunning = animator.GetBool(isRunningHash);

        if (!isWalking && move.magnitude > 0)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (move.magnitude == 0)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (isShiftPressed && !isRunning && playerSpeed != 5.0f)
        {
            playerSpeed = 5.0f;
            animator.SetBool(isRunningHash, true);
        }
        else if (!isShiftPressed && isRunning && playerSpeed != 2.0f)
        {
            playerSpeed = 2.0f;
            animator.SetBool(isRunningHash, false);
        }
    }

    private void HandleAttack()
{
    bool isLeftClickPressed = Input.GetMouseButtonDown(0); // Detect mouse click
    bool is1stAttack = animator.GetBool(is1stAttackHash);
    bool is2ndAttack = animator.GetBool(is2ndAttackHash);

    if (isLeftClickPressed)
    {
        if (!is1stAttack && damageScript.enemyCollision)
        {
            // Trigger first attack
            animator.SetBool(is1stAttackHash, true);
            timeSinceFirstClick = 0f;

            //Trigger damage
            damageScript.TriggerDamage();

        }
        else if (is1stAttack && timeSinceFirstClick < attackComboDelay && !is2ndAttack)
        {
            // Trigger second attack in combo
            animator.SetBool(is2ndAttackHash, true);

            // Trigger damage
            damageScript.TriggerDamage();
        }
    }

    // Track combo timing
    if (is1stAttack)
    {
        timeSinceFirstClick += Time.deltaTime;
        if (timeSinceFirstClick >= attackComboDelay)
        {
            // Reset combo if time exceeds delay
            animator.SetBool(is1stAttackHash, false);
            animator.SetBool(is2ndAttackHash, false);
        }
    }
}

}
