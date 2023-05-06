using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IHitboxResponder
{
    public GameAttackProperties projectileProperties;
    public FighterMain fighterParent;
    public Transform originalParent;
    public Hitbox hitbox;

    public Vector2 velocity;

    public bool projectileActive;

    public float lifetime = 20f;
    public float timer = 0f;

    public bool breakOnWallContact = true;
    public bool selfDamage = false;
    public bool breakOnHit = true;

    public float maxHitDistance = 100f;

    //public int hits = 1;

    bool IHitboxResponder.CollidedWith(Collider2D collider)
    {
        if (projectileProperties == null) throw new Exception("Hitbox hit without a current attack");


        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            if (hurtbox.fighterParent == fighterParent && !selfDamage) return false;
            if (Vector3.Distance(this.transform.position, collider.transform.position) > maxHitDistance) return false;

            HitReport report = hurtbox.fighterParent.GetHitWith(projectileProperties);

            //react to the hit report ???????

            bool successfulHit = report != HitReport.Whiff;

            if (successfulHit)
            {
                fighterParent.PlaySound(fighterParent.hitSounds[projectileProperties.parent.hitSoundIndex]);
                this.EndProjectile();
            }

            return successfulHit;
        }

        Projectile projectile = collider.GetComponent<Projectile>();

        if (projectile != null)
        {
            if (projectile.fighterParent == fighterParent) return false;

            fighterParent.PlaySound(fighterParent.hitSounds[projectileProperties.parent.hitSoundIndex]);

            projectile.EndProjectile();
            this.EndProjectile();
            return true;
        }

        return false;
    }

    public void StartProjectile()
    {
        StartProjectile(Vector3.zero);
    }

    public void StartProjectile(Vector3 offset)
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
        }
        gameObject.SetActive(true);
        projectileActive = true;

        bool facingLeft = fighterParent.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT;

        if (facingLeft)
        {
            offset.x *= -1;
        }

        Vector3 pos = originalParent.position + offset;

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
    }

    public void EndProjectile()
    {
        hitbox.OpenForCollision = false;
        gameObject.SetActive(false);
        projectileActive = false;
        transform.SetParent(originalParent, false);

        fighterParent.GotHit -= OnFighterGotHit;

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
                EndProjectile();
            }

        }
    }

    public void OnFighterGotHit(object sender, EventArgs e)
    {
        if (projectileActive && breakOnHit)
        {
            EndProjectile();
        }
    }

    public void SetMaterial(Material mat)
    {
        this.GetComponent<SpriteRenderer>().material = mat;
    }
}
