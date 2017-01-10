//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScreenHandler : MonoBehaviour
{	
    
    [SerializeField]
    private float m_maxTextSize;

    [SerializeField]
    private float m_minTextSize;

    [SerializeField]
    private float m_textSizeSpeed;

    [SerializeField]
    private float m_shiftStrength;

    [SerializeField]
    private float m_maxShift;

    [SerializeField]
    private string m_writeText;

    private int m_sizeDirection;

    private float m_shiftedAmount;

    private int m_shiftDirection;

    private GameObject m_startText;

    /// <summary>
    /// Script's entry point
    /// </summary>
    private void Start() 
    {
        StartCoroutine(WriteNow());
        m_sizeDirection = 1;
        m_shiftDirection = 1;
	}
	
	/// <summary>
	/// Script's main loop
	/// </summary>
    private void Update() 
    {
        if (m_startText != null)
        {
            ChangeSize();
            ChangePos();
            StartCoroutine(ChangeTextColor());
            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("levelOne");
        }
	}

    /// <summary>
    /// Shifts the text's position
    /// </summary>
    private void ChangePos()
    {
        if(m_shiftDirection == 1)
        {
            if (m_shiftedAmount >= m_maxShift) m_shiftDirection = 0;
            else
            {
                Vector2 newPos = new Vector2(m_startText.transform.position.x + m_shiftStrength, m_startText.transform.position.y);
                m_startText.transform.position = newPos;
                m_shiftedAmount += m_shiftStrength;
            }
        }
        else if(m_shiftDirection == 0)
        {
            if (m_shiftedAmount <= (m_maxShift * -1)) m_shiftDirection = 1;
            else
            {
                Vector2 newPos = new Vector2(m_startText.transform.position.x - m_shiftStrength, m_startText.transform.position.y);
                m_startText.transform.position = newPos;
                m_shiftedAmount -= m_shiftStrength;
            }
        }
    }

    /// <summary>
    /// Shifts the text's scale
    /// </summary>
    private void ChangeSize()
    {
        if(m_sizeDirection == 1)
        {
            if (m_startText.transform.localScale.x >= m_maxTextSize) m_sizeDirection = 0;
            else
            {
                Vector2 newSize = new Vector2(m_startText.transform.localScale.x + m_textSizeSpeed, m_startText.transform.localScale.y + m_textSizeSpeed);
                m_startText.transform.localScale = newSize;
            }
        }
        else if (m_sizeDirection == 0)
        {
            if (m_startText.transform.localScale.x <= m_minTextSize) m_sizeDirection = 1;
            else
            {
                Vector2 newSize = new Vector2(m_startText.transform.localScale.x - m_textSizeSpeed, m_startText.transform.localScale.y - m_textSizeSpeed);
                m_startText.transform.localScale = newSize;
            }
        }
    }

    /// <summary>
    /// Write function, delayed a bit to allow resource loading..
    /// </summary>
    private IEnumerator WriteNow()
    {
        yield return new WaitForSeconds(0.3F);
        m_startText = GameObject.FindObjectOfType<CustomFontWriter>().WriteString(new Vector2(-1.1F, -1F), m_writeText);
    }

    /// <summary>
    /// Changes the text color
    /// </summary>
    private IEnumerator ChangeTextColor()
    {
        Color[] colorList = { Color.red, Color.magenta, Color.grey, Color.blue, Color.green };
        int randCol = Random.Range(0, colorList.Length);
        float randTime = Random.Range(4.5F, 5.5F);
        for (int i = 0; i < m_startText.transform.childCount; i++) m_startText.transform.GetChild(i).GetComponent<SpriteRenderer>().material.color = colorList[randCol];

        
        yield return new WaitForSeconds(randTime);
        StartCoroutine(ChangeTextColor());
        
    }
    
}
