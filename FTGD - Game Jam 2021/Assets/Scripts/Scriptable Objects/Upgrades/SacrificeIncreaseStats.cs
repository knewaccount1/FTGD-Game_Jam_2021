using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sacrifice Increase Stats", menuName = ("Upgrades / Sacrifice Increase Stats"))]
public class SacrificeIncreaseStats : Upgrade
{

    public float amount;
    public float decreaseAmt;

    public enum Stats { DMG, MOVESPEED, ATTSPEED, HURTBOX, HEALTH }

    public Stats statToIncrease;
    public Stats statToDecrease;

    public override void DoUpgrade(Player player)
    {
        switch (statToIncrease)
        {
            case Stats.DMG:
                player.characterAttack.dmg += (int)amount;
                break;
            case Stats.MOVESPEED:
                player.characterMovement.hSpeed += amount;
                player.characterMovement.vSpeed += amount;
                break;
            case Stats.ATTSPEED:
                player.characterAttack.attSpeed -= amount;
                if (player.characterAttack.attSpeed < .5f)
                {
                    player.characterAttack.attSpeed = .5f;
                }
                break;
            case Stats.HURTBOX:
                Vector3 tempScale = player.transform.localScale;
                tempScale *= amount;
                player.transform.localScale = tempScale;
                break;
            case Stats.HEALTH:
                player.maxHealth += (int)amount;
                break;
        }
    }

    public override void DoEnemyUpgrade(EnemyAI enemyAI)
    {
        switch (statToIncrease)
        {
            case Stats.DMG:
                enemyAI.enemyAttack.dmg += (int)amount;
                break;
            case Stats.MOVESPEED:
                enemyAI.moveSpeedX += amount;
                enemyAI.moveSpeedZ += amount;
                break;
            case Stats.ATTSPEED:
                enemyAI.enemyAttack.attSpeed -= amount;
                if (enemyAI.enemyAttack.attSpeed < .5f)
                {
                    enemyAI.enemyAttack.attSpeed = .6f;
                }
                break;
            case Stats.HURTBOX:
                Vector3 tempScale = enemyAI.transform.localScale;
                tempScale *= amount;
                enemyAI.transform.localScale = tempScale;
                break;
            case Stats.HEALTH:
                enemyAI.maxHealth += (int)amount;
                break;
        }
    }

    public override void DoDowngrade(Player player)
    {
        switch (statToDecrease)
        {
            case Stats.DMG:
                player.characterAttack.dmg -= (int)decreaseAmt;
                break;
            case Stats.MOVESPEED:
                player.characterMovement.hSpeed -= decreaseAmt;
                player.characterMovement.vSpeed -= decreaseAmt;
                break;
            case Stats.ATTSPEED:
                player.characterAttack.attSpeed += decreaseAmt;
                if (player.characterAttack.attSpeed > 1.5f)
                {
                    player.characterAttack.attSpeed = 1.5f;
                }
                break;
            case Stats.HURTBOX:
                Vector3 tempScale = player.transform.localScale;
                tempScale *= decreaseAmt;
                player.transform.localScale = tempScale;
                break;
            case Stats.HEALTH:
                player.maxHealth -= (int)decreaseAmt;
                break;
        }
    }

    public override void DoDowngradeEnemy(EnemyAI enemyAI)
    {
        switch (statToDecrease)
        {
            case Stats.DMG:
                enemyAI.enemyAttack.dmg -= (int)decreaseAmt;
                break;
            case Stats.MOVESPEED:
                enemyAI.moveSpeedX -= decreaseAmt;
                enemyAI.moveSpeedZ -= decreaseAmt;
                break;
            case Stats.ATTSPEED:
                enemyAI.enemyAttack.attSpeed += amount;
                if (enemyAI.enemyAttack.attSpeed < 1.5f)
                {
                    enemyAI.enemyAttack.attSpeed = 1.5f;
                }
                break;
            case Stats.HURTBOX:
                Vector3 tempScale = enemyAI.transform.localScale;
                tempScale *= decreaseAmt;
                enemyAI.transform.localScale = tempScale;
                break;
            case Stats.HEALTH:
                enemyAI.maxHealth -= (int)decreaseAmt;
                break;
        }
    }

}
