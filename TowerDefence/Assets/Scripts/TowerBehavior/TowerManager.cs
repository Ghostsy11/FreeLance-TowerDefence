using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public static TowerManager instance;

    // Getting Tower Type
    public GameObject activeTower;
    public GameObject inActiveTowerVersion;

    // Indeicator to see where you place objects
    [SerializeField] Transform indicator;
    public bool isPlacing;
    Vector3 location;
    // Where should tower be palced
    public LayerMask whatIsPlacemeant;
    WayPoint wayPoint;

    // Building cost
    [SerializeField] AudioSource placeTowerSFX;
    [SerializeField] AudioSource noMoneySFX;

    private void Awake()
    {
        instance = this;
        indicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlacing)
        {
            indicator.position = GetGridPosition();

            if (Input.GetMouseButtonDown(0) && wayPoint.IsPlacable())
            {
                isPlacing = false;

                if (!placeTowerSFX.isPlaying)
                {
                    placeTowerSFX.Play();
                }

                var towertype = Instantiate(activeTower, indicator.position, activeTower.transform.rotation);
                wayPoint.isPlacable = false;
                indicator.gameObject.SetActive(false);
                if (towertype != null)
                {
                    if (towertype.GetComponent<LaserTower>())
                        towertype.GetComponent<LaserTower>().ringRange.SetActive(false);
                    else if (towertype.GetComponent<CannonTower>())
                        towertype.GetComponent<CannonTower>().ringRange.SetActive(false);
                    else if (towertype.GetComponent<ArrowTower>())
                        towertype.GetComponent<ArrowTower>().ringRange.SetActive(false);
                    else
                        return;

                }


                //  }
            }

            if (Input.GetMouseButtonDown(1))
            {
                activeTower.GetComponent<TowerCost>().ReturnBuildingCost();
                isPlacing = false;
                indicator.gameObject.SetActive(false);
                Debug.Log("Got everything back");
            }

        }
    }


    // Assinging towerType
    public void StartTowerPlacemeant(GameObject towerToPlace, GameObject indicatorTower)
    {
        activeTower = towerToPlace;
        inActiveTowerVersion = indicatorTower;
        if (activeTower.GetComponent<TowerCost>().VirfiyTowerCost())
        {
            isPlacing = true;
            Destroy(indicator.gameObject);
            GameObject placeTower = Instantiate(inActiveTowerVersion);
            indicator = placeTower.transform;


        }
        else
        {
            Debug.Log("Not enough money");
            if (!noMoneySFX.isPlaying)
            {
                noMoneySFX.Play();
            }
            // play SFX
        }
    }


    public Vector3 GetGridPosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 300, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300, whatIsPlacemeant))
        {
            wayPoint = hit.transform.GetComponent<WayPoint>();

            if (wayPoint != null)
            {
                location = wayPoint.GetCurrentNodeLocation();
                location.y = 0.3f;

            }

        }
        return location;
    }
}
