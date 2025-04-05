using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDodge : MonoBehaviour
{
    public float dodgeDistance = 3f;
    public float dodgeCooldown = 3f;
    public float dodgeDuration = 0.2f;

    private CharacterController controller;
    private Vector3 dodgeDirection;
    private float dodgeTimer = 0f;
    private bool isDodging = false;
    private float cooldownTimer = 0f;

    private GreyHealth playerHealth; // replace with your health script name
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<GreyHealth>(); // update class name accordingly
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0 && !isDodging)
        {
            Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized;
            if (inputDir != Vector3.zero)
            {
                dodgeDirection = inputDir;
                isDodging = true;
                dodgeTimer = dodgeDuration;
                cooldownTimer = dodgeCooldown;

                if (playerHealth != null)
                    playerHealth.canBeDamaged = false; // Activate I-frames
                    Debug.Log("iframes on");
            }
        }

        if (isDodging)
        {
            controller.Move(dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0)
            {
                isDodging = false;
                if (playerHealth != null)
                    playerHealth.canBeDamaged = true; // Turn off I-frames
                    Debug.Log("iframes off");
            }
        }
    }

    public bool IsDodging()
    {
        return isDodging;
    }
}