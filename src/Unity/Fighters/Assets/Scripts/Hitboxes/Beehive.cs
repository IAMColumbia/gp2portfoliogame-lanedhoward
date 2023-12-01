using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Beehive : Projectile
{
    public Vector2 startVelocityNeutral;
    public Vector2 startVelocityBack;
    public Vector2 startVelocityForward;
    public Vector2 friction;

    public AudioClip explosionSound;
    public ParticleSystem explosionParticles;

    private void OnEnable()
    {
        if (fighterParent != null)
        {
            fighterParent.StocksUpdated += FighterParent_StocksUpdated;
        }
    }

    private void OnDisable()
    {
        if (fighterParent != null)
        {
            fighterParent.StocksUpdated -= FighterParent_StocksUpdated;
        }
    }

    private void FighterParent_StocksUpdated(object sender, int e)
    {
        if (e <= 0)
        {
            StartCoroutine(DelayEndProjectile(0.1f));
        }
    }

    public override void StartProjectile()
    {
        base.StartProjectile();
        hitbox.OpenForCollision = false;
        hitbox._state = ColliderState.Closed;
    }

    public void StartProjectile(int forwardBack)
    {
        switch (forwardBack)
        {
            case 1: velocity = startVelocityForward; break;
            default:
            case 0: velocity = startVelocityNeutral; break;
            case -1: velocity = startVelocityBack; break;
        }
        StartProjectile();
    }

    private void Update()
    {
        if (velocity.x > 0)
        {
            velocity.x -= friction.x * Time.deltaTime;
        }
        else
        {
            velocity.x = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallTrigger"))
        {
            velocity.x = 0;
        }
    }


    public override void EndProjectile()
    {
        base.EndProjectile();
        fighterParent.SetStocks(0);
    }

    public void PlayExplosionFX()
    {
        ParticleSystem fx = Instantiate(explosionParticles, explosionParticles.transform.position, explosionParticles.transform.rotation);
        fx.GetComponent<SoundPlayer>().PlaySound(explosionSound);
        fx.Play();
    }
}