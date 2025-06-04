using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutEenmy : MonoBehaviour
{
    private GameObject healthBarUI;
    private MeshRenderer gameObjectMesh;
    private EnemyHealth enemyHealth;
    private CapsuleCollider capsuleCollider;
    private float timeCalpsed;

    public float timeToHideAgain;
    public bool isHidden;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyHealth = GetComponent<EnemyHealth>();
        healthBarUI = gameObject.transform.GetChild(0).gameObject;
        gameObjectMesh = gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
        gameObject.layer = 8;
        HideTheEnemy();

    }

    private void Update()
    {
        timeCalpsed += Time.deltaTime;
        TimeToReHide();
    }

    public void HideTheEnemy()
    {
        gameObject.layer = 8;
        isHidden = true;
        healthBarUI.SetActive(false);
        gameObjectMesh.gameObject.SetActive(false);
        gameObjectMesh.enabled = false;

    }

    public void EnemyDiscovered()
    {
        gameObject.layer = 6;
        isHidden = false;
        healthBarUI.SetActive(true);
        gameObjectMesh.gameObject.SetActive(true);
        gameObjectMesh.enabled = true;
    }

    private void TimeToReHide()
    {
        if (timeCalpsed > timeToHideAgain && !isHidden)
        {
            timeCalpsed = 0;
            HideTheEnemy();

        }
    }
}
