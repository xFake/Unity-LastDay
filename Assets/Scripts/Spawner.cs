using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<Transform> spawners;

    [SerializeField]
    EnemyController zombieSlowPrefab;
    [SerializeField]
    EnemyController zombieFastPrefab;

    EnemyController enemy;

    void Start()
    {
        spawners = new List<Transform>();
        foreach (Transform t in transform)
        {
            spawners.Add(t);
        }
    }

    public List<EnemyController> SpawnEnemy(int _enemyCount)
    {
        List<EnemyController> enemylist = new List<EnemyController>();
        for (int i = 0; i < _enemyCount; i++)
        {
            ChooseZombie();
            Transform spawnerPosition = (spawners[Random.Range(0, spawners.Count)]);
            Vector3 enemyPostition = new Vector3(spawnerPosition.position.x, spawnerPosition.position.y, spawnerPosition.position.z);
            enemylist.Add(Instantiate(enemy, enemyPostition, Quaternion.identity));
        }
        return enemylist;
    }

    void ChooseZombie()
    {
        int x = Random.Range(0, 2);
        if (x == 0)
        {
            enemy = zombieFastPrefab;
        }
        else
        {
            enemy = zombieSlowPrefab;
        }
            
    }
}
