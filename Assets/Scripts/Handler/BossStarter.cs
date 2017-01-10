//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class BossStarter : MonoBehaviour
{

    private GlobalData m_gData;
    
    [SerializeField]
    private GameObject m_doorOne;

    [SerializeField]
    private GameObject m_doorTwo;

    [SerializeField]
    private GameObject m_doorOpen;

    [SerializeField]
    private GameObject m_doorOpenTwo;

    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        m_gData = GameObject.FindObjectOfType<GlobalData>();
    }

    /// <summary>
    /// Called when something enters the triggerbox
    /// </summary>
    /// <param name="other">The collider that entered the triggerbox</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
        {
            if(m_gData.IsBossActive() == false)
            {
                Destroy(m_doorOne);
                Destroy(m_doorTwo);

                m_doorOpen.GetComponent<AudioSource>().Play();
                m_doorOpenTwo.GetComponent<AudioSource>().Play();

                m_doorOpen.GetComponent<SpriteRenderer>().enabled = true;
                m_doorOpenTwo.GetComponent<SpriteRenderer>().enabled = true;

                m_gData.ActivateBoss();
            }
        }
    }

}
