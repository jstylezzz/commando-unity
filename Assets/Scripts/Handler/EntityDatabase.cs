//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityDatabase : MonoBehaviour
{

    /// <summary>
    /// The enemy registry
    /// </summary>
    private List<GameObject> m_enemyRegistry = new List<GameObject>();

    /// <summary>
    /// The boss registry
    /// </summary>
    private List<GameObject> m_bossRegistry = new List<GameObject>();

    /// <summary>
    /// The player GameObject
    /// </summary>
    private GameObject m_thePlayer;

    /// <summary>
    /// Adds a new entity to the corresponding entityregistry
    /// </summary>
    /// <param name="type">The entity type to register</param>
    /// <param name="theEntity">The actual entity to register</param>
    public void RegisterEntity(m_entityTypes type, GameObject theEntity)
    {
        switch(type)
        {
            case m_entityTypes.TYPE_ENEMY:
            {
                m_enemyRegistry.Add(theEntity);
                break;
            }
            case m_entityTypes.TYPE_BOSSENEMY:
            {
                m_bossRegistry.Add(theEntity);
                break;
            }
            case m_entityTypes.TYPE_PLAYER:
            {
                if (m_thePlayer != null) Debug.LogError("A player entity was already registered! Something is going wrong...");
                else
                {
                    m_thePlayer = theEntity;
                }
                break;
            }   
        }
       
    }

    /// <summary>
    /// Removes an entity from the corresponding registry
    /// </summary>
    /// <param name="type">The entity type</param>
    /// <param name="theEntity">The actual entity</param>
    public void RemoveEntity(m_entityTypes type, GameObject theEntity)
    {
        switch(type)
        {
            case  m_entityTypes.TYPE_ENEMY:
            {
                m_enemyRegistry.Remove(theEntity);
                Destroy(theEntity);
                break;
            }
            case  m_entityTypes.TYPE_BOSSENEMY:
            {
                m_bossRegistry.Remove(theEntity);
                Destroy(theEntity);
                break;
            }
            case  m_entityTypes.TYPE_PLAYER:
            {
                if (m_thePlayer == null) Debug.LogError("A player entity was not yet registered! Something is going wrong...");
                else
                {
                    Destroy(m_thePlayer);
                    m_thePlayer = null;
                }
                break;
            }
        }
        

    }

    /// <summary>
    /// Getter for the amount of active bosses
    /// </summary>
    /// <returns>Amount of bosses in game</returns>
    public int GetActiveBosses()
    {
        return m_bossRegistry.Count;
    }

    /// <summary>
    /// Getter for the player entity
    /// </summary>
    /// <returns>The player entity from the entityregistry</returns>
    public GameObject GetPlayerEntity()
    {
        return m_thePlayer;
    }
}
