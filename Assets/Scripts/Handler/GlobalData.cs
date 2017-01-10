//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//


using UnityEngine;
using System.Collections;



/// <summary>
/// Global Enumerators
/// </summary>
public enum m_moveDirections
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft,
    None
}


/// <summary>
/// Enum with entity types
/// </summary>
public enum m_entityTypes
{
    TYPE_ENEMY,
    TYPE_PLAYER,
    TYPE_BOSSENEMY
}


public class GlobalData : MonoBehaviour 
{
    [SerializeField]
    private int m_levelTime;

    // Is enemy boss wave active?
    private bool m_bossActive;

    private EntityDatabase m_eDB;

    private int m_score;

    private UIDraw m_uID;

    private bool m_tickTime;

    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        m_uID = GameObject.FindObjectOfType<UIDraw>();
        m_eDB = GameObject.FindObjectOfType<EntityDatabase>();
        m_bossActive = false;
        m_score = 0;
        m_tickTime = true;
        StartCoroutine(LevelTimer());
    }

    /// <summary>
    /// The Leveltimer
    /// </summary>
    private IEnumerator LevelTimer()
    {
        if(m_tickTime == true)
        {
            yield return new WaitForSeconds(1F);
            --m_levelTime;
            m_uID.UpdateTimeLeft(m_levelTime);
            if (m_levelTime > 0) StartCoroutine(LevelTimer());
            else m_eDB.GetPlayerEntity().GetComponent<PlayerControl>().Death(); ;
        }
    }

    /// <summary>
    /// Script main loop
    /// </summary>
    private void Update()
    {
        if (m_bossActive == true) CheckGameWon();
    }

    /// <summary>
    /// Checks if the bosses are all defeated, and thus, if the game should end
    /// </summary>
    private void CheckGameWon()
    {
        //Game won!
        if (m_eDB.GetActiveBosses() == 0)
        {
            GameObject.FindObjectOfType<GameFinal>().GetComponent<SpriteRenderer>().enabled = true;
            GameObject.FindObjectOfType<GameFinal>().GetComponent<EdgeCollider2D>().enabled = true;
        }
    }

    /// <summary>
    /// Getter for whether the boss wave is active
    /// </summary>
    /// <returns>True if bosswave is active, false if not</returns>
    public bool IsBossActive()
    {
        return m_bossActive;
    }

    /// <summary>
    /// Activates the bosswave
    /// </summary>
    public void ActivateBoss()
    {
        m_bossActive = true;
    }

    /// <summary>
    /// Add score
    /// </summary>
    /// <param name="score">Amount of score to add</param>
    public void AddScore(int score)
    {
        m_score += score;
        m_uID.UpdateScore(m_score);
    }

    /// <summary>
    /// Stops the clock
    /// </summary>
    public void StopTime()
    {
        m_tickTime = false;
    }

}
