using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CodeStage.AntiCheat.Examples
{
	[AddComponentMenu("")]
	public class ActTesterGui : MonoBehaviour
	{
		private const string RED_COLOR = "#FF4040";

		private const string GREEN_COLOR = "#02C85F";

		private const string PREFS_STRING = "name";

		private const string PREFS_INT = "money";

		private const string PREFS_FLOAT = "lifeBar";

		private const string PREFS_BOOL = "gameComplete";

		private const string PREFS_UINT = "demoUint";

		private const string PREFS_LONG = "demoLong";

		private const string PREFS_DOUBLE = "demoDouble";

		private const string PREFS_VECTOR2 = "demoVector2";

		private const string PREFS_VECTOR3 = "demoVector3";

		private const string PREFS_QUATERNION = "demoQuaternion";

		private const string PREFS_RECT = "demoRect";

		private const string PREFS_COLOR = "demoColor";

		private const string PREFS_BYTE_ARRAY = "demoByteArray";

		private const string API_URL_LOCK_TO_DEVICE = "http://j.mp/1gxg1tf";

		private const string API_URL_PRESERVE_PREFS = "http://j.mp/1iBK5pz";

		private const string API_URL_EMERGENCY_MODE = "http://j.mp/1FRAL5L";

		private const string API_URL_READ_FOREIGN = "http://j.mp/1LCdpDa";

		private const string API_URL_UNOBSCURED_MODE = "http://j.mp/1KVrpxi";

		private const string API_URL_PLAYER_PREFS = "http://docs.unity3d.com/ScriptReference/PlayerPrefs.html";

		[Header("Regular variables")]
		public string regularString = "I'm regular string";

		public int regularInt = 1987;

		public float regularFloat = 2013.05237f;

		public Vector3 regularVector3 = new Vector3(10.5f, 11.5f, 12.5f);

		[Header("Obscured (secure) variables")]
		public ObscuredString obscuredString = "I'm obscured string";

		public ObscuredInt obscuredInt = 1987;

		public ObscuredFloat obscuredFloat = 2013.05237f;

		public ObscuredVector3 obscuredVector3 = new Vector3(10.5f, 11.5f, 12.5f);

		public ObscuredBool obscuredBool = true;

		public ObscuredLong obscuredLong = 945678987654123345L;

		public ObscuredDouble obscuredDouble = 9.45678987654;

		public ObscuredVector2 obscuredVector2 = new Vector2(8.5f, 9.5f);

		[Header("Other")]
		public string prefsEncryptionKey = "change me!";

		private readonly string[] tabs = new string[3]
		{
			"Variables protection",
			"Saves protection",
			"Cheating detectors"
		};

		private int currentTab;

		private string allSimpleObscuredTypes;

		private string regularPrefs;

		private string obscuredPrefs;

		private int savesLock;

		private bool savesAlterationDetected;

		private bool foreignSavesDetected;

		private bool injectionDetected;

		private bool speedHackDetected;

		private bool obscuredTypeCheatDetected;

		private bool wallHackCheatDetected;

		private readonly StringBuilder logBuilder = new StringBuilder();

		public void OnSpeedHackDetected()
		{
			speedHackDetected = true;
			UnityEngine.Debug.Log("Speed hack Detected!");
		}

		public void OnInjectionDetected()
		{
			injectionDetected = true;
			UnityEngine.Debug.Log("Injection Detected!");
		}

		public void OnInjectionDetectedWithCause(string cause)
		{
			injectionDetected = true;
			UnityEngine.Debug.Log("Injection Detected! Cause: " + cause);
		}

		public void OnObscuredTypeCheatingDetected()
		{
			obscuredTypeCheatDetected = true;
			UnityEngine.Debug.Log("Obscured Vars Cheating Detected!");
		}

		public void OnWallHackDetected()
		{
			wallHackCheatDetected = true;
			UnityEngine.Debug.Log("Wall hack Detected!");
		}

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				ObscuredPrefs.CryptoKey = prefsEncryptionKey;
			}
		}

		private void Awake()
		{
			ObscuredPrefs.CryptoKey = prefsEncryptionKey;
			ObscuredPrefs.onAlterationDetected = SavesAlterationDetected;
			ObscuredPrefs.onPossibleForeignSavesDetected = ForeignSavesDetected;
		}

		private void Start()
		{
			ObscuredStringExample();
			ObscuredIntExample();
			ObscuredFloatExample();
			ObscuredVector3Example();
			Invoke("RandomizeObscuredVars", UnityEngine.Random.Range(1f, 10f));
		}

		private void RandomizeObscuredVars()
		{
			obscuredInt.RandomizeCryptoKey();
			obscuredFloat.RandomizeCryptoKey();
			obscuredString.RandomizeCryptoKey();
			obscuredVector3.RandomizeCryptoKey();
			Invoke("RandomizeObscuredVars", UnityEngine.Random.Range(1f, 10f));
		}

		private void ObscuredStringExample()
		{
			logBuilder.Length = 0;
			logBuilder.AppendLine("[ACTk] <b>[ ObscuredString test ]</b>");
			ObscuredString.SetNewCryptoKey("I LOVE MY GIRLz");
			string text = "the Goscurry is not a lie ;)";
			logBuilder.AppendLine("Original string:\n" + text);
			ObscuredString obscuredString = text;
			logBuilder.AppendLine("How your string is stored in memory when obscured:\n" + obscuredString.GetEncrypted());
			UnityEngine.Debug.Log(logBuilder);
		}

		private void ObscuredIntExample()
		{
			logBuilder.Length = 0;
			logBuilder.AppendLine("[ACTk] <b>[ ObscuredInt test ]</b>");
			ObscuredInt.SetNewCryptoKey(434523);
			int num = 5;
			logBuilder.AppendLine("Original lives count: " + num);
			ObscuredInt value = num;
			logBuilder.AppendLine("How your lives count is stored in memory when obscured: " + value.GetEncrypted());
			ObscuredInt.SetNewCryptoKey(666);
			num = value;
			value = (int)value - 2;
			value = (int)value + num + 10;
			value = (int)value / 2;
			value = ++value;
			ObscuredInt.SetNewCryptoKey(999);
			value = ++value;
			value = --value;
			logBuilder.AppendLine("Lives count after few usual operations: " + value + " (" + value.ToString("X") + "h)");
			UnityEngine.Debug.Log(logBuilder);
		}

		private void ObscuredFloatExample()
		{
			logBuilder.Length = 0;
			logBuilder.AppendLine("[ACTk] <b>[ ObscuredFloat test ]</b>");
			ObscuredFloat.SetNewCryptoKey(404);
			float num = 99.9f;
			logBuilder.AppendLine("Original health bar: " + num);
			ObscuredFloat value = num;
			logBuilder.AppendLine("How your health bar is stored in memory when obscured: " + value.GetEncrypted());
			ObscuredFloat.SetNewCryptoKey(666);
			value = (float)value + 6f;
			value = (float)value - 1.5f;
			value = ++value;
			value = --value;
			value = --value;
			value = num - (float)value + 10.5f;
			logBuilder.AppendLine("Health bar after few usual operations: " + value);
			UnityEngine.Debug.Log(logBuilder);
		}

		private void ObscuredVector3Example()
		{
			logBuilder.Length = 0;
			logBuilder.AppendLine("[ACTk] <b>[ ObscuredVector3 test ]</b>");
			ObscuredVector3.SetNewCryptoKey(404);
			Vector3 vector = new Vector3(54.1f, 64.3f, 63.2f);
			logBuilder.AppendLine("Original position: " + vector);
			ObscuredVector3.RawEncryptedVector3 encrypted = ((ObscuredVector3)vector).GetEncrypted();
			logBuilder.AppendLine("How your position is stored in memory when obscured: (" + encrypted.x + ", " + encrypted.y + ", " + encrypted.z + ")");
			UnityEngine.Debug.Log(logBuilder);
		}

		private void SavesAlterationDetected()
		{
			savesAlterationDetected = true;
		}

		private void ForeignSavesDetected()
		{
			foreignSavesDetected = true;
		}

		private void OnGUI()
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.alignment = TextAnchor.UpperCenter;
			GUILayout.BeginArea(new Rect(10f, 5f, Screen.width - 20, Screen.height - 10));
			GUILayout.Label("<color=\"#0287C8\"><b>Anti-Cheat Toolkit Sandbox</b></color>", gUIStyle);
			GUILayout.Label("Here you can overview common ACTk features and try to cheat something yourself.", gUIStyle);
			GUILayout.Space(5f);
			currentTab = GUILayout.Toolbar(currentTab, tabs);
			if (currentTab == 0)
			{
				GUILayout.Label("ACTk offers own collection of the secure types to let you protect your variables from <b>ANY</b> memory hacking tools (Cheat Engine, ArtMoney, GameCIH, Game Guardian, etc.).");
				GUILayout.Space(5f);
				using (new HorizontalLayout())
				{
					GUILayout.Label("<b>Obscured types:</b>\n<color=\"#75C4EB\">" + GetAllSimpleObscuredTypes() + "</color>", GUILayout.MinWidth(130f));
					GUILayout.Space(10f);
					using (new VerticalLayout(GUI.skin.box))
					{
						GUILayout.Label("Below you can try to cheat few variables of the regular types and their obscured (secure) analogues (you may change initial values from Tester object inspector):");
						GUILayout.Space(10f);
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>string:</b> " + regularString, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								regularString += (char)UnityEngine.Random.Range(97, 122);
							}
							if (GUILayout.Button("Reset"))
							{
								regularString = string.Empty;
							}
						}
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>ObscuredString:</b> " + obscuredString, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								obscuredString = (string)obscuredString + (char)UnityEngine.Random.Range(97, 122);
							}
							if (GUILayout.Button("Reset"))
							{
								obscuredString = string.Empty;
							}
						}
						GUILayout.Space(10f);
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>int:</b> " + regularInt, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								regularInt += UnityEngine.Random.Range(1, 100);
							}
							if (GUILayout.Button("Reset"))
							{
								regularInt = 0;
							}
						}
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>ObscuredInt:</b> " + obscuredInt, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								obscuredInt = (int)obscuredInt + UnityEngine.Random.Range(1, 100);
							}
							if (GUILayout.Button("Reset"))
							{
								obscuredInt = 0;
							}
						}
						GUILayout.Space(10f);
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>float:</b> " + regularFloat, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								regularFloat += UnityEngine.Random.Range(1f, 100f);
							}
							if (GUILayout.Button("Reset"))
							{
								regularFloat = 0f;
							}
						}
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>ObscuredFloat:</b> " + obscuredFloat, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								obscuredFloat = (float)obscuredFloat + UnityEngine.Random.Range(1f, 100f);
							}
							if (GUILayout.Button("Reset"))
							{
								obscuredFloat = 0f;
							}
						}
						GUILayout.Space(10f);
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>Vector3:</b> " + regularVector3, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								regularVector3 += UnityEngine.Random.insideUnitSphere;
							}
							if (GUILayout.Button("Reset"))
							{
								regularVector3 = Vector3.zero;
							}
						}
						using (new HorizontalLayout())
						{
							GUILayout.Label("<b>ObscuredVector3:</b> " + obscuredVector3, GUILayout.Width(250f));
							if (GUILayout.Button("Add random value"))
							{
								obscuredVector3 += UnityEngine.Random.insideUnitSphere;
							}
							if (GUILayout.Button("Reset"))
							{
								obscuredVector3 = Vector3.zero;
							}
						}
					}
				}
			}
			else if (currentTab == 1)
			{
				GUILayout.Label("ACTk has secure layer for the PlayerPrefs: <color=\"#75C4EB\">ObscuredPrefs</color>. It protects data from view, detects any cheating attempts, optionally locks data to the current device and supports additional data types.");
				GUILayout.Space(5f);
				using (new HorizontalLayout())
				{
					GUILayout.Label("<b>Supported types:</b>\n" + GetAllObscuredPrefsDataTypes(), GUILayout.MinWidth(130f));
					using (new VerticalLayout(GUI.skin.box))
					{
						GUILayout.Label("Below you can try to cheat both regular PlayerPrefs and secure ObscuredPrefs:");
						using (new VerticalLayout())
						{
							GUILayout.Label("<color=\"#FF4040\"><b>PlayerPrefs:</b></color>\neasy to cheat, only 3 supported types", gUIStyle);
							GUILayout.Space(5f);
							if (string.IsNullOrEmpty(regularPrefs))
							{
								LoadRegularPrefs();
							}
							using (new HorizontalLayout())
							{
								GUILayout.Label(regularPrefs, GUILayout.Width(270f));
								using (new VerticalLayout())
								{
									using (new HorizontalLayout())
									{
										if (GUILayout.Button("Save"))
										{
											SaveRegularPrefs();
										}
										if (GUILayout.Button("Load"))
										{
											LoadRegularPrefs();
										}
									}
									if (GUILayout.Button("Delete"))
									{
										DeleteRegularPrefs();
									}
								}
							}
						}
						GUILayout.Space(5f);
						using (new VerticalLayout())
						{
							GUILayout.Label("<color=\"#02C85F\"><b>ObscuredPrefs:</b></color>\nsecure, lot of additional types and extra options", gUIStyle);
							GUILayout.Space(5f);
							if (string.IsNullOrEmpty(obscuredPrefs))
							{
								LoadObscuredPrefs();
							}
							using (new HorizontalLayout())
							{
								GUILayout.Label(obscuredPrefs, GUILayout.Width(270f));
								using (new VerticalLayout())
								{
									using (new HorizontalLayout())
									{
										if (GUILayout.Button("Save"))
										{
											SaveObscuredPrefs();
										}
										if (GUILayout.Button("Load"))
										{
											LoadObscuredPrefs();
										}
									}
									if (GUILayout.Button("Delete"))
									{
										DeleteObscuredPrefs();
									}
									using (new HorizontalLayout())
									{
										GUILayout.Label("LockToDevice level");
										PlaceUrlButton("http://j.mp/1gxg1tf");
									}
									savesLock = GUILayout.SelectionGrid(savesLock, new string[3]
									{
										ObscuredPrefs.DeviceLockLevel.None.ToString(),
										ObscuredPrefs.DeviceLockLevel.Soft.ToString(),
										ObscuredPrefs.DeviceLockLevel.Strict.ToString()
									}, 3);
									ObscuredPrefs.lockToDevice = (ObscuredPrefs.DeviceLockLevel)savesLock;
									GUILayout.Space(5f);
									using (new HorizontalLayout())
									{
										ObscuredPrefs.preservePlayerPrefs = GUILayout.Toggle(ObscuredPrefs.preservePlayerPrefs, "preservePlayerPrefs");
										PlaceUrlButton("http://j.mp/1iBK5pz");
									}
									using (new HorizontalLayout())
									{
										ObscuredPrefs.emergencyMode = GUILayout.Toggle(ObscuredPrefs.emergencyMode, "emergencyMode");
										PlaceUrlButton("http://j.mp/1FRAL5L");
									}
									using (new HorizontalLayout())
									{
										ObscuredPrefs.readForeignSaves = GUILayout.Toggle(ObscuredPrefs.readForeignSaves, "readForeignSaves");
										PlaceUrlButton("http://j.mp/1LCdpDa");
									}
									GUILayout.Space(5f);
									GUILayout.Label("<color=\"" + ((!savesAlterationDetected) ? "#02C85F" : "#FF4040") + "\">Saves modification detected: " + savesAlterationDetected + "</color>");
									GUILayout.Label("<color=\"" + ((!foreignSavesDetected) ? "#02C85F" : "#FF4040") + "\">Foreign saves detected: " + foreignSavesDetected + "</color>");
								}
							}
						}
						GUILayout.Space(5f);
						PlaceUrlButton("http://docs.unity3d.com/ScriptReference/PlayerPrefs.html", "Visit docs to see where PlayerPrefs are stored", -1);
					}
				}
			}
			else
			{
				GUILayout.Label("ACTk is able to detect some types of cheating to let you take action on the cheating players. This example scene has all possible detectors and all of them are automatically start on scene start.");
				GUILayout.Space(5f);
				using (new VerticalLayout(GUI.skin.box))
				{
					GUILayout.Label("<b>Speed Hack Detector</b>");
					GUILayout.Label("Allows to detect Cheat Engine's speed hack (and maybe some other speed hack tools) usage.");
					GUILayout.Label("<color=\"" + ((!speedHackDetected) ? "#02C85F" : "#FF4040") + "\">Detected: " + speedHackDetected.ToString().ToLower() + "</color>");
					GUILayout.Space(10f);
					GUILayout.Label("<b>Obscured Cheating Detector</b>");
					GUILayout.Label("Detects cheating of any Obscured type (except ObscuredPrefs, it has own detection features) used in project.");
					GUILayout.Label("<color=\"" + ((!obscuredTypeCheatDetected) ? "#02C85F" : "#FF4040") + "\">Detected: " + obscuredTypeCheatDetected.ToString().ToLower() + "</color>");
					GUILayout.Space(10f);
					GUILayout.Label("<b>WallHack Detector</b>");
					GUILayout.Label("Detects common types of wall hack cheating: walking through the walls (Rigidbody and CharacterController modules), shooting through the walls (Raycast module), looking through the walls (Wireframe module).");
					GUILayout.Label("<color=\"" + ((!wallHackCheatDetected) ? "#02C85F" : "#FF4040") + "\">Detected: " + wallHackCheatDetected.ToString().ToLower() + "</color>");
					GUILayout.Space(10f);
					GUILayout.Label("<b>Injection Detector</b>");
					GUILayout.Label("Allows to detect foreign managed assemblies in your application.");
					GUILayout.Label("<color=\"" + ((!injectionDetected) ? "#02C85F" : "#FF4040") + "\">Detected: " + injectionDetected.ToString().ToLower() + "</color>");
				}
			}
			GUILayout.EndArea();
		}

		private string GetAllSimpleObscuredTypes()
		{
			string result = "Can't get the list, sorry :(";
			string types = string.Empty;
			if (string.IsNullOrEmpty(allSimpleObscuredTypes))
			{
				IEnumerable<Type> source = from t in Assembly.GetExecutingAssembly().GetTypes()
					where t.IsPublic && t.Namespace == "CodeStage.AntiCheat.ObscuredTypes" && t.Name != "ObscuredPrefs"
					select t;
				source.ToList().ForEach(delegate(Type t)
				{
					if (types.Length > 0)
					{
						types = types + "\n" + t.Name;
					}
					else
					{
						types += t.Name;
					}
				});
				if (!string.IsNullOrEmpty(types))
				{
					result = types;
					allSimpleObscuredTypes = types;
				}
			}
			else
			{
				result = allSimpleObscuredTypes;
			}
			return result;
		}

		private string GetAllObscuredPrefsDataTypes()
		{
			return "int\nfloat\nstring\n<color=\"#75C4EB\">uint\ndouble\nlong\nbool\nbyte[]\nVector2\nVector3\nQuaternion\nColor\nRect</color>";
		}

		private void LoadRegularPrefs()
		{
			regularPrefs = "int: " + PlayerPrefs.GetInt("money", -1) + "\n";
			string text = regularPrefs;
			regularPrefs = text + "float: " + PlayerPrefs.GetFloat("lifeBar", -1f) + "\n";
			regularPrefs = regularPrefs + "string: " + PlayerPrefs.GetString("name", "No saved PlayerPrefs!");
		}

		private void SaveRegularPrefs()
		{
			PlayerPrefs.SetInt("money", 456);
			PlayerPrefs.SetFloat("lifeBar", 456.789f);
			PlayerPrefs.SetString("name", "Hey, there!");
			PlayerPrefs.Save();
		}

		private void DeleteRegularPrefs()
		{
			PlayerPrefs.DeleteKey("money");
			PlayerPrefs.DeleteKey("lifeBar");
			PlayerPrefs.DeleteKey("name");
			PlayerPrefs.Save();
		}

		private void LoadObscuredPrefs()
		{
			byte[] byteArray = ObscuredPrefs.GetByteArray("demoByteArray", 0, 4);
			obscuredPrefs = "int: " + ObscuredPrefs.GetInt("money", -1) + "\n";
			string text = obscuredPrefs;
			obscuredPrefs = text + "float: " + ObscuredPrefs.GetFloat("lifeBar", -1f) + "\n";
			obscuredPrefs = obscuredPrefs + "string: " + ObscuredPrefs.GetString("name", "No saved ObscuredPrefs!") + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "bool: " + ObscuredPrefs.GetBool("gameComplete", defaultValue: false) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "uint: " + ObscuredPrefs.GetUInt("demoUint", 0u) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "long: " + ObscuredPrefs.GetLong("demoLong", -1L) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "double: " + ObscuredPrefs.GetDouble("demoDouble", -1.0) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "Vector2: " + ObscuredPrefs.GetVector2("demoVector2", Vector2.zero) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "Vector3: " + ObscuredPrefs.GetVector3("demoVector3", Vector3.zero) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "Quaternion: " + ObscuredPrefs.GetQuaternion("demoQuaternion", Quaternion.identity) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "Rect: " + ObscuredPrefs.GetRect("demoRect", new Rect(0f, 0f, 0f, 0f)) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "Color: " + ObscuredPrefs.GetColor("demoColor", Color.black) + "\n";
			text = obscuredPrefs;
			obscuredPrefs = text + "byte[]: {" + byteArray[0] + "," + byteArray[1] + "," + byteArray[2] + "," + byteArray[3] + "}";
		}

		private void SaveObscuredPrefs()
		{
			ObscuredPrefs.SetInt("money", 123);
			ObscuredPrefs.SetFloat("lifeBar", 123.456f);
			ObscuredPrefs.SetString("name", "Goscurry is not a lie ;)");
			ObscuredPrefs.SetBool("gameComplete", value: true);
			ObscuredPrefs.SetUInt("demoUint", 1234567891u);
			ObscuredPrefs.SetLong("demoLong", 1234567891234567890L);
			ObscuredPrefs.SetDouble("demoDouble", 1.234567890123456);
			ObscuredPrefs.SetVector2("demoVector2", Vector2.one);
			ObscuredPrefs.SetVector3("demoVector3", Vector3.one);
			ObscuredPrefs.SetQuaternion("demoQuaternion", Quaternion.Euler(new Vector3(10f, 20f, 30f)));
			ObscuredPrefs.SetRect("demoRect", new Rect(1.5f, 2.6f, 3.7f, 4.8f));
			ObscuredPrefs.SetColor("demoColor", Color.red);
			ObscuredPrefs.SetByteArray("demoByteArray", new byte[4]
			{
				44,
				104,
				43,
				32
			});
			ObscuredPrefs.Save();
		}

		private void DeleteObscuredPrefs()
		{
			ObscuredPrefs.DeleteKey("money");
			ObscuredPrefs.DeleteKey("lifeBar");
			ObscuredPrefs.DeleteKey("name");
			ObscuredPrefs.DeleteKey("gameComplete");
			ObscuredPrefs.DeleteKey("demoUint");
			ObscuredPrefs.DeleteKey("demoLong");
			ObscuredPrefs.DeleteKey("demoDouble");
			ObscuredPrefs.DeleteKey("demoVector2");
			ObscuredPrefs.DeleteKey("demoVector3");
			ObscuredPrefs.DeleteKey("demoQuaternion");
			ObscuredPrefs.DeleteKey("demoRect");
			ObscuredPrefs.DeleteKey("demoColor");
			ObscuredPrefs.DeleteKey("demoByteArray");
			ObscuredPrefs.Save();
		}

		private void PlaceUrlButton(string url)
		{
			PlaceUrlButton(url, 30);
		}

		private void PlaceUrlButton(string url, int width)
		{
			PlaceUrlButton(url, "?", width);
		}

		private void PlaceUrlButton(string url, string buttonName, int width)
		{
			GUILayoutOption[] array = new GUILayoutOption[1];
			if (width != -1)
			{
				array[0] = GUILayout.Width(width);
			}
			else
			{
				array = null;
			}
			if (GUILayout.Button(buttonName, array))
			{
				Application.OpenURL(url);
			}
		}

		private void OnApplicationQuit()
		{
			DeleteRegularPrefs();
			DeleteObscuredPrefs();
		}
	}
}
