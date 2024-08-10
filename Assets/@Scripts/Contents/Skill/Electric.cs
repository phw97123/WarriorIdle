using System.Collections.Generic;
using UnityEngine;

public class Electric : SkillBase
{
    private float _radius = 7.0f;

    public override void Execute(SkillData data)
    {
        List<EnemyController> targets = Utils.FindEnemiesInRadius(_radius);

        string skillName = $"{data.BaseData.prefab.name}.prefab";

        foreach (EnemyController target in targets)
        {
            GameObject go = Managers.ResourceManager.Instantiate(skillName, null, true);
            go.transform.position = target.transform.position;
            target.OnDamaged(data.damage, false);

            ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                StartCoroutine(CheckParticleSystem(particleSystem));
            }
        }
    }
}
