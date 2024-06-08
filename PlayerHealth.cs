using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthText2; //text for Health:
    public Image healthImage;
    public GameObject medkit;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killsText;
    public Image skullImg;

    public Image GameOverImg;
    public TextMeshProUGUI GameOverText;


    

    //Sound
    public AudioClip healSoundClip;
    public GameObject MedkitSound;
    public AudioSource healSound;

    void Start()
    {
        healSound = MedkitSound.GetComponent<AudioSource>();

        currentHealth = maxHealth;
        UpdateHealthText();
        UpdateHealthImage();

    }

    void Update()
    {
        if (currentHealth <= 50 && !medkit.activeSelf)
        {
            medkit.SetActive(true);
        }
        else if (currentHealth > 50 && medkit.activeSelf)
        {
            medkit.SetActive(false);
        }

        if (currentHealth <=0 )
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20); 
        }

        if (collision.gameObject.CompareTag("Hunter"))
        {
            TakeDamage(10); 
        }

        if (collision.gameObject.CompareTag("Giant"))
        {
            TakeDamage(50); 
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Medkit"))
        {
            Heal(50);
            medkit.SetActive(false);
            healSound.PlayOneShot(healSoundClip);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Additional logic can be added here, such as checking if the player should die

       

        // Update za tekst i sliku
        UpdateHealthText();
        UpdateHealthImage();

    }

    void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthText(); //update for text and image (red overlay)
        UpdateHealthImage();
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = " " + currentHealth;
        }
    }

    void UpdateHealthImage()
    {
        // Update the alpha of the healthImage based on current health
        if (healthImage != null)
        {
            float alpha = 1.0f - ((float)currentHealth / maxHealth); // Calculate alpha as health decreases
            alpha = Mathf.Clamp(alpha, 0, 1); // Ensure alpha is within 0 to 1 range
            Color color = healthImage.color;
            color.a = alpha;
            healthImage.color = color;
        }
    }
}