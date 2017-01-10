//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class UIDraw : MonoBehaviour
{
    private CustomFontWriter m_cFW;

    private GameObject m_currentPoints;

    private GameObject m_timeLeftText;
    
    private int m_timeLeft;

    private Vector2 m_timePos = new Vector2(-1.6F, -1.0F);

    private Vector2 m_scorePos = new Vector2(0.2F, -1.0F);

    private int m_score;

	/// <summary>
	/// Script entry point
	/// </summary>
	void Start ()
    {
        m_cFW = GameObject.FindObjectOfType<CustomFontWriter>();
        m_currentPoints = m_cFW.WriteString(m_scorePos, "SCORE: 000000");
        m_currentPoints.transform.localScale = new Vector2(0.2F, 0.2F);
        m_currentPoints.transform.parent = this.transform;
        for (int i = 0; i < m_currentPoints.transform.childCount; i++)
        {
            m_currentPoints.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = "UI";    
        }

        m_timeLeftText = m_cFW.WriteString(m_timePos, "Time Left: 120");
        m_timeLeftText.transform.localScale = new Vector2(0.2F, 0.2F);
        m_timeLeftText.transform.parent = this.transform;
        for (int i = 0; i < m_timeLeftText.transform.childCount; i++)
        {
            m_timeLeftText.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        }
        

	}
	
    /// <summary>
    /// Script main loop
    /// </summary>
    private void Update()
    {
        // Sprites for time
        GameObject[] timeDigits = new GameObject[3];
        int childCountTime = m_timeLeftText.transform.childCount;
        timeDigits[0] = m_timeLeftText.transform.GetChild(childCountTime - 3).gameObject;
        timeDigits[1] = m_timeLeftText.transform.GetChild(childCountTime - 2).gameObject;
        timeDigits[2] = m_timeLeftText.transform.GetChild(childCountTime - 1).gameObject;

        Sprite[] spriteDigits = m_cFW.GetTimeDigitSprites(m_timeLeft.ToString());

        timeDigits[0].GetComponent<SpriteRenderer>().sprite = spriteDigits[0];
        timeDigits[1].GetComponent<SpriteRenderer>().sprite = spriteDigits[1];
        timeDigits[2].GetComponent<SpriteRenderer>().sprite = spriteDigits[2];

        // Sprites for score
        GameObject[] scoreDigits = new GameObject[6];
        int childCountScore = m_currentPoints.transform.childCount;
        scoreDigits[0] = m_currentPoints.transform.GetChild(childCountScore - 6).gameObject;
        scoreDigits[1] = m_currentPoints.transform.GetChild(childCountScore - 5).gameObject;
        scoreDigits[2] = m_currentPoints.transform.GetChild(childCountScore - 4).gameObject;
        scoreDigits[3] = m_currentPoints.transform.GetChild(childCountScore - 3).gameObject;
        scoreDigits[4] = m_currentPoints.transform.GetChild(childCountScore - 2).gameObject;
        scoreDigits[5] = m_currentPoints.transform.GetChild(childCountScore - 1).gameObject;

        Sprite[] scoreDigitSprites = m_cFW.GetScoreSprites(m_score.ToString());

        scoreDigits[0].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[0];
        scoreDigits[1].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[1];
        scoreDigits[2].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[2];
        scoreDigits[3].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[3];
        scoreDigits[4].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[4];
        scoreDigits[5].GetComponent<SpriteRenderer>().sprite = scoreDigitSprites[5];
    }

    /// <summary>
    /// Updates the time variable
    /// </summary>
    /// <param name="time">The new time</param>
    public void UpdateTimeLeft(int time)
    {
        m_timeLeft = time;
    }

    /// <summary>
    /// Updates the score variable
    /// </summary>
    /// <param name="score">The new score amount</param>
    public void UpdateScore(int score)
    {
        m_score = score;
    }
}
