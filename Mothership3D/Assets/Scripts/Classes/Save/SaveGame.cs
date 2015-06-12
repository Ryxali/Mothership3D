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
	private Dictionary<string, object> data;

	protected static readonly string CORE_FIELD_PREFIX = "msp_";

	// Define core fields below
	private static readonly string EX0_KEY = CORE_FIELD_PREFIX + "EX0";
	public string ex0 {
		get { return GetValue<string>(EX0_KEY); }
		set { SetValue(EX0_KEY, value); }
	}

	private static readonly string SHIP_POSITION_KEY = CORE_FIELD_PREFIX + "SHIP_POSITION";
	/// <summary>
	/// Gets or sets the player's ship's position.
	/// </summary>
	/// <value>The ship position.</value>
	public Vector3 shipPosition {
		get { 
			if(!HasValue(SHIP_POSITION_KEY)) SetValue(SHIP_POSITION_KEY, Vector3.zero);
			return GetValue<Vector3>(SHIP_POSITION_KEY); 
		}
		set { SetValue(SHIP_POSITION_KEY, value); }
	}


	/// <summary>
	/// Get the value stored at the specified key
	/// as the specified type.
	/// Returns - the object with the specified key,
	/// or null - should none exist.
	/// </summary>
	/// <returns>The raw value.</returns>
	/// <param name="key">Key.</param>
	protected T GetValue<T>(string key) {
		object val = null;
		if (data.TryGetValue (key, out val)) {
			if(val is T){
				return (T) val;
			} else {
				Debug.LogError("Could not get save game value with key: " + key + "\nReason: Could not cast to value type. (" + val.GetType() + " to " + typeof(T) + ")");
			}
		} else {
			Debug.LogError("Could not get save game value with key: " + key + "\nReason: key doesn't exist.");

		}
		return default(T);
	}

	/// <summary>
	/// Check whether the key currently exists.
	/// </summary>
	/// <returns><c>true</c> if this instance has value the specified key; otherwise, <c>false</c>.</returns>
	/// <param name="key">Key.</param>
	protected bool HasValue(string key) {
		return data.ContainsKey (key);
	}

	/// <summary>
	/// Set a value among the SaveGameData,
	/// inserting the value with the specified key.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	protected void SetValue(string key, object value) {
		data.Add (key, value);
	}

	/// <summary>
	/// Initializes a new empty instance of the <see cref="SaveGameData"/> class.
	/// </summary>
	protected SaveGameData() {
		data = new Dictionary<string, object>();
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="SaveGameData"/> class, filling
	/// it with the data provided.
	/// </summary>
	/// <param name="data">Data.</param>
	protected SaveGameData(Dictionary<string, object> data) {
		this.data = data;
	}


}

/// <summary>
/// Used to identify a save file, containing both a nice
/// name suited for display and the true name of the file.
/// </summary>
public class SaveGameFileHandle
{
	/// <summary>
	/// Get the nice name of the file ("foo")
	/// </summary>
	public string niceName { get; protected set; }
	/// <summary>
	/// Get the true name of the file ("foo.mgs")
	/// </summary>
	public string trueName { get; protected set; }
	protected SaveGameFileHandle() {

	}
}





/// <summary>
/// Contains all relevant functionality regarding the saving
/// and loading of savegames, also providing access to the most
/// recently loaded save.
/// </summary>
public class SaveGame {
	/// <summary>
	/// The standardized ending for all game save files
	/// </summary>
	public static readonly string saveFileEndingName = ".mgs";
	public static readonly char[] saveFileEndingAsCharArray = saveFileEndingName.ToCharArray();

	/// <summary>
	/// Get the currently loaded Save Game Data.
	/// </summary>
	/// <value>The current data.</value>
	public static SaveGameData currentData {	get { return cur.data; } }
	/// <summary>
	/// Get the nice name of the currently loaded save ("foo")
	/// </summary>
	/// <value>The name of the current slot nice.</value>
	public static string currentSlotNiceName { get { return cur.currentSlotNiceName; } }
	/// <summary>
	/// Get the true name of the currently loaded save ("foo.mgs")
	/// </summary>
	/// <value>The name of the current slot true.</value>
	public static string currentSlotTrueName { get { return cur.currentSlotTrueName; } }
	/// <summary>
	/// Whether we have a current Save Game Data
	/// </summary>
	/// <value><c>true</c> if current valid; otherwise, <c>false</c>.</value>
	public static bool currentValid { get { return cur.currentValid; } }
	
	private static CurrentSlot cur = new CurrentSlot();

	/// <summary>
	/// Used to hide the constructablility of the SaveGameFileHandle
	/// </summary>
	private class SaveGameFileHandle_Impl : SaveGameFileHandle {
		public SaveGameFileHandle_Impl(string trueName) {
			this.trueName = trueName;
			this.niceName = trueName.TrimEnd(SaveGame.saveFileEndingAsCharArray);
		}
	}
	/// <summary>
	/// Defines the current save.
	/// </summary>
	private class CurrentSlot
	{
		public SaveGameData data { get { return c; } }
		
		/// <summary>
		/// The name of the current save file in nice format ("foo")
		/// </summary>
		public string currentSlotNiceName {
			get { return n.niceName; }
		}
		
		/// <summary>
		/// The name of the current save file in true format ("foo.mgs")
		/// </summary>
		public string currentSlotTrueName {
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

	/// <summary>
	/// Used to hide the construction of SaveGameData
	/// </summary>
	[System.Serializable]
	private class SaveGameData_Impl : SaveGameData {

		public SaveGameData_Impl() : base() {
		}
		public SaveGameData_Impl(Dictionary<string, object> data) : base(data) {
		}
	}

	/// <summary>
	/// Get a list of all available save files.
	/// </summary>
	/// <returns>The available saves.</returns>
	public static SaveGameFileHandle[] ListAvailableSaves() {
		string[] arr = Directory.GetFiles (Application.persistentDataPath, "*" + saveFileEndingName);
		SaveGameFileHandle[] arrtrim = new SaveGameFileHandle[arr.Length];
		for (int i = 0; i < arrtrim.Length; i++) {
			arrtrim[i] = new SaveGameFileHandle_Impl(arr[i].Substring(arr[i].LastIndexOfAny(new char[]{'/', '\\'}) + 1));//arr[i].TrimEnd(saveEndingAsCharArray);
		}
		return arrtrim;
	}

	/// <summary>
	/// Saves the current SaveGameData to disk.
	/// </summary>
	public static void Save() {
		if (cur.currentValid) {
			SaveFile.SaveData<SaveGameData> (currentSlotTrueName, currentData);
		} else {
			Debug.LogError("Current is empty! Cannot save.");
		}
	}

	/// <summary>
	/// Creates a new empty save.
	/// </summary>
	/// <param name="niceName">Nice name.</param>
	public static void CreateEmpty(string niceName) {
		cur.SetCurrent(new SaveGameData_Impl(), new SaveGameFileHandle_Impl(niceName + saveFileEndingName));
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
				cur.SetCurrent(c, handle);
				break;
			}

		} else {
			Debug.LogError("Could not load save game. Name invalid!");
		}
	}
}
