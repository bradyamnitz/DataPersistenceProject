using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static string PlayerName;
	public static int Score = 0;
	public static string HighScoreName = "Nobody";
	public static int HighScoreValue = 0;

	// Start is called before the first frame update
	private void Awake()
	{
		if(Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		LoadHighScore();
	}

	public void SetName(string name)
	{
		PlayerName = name;
	}

	public void playGame()
	{
		SceneManager.LoadScene(1);
	}

	public void exitGame()
	{
		SaveHighScore();
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
	}

	[System.Serializable]
	class SaveData
	{
		public string highScoreName;
		public int highScoreValue;
	}

	public static void SaveHighScore()
	{
		
		if (Score > HighScoreValue)
		{
			SaveData data = new SaveData();
			data.highScoreValue = Score;
			data.highScoreName = PlayerName;
			string json = JsonUtility.ToJson(data);
			File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
		}

		
	}

	public static void LoadHighScore()
	{
		string path = Application.persistentDataPath + "/savefile.json";
		if (File.Exists(path))
		{
			string json = File.ReadAllText(path);
			SaveData data = JsonUtility.FromJson<SaveData>(json);

			HighScoreName = data.highScoreName;
			HighScoreValue = data.highScoreValue;
		}
	}
}
