using UnityEngine;
using System.Collections;

public enum TurretMType {
	INTERNAL = 1 << 1,
	EXTERNAL = 1 << 2,
	/// <summary>
	/// ANY is only used for TurretSlots
	/// </summary>
	ANY = INTERNAL | EXTERNAL
}