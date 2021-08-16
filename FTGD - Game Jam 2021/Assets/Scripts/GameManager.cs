using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private PersistingData PD;
    public Camera MainCamera;
    public CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;
    public List<Wave> Waves;
    public Player playerPrefab;
    public Player playerRef;
    public Transform playerSpawn;

    public int killCount;
    public int killsNeeded;
    public TextMeshProUGUI killText;

    public float spawnTimer = 5f;

    public Transform[] spawnPoints;

    private float timeBtwSpawn;

    public Image[] heartContainers;

    public GameObject stageClearPanel;

    private void Awake()
    {
        PD = FindObjectOfType<PersistingData>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if(PD != null)
            killsNeeded = PD.roundCount * 5 + 5;

        GameObject tempPlayer = Instantiate(playerPrefab.gameObject, playerSpawn.transform.position, Quaternion.identity);
        playerRef = tempPlayer.GetComponent<Player>();
        playerRef.GM = this;
        vCam.m_Follow = tempPlayer.transform;

        if (PD != null)
            ApplyUpgrades();

        //First wave spawns in 5 seconds;
        timeBtwSpawn = 3f;
   
        //Enable hearts
        for(int i = 0; i < playerRef.maxHealth; i++)
        {
            Debug.Log("enabling heart");
            heartContainers[i].gameObject.SetActive(true);
        }



        UpdateKillText();
    }

    public void UpdateHearts()
    {
        foreach(Image heart in heartContainers)
        {
            heart.gameObject.SetActive(false);
        }

        for (int i = 0; i <= playerRef.health; i++)
        {
            heartContainers[i].gameObject.SetActive(true);
        }
        
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        Invoke("StopShake", duration);
    }

    public void StopShake()
    {
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    public void ApplyUpgrades()
    {

        foreach (Upgrade upgrade in PD.upgradeList)
        {
            upgrade.DoUpgrade(playerRef);

            if (upgrade.isSacrifice)
                upgrade.DoDowngrade(playerRef);

        }

    }

    // Update is called once per frame
    void Update()
    {
        timeBtwSpawn -= Time.deltaTime;
        if(timeBtwSpawn <= 0)
        {
            timeBtwSpawn = spawnTimer;
            SpawnNextEnemy();
        }
        
        if(killCount >= killsNeeded)
        {
            if(PD.roundCount == 3)
            {
                Invoke("LoadCreditScene",3);
                stageClearPanel.SetActive(true);
            }
            else
            {
                stageClearPanel.SetActive(true);

                Invoke("LoadTrialScene", 3);
            }

        }
    }

    
    public void LoadTrialScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadCreditScene()
    {
        SceneManager.LoadScene(3);
    }

    public void UpdateKillText()
    {
        killText.text = killCount + "/" + killsNeeded;
    }

    public void EnemyDied()
    {
        killCount++;
        UpdateKillText();
    }
    private void Start()
    {

    }

    public void SpawnNextEnemy()
    {
   
        int j = Random.Range(0, Waves.Count);
        int previousSpawn = 0;

        for (int k = 0; k < Waves[j].enemyPrefab.Length; k++)
        {
            int i = Random.Range(0, spawnPoints.Length);

            while (i == previousSpawn)
            {
                i = Random.Range(0, spawnPoints.Length);
            }

            previousSpawn = i;

            GameObject tempEnemy = Instantiate(Waves[j].enemyPrefab[k], spawnPoints[i].transform.position, Quaternion.identity);

            EnemyAI tempAI = tempEnemy.GetComponent<EnemyAI>();

            if (PD != null)
            {
                foreach (Upgrade upgrade in PD.upgradeList)
                {
             
                    upgrade.DoEnemyUpgrade(tempAI);

                    if (upgrade.isSacrifice)
                        upgrade.DoDowngradeEnemy(tempAI);
                }

            }
          
        }

    }


    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefab;
    }
}
