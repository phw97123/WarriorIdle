using UnityEngine;

public class Explosion : SkillBase
{
    public override void Execute(SkillData data)
    {
        EnemyController target = Utils.FindNearestEnemy();

        string skillName = $"{data.baseData.prefab.name}.prefab";
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
