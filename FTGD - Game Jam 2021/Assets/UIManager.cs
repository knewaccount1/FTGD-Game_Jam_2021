using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public int trialIndex;
    private int tempIndex;

    public List<TrialScenario> trialScenarios;

    [Header("UI References")] public TextMeshProUGUI scenarioText;
    public GameObject buffTextPanel;
    public Animator buffTextAnimator;
    public GameObject ButtonPanel;

    public GameObject playerObject; //this refers to the objects spawned on the trial stand
    public GameObject enemyObject;
    public SpriteRenderer[] enemySprites;
    public Player playerPrefab;
    public EnemyAI[] enemyPrefabs;

    public List<Upgrade> upgradeList;

    public AudioSource audioSourceBuffSound;
    private PersistingData PD;


    private void Awake()
    {
        PD = FindObjectOfType<PersistingData>();
        trialIndex = PD.trialIndex;
        tempIndex = 0;

        AssignNewEnemySprite();

    }

    private void AssignNewEnemySprite()
    {
        enemyObject.GetComponent<SpriteRenderer>().sprite = enemySprites[Random.Range(0, enemySprites.Length)].sprite;
    }
    private void Start()
    {
        upgradeList = new List<Upgrade>();
        UpdateTrialScreen();
    }
    public void UpdateTrialScreen()
    {

        scenarioText.text = trialScenarios[trialIndex].scenarioText;

    }


    public void DecisionButton1()
    {
        upgradeList.Add(trialScenarios[trialIndex].upgradeNegative);

        //trialScenarios[scenarioIndex].upgradeNegative.DoUpgrade(playerPrefab);
        //trialScenarios[scenarioIndex].upgradeNegative.DoDowngrade(playerPrefab);

        //foreach (EnemyAI enemyPrefab in enemyPrefabs)
        //{
        //    trialScenarios[scenarioIndex].upgradeNegative.DoEnemyUpgrade(enemyPrefab);
        //    trialScenarios[scenarioIndex].upgradeNegative.DoDowngradeEnemy(enemyPrefab);

        //}

        if (trialScenarios[trialIndex].sacrificeParticle != null)
        {
            audioSourceBuffSound.Play();
            Instantiate(trialScenarios[trialIndex].positiveParticle, enemyObject.transform.position, trialScenarios[trialIndex].positiveParticle.transform.rotation);
            Instantiate(trialScenarios[trialIndex].sacrificeParticle, new Vector3( playerObject.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.z-.3f), trialScenarios[trialIndex].sacrificeParticle.transform.rotation);
        }
        
        buffTextPanel.gameObject.SetActive(true);
        buffTextAnimator.Play("buffTextAnim");

        buffTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = trialScenarios[trialIndex].negativeDescription;

        trialIndex += 1;
        tempIndex++;
        UpdateTrialScreen();
        AssignNewEnemySprite();
        CheckStartCondition();
    }

    public void DecisionButton2()
    {

        upgradeList.Add(trialScenarios[trialIndex].upgradeNegative);

        if (trialScenarios[trialIndex].positiveParticle != null)
        {
            audioSourceBuffSound.Play();
            Instantiate(trialScenarios[trialIndex].positiveParticle, playerObject.transform.position, trialScenarios[trialIndex].positiveParticle.transform.rotation);
            Instantiate(trialScenarios[trialIndex].positiveParticle, enemyObject.transform.position, trialScenarios[trialIndex].positiveParticle.transform.rotation);
        }

        buffTextPanel.SetActive(true);
        buffTextAnimator.Play("buffTextAnim");
        buffTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = trialScenarios[trialIndex].positiveDescription;

        trialIndex += 1;
        tempIndex++;
        UpdateTrialScreen();
        AssignNewEnemySprite();
        CheckStartCondition();
    }

    public void CheckStartCondition()
    {
        //Made 5 decisions
        if(tempIndex == 5)
        {
            //load battle scene here

            ButtonPanel.SetActive(false);

            PD.StoreMenuSettings(trialIndex, upgradeList);
            Invoke("LoadBattle", 1f);

            
            
        }
    }

    public void LoadBattle()
    {
        SceneManager.LoadScene(1);
    }
    
}
