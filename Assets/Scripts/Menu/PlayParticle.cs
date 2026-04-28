using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void ParticlePlayer()
    {         if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

}
