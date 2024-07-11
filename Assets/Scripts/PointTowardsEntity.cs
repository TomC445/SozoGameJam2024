using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsEntity : MonoBehaviour
{
    private GameManager gameManager;
    private Transform enemyTransform;
    public Material objMaterial;
    public int listNumber = 0;
    public float rotationSpeed = 5;
    private Transform defaultTransform;

    public Color enemyBoat;
    public Color traderBoat;
    public Color tower;

    public GameObject towerBackground;
    public GameObject enemyBackground;
    public GameObject traderBackground;

    public GameObject arrowModel;
    public GameObject countUI;

    public bool isActive = true;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        defaultTransform = transform;
        arrowModel.SetActive(isActive);
        countUI.SetActive(isActive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            listNumber = (listNumber + 1) % 3;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isActive = !isActive;
            arrowModel.SetActive(isActive);
            countUI.SetActive(isActive);
        }

        switch (listNumber)
        {
            case 0:
                if(gameManager.traderShips.Count > 0)
                {
                    arrowModel.SetActive(isActive);
                    enemyTransform = GetClosestTransform(gameManager.traderShips);
                    objMaterial.color = traderBoat;
                    traderBackground.SetActive(true);
                    enemyBackground.SetActive(false);
                    towerBackground.SetActive(false);
                } else
                {
                    arrowModel.SetActive(false);
                    enemyTransform = defaultTransform;
                    objMaterial.color = Color.clear;
                    traderBackground.SetActive(false);
                    enemyBackground.SetActive(false);
                    towerBackground.SetActive(false);
                }
                
                break;
            case 1:
                if(gameManager.enemyShips.Count > 0)
                {
                    arrowModel.SetActive(isActive);
                    enemyTransform = GetClosestTransform(gameManager.enemyShips);
                    objMaterial.color = enemyBoat;
                    traderBackground.SetActive(false);
                    enemyBackground.SetActive(true);
                    towerBackground.SetActive(false);
                } else
                {
                    arrowModel.SetActive(false);
                    enemyTransform = defaultTransform;
                    objMaterial.color = Color.clear;
                    traderBackground.SetActive(false);
                    enemyBackground.SetActive(false);
                    towerBackground.SetActive(false);
                }
                
                break;
            case 2:
                if(gameManager.towers.Count > 0)
                {
                    arrowModel.SetActive(isActive);
                    enemyTransform = GetClosestTransform(gameManager.towers);
                    objMaterial.color = tower;
                    traderBackground.SetActive(false);
                    enemyBackground.SetActive(false);
                    towerBackground.SetActive(true);
                } else
                {
                    arrowModel.SetActive(false);
                    enemyTransform = defaultTransform;
                    objMaterial.color = Color.clear;
                    traderBackground.SetActive(false);
                    enemyBackground.SetActive(false);
                    towerBackground.SetActive(false);
                }
                break;
        }

        PointTowards(enemyTransform);
    }

    //get closest transform of entity type
    private Transform GetClosestTransform(List<GameObject> objList)
    {
        Transform closestTransform = objList[0].transform;
        float distance = Vector3.Distance(transform.position, closestTransform.position);
        float curDistance = 0;
        foreach (GameObject obj in objList)
        {
            curDistance = Vector3.Distance(transform.position, obj.transform.position);
            if (curDistance < distance)
            {
                closestTransform = obj.transform;
                distance = curDistance;
            }
        }

        return closestTransform;
    }

    //point towards entity
    void PointTowards(Transform enemyTransform)
    {
        Vector2 mypos2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 enemyPos2D = new Vector2(enemyTransform.position.x, enemyTransform.position.z);

        Vector2 playerDirection = (enemyPos2D - mypos2D);
        Vector3 directionToEntity = new Vector3(playerDirection.x, 0, playerDirection.y);
        Quaternion targetRotation = Quaternion.LookRotation(directionToEntity);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
