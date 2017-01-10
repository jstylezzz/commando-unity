//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    // Despawn Delay
    [SerializeField]
    private float m_dDelay;

    [SerializeField]
    private float m_bulletSpeed;

    [SerializeField]
    private m_bulletTypes m_bulletType;

    private AudioClip m_impactSound;

    private enum m_bulletTypes
    {
        enemy,
        cannon
    }


    private m_moveDirections m_bulletDirection;
    private m_entityTypes m_bulletOwner;
    private Vector2 m_movement;

    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        StartCoroutine(RemoveBullet());
        m_impactSound = Resources.Load<AudioClip>("Audio/bulletImpact");
    }

	/// <summary>
    /// Script main loop
    /// </summary>
	void Update()
    {
        MoveBullet();
	}

    /// <summary>
    /// Sets the owner of the bullet
    /// </summary>
    /// <param name="entity">Entity type that shot the bullet</param>
    public void SetOwner(m_entityTypes entity)
    {
        m_bulletOwner = entity;
    }

    /// <summary>
    /// Sets the direction of the bullet
    /// </summary>
    /// <param name="direction">The direction the bullet should follow</param>
    public void SetDirection(m_moveDirections direction)
    {
        if (direction == m_moveDirections.None) m_bulletDirection = m_moveDirections.Up;
        else m_bulletDirection = direction;
    }

    /// <summary>
    /// Moves the bullet
    /// </summary>
    private void MoveBullet()
    {
        m_movement = Vector2.zero;

        switch (m_bulletDirection)
        {
            case m_moveDirections.Up:
            {
                m_movement.y += m_bulletSpeed;
                break;
            }
            case m_moveDirections.UpRight:
            {
                m_movement.y += m_bulletSpeed;
                m_movement.x += m_bulletSpeed;
                break;
            }
            case m_moveDirections.Right:
            {
                m_movement.x += m_bulletSpeed;
                break;
            }
            case m_moveDirections.DownRight:
            {
                m_movement.x += m_bulletSpeed;
                m_movement.y -= m_bulletSpeed;
                break;
            }
            case m_moveDirections.Down:
            {
                m_movement.y -= m_bulletSpeed;
                break;
            }
            case m_moveDirections.DownLeft:
            {
                m_movement.x -= m_bulletSpeed;
                m_movement.y -= m_bulletSpeed;
                break;
            }
            case m_moveDirections.Left:
            {
                m_movement.x -= m_bulletSpeed;
                break;
            }
            case m_moveDirections.UpLeft:
            {
                m_movement.x -= m_bulletSpeed;
                m_movement.y += m_bulletSpeed;
                break;
            }
            
        }

        transform.Translate(m_movement * Time.deltaTime);

    }

    /// <summary>
    /// Getter for the bullet owner
    /// </summary>
    /// <returns>The entity type in a string</returns>
    public string GetOwner()
    {
        switch (m_bulletOwner)
        {
            case m_entityTypes.TYPE_ENEMY:
                return "Enemy";

            case m_entityTypes.TYPE_PLAYER:
                return "Player";
            case m_entityTypes.TYPE_BOSSENEMY:
                return "Enemy";
        }

        return "ERROR";
    }

    /// <summary>
    /// Removes the bullet after a short while
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveBullet()
    {
        yield return new WaitForSeconds(m_dDelay);
        if (m_bulletType == m_bulletTypes.enemy)
        {
            GetComponent<Animator>().Play("bulletExplosion");
            StartCoroutine(DelayedRemoval(0.3F));
        }
        else Destroy(this.gameObject);
        
    }

    /// <summary>
    /// Called when the object collides with a collider
    /// </summary>
    /// <param name="coll">The other collider</param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        string entity = "NONE";
        entity = GetOwner();

        if (!coll.collider.tag.Equals(entity))
        {
            StopCoroutine(RemoveBullet());
            if (m_bulletType == m_bulletTypes.enemy)
            {
                GetComponent<Animator>().Play("bulletExplosion");
                StartCoroutine(DelayedRemoval(0.3F));
                GetComponent<AudioSource>().PlayOneShot(m_impactSound);
            }
            else Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Removes the bullet after the specified delay
    /// </summary>
    /// <param name="delay">The time in seconds to wait before removing the bullet</param>
    private IEnumerator DelayedRemoval(float delay = 0)
    {
        m_bulletSpeed = 0;
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

}
