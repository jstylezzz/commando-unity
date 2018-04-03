//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameFinal : MonoBehaviour
{
    /// <summary>
    /// Called when something collides with the collider
    /// </summary>
    /// <param name="coll">The colliding object's collider</param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.transform.tag.Equals("Player"))
        {
            Destroy(GameObject.FindGameObjectWithTag("GameMusic"));
            this.transform.GetComponent<AudioSource>().Play();
            Destroy(coll.gameObject);
            StartCoroutine(EndGame());
        }  
    }

    /// <summary>
    /// Ends the game after a short delay (default 3 seconds)
    /// </summary>
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3F);
		LevelsManager.Instance.NextLevel();
    }
}
