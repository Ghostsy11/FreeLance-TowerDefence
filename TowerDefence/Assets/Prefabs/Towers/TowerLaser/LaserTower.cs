using System.Collections;
using UnityEngine;

public class LaserTower : TowerBaseScript
{
    [Header("Laser Settings")]
    public LineRenderer laserLine; // The LineRenderer component used to render the laser beam
    public GameObject impactEffectPrefab; // The effect that appears where the laser hits

    [Header("Damage Settings")]
    [SerializeField] private float laserDamage = 5f; // The amount of damage the laser deals per second
    [SerializeField] AudioSource laserAudioSource;
    [SerializeField] AudioSource enemyDeathAudio;


    [Header("Laser Visuals")]
    [SerializeField] private float baseStartWidth = 0.2f; // Default starting width of the laser
    [SerializeField] private float baseEndWidth = 0.1f; // Default ending width of the laser
    [SerializeField] private float pulseSpeed = 0.5f; // Speed of the pulsating effect on the laser
    [SerializeField] private float pulseAmount = 0.05f; // How much the laser width changes while pulsating

    [Header("Color Settings")]
    [SerializeField] private Color laserStartColor = Color.red; // The main color of the laser
    [SerializeField] private Color laserEndColor = new Color(1, 0, 0, 0.5f); // The color at the end of the laser
    [SerializeField] private float colorLerpSpeed = 2f; // How fast the laser changes color between red and white

    [Header("Laser Fade Settings")]
    [SerializeField] private float fadeDuration = 0.2f; // Duration of the laser fade-in/out effect

    private EnemyHealth currentTarget; // Reference to the enemy being attacked
    private Coroutine laserCoroutine; // Used to control the laser firing process

    private void Start()
    {
        // Make sure the laser and impact effect are disabled at the beginning
        LineRenderer laser = Instantiate(laserLine);
        laserLine = laser;
        laserLine.enabled = false;
        impactEffectPrefab.SetActive(false);
    }

    public override void LookAtTheNearstEnemy()
    {
        // Call the base method to rotate towards the nearest enemy
        base.LookAtTheNearstEnemy();

        if (closetTarget != null)
        {
            // If there's a valid target, start the laser firing process
            if (laserCoroutine == null)
            {
                laserCoroutine = StartCoroutine(FireLaser());
            }
        }
        else
        {
            // If no target is found, stop the laser
            StopLaser();
        }
    }

    IEnumerator FireLaser()
    {
        while (closetTarget != null) // Keep firing as long as there's a target
        {
            // Get the EnemyHealth component of the target
            currentTarget = closetTarget.GetComponent<EnemyHealth>();

            if (currentTarget == null) // If target is invalid, stop the laser
            {
                StopLaser();
                yield break;
            }

            // Enable and position the laser
            EnableLaser(closetTarget.position);
            if (currentTarget.objectCurrentHealth() > 0)
            {
                if (!laserAudioSource.isPlaying)
                {
                    laserAudioSource.Play();
                }
                // Apply damage over time
                currentTarget.TakeDamage(laserDamage * Time.deltaTime);

            }
            else
            {
                if (!enemyDeathAudio.isPlaying)
                {
                    enemyDeathAudio.Play();
                }

                EnemiesPool.Instance.DeactivateEnemy(currentTarget.gameObject);
                EconomyManager.Instance.KillReward(5, 5);
                Rewards.Instance.UpdateKills();
                GameManager.instance.Questing();

            }

            yield return null; // Wait for the next frame and continue
        }

        StopLaser(); // If the loop exits, stop the laser
    }

    void EnableLaser(Vector3 hitPoint)
    {
        if (!laserLine.enabled) // If the laser isn't already active
        {
            laserLine.enabled = true; // Enable the laser
            StartCoroutine(FadeLaser(true)); // Start the fade-in effect
        }

        // Set laser positions
        laserLine.SetPosition(0, firePoint.position);
        laserLine.SetPosition(1, hitPoint);

        // Determine if we are hitting an enemy
        bool isHittingEnemy = currentTarget != null;

        // Pulsing effect for better visuals
        float pulse = Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);

        // Apply bloom effect when hitting an enemy
        float bloomMultiplier = isHittingEnemy ? 2.0f : 1.0f; // If hitting, make it 2x wider

        laserLine.startWidth = (baseStartWidth + pulse) * bloomMultiplier;
        laserLine.endWidth = (baseEndWidth + (pulse * 0.5f)) * bloomMultiplier;

        // Increase brightness by modifying color intensity
        Color hitColor = isHittingEnemy ? Color.yellow : laserStartColor;
        laserLine.startColor = Color.Lerp(hitColor, Color.white, Mathf.PingPong(Time.time * colorLerpSpeed, 1));
        laserLine.endColor = laserEndColor;

        // Activate impact effect at hit point
        impactEffectPrefab.transform.position = hitPoint;
        impactEffectPrefab.SetActive(true);
    }

    void StopLaser()
    {
        if (laserCoroutine != null) // If there's an active laser coroutine
        {
            StopCoroutine(laserCoroutine); // Stop it
            laserCoroutine = null;
        }

        StartCoroutine(FadeLaser(false)); // Start the fade-out effect
    }

    IEnumerator FadeLaser(bool fadeIn)
    {
        float elapsed = 0f; // Timer for the fade effect
        float targetWidth = fadeIn ? baseStartWidth : 0f; // Target width (0 when fading out)

        while (elapsed < fadeDuration) // Gradually change the width over time
        {
            elapsed += Time.deltaTime;
            float width = Mathf.Lerp(laserLine.startWidth, targetWidth, elapsed / fadeDuration);
            laserLine.startWidth = width;
            laserLine.endWidth = width * 0.5f;
            yield return null; // Wait for the next frame
        }

        if (!fadeIn) // If fading out, disable the laser and impact effect
        {
            laserLine.enabled = false;
            impactEffectPrefab.SetActive(false);
        }
    }
}
