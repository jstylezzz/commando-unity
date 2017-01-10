//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;

using System.Collections;
public class PersistenceScript : MonoBehaviour 
{
    /// <summary>
    /// Script entry point
    /// </summary>
    private void Start()
    {
        // Make sure there is only one GameMusic object
        GameObject[] existingSources = GameObject.FindGameObjectsWithTag("GameMusic");
        if(existingSources.Length > 1) Destroy(existingSources[1].transform.gameObject);
    }
    
    /// <summary>
    /// Called on awake
    /// </summary>
    private void Awake()
    {
        //Make our object persistent
        DontDestroyOnLoad(this.gameObject);
    }
}
