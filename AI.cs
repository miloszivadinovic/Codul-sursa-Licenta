using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerAI : MonoBehaviour
{
    public Transform player;
    public float aiSpeed = 5f; // Adjust the speed in the inspector
    public int maxHealth = 100;
    private int currentHealth;
    private AudioSource audioSource;
    private Animator animator; // Reference to the Animator component

    public AudioClip[] enemySounds; // Array to hold the specific sounds for this enemy
    public float minTimeBetweenSounds = 7f; // Minimum time interval between sounds
    public float maxTimeBetweenSounds = 14f; // Maximum time interval between sounds
    private float nextSoundTime;

    private NavMeshAgent agent;
    private Rigidbody[] ragdollBodies; // Array to store all the rigidbodies in the ragdoll
    private Collider[] ragdollColliders; // Array to store all the colliders in the ragdoll
    private bool isDead = false; // Track if the enemy is dead

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = aiSpeed; // Set the speed initially
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>(); // Get the Animator component

        // Initialize ragdoll components
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Disable ragdoll physics at the start
        SetRagdollState(false);

        // Start playing random sounds
        ScheduleNextSound();
    }

    void Update()
    {
        if (player != null && agent.enabled && !isDead)
        {
            SetDestinationToPlayer();
        }

        // Check if it's time to play a random sound
        if (Time.time >= nextSoundTime)
        {
            PlayRandomSound();
            ScheduleNextSound();
        }
    }

    void SetDestinationToPlayer()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Reduce health by the damage amount
        currentHealth -= damageAmount;

        // Check if the enemy should be destroyed
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Mark the enemy as dead
        KillCount.IncreaseKillCount();

        // Transition to ragdoll mode
        if (animator != null)
        {
            animator.enabled = false; // Disable the Animator component
        }

        if (agent != null)
        {
            agent.enabled = false; // Disable the NavMeshAgent component
        }

        SetRagdollState(true);

        // After a short delay, make the ragdoll kinematic to stop further interactions
        Invoke("MakeRagdollStatic", 2f); // Adjust the delay as needed
    }

    void SetRagdollState(bool state)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state; // Enable/disable physics
            if (!state)
            {
                rb.velocity = Vector3.zero; // Stop any current movement
                rb.angularVelocity = Vector3.zero; // Stop any current rotation
            }
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                col.enabled = state; // Enable/disable colliders
            }
            else
            {
                col.enabled = false; // Disable collider for this GameObject (enemy)
            }
        }

        // If there's a Rigidbody on the root, handle it
        Rigidbody rootRigidbody = GetComponent<Rigidbody>();
        if (rootRigidbody != null)
        {
            rootRigidbody.isKinematic = state;
            if (!state)
            {
                rootRigidbody.velocity = Vector3.zero;
                rootRigidbody.angularVelocity = Vector3.zero;
            }
        }

        // If there's a Collider on the root, handle it
        Collider rootCollider = GetComponent<Collider>();
        if (rootCollider != null)
        {
            rootCollider.enabled = !state;
        }
    }

    void MakeRagdollStatic()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true; // Make all ragdoll parts kinematic
        }
    }

    private void PlayRandomSound()
    {
        if (enemySounds.Length == 0 || audioSource == null) return;

        int randomIndex = Random.Range(0, enemySounds.Length);
        audioSource.clip = enemySounds[randomIndex];
        audioSource.Play();
    }

    private void ScheduleNextSound()
    {
        nextSoundTime = Time.time + Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }
}
