using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : SoundPlayer, IHitboxResponder
{
    public GameAttackProperties projectileProperties;
    public FighterMain fighterParent;
    public Transform originalParent;
    public Hitbox hitbox;

    public ParticleSystem particles;

    public Vector2 velocity;

    public bool projectileActive;

    public float lifetime = 20f;
    public float timer = 0f;

    public bool breakOnWallContact = true;
    public bool selfDamage = false;
    public bool breakOnHit = true;
    public bool breakOnCollisionWithPlayer = true;
    public bool breakOnCollisionWithOtherProjectile = true;

    public float maxHitDistance = 100f;

    public AudioClip fireballStartSound;

    //public int hits = 1;

    bool IHitboxResponder.CollidedWith(Collider2D collider, Vector3 hitPosition)
    {
        if (projectileProperties == null) throw new Exception("Hitbox hit without a current attack");


        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            if (hurtbox.fighterParent == fighterParent && !selfDamage) return false;
            if (Vector3.Distance(this.transform.position, collider.transform.position) > maxHitDistance) return false;

            HitReport report = hurtbox.fighterParent.GetHitWith(projectileProperties);

            //react to the hit report ???????

            // TODO: specific parried code? but it should work fine without any

            bool successfulHit = report != HitReport.Whiff;

            if (successfulHit)
            {
                fighterParent.PlaySound(fighterParent.hitSounds[projectileProperties.parent.hitSoundIndex]);

                if (report == HitReport.Hit)
                {
                    fighterParent.PlayHitVFX(hitPosition, projectileProperties);
                }
                else
                {
                    fighterParent.PlayBlockVFX(hitPosition, projectileProperties);
                }

                if (breakOnCollisionWithPlayer)
                {
                    this.EndProjectile();
                }
            }

            return successfulHit;
        }

        Projectile projectile = collider.GetComponent<Projectile>();

        if (projectile != null)
        {
            if (projectile.fighterParent == fighterParent) return false;

            if (this.breakOnCollisionWithOtherProjectile && projectile.breakOnCollisionWithOtherProjectile)
            {
                fighterParent.PlaySound(fighterParent.hitSounds[projectileProperties.parent.hitSoundIndex]);

                projectile.EndProjectile();
                this.EndProjectile();
                return true;
            }
        }

        return false;
    }

    public virtual void StartProjectile()
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
        }
        StartProjectile(originalParent.position);
    }

    public virtual void StartProjectile(Vector3 position)
    {

        gameObject.SetActive(true);
        projectileActive = true;

        PlaySound(fireballStartSound);

        bool facingLeft = fighterParent.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT;

        Vector3 pos = position;

        transform.SetParent(null);

        transform.position = pos;

        transform.localScale = new Vector3(1, 1, 1);


        if (facingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        hitbox.SetResponder(this);
        hitbox.OpenForCollision = true;
        hitbox._state = ColliderState.Open;

        fighterParent.GotHit += OnFighterGotHit;

        timer = 0f;

        if (particles != null)
        {
            particles.Play();
        }
    }

    public virtual void EndProjectile()
    {
        hitbox.OpenForCollision = false;
        gameObject.SetActive(false);
        projectileActive = false;
        this.transform.position = Vector3.zero;
        transform.SetParent(originalParent, false);

        fighterParent.GotHit -= OnFighterGotHit;

        if (particles != null)
        {
            particles.Stop();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (projectileActive)
        {
            this.transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime * Mathf.Sign(transform.lossyScale.x), transform.position.y + velocity.y * Time.deltaTime, transform.position.z);

            if (timer >= lifetime)
            {
                EndProjectile();
            }
            timer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breakOnWallContact)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                fighterParent.PlaySound(fighterParent.hitSounds[projectileProperties.parent.hitSoundIndex]);
                StartCoroutine(DelayEndProjectile(0.2f));
            }

        }
    }

    public virtual void OnFighterGotHit(object sender, EventArgs e)
    {
        if (projectileActive && breakOnHit)
        {
            EndProjectile();
        }
    }

    public virtual void SetMaterial(Material mat)
    {
        this.GetComponent<SpriteRenderer>().material = mat;

        if (particles != null)
        {
            var psrs = GetComponentsInChildren<ParticleSystemRenderer>();

            foreach (var ps in psrs)
            {
                ps.material = mat;
            }
        }
    }

    public IEnumerator DelayEndProjectile(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (projectileActive)
        {
            EndProjectile();
        }
    }
}
