using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectile : MonoBehaviour
{

    public GameObject ShellExplosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(ShellExplosionPrefab, transform.position, Quaternion.identity);
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(100);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(ShellExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
