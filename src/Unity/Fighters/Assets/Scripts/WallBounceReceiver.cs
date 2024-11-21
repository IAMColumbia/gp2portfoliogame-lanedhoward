using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounceReceiver : MonoBehaviour
{
    public Directions.FacingDirection wallDirection;
    public ParticleSystem bounceParticles;

    private void OnEnable()
    {
        FighterMain.WallBounced += Fighter_WallBounced;
    }

    private void OnDisable()
    {
        FighterMain.WallBounced -= Fighter_WallBounced;
    }

    public void Fighter_WallBounced(object sender, FighterMain.WallBounceEventArgs e)
    {
        if (e.wallDirection == wallDirection)
        {
            bounceParticles.transform.position = new Vector3(bounceParticles.transform.position.x, e.position.y, 0);
            bounceParticles.Play();
        }
    }
}
