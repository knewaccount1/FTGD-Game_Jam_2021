using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public bool isSacrifice;
    
    public virtual void DoUpgrade(Player player)
    {

    }

    public virtual void DoEnemyUpgrade(EnemyAI enemyAI)
    {

    }

    public virtual void DoDowngrade(Player player)
    {

    }
    public virtual void DoDowngradeEnemy(EnemyAI enemyAI)
    {

    }
}
