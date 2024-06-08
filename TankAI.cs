using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{

    public Transform player; // Assign the player's transform in the Unity Inspector
    public float rotationSpeed = 1f;
    private Quaternion initialRotation;
    public float shootingRange = 1000f;

    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootingInterval = 10f;
    public float projectileForce = 500f;

    public GameObject TurretExplosiontPrefab;

    private AudioSource Shotsound;

    private float shootTimer = 0f;

    public int maxHealth = 500;
    private int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
        Shotsound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation to face the player's position
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * initialRotation, Time.deltaTime * rotationSpeed);

        // Update the shoot timer
        shootTimer += Time.deltaTime;

        // Check if it's time to shoot
        if (shootTimer >= shootingInterval)
        {
            Shoot();
            // Reset the timer
            shootTimer = 0f;
        }


    }



    // Shooting logic
    private void Shoot()
    {
        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Get the Rigidbody component of the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Check if the projectile has a Rigidbody
        if (projectileRb != null)
        {
            // Apply force to the projectile
            projectileRb.AddForce(projectile.transform.forward * projectileForce, ForceMode.Impulse);
        }
        PlayShootSound();
        InstantiateParticleEffect();

    }

    private void InstantiateParticleEffect()
    {
        // Check if a particle effect prefab is assigned
        if (TurretExplosiontPrefab != null)
        {
            // Instantiate the particle effect at the shoot point
            GameObject explosion = Instantiate(TurretExplosiontPrefab, shootPoint.position, shootPoint.rotation);

            // Get the ParticleSystem component
            ParticleSystem explosionParticleSystem = explosion.GetComponent<ParticleSystem>();

            // Check if the ParticleSystem component is present
            if (explosionParticleSystem != null)
            {
                // Destroy the particle effect after its duration
                Destroy(explosion, explosionParticleSystem.main.duration);
            }
            else
            {
                // If there's no ParticleSystem component, destroy the game object immediately
                Destroy(explosion);
            }
        }
    }

    private void PlayShootSound()
    {
            Shotsound.PlayOneShot(Shotsound.clip);
    }
}
