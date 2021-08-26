using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticles : MonoBehaviour
{
    [SerializeField] SpellShield spellShield;
    public void StopParticlesSystems()
    {
        spellShield._particlesRings.Stop();
    }
}

