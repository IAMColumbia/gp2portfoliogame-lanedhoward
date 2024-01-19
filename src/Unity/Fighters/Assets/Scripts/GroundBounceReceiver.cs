using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBounceReceiver : MonoBehaviour
{
    public ParticleSystem bounceParticles;

    private void OnEnable()
    {
        FighterMain.GroundBounced += Fighter_GroundBounced;
    }

    private void OnDisable()
    {
        FighterMain.GroundBounced -= Fighter_GroundBounced;
    }

    public void Fighter_GroundBounced(object sender, Vector2 position)
    {
        bounceParticles.transform.position = new Vector3(position.x, bounceParticles.transform.position.y, 0);
        bounceParticles.Play();
    }
}
