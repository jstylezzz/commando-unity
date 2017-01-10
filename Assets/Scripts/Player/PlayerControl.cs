//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject m_theCamera;

    [SerializeField]
    private float m_moveSpeed;

    private Vector2 m_pMovement;
    private bool m_keyUp;
    private bool m_keyDown;
    private bool m_keyLeft;
    private bool m_keyRight;
    private m_moveDirections m_pDirection;
    private Animator m_playerAnimator;
    private bool m_isAlive;

    private const m_entityTypes m_entityType = m_entityTypes.TYPE_PLAYER;

    private EntityDatabase m_eDB;

    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        m_eDB = GameObject.FindObjectOfType<EntityDatabase>();
        m_pMovement = Vector2.zero;
        m_playerAnimator = GetComponent<Animator>();
        m_eDB.RegisterEntity(m_entityType, this.gameObject);
        m_isAlive = true;
    }

    /// <summary>
    /// Script main loop
    /// </summary>
    private void Update()
    {
        if(m_isAlive == true)
        {
            DoMovement();
            SetCamPos();
            if (Input.GetKeyDown(KeyCode.Space)) ShootBullet();
        }
           
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    private void SetCamPos()
    {
        if(transform.position.y > -4.13F && m_theCamera.transform.position.y < 8.62F) m_theCamera.transform.position = new Vector3(m_theCamera.transform.position.x, transform.position.y, m_theCamera.transform.position.z);
    }

    /// <summary>
    /// Calculates the movement things
    /// </summary>
    private void DoMovement()
    {
        m_pMovement = Vector2.zero;
        Vector2 currentPosition = transform.position;

        m_keyUp = Input.GetKey(KeyCode.W);
        m_keyDown = Input.GetKey(KeyCode.S);
        m_keyLeft = Input.GetKey(KeyCode.A);
        m_keyRight = Input.GetKey(KeyCode.D);

        SetDirection();

        switch (m_pDirection)
        {
            case m_moveDirections.Up: MovePlayer(new Vector2(m_pMovement.x, m_pMovement.y + m_moveSpeed));
                break;

            case m_moveDirections.Down:
                if (Camera.main.WorldToViewportPoint(transform.position).y > 0.06F) MovePlayer(new Vector2(m_pMovement.x, m_pMovement.y - m_moveSpeed));
                break;

            case m_moveDirections.Left:
                if(Camera.main.WorldToViewportPoint(transform.position).x > 0) MovePlayer(new Vector2(m_pMovement.x - m_moveSpeed, m_pMovement.y));
                break;

            case m_moveDirections.Right:
                if (Camera.main.WorldToViewportPoint(transform.position).x < 1) MovePlayer(new Vector2(m_pMovement.x + m_moveSpeed, m_pMovement.y));
                break;

            case m_moveDirections.UpRight:
                if (Camera.main.WorldToViewportPoint(transform.position).x < 1) MovePlayer(new Vector2(m_pMovement.x + m_moveSpeed, m_pMovement.y + m_moveSpeed));
                break;

            case m_moveDirections.UpLeft:
                if (Camera.main.WorldToViewportPoint(transform.position).x > 0) MovePlayer(new Vector2(m_pMovement.x - m_moveSpeed, m_pMovement.y + m_moveSpeed));
                break;

            case m_moveDirections.DownRight:
                if (Camera.main.WorldToViewportPoint(transform.position).x < 1 && Camera.main.WorldToViewportPoint(transform.position).y > 0.06F) MovePlayer(new Vector2(m_pMovement.x + m_moveSpeed, m_pMovement.y - m_moveSpeed));
                break;

            case m_moveDirections.DownLeft:
                if (Camera.main.WorldToViewportPoint(transform.position).x > 0 && Camera.main.WorldToViewportPoint(transform.position).y > 0.06F) MovePlayer(new Vector2(m_pMovement.x - m_moveSpeed, m_pMovement.y - m_moveSpeed));
                break;

            default: m_pDirection = m_moveDirections.None;
                break;
        }
        SetDirection();


    }

    /// <summary>
    /// Sets the direction variables
    /// </summary>
    private void SetDirection()
    {
        if (m_keyUp)
        {
            if (m_keyLeft) m_pDirection = m_moveDirections.UpLeft;
            else if (m_keyRight) m_pDirection = m_moveDirections.UpRight;
            else m_pDirection = m_moveDirections.Up;
        }
        else if(m_keyDown)
        {
            if (m_keyLeft) m_pDirection = m_moveDirections.DownLeft;
            else if (m_keyRight) m_pDirection = m_moveDirections.DownRight;
            else m_pDirection = m_moveDirections.Down;
        }
        else
        {
            if (m_keyLeft) m_pDirection = m_moveDirections.Left;
            else if (m_keyRight) m_pDirection = m_moveDirections.Right;
            else m_pDirection = m_moveDirections.None;
        }
        m_playerAnimator.SetFloat("Direction", (float)m_pDirection);
    }

    /// <summary>
    /// Does the actual movement
    /// </summary>
    /// <param name="newPos">The new position</param>
    private void MovePlayer(Vector2 newPos)
    {
        transform.Translate(newPos * Time.deltaTime);
    }

    /// <summary>
    /// Shoots a bullet
    /// </summary>
    private void ShootBullet()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/bullet");

        GameObject theBullet = Instantiate(bulletPrefab);
        theBullet.GetComponent<BulletController>().SetOwner(m_entityType);
        theBullet.transform.position = transform.position;
        theBullet.GetComponent<BulletController>().SetDirection(m_pDirection);
       
    }

    /// <summary>
    /// Kill the player
    /// </summary>
    public void Death()
    {
        m_isAlive = false;
        GameObject.FindObjectOfType<GlobalData>().StopTime();
        AudioClip dieClip = Resources.Load<AudioClip>("Audio/gameOver");
        GetComponent<AudioSource>().PlayOneShot(dieClip);
        GetComponent<PolygonCollider2D>().enabled = false;
        m_playerAnimator.Play("Dead");
        StartCoroutine(DelayedRemoval());
    }

    /// <summary>
    /// Removes the entity after a short period of time
    /// </summary>
    private IEnumerator DelayedRemoval()
    {
        yield return new WaitForSeconds(1F);
        SceneManager.LoadScene("mainMenu");
    }

    /// <summary>
    /// Called when something collides with the player
    /// </summary>
    /// <param name="coll">The collider</param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag.Equals("Bullet"))
        {
            if (coll.gameObject.GetComponent<BulletController>().GetOwner().Equals("Enemy")) Death();
        }
    }

    /// <summary>
    /// Getter for if the player is alive or not
    /// </summary>
    /// <returns>True if player is alive, false if not</returns>
    public bool IsPlayerAlive()
    {
        return m_isAlive;
    }
}
