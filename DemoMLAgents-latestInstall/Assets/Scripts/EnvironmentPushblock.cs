using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPushblock : MonoBehaviour
{

    
    public GameObject EnemyPrefab;
    private GameObject Enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Enemies = transform.Find("Enemies").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearEnvironment()
    {
        foreach (Transform enemy in Enemies.transform)
        {
            GameObject.Destroy(enemy.gameObject);
        }

    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(EnemyPrefab.gameObject);

        newEnemy.transform.SetParent(Enemies.transform);
        float rx = Random.Range(-3f, 3);
        float rz = Random.Range(-2.5f, 1.5f);
        newEnemy.transform.localPosition = new Vector3(rx, 0.5f, rz);
    }
}
