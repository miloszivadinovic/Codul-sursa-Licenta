using System.Collections;
using UnityEngine;
using TMPro;

public class Beretta : MonoBehaviour
{
    public GameObject[] weapons; // Array which has the number of every weapon
    private int currentWeaponIndex = 0; // Index of the current active weapon

    public GameObject bulletPrefab;
    public GameObject bulletPrefabShotgun;
    public Transform firePoint;
    public float bulletSpeed = 500f;
    public float fireRate = 0f;
    public float fireRateBeretta = 2f;
    public float fireRateMac = 6f;
    public float fireRateShotgun = 0.1f;

    private float[] nextFireTimes;

    public AudioClip shotSoundColt;
    public AudioClip shotSoundMac;
    public AudioClip shotSoundShotgun;

    private AudioSource audioSource;

    public GameObject gunLightObject;
    public GameObject gunLightObjectShotgun;

    public CameraShake cameraShake;

    private Animator w_animator;
    public Animator mac10Animator;
    public Animator shotgunAnimator;

    public GameObject colt;
    public GameObject mac10;
    public GameObject shotgun;

    //gets enabled when time reaches 40 seconds
    public GameObject WeaponUnlockSound;
    public GameObject WeaponUnlockSound2;

    private float weaponUnlockTimeMac10;
    private float weaponUnlockTimeShotgun;

    private Recoil Recoil_Script;

    private AudioSource weaponSwitching;
    private AudioClip weaponSwitchingClip;

    // Shotgun spread parameters
    public int minShotgunPellets = 5;
    public int maxShotgunPellets = 8;
    public float spreadAngle = 10f;

    void Start()
    {
        weaponUnlockTimeMac10 = Time.time;
        weaponUnlockTimeShotgun = Time.time;
        GameObject coltObject = GameObject.Find("colt");
        mac10Animator = mac10.GetComponent<Animator>();
        shotgunAnimator = shotgun.GetComponent<Animator>();

        w_animator = coltObject.GetComponent<Animator>();
        Recoil_Script = transform.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();

        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
        weaponSwitching = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetActiveWeapon(currentWeaponIndex); // Set the first weapon active

        // Ensure the light is initially turned off
        if (gunLightObject != null)
        {
            gunLightObject.SetActive(false);
            gunLightObjectShotgun.SetActive(false);
        }

        cameraShake = FindObjectOfType<CameraShake>();

        // Initialize the nextFireTimes array
        nextFireTimes = new float[weapons.Length];
        for (int i = 0; i < nextFireTimes.Length; i++)
        {
            nextFireTimes[i] = Time.time;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(0); // Set Beretta as active weapon
            colt.SetActive(true);
            mac10.SetActive(false);
            shotgun.SetActive(false);
            weaponSwitching.PlayOneShot(weaponSwitchingClip);

        }

        if (Time.time >= weaponUnlockTimeMac10 + 40f)
        {
            WeaponUnlockSound.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon(1); // Set Mac10 as active weapon
                colt.SetActive(false);
                mac10.SetActive(true);
                shotgun.SetActive(false);
                weaponSwitching.PlayOneShot(weaponSwitchingClip);
            }
        }

        if (Time.time >= weaponUnlockTimeShotgun + 100f)
        {
            WeaponUnlockSound2.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetActiveWeapon(2); // Set shotgun as active
                colt.SetActive(false);
                mac10.SetActive(false);
                shotgun.SetActive(true);
                weaponSwitching.PlayOneShot(weaponSwitchingClip);
            }
        }

        // Check for input (e.g., mouse click) and cooldown before firing
        if (Input.GetButton("Fire1") && Time.time >= nextFireTimes[currentWeaponIndex])
        {
            Fire();
            // Use the correct fire rate based on the active weapon
            float currentFireRate;
            switch (currentWeaponIndex)
            {
                case 0:
                    currentFireRate = fireRateBeretta;
                    break;
                case 1:
                    currentFireRate = fireRateMac;
                    mac10Animator.SetTrigger("ShootUzi");
                    break;
                case 2:
                    currentFireRate = fireRateShotgun;
                    shotgunAnimator.SetTrigger("ShootShotgun");
                    break;
                default:
                    currentFireRate = fireRateBeretta; // Default to Beretta if index is out of range
                    break;
            }

            nextFireTimes[currentWeaponIndex] = Time.time + 1f / currentFireRate;

            cameraShake.StartShake();
        }
    }

    void SetActiveWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        // Update the fire rate based on the active weapon
        switch (weaponIndex)
        {
            case 0:
                fireRate = fireRateBeretta;
                break;
            case 1:
                fireRate = fireRateMac;
                break;
            case 2:
                fireRate = fireRateShotgun;
                break;
        }
    }

    void Fire()
    {
        GameObject activeWeapon = weapons[currentWeaponIndex];

        if (currentWeaponIndex == 0)
        {
            w_animator.SetTrigger("Shoot");

            if (audioSource != null && shotSoundColt != null)
            {
                audioSource.PlayOneShot(shotSoundColt);
                // Enable the light for a short duration
                StartCoroutine(EnableLightForDuration(0.05f));
            }
        }
        else if (currentWeaponIndex == 1)
        {
            w_animator.SetTrigger("ShootUzi");

            audioSource.PlayOneShot(shotSoundMac);
            StartCoroutine(EnableLightForDuration(0.05f));
        }
        else if (currentWeaponIndex == 2)
        {
            audioSource.PlayOneShot(shotSoundShotgun);
            StartCoroutine(EnableLightForDurationShotgun(0.07f));

            // Shotgun shooting logic
            int bulletCount = Random.Range(minShotgunPellets, maxShotgunPellets);
            for (int i = 0; i < bulletCount; i++)
            {
                // Calculate a random spread within the specified angle
                Vector3 spreadDirection = firePoint.forward + new Vector3(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle)
                );

                // Normalize the direction
                spreadDirection.Normalize();

                // Instantiate a new bullet at the fire point
                GameObject shotgunBullet = Instantiate(bulletPrefabShotgun, firePoint.position, Quaternion.LookRotation(spreadDirection));

                // Apply force to the bullet to make it move
                shotgunBullet.GetComponent<Rigidbody>().AddForce(spreadDirection * bulletSpeed, ForceMode.VelocityChange);
            }

            Recoil_Script.RecoilFire();
            return; // Skip the rest of the method for the shotgun
        }

        // Calculate the shooting direction based on the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Vector3 fireDirection = ray.direction.normalized;

        // Instantiate a new bullet at the fire point
        GameObject bullet = Instantiate(currentWeaponIndex == 2 ? bulletPrefabShotgun : bulletPrefab, firePoint.position, Quaternion.LookRotation(fireDirection));

        // Apply force to the bullet to make it move
        bullet.GetComponent<Rigidbody>().AddForce(fireDirection * bulletSpeed, ForceMode.VelocityChange);

        Recoil_Script.RecoilFire();
    }

    // Coroutine to enable the light for a specified duration
    IEnumerator EnableLightForDuration(float duration)
    {
        if (gunLightObject != null)
        {
            gunLightObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            gunLightObject.SetActive(false);
        }
    }

    IEnumerator EnableLightForDurationShotgun(float duration)
    {
        if (gunLightObject != null)
        {
            gunLightObjectShotgun.SetActive(true);
            yield return new WaitForSeconds(duration);
            gunLightObjectShotgun.SetActive(false);
        }
    }

    float GetCurrentFireRate()
    {
        GameObject activeWeapon = weapons[currentWeaponIndex];
        return activeWeapon.GetComponent<BerettaStats>().fireRate;
    }
}
