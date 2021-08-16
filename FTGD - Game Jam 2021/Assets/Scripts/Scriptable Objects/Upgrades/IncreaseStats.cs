using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Increase Stats", menuName = ("Upgrades / Increase Stats"))]
public class IncreaseStats : Upgrade
{
    public float amount;

    public enum StatToIncrease { DMG, MOVESPEED, ATTSPEED, HURTBOX, HEALTH }

    public StatToIncrease statToIncrease;

    public override void DoUpgrade(Player player)
    {
        switch (statToIncrease)
        {
            case StatToIncrease.DMG:
                player.characterAttack.dmg += (int)amount;
                break;
            case StatToIncrease.MOVESPEED:
                player.characterMovement.hSpeed += amount;
                player.characterMovement.vSpeed += amount;
                break;
            case StatToIncrease.ATTSPEED:
                player.characterAttack.attSpeed -= amount;
                if (player.characterAttack.attSpeed < .5f)
                {
                    player.characterAttack.attSpeed = .5f;
                }
                break;
            case StatToIncrease.HURTBOX:
                Vector3 tempScale = player.transform.localScale;
                tempScale *= amount;
                player.transform.localScale = tempScale;
                break;
            case StatToIncrease.HEALTH:
                player.maxHealth += (int)amount;
                break;
        }
    }

    public override void DoEnemyUpgrade(EnemyAI enemyAI)
    {
        switch (statToIncrease)
        {
            case StatToIncrease.DMG:
                enemyAI.enemyAttack.dmg += (int)amount;
                break;
            case StatToIncrease.MOVESPEED:
                enemyAI.moveSpeedX += amount;
                enemyAI.moveSpeedZ += amount;
                break;
            case StatToIncrease.ATTSPEED:
                enemyAI.enemyAttack.attSpeed -= amount;
                if (enemyAI.enemyAttack.attSpeed < .5f)
                {
                    enemyAI.enemyAttack.attSpeed = .6f;
                }
                break;
            case StatToIncrease.HURTBOX:
                Vector3 tempScale = enemyAI.transform.localScale;
                tempScale *= amount;
                enemyAI.transform.localScale = tempScale;
                break;
            case StatToIncrease.HEALTH:
                enemyAI.maxHealth += (int)amount;
                break;
        }
    }
}
