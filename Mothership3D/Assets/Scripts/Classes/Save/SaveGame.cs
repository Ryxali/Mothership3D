using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// Used for storing fields relevant
/// to a single game session.
/// </summary>
/// 
[System.Serializable]
public class SaveGameData {
	private object[] data;

	private enum SaveGameFields {
		SHIP, SOMEVAL, EX0,
		
		
		
		END_LIST
	}

	public string ex0 {
		get { return GetValue<string>(SaveGameFields.EX0); }
		set { SetValue(SaveGameFields.EX0, value); }
	}




	/// <summary>
	/// Get the value stored at the specified key
	/// as the specified type.
	/// Returns - the object with the specified key,
	/// or null - should none exist.
	/// </summary>
	/// <returns>The raw value.</returns>
	/// <param name="key">Key.</param>
	private T GetValue<T>(SaveGameFields field) {
		if (field >= SaveGameFields.END_LIST) {
			Debug.LogError ("Could not get field [" + field + "]" +
				"\nReason: " + field + " >= " + SaveGameFields.END_LIST);
		} else if(data[(int)field] == null) {
			return default(T);
		}else if (!(data[(int)field] is T)) {
			Debug.LogError ("Could not get field [" + field + "]" +
			                "\nReason: " + data[(int) field].GetType() +
			                " is not compatible with " + typeof(T));

		} else {
			return (T)data[(int)field];
		}
		return default(T);
	}

	/// <summary>
	/// Set a value among the SaveGameData,
	/// inserting the value with the specified key.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	private void SetValue(SaveGameFields field, object value) {
		if (field >= SaveGameFields.END_LIST) {
			Debug.LogError ("Could not set field [" + field + "]" +
				"\nReason: " + field + " >= " + SaveGameFields.END_LIST);
		} else {
			data [(int)field] = value;
		}
	}

	public SaveGameData() {
		data = new object[(int)SaveGameFields.END_LIST];
	}

	public SaveGameData(object[] data) {
		if (data.Length == (int) SaveGameFields.END_LIST) {
			this.data = data;
		} else {
			Debug.LogError ("Could not load data. \nReason: array length " +
			                data.Length + " != " + SaveGameFields.END_LIST);
			data = new object[(int)SaveGameFields.END_LIST];
		}
	}


}

/// <summary>
/// Used to hide the ablility to save the data to disk,
/// preventing misuse.
/// </summary>
/*[System.Serializable]
internal class SaveGameContainer {
	internal object[] fields;

	internal SaveGameContainer (SaveGameFields size) {
			fields = new object[size] ();
	}
	
}*/


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
			SaveFile.SaveData<SaveGameData> (current.currentTrueName, current.data);
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

			SaveGameData c;
			switch(SaveFile.LoadData<SaveGameData>(handle.trueName, out c)) {
			case SaveFileResult.FILE_NO_EXISTS:
			case SaveFileResult.INVALID_TYPE:
			case SaveFileResult.NOT_SERIALIZEABLE:
				Debug.LogError("A fatal error occured while loading SaveGame");
				break;
			default:
				current.SetCurrent(c, handle);
				break;
			}

		} else {
			Debug.LogError("Could not load save game. Name invalid!");
		}
	}
}
