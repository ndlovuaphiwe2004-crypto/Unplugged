using System.Collections;
using UnityEngine;
        
public class Particleeffect : MonoBehaviour
{
    [Tooltip("Time in seconds until the particle effect stops.")]
    public float lifetime = 2f;

    [Tooltip("If true, destroy the GameObject after all particles have died.")]
    public bool destroyAfterStop = true;

    [Tooltip("If true, disable emission (no new particles). If false, call Stop to stop emission.")]
    public bool disableEmissionModule = true;

    ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (lifetime > 0f)
            StartCoroutine(StopAfterSeconds(lifetime));
    }

    IEnumerator StopAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        foreach (var ps in particleSystems)
        {
            if (ps == null) continue;
            if (disableEmissionModule)
            {
                var emission = ps.emission;
                emission.enabled = false;
            }
            else
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        if (destroyAfterStop)
        {
           
            while (IsAnyParticleSystemAlive())
                yield return null;

            Destroy(gameObject);
        }
    }

    bool IsAnyParticleSystemAlive()
    {
        foreach (var ps in particleSystems)
        {
            if (ps == null) continue;
            if (ps.IsAlive(true)) return true;
        }
        return false;
    }
}
