/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour 
{
	public static LevelsManager Instance { get; private set; }

	[SerializeField]
	private List<string> m_levelNames;

	private void Awake () 
	{
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		if(SceneManager.GetActiveScene().name.Equals("Loader"))
			SceneManager.LoadScene("MainMenu");
	}

	public void NextLevel()
	{
		if(HasNextLevel())
			SceneManager.LoadScene(m_levelNames[GetCurrentLevelNumber() + 1]);
		else
			SceneManager.LoadScene("MainMenu");
	}

	private int GetCurrentLevelNumber()
	{
		return m_levelNames.IndexOf(SceneManager.GetActiveScene().name);
	}

	private bool HasNextLevel()
	{
		return (GetCurrentLevelNumber() < m_levelNames.Count-1);
	}
}
