using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region fields
    [SerializeField]
    List<EnemyController> enemyList;
    Spawner spawner;

    [SerializeField]
    GameObject player;

    [SerializeField]
    CameraController cameraController;

    [Header("Audio")]
    [SerializeField]
    TextMeshProUGUI playerVolumeSliderValue;

    [SerializeField]
    Slider playerVolumeSlider;

    [SerializeField]
    TextMeshProUGUI enemyVolumeSliderValue;

    [SerializeField]
    Slider enemyVolumeSlider;

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI textEndGame;
    static bool isGameInProgress = false;
    
    [SerializeField]
    float breakTime;
    float timeTillNextRound;

    [SerializeField]
    int maxRounds;
    int roundCount;

    int enemyCountThisRound = 1;
    int enemyCountPreviousRound = 0;


    #endregion

    void Start()
    {
        cameraController.SetPlayer(player.transform);
        enemyList = new List<EnemyController>();
        timeTillNextRound = breakTime;
        roundCount = 0;
        spawner = FindObjectOfType<Spawner>();
        playerVolumeSlider.value = PlayerPrefs.GetFloat("PlayerVolume");
        enemyVolumeSlider.value = PlayerPrefs.GetFloat("EnemyVolume");
    }

    void Update()
    {
        if (enemyList.Count == 0 && isGameInProgress)
        {
            NextRoundTimer();
        }

        if (timeTillNextRound <= 0 && isGameInProgress)
        {
            RoundCounter();
            enemyList = spawner.SpawnEnemy(CalculateNumberOfEnemies());
            foreach (EnemyController enemy in enemyList)
            {
                enemy.EnemyDied += RemoveEnemy;
                enemy.SetPlayer(player.GetComponent<PlayerController>());
            }
        }

        if (roundCount == maxRounds && enemyList.Count <= 0)
        {
            LevelComplete();
        }

        foreach(EnemyController enemy in enemyList)
        {
            enemy.GetComponent<AudioSource>().volume = enemyVolumeSlider.value / 100;
        }

        enemyVolumeSliderValue.text = "Zombie volume " + enemyVolumeSlider.value + "%";
        PlayerPrefs.SetFloat("EnemyVolume", enemyVolumeSlider.value);

        player.GetComponent<AudioSource>().volume = playerVolumeSlider.value / 100;
        playerVolumeSliderValue.text = "Player volume " + playerVolumeSlider.value + "%";
        PlayerPrefs.SetFloat("PlayerVolume", playerVolumeSlider.value);
    }

    public static void SetGameState(bool _isGameInProgress)
    {
        isGameInProgress = _isGameInProgress;
    }

    public static bool GetGameState()
    {
        return isGameInProgress;
    }


    #region UI

    void RoundCounter()
    {
        timeTillNextRound = breakTime;
        roundCount++;
    }

    void NextRoundTimer()
    {
        timeTillNextRound -= Time.deltaTime;
    }

    void LevelComplete()
    {
        isGameInProgress = false;
        textEndGame.enabled = true;
        textEndGame.text = "You won";
    }

    #endregion

    #region Spawning && Removing

    int CalculateNumberOfEnemies()
    {
        int enemyCount;
        enemyCount = enemyCountThisRound + enemyCountPreviousRound;
        enemyCountPreviousRound = enemyCountThisRound;
        enemyCountThisRound = enemyCount;
        return enemyCount;
    }

    void RemoveEnemy(EnemyController enemy)
    {
        enemyList.Remove(enemy);
    }

    
    #endregion
}
