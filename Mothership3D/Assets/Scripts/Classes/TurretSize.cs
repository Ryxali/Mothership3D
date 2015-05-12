using UnityEngine;
using System.Collections;

public enum TurretSize {
	TINY	=	1 << 1,
	SMALL	=	1 << 2 | TINY,
	MEDIUM	=	1 << 3 | SMALL,
	LARGE	=	1 << 4 | MEDIUM
}
