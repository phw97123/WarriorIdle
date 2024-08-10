using System.Collections;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    private WaitForSeconds _waitTime;

    public void Start()
    {
        _waitTime = new WaitForSeconds(0.5f);
    }

    public virtual void Execute(SkillData data)
    {
    }

    protected IEnumerator CheckParticleSystem(ParticleSystem particleSystem)
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }

        yield return _waitTime;

        Managers.ResourceManager.Destroy(particleSystem.gameObject);
    }
}
