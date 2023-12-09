using System;
using UnityEngine;

public class F3DFXController : MonoBehaviour
{
	public static F3DFXController instance;

	private string[] fxTypeName = new string[12]
	{
		"Vulcan",
		"Sologun",
		"Sniper",
		"Shotgun",
		"Seeker",
		"Railgun",
		"Plasmagun",
		"Plasma beam",
		"Heavy plasma beam",
		"Lightning gun",
		"Flamethrower",
		"Pulse laser"
	};

	private int curSocket;

	private int timerID = -1;

	[Header("Turret setup")]
	public Transform[] TurretSocket;

	public ParticleSystem[] ShellParticles;

	public F3DFXType DefaultFXType;

	[Header("Vulcan")]
	public Transform vulcanProjectile;

	public Transform vulcanMuzzle;

	public Transform vulcanImpact;

	[Header("Solo gun")]
	public Transform soloGunProjectile;

	public Transform soloGunMuzzle;

	public Transform soloGunImpact;

	[Header("Sniper")]
	public Transform sniperBeam;

	public Transform sniperMuzzle;

	public Transform sniperImpact;

	[Header("Shotgun")]
	public Transform shotGunProjectile;

	public Transform shotGunMuzzle;

	public Transform shotGunImpact;

	[Header("Seeker")]
	public Transform seekerProjectile;

	public Transform seekerMuzzle;

	public Transform seekerImpact;

	[Header("Rail gun")]
	public Transform railgunBeam;

	public Transform railgunMuzzle;

	public Transform railgunImpact;

	[Header("Plasma gun")]
	public Transform plasmagunProjectile;

	public Transform plasmagunMuzzle;

	public Transform plasmagunImpact;

	[Header("Plasma beam")]
	public Transform plasmaBeam;

	[Header("Plasma beam heavy")]
	public Transform plasmaBeamHeavy;

	[Header("Lightning gun")]
	public Transform lightningGunBeam;

	[Header("Flame")]
	public Transform flameRed;

	[Header("Laser impulse")]
	public Transform laserImpulseProjectile;

	public Transform laserImpulseMuzzle;

	public Transform laserImpulseImpact;

	private void Awake()
	{
		instance = this;
		for (int i = 0; i < ShellParticles.Length; i++)
		{
			ShellParticles[i].enableEmission = false;
			ShellParticles[i].gameObject.SetActive(value: true);
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.fontSize = 25;
		gUIStyle.fontStyle = FontStyle.Bold;
		gUIStyle.wordWrap = false;
		GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
		gUIStyle2.fontSize = 11;
		gUIStyle2.wordWrap = false;
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height - 150, 300f, 120f));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(fxTypeName[(int)DefaultFXType], gUIStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Press Left / Right arrow keys to switch", gUIStyle2);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Previous", GUILayout.Width(90f), GUILayout.Height(30f)))
		{
			PrevWeapon();
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Next", GUILayout.Width(90f), GUILayout.Height(30f)))
		{
			NextWeapon();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			NextWeapon();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			PrevWeapon();
		}
	}

	private void NextWeapon()
	{
		if ((int)DefaultFXType < Enum.GetNames(typeof(F3DFXType)).Length - 1)
		{
			Stop();
			DefaultFXType++;
		}
	}

	private void PrevWeapon()
	{
		if (DefaultFXType > F3DFXType.Vulcan)
		{
			Stop();
			DefaultFXType--;
		}
	}

	private void AdvanceSocket()
	{
		curSocket++;
		if (curSocket > 3)
		{
			curSocket = 0;
		}
	}

	public void Fire()
	{
		switch (DefaultFXType)
		{
		case F3DFXType.Vulcan:
			timerID = F3DTime.time.AddTimer(0.05f, Vulcan);
			Vulcan();
			break;
		case F3DFXType.SoloGun:
			timerID = F3DTime.time.AddTimer(0.2f, SoloGun);
			SoloGun();
			break;
		case F3DFXType.Sniper:
			timerID = F3DTime.time.AddTimer(0.3f, Sniper);
			Sniper();
			break;
		case F3DFXType.ShotGun:
			timerID = F3DTime.time.AddTimer(0.3f, ShotGun);
			ShotGun();
			break;
		case F3DFXType.Seeker:
			timerID = F3DTime.time.AddTimer(0.2f, Seeker);
			Seeker();
			break;
		case F3DFXType.RailGun:
			timerID = F3DTime.time.AddTimer(0.2f, RailGun);
			RailGun();
			break;
		case F3DFXType.PlasmaGun:
			timerID = F3DTime.time.AddTimer(0.2f, PlasmaGun);
			PlasmaGun();
			break;
		case F3DFXType.PlasmaBeam:
			PlasmaBeam();
			break;
		case F3DFXType.PlasmaBeamHeavy:
			PlasmaBeamHeavy();
			break;
		case F3DFXType.LightningGun:
			LightningGun();
			break;
		case F3DFXType.FlameRed:
			FlameRed();
			break;
		case F3DFXType.LaserImpulse:
			timerID = F3DTime.time.AddTimer(0.15f, LaserImpulse);
			LaserImpulse();
			break;
		}
	}

	public void Stop()
	{
		if (timerID != -1)
		{
			F3DTime.time.RemoveTimer(timerID);
			timerID = -1;
		}
	}

	private void Vulcan()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(vulcanMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(vulcanProjectile, TurretSocket[curSocket].position + TurretSocket[curSocket].forward, lhs * TurretSocket[curSocket].rotation, null);
		ShellParticles[curSocket].Emit(1);
		F3DAudioController.instance.VulcanShot(TurretSocket[curSocket].position);
		AdvanceSocket();
	}

	public void VulcanImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(vulcanImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.VulcanHit(pos);
	}

	private void SoloGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(soloGunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(soloGunProjectile, TurretSocket[curSocket].position + TurretSocket[curSocket].forward, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.SoloGunShot(TurretSocket[curSocket].position);
		AdvanceSocket();
	}

	public void SoloGunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(soloGunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SoloGunHit(pos);
	}

	private void Sniper()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(sniperMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(sniperBeam, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.SniperShot(TurretSocket[curSocket].position);
		ShellParticles[curSocket].Emit(1);
		AdvanceSocket();
	}

	public void SniperImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(sniperImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SniperHit(pos);
	}

	private void ShotGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(shotGunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(shotGunProjectile, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.ShotGunShot(TurretSocket[curSocket].position);
		ShellParticles[curSocket].Emit(1);
		AdvanceSocket();
	}

	private void Seeker()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(seekerMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(seekerProjectile, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.SeekerShot(TurretSocket[curSocket].position);
		AdvanceSocket();
	}

	public void SeekerImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(seekerImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SeekerHit(pos);
	}

	private void RailGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(railgunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(railgunBeam, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.RailGunShot(TurretSocket[curSocket].position);
		ShellParticles[curSocket].Emit(1);
		AdvanceSocket();
	}

	public void RailgunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(railgunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.RailGunHit(pos);
	}

	private void PlasmaGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(plasmagunMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(plasmagunProjectile, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.PlasmaGunShot(TurretSocket[curSocket].position);
		AdvanceSocket();
	}

	public void PlasmaGunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(plasmagunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.PlasmaGunHit(pos);
	}

	private void PlasmaBeam()
	{
		F3DPool.instance.Spawn(plasmaBeam, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
		F3DPool.instance.Spawn(plasmaBeam, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
	}

	private void PlasmaBeamHeavy()
	{
		F3DPool.instance.Spawn(plasmaBeamHeavy, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
		F3DPool.instance.Spawn(plasmaBeamHeavy, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
	}

	private void LightningGun()
	{
		F3DPool.instance.Spawn(lightningGunBeam, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
		F3DPool.instance.Spawn(lightningGunBeam, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
	}

	private void FlameRed()
	{
		F3DPool.instance.Spawn(flameRed, TurretSocket[0].position, TurretSocket[0].rotation, TurretSocket[0]);
		F3DPool.instance.Spawn(flameRed, TurretSocket[2].position, TurretSocket[2].rotation, TurretSocket[2]);
	}

	private void LaserImpulse()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(laserImpulseMuzzle, TurretSocket[curSocket].position, TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
		F3DPool.instance.Spawn(laserImpulseProjectile, TurretSocket[curSocket].position, lhs * TurretSocket[curSocket].rotation, null);
		F3DAudioController.instance.LaserImpulseShot(TurretSocket[curSocket].position);
		AdvanceSocket();
	}

	public void LaserImpulseImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(laserImpulseImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.LaserImpulseHit(pos);
	}
}
