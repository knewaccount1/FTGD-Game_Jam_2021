using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistingData : MonoBehaviour
{

    public int trialIndex = 0;
    public List<Upgrade> upgradeList;


    public int roundCount;

    private void Awake()
    {
        upgradeList = new List<Upgrade>();
    }
    public void StoreMenuSettings(int index, List<Upgrade> list)
    {
        roundCount++;
        trialIndex = index;
        upgradeList.AddRange(list);
    }

    

}
