//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class CustomFontWriter : MonoBehaviour
{

	// The sprite array
    private Sprite[] m_fontSprite;

    /// <summary>
    /// Enumerator for all letter locations in spritesheet
    /// </summary>
    private enum m_letter
    {
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        seven, 
        eight, 
        nine,
        bracketOpen,
        bracketClose,
        underScore,
        A,
        B,
        C,
        D,
        doublePoint,
        E,
        F,
        forwardSlash,
        G,
        H,
        heart,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        SPACE,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    /// <summary>
    /// Script entry point
    /// </summary>
	void Start ()
    {
        // Load all our sprites
        m_fontSprite = Resources.LoadAll<Sprite>("Prefabs/Visual/Letters/letterSprite");

    }

    /// <summary>
    /// Writes a string in the custom font
    /// </summary>
    /// <param name="screenPos">The place to start writing the string</param>
    /// <param name="text">The text to write</param>
    /// <returns>The gameobject with the object string</returns>
    public GameObject WriteString(Vector2 screenPos, string text)
    {
        GameObject letterPrefab = Resources.Load<GameObject>("Prefabs/Visual/LetterPrefab");
        GameObject textContainer = Instantiate(Resources.Load<GameObject>("Prefabs/Visual/TextContainer"));
        
        textContainer.transform.position = screenPos;
        textContainer.name = text;

        float letterOffset = 0.3F;
        for (int i = 0; i < text.Length; i++)
        {
            GameObject theLetter = Instantiate(letterPrefab);
            theLetter.GetComponent<SpriteRenderer>().sprite = GetLetterSprite(text[i]);
            theLetter.transform.position = screenPos + new Vector2(screenPos.x + (letterOffset * i), screenPos.y);
            theLetter.transform.parent = textContainer.transform;
            theLetter.name = text[i].ToString();
        }

        return textContainer;
    }

    /// <summary>
    /// Getter for the letter location in the spritesheet
    /// </summary>
    /// <param name="letter">The letter to return the sprite from</param>
    /// <returns>The letter sprite, underscore sprite if the character was invalid</returns>
    private Sprite GetLetterSprite(char letter)
    {
        switch(char.ToUpper(letter))
        {
            case 'A': return m_fontSprite[(int)m_letter.A];
            case 'B': return m_fontSprite[(int)m_letter.B];
            case 'C': return m_fontSprite[(int)m_letter.C];
            case 'D': return m_fontSprite[(int)m_letter.D];
            case 'E': return m_fontSprite[(int)m_letter.E];
            case 'F': return m_fontSprite[(int)m_letter.F];
            case 'G': return m_fontSprite[(int)m_letter.G];
            case 'H': return m_fontSprite[(int)m_letter.H];
            case 'I': return m_fontSprite[(int)m_letter.I];
            case 'J': return m_fontSprite[(int)m_letter.J];
            case 'K': return m_fontSprite[(int)m_letter.K];
            case 'L': return m_fontSprite[(int)m_letter.L];
            case 'M': return m_fontSprite[(int)m_letter.M];
            case 'N': return m_fontSprite[(int)m_letter.N];
            case 'O': return m_fontSprite[(int)m_letter.O];
            case 'P': return m_fontSprite[(int)m_letter.P];
            case 'Q': return m_fontSprite[(int)m_letter.Q];
            case 'R': return m_fontSprite[(int)m_letter.R];
            case 'S': return m_fontSprite[(int)m_letter.S];
            case 'T': return m_fontSprite[(int)m_letter.T];
            case 'U': return m_fontSprite[(int)m_letter.U];
            case 'V': return m_fontSprite[(int)m_letter.V];
            case 'W': return m_fontSprite[(int)m_letter.W];
            case 'X': return m_fontSprite[(int)m_letter.X];
            case 'Y': return m_fontSprite[(int)m_letter.Y];
            case 'Z': return m_fontSprite[(int)m_letter.Z];
            case ' ': return m_fontSprite[(int)m_letter.SPACE];
            case ':': return m_fontSprite[(int)m_letter.doublePoint];
            case '0': return m_fontSprite[(int)m_letter.zero];
            case '1': return m_fontSprite[(int)m_letter.one];
            case '2': return m_fontSprite[(int)m_letter.two];
            case '3': return m_fontSprite[(int)m_letter.three];
            case '4': return m_fontSprite[(int)m_letter.four];
            case '5': return m_fontSprite[(int)m_letter.five];
            case '6': return m_fontSprite[(int)m_letter.six];
            case '7': return m_fontSprite[(int)m_letter.seven];
            case '8': return m_fontSprite[(int)m_letter.eight];
            case '9': return m_fontSprite[(int)m_letter.nine];
            default: return m_fontSprite[(int)m_letter.underScore];
        }
    }

    /// <summary>
    /// Searches for the correct sprites for the time digits from the UI
    /// </summary>
    /// <param name="text">The time text</param>
    /// <returns>The sprites for the time digits</returns>
    public Sprite[] GetTimeDigitSprites(string text)
    {
        Sprite[] returnSprites = new Sprite[3];
        switch(text.Length)
        {
            case 1:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite(text[0]);
                break;
            }
            case 2:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite(text[0]);
                returnSprites[2] = GetLetterSprite(text[1]);
                break;
            }
            case 3:
            {
                returnSprites[0] = GetLetterSprite(text[0]);
                returnSprites[1] = GetLetterSprite(text[1]);
                returnSprites[2] = GetLetterSprite(text[2]);
                break;
            }
            default:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite('0');
                break;
            }
        }

        return returnSprites;
    }

    /// <summary>
    /// Searches for the correct sprites for the score digits from the UI
    /// </summary>
    /// <param name="text">The score text</param>
    /// <returns>The sprites for the score digits</returns>
    public Sprite[] GetScoreSprites(string text)
    {
        Sprite[] returnSprites = new Sprite[6];
        switch (text.Length)
        {
            case 1:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite('0');
                returnSprites[3] = GetLetterSprite('0');
                returnSprites[4] = GetLetterSprite('0');
                returnSprites[5] = GetLetterSprite(text[0]);
                break;
            }
            case 2:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite('0');
                returnSprites[3] = GetLetterSprite('0');
                returnSprites[4] = GetLetterSprite(text[0]);
                returnSprites[5] = GetLetterSprite(text[1]);
                break;
            }
            case 3:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite('0');
                returnSprites[3] = GetLetterSprite(text[0]);
                returnSprites[4] = GetLetterSprite(text[1]);
                returnSprites[5] = GetLetterSprite(text[2]);
                break;
            }
            case 4:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite(text[0]);
                returnSprites[3] = GetLetterSprite(text[1]);
                returnSprites[4] = GetLetterSprite(text[2]);
                returnSprites[5] = GetLetterSprite(text[3]);
                break;
            }
            case 5:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite(text[0]);
                returnSprites[2] = GetLetterSprite(text[1]);
                returnSprites[3] = GetLetterSprite(text[2]);
                returnSprites[4] = GetLetterSprite(text[3]);
                returnSprites[5] = GetLetterSprite(text[4]);
                break;
            }
            case 6:
            {
                returnSprites[0] = GetLetterSprite(text[0]);
                returnSprites[1] = GetLetterSprite(text[1]);
                returnSprites[2] = GetLetterSprite(text[2]);
                returnSprites[0] = GetLetterSprite(text[3]);
                returnSprites[1] = GetLetterSprite(text[4]);
                returnSprites[2] = GetLetterSprite(text[5]);
                break;
            }
            default:
            {
                returnSprites[0] = GetLetterSprite('0');
                returnSprites[1] = GetLetterSprite('0');
                returnSprites[2] = GetLetterSprite('0');
                returnSprites[3] = GetLetterSprite('0');
                returnSprites[4] = GetLetterSprite('0');
                returnSprites[5] = GetLetterSprite('0');
                break;
            }
        }

        return returnSprites;
    }
}
