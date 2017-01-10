//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_xMoveRange;

    [SerializeField]
    private float m_yMoveRange;

    [SerializeField]
    private float m_attackRange;

    [SerializeField]
    private float m_shootCooldown;

    [SerializeField]
    private m_entityTypes m_entityType = m_entityTypes.TYPE_ENEMY; //The entity type ID, 0 is enemy (by default)

    private Vector2 m_movement;

    private float m_currentMoveX;

    private float m_currentMoveY;

    private m_moveDirections m_lookDirection;

    private m_moveDirections m_currentDirectionX;

    private m_moveDirections m_currentDirectionY;

    private EntityDatabase m_eDB;

    private GlobalData m_gData;

    private Animator m_enemyAnimator;

    private bool m_canShootBullet;

    private bool m_isAlive;

    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        m_eDB = GameObject.FindObjectOfType<EntityDatabase>();
        m_gData = GameObject.FindObjectOfType<GlobalData>();
        m_enemyAnimator = GetComponent<Animator>();

        m_eDB.RegisterEntity(m_entityType, this.gameObject);
        m_currentMoveX = 0;
        m_currentMoveY = 0;

        

        if(m_entityType == m_entityTypes.TYPE_ENEMY)
        {
            int randChance = Random.Range(0, 2);

            if (m_xMoveRange != 0 && randChance == 0) m_currentDirectionX = m_moveDirections.Left;
            else if (m_xMoveRange != 0 && randChance == 1) m_currentDirectionX = m_moveDirections.Right;
            else m_currentDirectionX = m_moveDirections.None;

            if (m_yMoveRange != 0 && randChance == 0) m_currentDirectionY = m_moveDirections.Up;
            else if (m_yMoveRange != 0 && randChance == 1) m_currentDirectionY = m_moveDirections.Down;
            else m_currentDirectionY = m_moveDirections.None;
        }
        else if(m_entityType == m_entityTypes.TYPE_BOSSENEMY)
        {
            if (m_yMoveRange != 0) m_currentDirectionY = m_moveDirections.Down;
            else m_currentDirectionY = m_moveDirections.None;

            m_currentDirectionX = m_moveDirections.None;
        }
        

        m_canShootBullet = true;
        m_isAlive = true;

    }

    /// <summary>
    /// Script main loop
    /// </summary>
    private void Update()
    {
        if(m_eDB.GetPlayerEntity() != null)
        {
            if (m_isAlive == true && m_eDB.GetPlayerEntity().GetComponent<PlayerControl>().IsPlayerAlive() == true)
            {
                if (m_entityType == m_entityTypes.TYPE_BOSSENEMY && m_gData.IsBossActive() == true) DoEnemyAction();
                else if (m_entityType == m_entityTypes.TYPE_ENEMY) DoEnemyAction();
            }
            if (m_currentDirectionX == m_moveDirections.None && m_currentDirectionY == m_moveDirections.None) m_enemyAnimator.speed = 0;
            else m_enemyAnimator.speed = 1;
        }
    }

    /// <summary>
    /// Do the thing the enemy should do. Either walking or shooting
    /// </summary>
    private void DoEnemyAction()
    {
        
        if (GetDistanceFromPlayer() > m_attackRange)
        {
            m_enemyAnimator.speed = 1;
            WalkPath();
        }
        else if (GetDistanceFromPlayer() <= m_attackRange && m_canShootBullet == true)
        {
            m_lookDirection = GetPlayerDirection();
            m_enemyAnimator.speed = 0;
            ShootBullet();
            m_enemyAnimator.SetFloat("Direction", (float)m_lookDirection);
        }
    }

    /// <summary>
    /// Apply a cooldown period on the shooting feature
    /// </summary>
    private IEnumerator ShotCooldown()
    {
        m_canShootBullet = false;
        yield return new WaitForSeconds(m_shootCooldown);
        m_canShootBullet = true;
    }

    /// <summary>
    /// Shoot a bullet
    /// </summary>
    private void ShootBullet()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/bullet");
        GameObject theBullet = Instantiate(bulletPrefab);
        theBullet.GetComponent<BulletController>().SetOwner(m_entityType);
        theBullet.transform.position = transform.position;
        theBullet.GetComponent<BulletController>().SetDirection(m_lookDirection);
        StartCoroutine(ShotCooldown());
    }

    /// <summary>
    /// Get the direction the player is relative to the enemy
    /// </summary>
    /// <returns>The relative direction the player is for the enemy</returns>
    private m_moveDirections GetPlayerDirection()
    {
        Vector2 playerPos = m_eDB.GetPlayerEntity().transform.position;
        float offset = 0.2F;

        // Player is above enemy
        if(playerPos.y > transform.position.y + offset)
        {
            if (playerPos.x > transform.position.x + offset) return m_moveDirections.UpRight;
            else if (playerPos.x < transform.position.x - offset) return m_moveDirections.UpLeft;
            else return m_moveDirections.Up;
        }
        // Player is below enemy
        else if(playerPos.y < transform.position.y - offset)
        {
            if (playerPos.x > transform.position.x + offset) return m_moveDirections.DownRight;
            else if (playerPos.x < transform.position.x - offset) return m_moveDirections.DownLeft;
            else return m_moveDirections.Down;
        }
        // Player is on the same Y as we (the enemy) are
        else
        {
            if (playerPos.x > transform.position.x) return m_moveDirections.Right;
            else if (playerPos.x < transform.position.x) return m_moveDirections.Left;
        }

        //Something is wrong..
        return m_moveDirections.None;
    }

    /// <summary>
    /// Gets the amount of distance between the player and the enemy
    /// </summary>
    /// <returns>Amount of distance between player and enemy</returns>
    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, m_eDB.GetPlayerEntity().transform.position);
    }

    /// <summary>
    /// Makes the enemy walk over it's predefined move range
    /// </summary>
    private void WalkPath()
    {
        m_movement = Vector2.zero;
        if (m_xMoveRange != 0)
        {
            if (m_currentDirectionX == m_moveDirections.Left)
            {
                m_movement.x -= m_moveSpeed;
                m_currentMoveX -= m_moveSpeed;
                if (m_currentMoveX <= m_xMoveRange * -1)
                {
                    if (m_entityType == m_entityTypes.TYPE_BOSSENEMY) m_currentDirectionX = m_moveDirections.None;
                    else m_currentDirectionX = m_moveDirections.Right;
                }
            }
            else if (m_currentDirectionX == m_moveDirections.Right)
            {
                m_movement.x += m_moveSpeed;
                m_currentMoveX += m_moveSpeed;
                if (m_currentMoveX >= m_xMoveRange)
                {
                    if (m_entityType == m_entityTypes.TYPE_BOSSENEMY) m_currentDirectionX = m_moveDirections.None;
                    else m_currentDirectionX = m_moveDirections.Left;
                }
            }
        }

        if (m_yMoveRange != 0)
        {
            if (m_currentDirectionY == m_moveDirections.Up)
            {
                m_movement.y += m_moveSpeed;
                m_currentMoveY += m_moveSpeed;
                if (m_currentMoveY >= m_yMoveRange)
                {
                    if(m_entityType == m_entityTypes.TYPE_BOSSENEMY)m_currentDirectionY = m_moveDirections.None;
                    else m_currentDirectionY = m_moveDirections.Down;
                } 
            }
            else if (m_currentDirectionY == m_moveDirections.Down)
            {
                m_movement.y -= m_moveSpeed;
                m_currentMoveY -= m_moveSpeed;
                if (m_currentMoveY <= m_yMoveRange * -1)
                {
                    if (m_entityType == m_entityTypes.TYPE_BOSSENEMY) m_currentDirectionY = m_moveDirections.None;
                    else m_currentDirectionY = m_moveDirections.Up;
                }
            }
        }
        transform.Translate(m_movement * Time.deltaTime);
        SetLookDirection();
    }

    /// <summary>
    /// Sets the looking direction
    /// </summary>
    private void SetLookDirection()
    {
        if (m_currentDirectionY == m_moveDirections.Up)
        {
            if (m_currentDirectionX == m_moveDirections.Left) m_lookDirection = m_moveDirections.UpLeft;
            else if (m_currentDirectionX == m_moveDirections.Right) m_lookDirection = m_moveDirections.UpRight;
            else m_lookDirection = m_moveDirections.Up;
        }
        else if (m_currentDirectionY == m_moveDirections.Down)
        {
            if (m_currentDirectionX == m_moveDirections.Left) m_lookDirection = m_moveDirections.DownLeft;
            else if (m_currentDirectionX == m_moveDirections.Right) m_lookDirection = m_moveDirections.DownRight;
            else m_lookDirection = m_moveDirections.Down;
        }
        else
        {
            if (m_currentDirectionX == m_moveDirections.Left) m_lookDirection = m_moveDirections.Left;
            else if (m_currentDirectionX == m_moveDirections.Right) m_lookDirection = m_moveDirections.Right;
            else if (m_currentDirectionX == m_moveDirections.None) m_lookDirection = m_moveDirections.None;
        }

        m_enemyAnimator.SetFloat("Direction", (float)m_lookDirection);
    }

    /// <summary>
    /// Getter for looking direction
    /// </summary>
    /// <returns></returns>
    private m_moveDirections GetLookDirection()
    {
        return m_lookDirection;
    }

    /// <summary>
    /// Kills the enemy
    /// </summary>
    public void Death()
    {
        if(m_entityType == m_entityTypes.TYPE_ENEMY)  m_gData.AddScore(100);
        else if (m_entityType == m_entityTypes.TYPE_BOSSENEMY) m_gData.AddScore(1500);

        GetComponent<AudioSource>().Play();
        m_isAlive = false;
        m_enemyAnimator.Play("Dead");
        StartCoroutine(DelayedRemoval());
        GetComponent<PolygonCollider2D>().enabled = false;
        
    }

    /// <summary>
    /// Removes the entity after a short period of time
    /// </summary>
    private IEnumerator DelayedRemoval()
    {
        yield return new WaitForSeconds(1F);
        m_eDB.RemoveEntity(m_entityType, this.gameObject);
    }

    /// <summary>
    /// Called when something collides with the enemy
    /// </summary>
    /// <param name="coll">The collider</param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag.Equals("Bullet"))
        {
            if (coll.gameObject.GetComponent<BulletController>().GetOwner().Equals("Player")) Death();
        }
    }
}