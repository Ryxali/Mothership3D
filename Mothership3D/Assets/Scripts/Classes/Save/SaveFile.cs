using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

/// <summary>
/// Generic class to handle loading and saving files in a safe manner. It only
/// suppets saving/loading a single serialized class per file.
/// </summary>
public class SaveFile {
	// Hide this
	private SaveFile() { }
	/// <summary>
	/// Saves the data, overwriting any existing file with the same name.
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="item">Item to save.</param>
	public static SaveFileResult SaveData<T>(string fileName, T item) {
		SaveFileResult success = SaveFileResult.SUCCESS;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + fileName);
		try {
			bf.Serialize(file, item);
		} catch (SerializationException e) {
			Debug.LogError ("Could not deserialize file: " + fileName + " \nReason: " + e.Message);
			success = SaveFileResult.NOT_SERIALIZEABLE;
		}
		file.Close();
		return success;
	}

	/// <summary>
	/// Loads data, outputting it into the data parameter.
	/// </summary>
	/// <returns><c>true</c>, if data was loaded, returning <c>false</c> if the data loaded cannot
	/// be cast to assigned type, if the file does not exist, or the file is corrupt.</returns>
	/// <param name="fileName">File name.</param>
	/// <param name="data">Object to put the data in. Will be null if the load fails.</param>
	public static SaveFileResult LoadData<T>(string fileName, out T data) {
		SaveFileResult success = SaveFileResult.SUCCESS;
		data = default(T);
		if (File.Exists (Application.persistentDataPath + "/" + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + fileName, FileMode.Open);
			try {
				object o = bf.Deserialize (file);
				if (typeof(T) == o.GetType ()) {
					data = (T)o;
				} else {
					Debug.LogError ("Could not deserialize file: " + fileName + " \nReason: Invalid Type");
					success = SaveFileResult.INVALID_TYPE;
				}

			} catch (SerializationException e) {
				Debug.LogError ("Could not deserialize file: " + fileName + " \nReason: " + e.Message);
				success = SaveFileResult.NOT_SERIALIZEABLE;
			} catch (EndOfStreamException ex) {
				Debug.LogError("Read error \n Reason: " + ex.Message);
			}
			file.Close ();
		} else {
			success = SaveFileResult.FILE_NO_EXISTS;
		}
		return success;
	}

	public static bool Exists(string fileName) {
		return File.Exists(Application.persistentDataPath + "/" + fileName);
	}
}
