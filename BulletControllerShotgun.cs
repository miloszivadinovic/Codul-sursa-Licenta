using UnityEngine;

public class BulletControllerShotgun : MonoBehaviour
{
    public int damageAmount = 100; // Amount of damage the bullet deals
    public ParticleSystem hitParticlePrefab; // Blood particle
    public ParticleSystem stoneHitParticlePrefab;
    public AudioClip[] ricochetSounds; // Array of ricochet sound clips

    private AudioSource audioSource;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ricochetSounds[0];
        audioSource.playOnAwake = false;
        playerTransform = GameObject.FindWithTag("Player").transform; // Find the player transform
        StartCoroutine(DestroyBulletDelayed(5f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hunter"))
        {
            // Trigger the hit particle effect for enemies at the collision point
            PlayHitParticleEffect(hitParticlePrefab, collision.contacts[0].point);

            // Apply damage to the enemy
            collision.gameObject.GetComponent<FollowPlayerAI>().TakeDamage(damageAmount);
        }

        if (collision.gameObject.CompareTag("Giant"))
        {
            // Trigger the hit particle effect for enemies at the collision point
            PlayHitParticleEffect(hitParticlePrefab, collision.contacts[0].point);

            // Apply damage to the enemy
            collision.gameObject.GetComponent<FollowPlayerAI>().TakeDamage(damageAmount);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Trigger the hit particle effect for enemies at the collision point
            PlayHitParticleEffect(hitParticlePrefab, collision.contacts[0].point);

            // Apply damage to the enemy
            collision.gameObject.GetComponent<FollowPlayerAI>().TakeDamage(damageAmount);
        }
        else if (collision.gameObject.CompareTag("Stone"))
        {
            // Trigger the hit particle effect for stones at the collision point
            PlayHitParticleEffect(stoneHitParticlePrefab, collision.contacts[0].point);
            int randomIndex = Random.Range(0, ricochetSounds.Length);
            audioSource.PlayOneShot(ricochetSounds[randomIndex]);
        }

        DestroyBullet();
    }

    // Coroutine to destroy the bullet after a delay
    private System.Collections.IEnumerator DestroyBulletDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check if the bullet still exists (not destroyed by a hit)
        if (gameObject != null)
        {
            DestroyBullet();
        }
    }

    // Destroy the bullet immediately
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    // Trigger the hit particle effect at the specified position and make it face the player
    private void PlayHitParticleEffect(ParticleSystem particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            // Calculate the rotation to face the player
            Vector3 directionToPlayer = playerTransform.position - position;
            Quaternion particleRotation = Quaternion.LookRotation(directionToPlayer);

            // Instantiate the hit particle effect with the calculated rotation
            ParticleSystem hitParticle = Instantiate(particlePrefab, position, particleRotation);

            // Play the particle effect
            hitParticle.Play();

            // Destroy the particle system after its duration
            Destroy(hitParticle.gameObject, hitParticle.main.duration);
        }
    }
}
