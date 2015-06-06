using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Used for storing fields relevant
/// to a single game session.
/// </summary>
/// 
public class SaveGameData {
	internal SaveGameContainer data;
	public string ship { get { return data.ship; } set { data.ship = value; } }
	public SaveGameData() {
		data = new SaveGameContainer ();
	}
	internal SaveGameData(SaveGameContainer c) {
		data = c;
	}
}

/// <summary>
/// Used to hide the ablility to save the data to disk,
/// preventing misuse.
/// </summary>
[System.Serializable]
internal class SaveGameContainer {
	public string ship;
	internal SaveGameContainer () : this("") {
	}

	internal SaveGameContainer(string s) {
		this.ship = s;
	}
	
}


/// <summary>
/// Used to identify a save file
/// </summary>
public class SaveGameFileHandle
{
	public string niceName { get; private set; }
	public string trueName { get; private set; }
	internal SaveGameFileHandle(string trueName) {
		this.trueName = trueName;
		this.niceName = trueName.TrimEnd(SaveGame.saveEndingAsCharArray);
	}
}

public class CurrentSlot
{
	public SaveGameData data { get { return c; } }
	
	/// <summary>
	/// The name of the current save file in nice format ("foo")
	/// </summary>
	public string currentSlotName {
		get { return n.niceName; }
	}
	
	/// <summary>
	/// The name of the current save file in true format ("foo.save")
	/// </summary>
	public string currentTrueName {
		get { return n.trueName; }
	}
	
	/// <summary>
	/// Whether we have a current SaveGameData
	/// </summary>
	public bool currentValid { get { return valid; } }
	
	internal void SetCurrent(SaveGameData data, SaveGameFileHandle name) {
		c = data;
		valid = true;
		n = name;
	}
	
	/// <summary>
	/// The name of the current save file in true format ("foo.save")
	/// </summary>
	private SaveGameFileHandle n = null;
	private bool valid = false;
	private SaveGameData c = null;
}




public class SaveGame {
	internal static readonly string saveFileEndingName = ".save";
	internal static readonly char[] saveEndingAsCharArray = saveFileEndingName.ToCharArray();
	public static CurrentSlot current {
		get { return cur; }
	}
	private static CurrentSlot cur = new CurrentSlot();

	/// <summary>
	/// Ensures no funny business happens to the saveName;
	/// </summary>
	///



	

	/// <summary>
	/// Get a list of all available save files.
	/// </summary>
	/// <returns>The available saves.</returns>
	public static SaveGameFileHandle[] ListAvailableSaves() {
		string[] arr = Directory.GetFiles (Application.persistentDataPath, "*" + saveFileEndingName);
		SaveGameFileHandle[] arrtrim = new SaveGameFileHandle[arr.Length];
		for (int i = 0; i < arrtrim.Length; i++) {
			arrtrim[i] = new SaveGameFileHandle(arr[i].Substring(arr[i].LastIndexOfAny(new char[]{'/', '\\'}) + 1));//arr[i].TrimEnd(saveEndingAsCharArray);
		}
		return arrtrim;
	}

	/// <summary>
	/// Saves the current SaveGameData to disk.
	/// </summary>
	public static void Save() {
		if (current.currentValid) {
			SaveFile.SaveData<SaveGameContainer> (current.currentTrueName, current.data.data);
		} else {
			Debug.LogError("Current is empty! Cannot save.");
		}
	}

	public static void CreateEmpty(string niceName) {
		current.SetCurrent(new SaveGameData(), new SaveGameFileHandle(niceName + saveFileEndingName));
	}

	/// <summary>
	/// Load a save file, replacing the current SaveGameData.
	/// </summary>
	/// <param name="name">Name.</param>
	public static void Load(SaveGameFileHandle handle) {

		if (SaveFile.Exists(handle.trueName)) {

			SaveGameContainer c;
			switch(SaveFile.LoadData<SaveGameContainer>(handle.trueName, out c)) {
			case SaveFileResult.FILE_NO_EXISTS:
			case SaveFileResult.INVALID_TYPE:
			case SaveFileResult.NOT_SERIALIZEABLE:
				Debug.LogError("A fatal error occured while loading SaveGame");
				break;
			default:
				current.SetCurrent(new SaveGameData(c), handle);
				break;
			}

		} else {
			Debug.LogError("Could not load save game. Name invalid!");
		}
	}
}
