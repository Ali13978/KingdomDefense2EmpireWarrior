using System;
using UnityEngine;

[Serializable]
public class UIEffect
{
	[Header("Start Game SFX")]
	public AudioClip startGameAtMainMenu;

	public AudioClip startGameAtMapLevel;

	[Space]
	[Header("Unlock/Upgrade/Buy Items SFX")]
	public AudioClip unlockSuccess;

	public AudioClip upgradeSuccess;

	public AudioClip buySuccess;
}
