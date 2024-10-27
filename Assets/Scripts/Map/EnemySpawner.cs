using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人生成器
/// </summary>
public class EnemySpawner : MonoBehaviour, ISaveable
{
    public enum CheckShape
    {
        Circle,
        Square,
    }

    [Tooltip("敌人预制体")] public GameObject enemyPrefab;
    public GameObject spawnedEnemy;
    [Tooltip("已经生成")] public bool isSpawned;
    [Tooltip("敌人已死")] public bool isDead;
    [Tooltip("可重复生成")] public bool canSpawnAgain = true;

    [Tooltip("玩家层级")] public LayerMask playerLayer = 1 << 3;
    [Tooltip("检测形状")] public CheckShape checkShape;

    [Header("Circle")]

    [Tooltip("检测半径")] public float radius = 20;

    [Header("Square")]

    [Tooltip("检测长宽")] public Vector2 size = new Vector2(40, 40);

    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
    }

    private void Update()
    {
        if (!isSpawned)
        {
            if ((checkShape == CheckShape.Circle && CircleCheck()) || (checkShape == CheckShape.Square && SquareCheck()))
            {
                spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                spawnedEnemy.GetComponent<Enemy>().spawner = this;
                isSpawned = true;
            }
        }
    }

    private bool CircleCheck() => Physics2D.OverlapCircle(transform.position, radius, playerLayer);
    private bool SquareCheck() => Physics2D.OverlapBox(transform.position, size, 0, playerLayer);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        switch (checkShape)
        {
            case CheckShape.Circle:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case CheckShape.Square:
                Gizmos.DrawWireCube(transform.position, size);
                break;
            default:
                break;
        }    
    }

    public void GetSaveData(SaveData saveData)
    {
        if (!canSpawnAgain)
        {
            if (spawnedEnemy != null)
                Destroy(spawnedEnemy);
            spawnedEnemy = null;
            isSpawned = false;
        }
        else
        {
            if (!saveData.savedSpawnerDict.ContainsKey(gameObject.name))
                saveData.savedSpawnerDict.Add(gameObject.name, isSpawned && isDead);
            else
                saveData.savedSpawnerDict[gameObject.name] = isSpawned && isDead;

            if (spawnedEnemy != null)
                Destroy(spawnedEnemy);
            spawnedEnemy = null;

            isSpawned = saveData.savedSpawnerDict[gameObject.name];
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        if (saveData.savedSpawnerDict.ContainsKey(gameObject.name))
            isSpawned = isSpawned = saveData.savedSpawnerDict[gameObject.name];
        else
            isSpawned = false;
    }
}
