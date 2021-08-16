using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trial Scenario", menuName = "Scenario")]
public class TrialScenario : ScriptableObject
{
    [TextArea(3,20)]public string scenarioText;
    public Upgrade upgradePositive;
    public Upgrade upgradeNegative;

    [TextArea(2,2)]public string positiveDescription;
    [TextArea(2, 2)]public string negativeDescription;

    public ParticleSystem positiveParticle;
    public ParticleSystem sacrificeParticle;

}
