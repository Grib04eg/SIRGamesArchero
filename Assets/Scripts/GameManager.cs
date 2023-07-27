using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] Transform enemiesSpawner;
    [SerializeField] Transform canvas;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject enemyHPBarPrefab;
    [SerializeField] PlayerController player;
    [SerializeField] Transform gate;
    public static GameManager instance;

    List<Enemy> enemies = new List<Enemy>();
    int coins = 0;
    void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        AddCoins(PlayerPrefs.GetInt("Coins", 0));
        float areaWidth = enemiesSpawner.localScale.x/2;
        float areaLength = enemiesSpawner.localScale.x/2;
        for (int i = 0; i < Random.Range(4,7); i++)
        {
            do
            {
                Vector3 position = new Vector3(Random.Range(-areaWidth, areaWidth), 0, Random.Range(-areaLength, areaLength)) + enemiesSpawner.position;
                Ray ray = new Ray(position + Vector3.up * 5, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        Enemy enemy = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)], position, Quaternion.identity).GetComponent<Enemy>();
                        enemies.Add(enemy);
                        var hpbar = Instantiate(enemyHPBarPrefab, canvas).GetComponent<Slider>();
                        enemy.Init(hpbar, player.transform);
                        enemy.OnDeath += EnemyDeath;
                    }
                    else
                        continue;
                }
            } while (false);
        }
    }

    public void EnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            gate.DOMoveY(-0.7f, 1f);
        }
    }

    public void AddCoins(int count)
    {
        coins += count;
        coinsText.text = coins.ToString();
        PlayerPrefs.SetInt("Coins", coins);
    }

    public Enemy GetClosestEnemy(Vector3 position)
    {
        Enemy enemy = null;
        float shortestDistance = float.MaxValue;
        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = Vector3.Distance(enemies[i].transform.position, position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemy = enemies[i];
            }
        }
        return enemy;
    }
}