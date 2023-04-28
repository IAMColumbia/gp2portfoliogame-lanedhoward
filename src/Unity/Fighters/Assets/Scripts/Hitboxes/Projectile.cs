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

    //public int hits = 1;

    bool IHitboxResponder.CollidedWith(Collider2D collider)
    {
        if (projectileProperties == null) throw new Exception("Hitbox hit without a current attack");


        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            if (hurtbox.fighterParent == fighterParent) return false;

            HitReport report = hurtbox.fighterParent.GetHitWith(projectileProperties);

            //react to the hit report ???????

            bool successfulHit = report != HitReport.Whiff;

            if (successfulHit)
            {
                this.EndProjectile();
            }

            return successfulHit;
        }

        Projectile projectile = collider.GetComponent<Projectile>();

        if (projectile != null)
        {
            if (projectile.fighterParent == fighterParent) return false;

            projectile.EndProjectile();
            this.EndProjectile();
            return true;
        }

        return false;
    }

    public void StartProjectile()
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
        }
        gameObject.SetActive(true);
        projectileActive = true;

        Vector3 pos = originalParent.position;

        transform.SetParent(null);

        transform.position = pos;

        transform.localScale = new Vector3(1, 1, 1);

        //var otherXdir = Mathf.Sign(originalParent.lossyScale.x);
        //var myXdir = Mathf.Sign(transform.localScale.x);

        if (fighterParent.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT)
        {
            //transform.localScale.Set(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        hitbox.SetResponder(this);
        hitbox.OpenForCollision = true;
        hitbox._state = ColliderState.Open;

    }

    public void EndProjectile()
    {
        hitbox.OpenForCollision = false;
        gameObject.SetActive(false);
        projectileActive = false;
        //transform.SetPositionAndRotation(originalParent.transform.position, originalParent.transform.rotation);
        /*if (Mathf.Sign(originalParent.lossyScale.x) != Mathf.Sign(transform.lossyScale.x))
        {
            transform.localScale.Set(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }*/
        //transform.localScale.Set(1, 1, 1);
        transform.SetParent(originalParent, false);

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (projectileActive)
        {
            this.transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime * Mathf.Sign(transform.lossyScale.x), transform.position.y + velocity.y * Time.deltaTime, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            EndProjectile();
        }
    }
}
