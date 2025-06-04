using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTarget : MonoBehaviour
{
    private EnemiesPool enemiesPool;
    private ObjectHealth playerCurrentHealth;

    public delegate void playerGotHit();
    public playerGotHit playerDamaged;

    [SerializeField] AudioSource playDamageEffectAudio;

    private void Start()
    {
        enemiesPool = FindObjectOfType<EnemiesPool>();
        playerDamaged += PlayerGotHurt;
        playDamageEffectAudio = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider != null)
        {
            // Try to get the PlayerHealth component
            if (collider.gameObject.TryGetComponent<ObjectHealth>(out playerCurrentHealth) && collider.gameObject.tag == "PlayerHealth")
            {
                if (!playDamageEffectAudio.isPlaying)
                {
                    playDamageEffectAudio.Play();
                }
                playerCurrentHealth.OnHit();
                playerDamaged?.Invoke();
                gameObject.SetActive(false);
                EnemiesPool.Instance.DeactivateEnemy(gameObject);
                transform.position = enemiesPool.SpawnPoint.transform.position;
                if (playerCurrentHealth.objectCurrentHealth() <= 0 && GameManager.instance.creaitiveMode)
                {
                    Debug.Log("ops! reset");
                    playerCurrentHealth.ResetHealth();

                }
                if (playerCurrentHealth.objectCurrentHealth() <= 0 && GameManager.instance.hardMode)
                {
                    Debug.Log("Player lost");
                    GameManager.instance.gameObject.GetComponent<GameOverPanelHandler>().SetGameObjectT();
                    GameTimer.Instance.SaveGameTime();
                    Time.timeScale = 0f;
                }
                if (playerCurrentHealth.objectCurrentHealth() <= 0 && GameManager.instance.normalMode)
                {
                    Debug.Log("Player lost");
                    GameManager.instance.gameObject.GetComponent<GameOverPanelHandler>().SetGameObjectT();
                    GameTimer.Instance.SaveGameTime();
                    Time.timeScale = 0f;
                }
            }
        }

    }

    private void PlayerGotHurt()
    {
        FindObjectOfType<ScreenHealthDisplayer>().UpdateScreenHealth();
        // PLAY SOUND EFFECT OTHER....

        Debug.Log("lol");

    }

}
