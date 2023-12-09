using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reporter : MonoBehaviour
{
	public enum _LogType
	{
		Assert = 1,
		Error = 0,
		Exception = 4,
		Log = 3,
		Warning = 2
	}

	public class Sample
	{
		public float time;

		public byte loadedScene;

		public float memory;

		public float fps;

		public string fpsText;

		public static float MemSize()
		{
			return 13f;
		}

		public string GetSceneName()
		{
			if (loadedScene == -1)
			{
				return "AssetBundleScene";
			}
			return scenes[loadedScene];
		}
	}

	public class Log
	{
		public int count = 1;

		public _LogType logType;

		public string condition;

		public string stacktrace;

		public int sampleId;

		public Log CreateCopy()
		{
			return (Log)MemberwiseClone();
		}

		public float GetMemoryUsage()
		{
			return 8 + condition.Length * 2 + stacktrace.Length * 2 + 4;
		}
	}

	private enum ReportView
	{
		None,
		Logs,
		Info,
		Snapshot
	}

	private enum DetailView
	{
		None,
		StackTrace,
		Graph
	}

	private List<Sample> samples = new List<Sample>(216000);

	private List<Log> logs = new List<Log>();

	private List<Log> collapsedLogs = new List<Log>();

	private List<Log> currentLog = new List<Log>();

	private MultiKeyDictionary<string, string, Log> logsDic = new MultiKeyDictionary<string, string, Log>();

	private Dictionary<string, string> cachedString = new Dictionary<string, string>();

	[HideInInspector]
	public bool show;

	private bool collapse;

	private bool clearOnNewSceneLoaded;

	private bool showTime;

	private bool showScene;

	private bool showMemory;

	private bool showFps;

	private bool showGraph;

	private bool showLog = true;

	private bool showWarning = true;

	private bool showError = true;

	private int numOfLogs;

	private int numOfLogsWarning;

	private int numOfLogsError;

	private int numOfCollapsedLogs;

	private int numOfCollapsedLogsWarning;

	private int numOfCollapsedLogsError;

	private bool showClearOnNewSceneLoadedButton = true;

	private bool showTimeButton = true;

	private bool showSceneButton = true;

	private bool showMemButton = true;

	private bool showFpsButton = true;

	private bool showSearchText = true;

	private string buildDate;

	private string logDate;

	private float logsMemUsage;

	private float graphMemUsage;

	private float gcTotalMemory;

	public string UserData = string.Empty;

	public float fps;

	public string fpsText;

	private ReportView currentView = ReportView.Logs;

	private static bool created;

	public Images images;

	private GUIContent clearContent;

	private GUIContent collapseContent;

	private GUIContent clearOnNewSceneContent;

	private GUIContent showTimeContent;

	private GUIContent showSceneContent;

	private GUIContent userContent;

	private GUIContent showMemoryContent;

	private GUIContent softwareContent;

	private GUIContent dateContent;

	private GUIContent showFpsContent;

	private GUIContent infoContent;

	private GUIContent searchContent;

	private GUIContent closeContent;

	private GUIContent buildFromContent;

	private GUIContent systemInfoContent;

	private GUIContent graphicsInfoContent;

	private GUIContent backContent;

	private GUIContent logContent;

	private GUIContent warningContent;

	private GUIContent errorContent;

	private GUIStyle barStyle;

	private GUIStyle buttonActiveStyle;

	private GUIStyle nonStyle;

	private GUIStyle lowerLeftFontStyle;

	private GUIStyle backStyle;

	private GUIStyle evenLogStyle;

	private GUIStyle oddLogStyle;

	private GUIStyle logButtonStyle;

	private GUIStyle selectedLogStyle;

	private GUIStyle selectedLogFontStyle;

	private GUIStyle stackLabelStyle;

	private GUIStyle scrollerStyle;

	private GUIStyle searchStyle;

	private GUIStyle sliderBackStyle;

	private GUIStyle sliderThumbStyle;

	private GUISkin toolbarScrollerSkin;

	private GUISkin logScrollerSkin;

	private GUISkin graphScrollerSkin;

	public Vector2 size = new Vector2(32f, 32f);

	public float maxSize = 20f;

	public int numOfCircleToShow = 1;

	private static string[] scenes;

	private string currentScene;

	private string filterText = string.Empty;

	private string deviceModel;

	private string deviceType;

	private string deviceName;

	private string graphicsMemorySize;

	private string maxTextureSize;

	private string systemMemorySize;

	public bool Initialized;

	private Rect screenRect;

	private Rect toolBarRect;

	private Rect logsRect;

	private Rect stackRect;

	private Rect graphRect;

	private Rect graphMinRect;

	private Rect graphMaxRect;

	private Rect buttomRect;

	private Vector2 stackRectTopLeft;

	private Rect detailRect;

	private Vector2 scrollPosition;

	private Vector2 scrollPosition2;

	private Vector2 toolbarScrollPosition;

	private Log selectedLog;

	private float toolbarOldDrag;

	private float oldDrag;

	private float oldDrag2;

	private float oldDrag3;

	private int startIndex;

	private Rect countRect;

	private Rect timeRect;

	private Rect timeLabelRect;

	private Rect sceneRect;

	private Rect sceneLabelRect;

	private Rect memoryRect;

	private Rect memoryLabelRect;

	private Rect fpsRect;

	private Rect fpsLabelRect;

	private GUIContent tempContent = new GUIContent();

	private Vector2 infoScrollPosition;

	private Vector2 oldInfoDrag;

	private Rect tempRect;

	private float graphSize = 4f;

	private int startFrame;

	private int currentFrame;

	private Vector3 tempVector1;

	private Vector3 tempVector2;

	private Vector2 graphScrollerPos;

	private float maxFpsValue;

	private float minFpsValue;

	private float maxMemoryValue;

	private float minMemoryValue;

	private List<Vector2> gestureDetector = new List<Vector2>();

	private Vector2 gestureSum = Vector2.zero;

	private float gestureLength;

	private int gestureCount;

	private float lastClickTime = -1f;

	private Vector2 startPos;

	private Vector2 downPos;

	private Vector2 mousePosition;

	private int frames;

	private bool firstTime = true;

	private float lastUpdate;

	private const int requiredFrames = 10;

	private const float updateInterval = 0.25f;

	private List<Log> threadedLogs = new List<Log>();

	public float TotalMemUsage => logsMemUsage + graphMemUsage;

	private void Awake()
	{
		if (!Initialized)
		{
			Initialize();
		}
	}

	private void OnEnable()
	{
		if (logs.Count == 0)
		{
			clear();
		}
	}

	private void OnDisable()
	{
	}

	private void addSample()
	{
		Sample sample = new Sample();
		sample.fps = fps;
		sample.fpsText = fpsText;
		sample.loadedScene = (byte)SceneManager.GetActiveScene().buildIndex;
		sample.time = Time.realtimeSinceStartup;
		sample.memory = gcTotalMemory;
		samples.Add(sample);
		graphMemUsage = (float)samples.Count * Sample.MemSize() / 1024f / 1024f;
	}

	public void Initialize()
	{
		if (!created)
		{
			try
			{
				base.gameObject.SendMessage("OnPreStart");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			scenes = new string[SceneManager.sceneCountInBuildSettings];
			currentScene = SceneManager.GetActiveScene().name;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.logMessageReceivedThreaded += CaptureLogThread;
			created = true;
			clearContent = new GUIContent(string.Empty, images.clearImage, "Clear logs");
			collapseContent = new GUIContent(string.Empty, images.collapseImage, "Collapse logs");
			clearOnNewSceneContent = new GUIContent(string.Empty, images.clearOnNewSceneImage, "Clear logs on new scene loaded");
			showTimeContent = new GUIContent(string.Empty, images.showTimeImage, "Show Hide Time");
			showSceneContent = new GUIContent(string.Empty, images.showSceneImage, "Show Hide Scene");
			showMemoryContent = new GUIContent(string.Empty, images.showMemoryImage, "Show Hide Memory");
			softwareContent = new GUIContent(string.Empty, images.softwareImage, "Software");
			dateContent = new GUIContent(string.Empty, images.dateImage, "Date");
			showFpsContent = new GUIContent(string.Empty, images.showFpsImage, "Show Hide fps");
			infoContent = new GUIContent(string.Empty, images.infoImage, "Information about application");
			searchContent = new GUIContent(string.Empty, images.searchImage, "Search for logs");
			closeContent = new GUIContent(string.Empty, images.closeImage, "Hide logs");
			userContent = new GUIContent(string.Empty, images.userImage, "User");
			buildFromContent = new GUIContent(string.Empty, images.buildFromImage, "Build From");
			systemInfoContent = new GUIContent(string.Empty, images.systemInfoImage, "System Info");
			graphicsInfoContent = new GUIContent(string.Empty, images.graphicsInfoImage, "Graphics Info");
			backContent = new GUIContent(string.Empty, images.backImage, "Back");
			logContent = new GUIContent(string.Empty, images.logImage, "show or hide logs");
			warningContent = new GUIContent(string.Empty, images.warningImage, "show or hide warnings");
			errorContent = new GUIContent(string.Empty, images.errorImage, "show or hide errors");
			currentView = (ReportView)PlayerPrefs.GetInt("Reporter_currentView", 1);
			show = ((PlayerPrefs.GetInt("Reporter_show") == 1) ? true : false);
			collapse = ((PlayerPrefs.GetInt("Reporter_collapse") == 1) ? true : false);
			clearOnNewSceneLoaded = ((PlayerPrefs.GetInt("Reporter_clearOnNewSceneLoaded") == 1) ? true : false);
			showTime = ((PlayerPrefs.GetInt("Reporter_showTime") == 1) ? true : false);
			showScene = ((PlayerPrefs.GetInt("Reporter_showScene") == 1) ? true : false);
			showMemory = ((PlayerPrefs.GetInt("Reporter_showMemory") == 1) ? true : false);
			showFps = ((PlayerPrefs.GetInt("Reporter_showFps") == 1) ? true : false);
			showGraph = ((PlayerPrefs.GetInt("Reporter_showGraph") == 1) ? true : false);
			showLog = ((PlayerPrefs.GetInt("Reporter_showLog", 1) == 1) ? true : false);
			showWarning = ((PlayerPrefs.GetInt("Reporter_showWarning", 1) == 1) ? true : false);
			showError = ((PlayerPrefs.GetInt("Reporter_showError", 1) == 1) ? true : false);
			filterText = PlayerPrefs.GetString("Reporter_filterText");
			size.x = (size.y = PlayerPrefs.GetFloat("Reporter_size", 32f));
			showClearOnNewSceneLoadedButton = ((PlayerPrefs.GetInt("Reporter_showClearOnNewSceneLoadedButton", 1) == 1) ? true : false);
			showTimeButton = ((PlayerPrefs.GetInt("Reporter_showTimeButton", 1) == 1) ? true : false);
			showSceneButton = ((PlayerPrefs.GetInt("Reporter_showSceneButton", 1) == 1) ? true : false);
			showMemButton = ((PlayerPrefs.GetInt("Reporter_showMemButton", 1) == 1) ? true : false);
			showFpsButton = ((PlayerPrefs.GetInt("Reporter_showFpsButton", 1) == 1) ? true : false);
			showSearchText = ((PlayerPrefs.GetInt("Reporter_showSearchText", 1) == 1) ? true : false);
			initializeStyle();
			Initialized = true;
			if (show)
			{
				doShow();
			}
			deviceModel = SystemInfo.deviceModel.ToString();
			deviceType = SystemInfo.deviceType.ToString();
			deviceName = SystemInfo.deviceName.ToString();
			graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
			maxTextureSize = SystemInfo.maxTextureSize.ToString();
			systemMemorySize = SystemInfo.systemMemorySize.ToString();
		}
		else
		{
			UnityEngine.Debug.LogWarning("tow manager is exists delete the second");
			UnityEngine.Object.DestroyImmediate(base.gameObject, allowDestroyingAssets: true);
		}
	}

	private void initializeStyle()
	{
		int num = (int)(size.x * 0.2f);
		int num2 = (int)(size.y * 0.2f);
		nonStyle = new GUIStyle();
		nonStyle.clipping = TextClipping.Clip;
		nonStyle.border = new RectOffset(0, 0, 0, 0);
		nonStyle.normal.background = null;
		nonStyle.fontSize = (int)(size.y / 2f);
		nonStyle.alignment = TextAnchor.MiddleCenter;
		lowerLeftFontStyle = new GUIStyle();
		lowerLeftFontStyle.clipping = TextClipping.Clip;
		lowerLeftFontStyle.border = new RectOffset(0, 0, 0, 0);
		lowerLeftFontStyle.normal.background = null;
		lowerLeftFontStyle.fontSize = (int)(size.y / 2f);
		lowerLeftFontStyle.fontStyle = FontStyle.Bold;
		lowerLeftFontStyle.alignment = TextAnchor.LowerLeft;
		barStyle = new GUIStyle();
		barStyle.border = new RectOffset(1, 1, 1, 1);
		barStyle.normal.background = images.barImage;
		barStyle.active.background = images.button_activeImage;
		barStyle.alignment = TextAnchor.MiddleCenter;
		barStyle.margin = new RectOffset(1, 1, 1, 1);
		barStyle.clipping = TextClipping.Clip;
		barStyle.fontSize = (int)(size.y / 2f);
		buttonActiveStyle = new GUIStyle();
		buttonActiveStyle.border = new RectOffset(1, 1, 1, 1);
		buttonActiveStyle.normal.background = images.button_activeImage;
		buttonActiveStyle.alignment = TextAnchor.MiddleCenter;
		buttonActiveStyle.margin = new RectOffset(1, 1, 1, 1);
		buttonActiveStyle.fontSize = (int)(size.y / 2f);
		backStyle = new GUIStyle();
		backStyle.normal.background = images.even_logImage;
		backStyle.clipping = TextClipping.Clip;
		backStyle.fontSize = (int)(size.y / 2f);
		evenLogStyle = new GUIStyle();
		evenLogStyle.normal.background = images.even_logImage;
		evenLogStyle.fixedHeight = size.y;
		evenLogStyle.clipping = TextClipping.Clip;
		evenLogStyle.alignment = TextAnchor.UpperLeft;
		evenLogStyle.imagePosition = ImagePosition.ImageLeft;
		evenLogStyle.fontSize = (int)(size.y / 2f);
		oddLogStyle = new GUIStyle();
		oddLogStyle.normal.background = images.odd_logImage;
		oddLogStyle.fixedHeight = size.y;
		oddLogStyle.clipping = TextClipping.Clip;
		oddLogStyle.alignment = TextAnchor.UpperLeft;
		oddLogStyle.imagePosition = ImagePosition.ImageLeft;
		oddLogStyle.fontSize = (int)(size.y / 2f);
		logButtonStyle = new GUIStyle();
		logButtonStyle.fixedHeight = size.y;
		logButtonStyle.clipping = TextClipping.Clip;
		logButtonStyle.alignment = TextAnchor.UpperLeft;
		logButtonStyle.fontSize = (int)(size.y / 2f);
		logButtonStyle.padding = new RectOffset(num, num, num2, num2);
		selectedLogStyle = new GUIStyle();
		selectedLogStyle.normal.background = images.selectedImage;
		selectedLogStyle.fixedHeight = size.y;
		selectedLogStyle.clipping = TextClipping.Clip;
		selectedLogStyle.alignment = TextAnchor.UpperLeft;
		selectedLogStyle.normal.textColor = Color.white;
		selectedLogStyle.fontSize = (int)(size.y / 2f);
		selectedLogFontStyle = new GUIStyle();
		selectedLogFontStyle.normal.background = images.selectedImage;
		selectedLogFontStyle.fixedHeight = size.y;
		selectedLogFontStyle.clipping = TextClipping.Clip;
		selectedLogFontStyle.alignment = TextAnchor.UpperLeft;
		selectedLogFontStyle.normal.textColor = Color.white;
		selectedLogFontStyle.fontSize = (int)(size.y / 2f);
		selectedLogFontStyle.padding = new RectOffset(num, num, num2, num2);
		stackLabelStyle = new GUIStyle();
		stackLabelStyle.wordWrap = true;
		stackLabelStyle.fontSize = (int)(size.y / 2f);
		stackLabelStyle.padding = new RectOffset(num, num, num2, num2);
		scrollerStyle = new GUIStyle();
		scrollerStyle.normal.background = images.barImage;
		searchStyle = new GUIStyle();
		searchStyle.clipping = TextClipping.Clip;
		searchStyle.alignment = TextAnchor.LowerCenter;
		searchStyle.fontSize = (int)(size.y / 2f);
		searchStyle.wordWrap = true;
		sliderBackStyle = new GUIStyle();
		sliderBackStyle.normal.background = images.barImage;
		sliderBackStyle.fixedHeight = size.y;
		sliderBackStyle.border = new RectOffset(1, 1, 1, 1);
		sliderThumbStyle = new GUIStyle();
		sliderThumbStyle.normal.background = images.selectedImage;
		sliderThumbStyle.fixedWidth = size.x;
		GUISkin reporterScrollerSkin = images.reporterScrollerSkin;
		toolbarScrollerSkin = UnityEngine.Object.Instantiate(reporterScrollerSkin);
		toolbarScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		toolbarScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		toolbarScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		toolbarScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		logScrollerSkin = UnityEngine.Object.Instantiate(reporterScrollerSkin);
		logScrollerSkin.verticalScrollbar.fixedWidth = size.x * 2f;
		logScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		logScrollerSkin.verticalScrollbarThumb.fixedWidth = size.x * 2f;
		logScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		graphScrollerSkin = UnityEngine.Object.Instantiate(reporterScrollerSkin);
		graphScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		graphScrollerSkin.horizontalScrollbar.fixedHeight = size.x * 2f;
		graphScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		graphScrollerSkin.horizontalScrollbarThumb.fixedHeight = size.x * 2f;
	}

	private void Start()
	{
		logDate = DateTime.Now.ToString();
		StartCoroutine("readInfo");
	}

	private void clear()
	{
		logs.Clear();
		collapsedLogs.Clear();
		currentLog.Clear();
		logsDic.Clear();
		selectedLog = null;
		numOfLogs = 0;
		numOfLogsWarning = 0;
		numOfLogsError = 0;
		numOfCollapsedLogs = 0;
		numOfCollapsedLogsWarning = 0;
		numOfCollapsedLogsError = 0;
		logsMemUsage = 0f;
		graphMemUsage = 0f;
		samples.Clear();
		GC.Collect();
		selectedLog = null;
	}

	private void calculateCurrentLog()
	{
		bool flag = !string.IsNullOrEmpty(filterText);
		string value = string.Empty;
		if (flag)
		{
			value = filterText.ToLower();
		}
		currentLog.Clear();
		if (collapse)
		{
			for (int i = 0; i < collapsedLogs.Count; i++)
			{
				Log log = collapsedLogs[i];
				if ((log.logType == _LogType.Log && !showLog) || (log.logType == _LogType.Warning && !showWarning) || (log.logType == _LogType.Error && !showError) || (log.logType == _LogType.Assert && !showError) || (log.logType == _LogType.Exception && !showError))
				{
					continue;
				}
				if (flag)
				{
					if (log.condition.ToLower().Contains(value))
					{
						currentLog.Add(log);
					}
				}
				else
				{
					currentLog.Add(log);
				}
			}
		}
		else
		{
			for (int j = 0; j < logs.Count; j++)
			{
				Log log2 = logs[j];
				if ((log2.logType == _LogType.Log && !showLog) || (log2.logType == _LogType.Warning && !showWarning) || (log2.logType == _LogType.Error && !showError) || (log2.logType == _LogType.Assert && !showError) || (log2.logType == _LogType.Exception && !showError))
				{
					continue;
				}
				if (flag)
				{
					if (log2.condition.ToLower().Contains(value))
					{
						currentLog.Add(log2);
					}
				}
				else
				{
					currentLog.Add(log2);
				}
			}
		}
		if (selectedLog == null)
		{
			return;
		}
		int num = currentLog.IndexOf(selectedLog);
		if (num == -1)
		{
			Log item = logsDic[selectedLog.condition][selectedLog.stacktrace];
			num = currentLog.IndexOf(item);
			if (num != -1)
			{
				scrollPosition.y = (float)num * size.y;
			}
		}
		else
		{
			scrollPosition.y = (float)num * size.y;
		}
	}

	private void DrawInfo()
	{
		GUILayout.BeginArea(screenRect, backStyle);
		Vector2 drag = getDrag();
		if (drag.x != 0f && downPos != Vector2.zero)
		{
			infoScrollPosition.x -= drag.x - oldInfoDrag.x;
		}
		if (drag.y != 0f && downPos != Vector2.zero)
		{
			infoScrollPosition.y += drag.y - oldInfoDrag.y;
		}
		oldInfoDrag = drag;
		GUI.skin = toolbarScrollerSkin;
		infoScrollPosition = GUILayout.BeginScrollView(infoScrollPosition);
		GUILayout.Space(size.x);
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(buildFromContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(buildDate, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(systemInfoContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(deviceModel, nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(deviceType, nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(deviceName, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(graphicsInfoContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(SystemInfo.graphicsDeviceName, nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(graphicsMemorySize, nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(maxTextureSize, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Label("Screen Width " + Screen.width, nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label("Screen Height " + Screen.height, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(systemMemorySize + " mb", nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Space(size.x);
		GUILayout.Label("Mem Usage Of Logs " + logsMemUsage.ToString("0.000") + " mb", nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label("GC Memory " + gcTotalMemory.ToString("0.000") + " mb", nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(softwareContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(SystemInfo.operatingSystem, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(dateContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(DateTime.Now.ToString(), nonStyle, GUILayout.Height(size.y));
		GUILayout.Label(" - Application Started At " + logDate, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(Time.realtimeSinceStartup.ToString("000"), nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(fpsText, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(userContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(UserData, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label(currentScene, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.Label("Unity Version = " + Application.unityVersion, nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		drawInfo_enableDisableToolBarButtons();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Label("Size = " + size.x.ToString("0.0"), nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		float num = GUILayout.HorizontalSlider(size.x, 16f, 64f, sliderBackStyle, sliderThumbStyle, GUILayout.Width((float)Screen.width * 0.5f));
		if (size.x != num)
		{
			size.x = (size.y = num);
			initializeStyle();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		if (GUILayout.Button(backContent, barStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			currentView = ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void drawInfo_enableDisableToolBarButtons()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		GUILayout.Label("Hide or Show tool bar buttons", nonStyle, GUILayout.Height(size.y));
		GUILayout.Space(size.x);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(size.x);
		if (GUILayout.Button(clearOnNewSceneContent, (!showClearOnNewSceneLoadedButton) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showClearOnNewSceneLoadedButton = !showClearOnNewSceneLoadedButton;
		}
		if (GUILayout.Button(showTimeContent, (!showTimeButton) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showTimeButton = !showTimeButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
		if (GUILayout.Button(showSceneContent, (!showSceneButton) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showSceneButton = !showSceneButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
		if (GUILayout.Button(showMemoryContent, (!showMemButton) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showMemButton = !showMemButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);
		if (GUILayout.Button(showFpsContent, (!showFpsButton) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showFpsButton = !showFpsButton;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
		if (GUILayout.Button(searchContent, (!showSearchText) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showSearchText = !showSearchText;
		}
		tempRect = GUILayoutUtility.GetLastRect();
		GUI.TextField(tempRect, filterText, searchStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void DrawReport()
	{
		screenRect.x = 0f;
		screenRect.y = 0f;
		screenRect.width = Screen.width;
		screenRect.height = Screen.height;
		GUILayout.BeginArea(screenRect, backStyle);
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Select Photo", nonStyle, GUILayout.Height(size.y));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Coming Soon", nonStyle, GUILayout.Height(size.y));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(backContent, barStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
		{
			currentView = ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void drawToolBar()
	{
		toolBarRect.x = 0f;
		toolBarRect.y = 0f;
		toolBarRect.width = Screen.width;
		toolBarRect.height = size.y * 2f;
		GUI.skin = toolbarScrollerSkin;
		Vector2 drag = getDrag();
		if (drag.x != 0f && downPos != Vector2.zero && downPos.y > (float)Screen.height - size.y * 2f)
		{
			toolbarScrollPosition.x -= drag.x - toolbarOldDrag;
		}
		toolbarOldDrag = drag.x;
		GUILayout.BeginArea(toolBarRect);
		toolbarScrollPosition = GUILayout.BeginScrollView(toolbarScrollPosition);
		GUILayout.BeginHorizontal(barStyle);
		if (GUILayout.Button(clearContent, barStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			clear();
		}
		if (GUILayout.Button(collapseContent, (!collapse) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			collapse = !collapse;
			calculateCurrentLog();
		}
		if (showClearOnNewSceneLoadedButton && GUILayout.Button(clearOnNewSceneContent, (!clearOnNewSceneLoaded) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			clearOnNewSceneLoaded = !clearOnNewSceneLoaded;
		}
		if (showTimeButton && GUILayout.Button(showTimeContent, (!showTime) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showTime = !showTime;
		}
		if (showSceneButton)
		{
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
			if (GUILayout.Button(showSceneContent, (!showScene) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
			{
				showScene = !showScene;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
		}
		if (showMemButton)
		{
			if (GUILayout.Button(showMemoryContent, (!showMemory) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
			{
				showMemory = !showMemory;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);
		}
		if (showFpsButton)
		{
			if (GUILayout.Button(showFpsContent, (!showFps) ? barStyle : buttonActiveStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
			{
				showFps = !showFps;
			}
			tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
		}
		if (showSearchText)
		{
			GUILayout.Box(searchContent, barStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f));
			tempRect = GUILayoutUtility.GetLastRect();
			string a = GUI.TextField(tempRect, filterText, searchStyle);
			if (a != filterText)
			{
				filterText = a;
				calculateCurrentLog();
			}
		}
		if (GUILayout.Button(infoContent, barStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			currentView = ReportView.Info;
		}
		GUILayout.FlexibleSpace();
		string arg = " ";
		arg = ((!collapse) ? (arg + numOfLogs) : (arg + numOfCollapsedLogs));
		string arg2 = " ";
		arg2 = ((!collapse) ? (arg2 + numOfLogsWarning) : (arg2 + numOfCollapsedLogsWarning));
		string arg3 = " ";
		arg3 = ((!collapse) ? (arg3 + numOfLogsError) : (arg3 + numOfCollapsedLogsError));
		GUILayout.BeginHorizontal((!showLog) ? barStyle : buttonActiveStyle);
		if (GUILayout.Button(logContent, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showLog = !showLog;
			calculateCurrentLog();
		}
		if (GUILayout.Button(arg, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showLog = !showLog;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!showWarning) ? barStyle : buttonActiveStyle);
		if (GUILayout.Button(warningContent, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showWarning = !showWarning;
			calculateCurrentLog();
		}
		if (GUILayout.Button(arg2, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showWarning = !showWarning;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!showError) ? nonStyle : buttonActiveStyle);
		if (GUILayout.Button(errorContent, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showError = !showError;
			calculateCurrentLog();
		}
		if (GUILayout.Button(arg3, nonStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			showError = !showError;
			calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button(closeContent, barStyle, GUILayout.Width(size.x * 2f), GUILayout.Height(size.y * 2f)))
		{
			show = false;
			ReporterGUI component = base.gameObject.GetComponent<ReporterGUI>();
			UnityEngine.Object.DestroyImmediate(component);
			try
			{
				base.gameObject.SendMessage("OnHideReporter");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void DrawLogs()
	{
		GUILayout.BeginArea(logsRect, backStyle);
		GUI.skin = logScrollerSkin;
		Vector2 drag = getDrag();
		if (drag.y != 0f && logsRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
		{
			scrollPosition.y += drag.y - oldDrag;
		}
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		oldDrag = drag.y;
		int a = (int)((float)Screen.height * 0.75f / size.y);
		int count = currentLog.Count;
		a = Mathf.Min(a, count - startIndex);
		int num = 0;
		int num2 = (int)((float)startIndex * size.y);
		if (num2 > 0)
		{
			GUILayout.BeginHorizontal(GUILayout.Height(num2));
			GUILayout.Label("---");
			GUILayout.EndHorizontal();
		}
		int value = startIndex + a;
		value = Mathf.Clamp(value, 0, count);
		bool flag = a < count;
		int num3 = startIndex;
		while (startIndex + num < value && num3 < currentLog.Count)
		{
			Log log = currentLog[num3];
			if ((log.logType != _LogType.Log || showLog) && (log.logType != _LogType.Warning || showWarning) && (log.logType != 0 || showError) && (log.logType != _LogType.Assert || showError) && (log.logType != _LogType.Exception || showError))
			{
				if (num >= a)
				{
					break;
				}
				GUIContent gUIContent = null;
				gUIContent = ((log.logType == _LogType.Log) ? logContent : ((log.logType != _LogType.Warning) ? errorContent : warningContent));
				GUIStyle gUIStyle = ((startIndex + num) % 2 != 0) ? oddLogStyle : evenLogStyle;
				if (log == selectedLog)
				{
					gUIStyle = selectedLogStyle;
				}
				tempContent.text = log.count.ToString();
				float num4 = 0f;
				if (collapse)
				{
					Vector2 vector = barStyle.CalcSize(tempContent);
					num4 = vector.x + 3f;
				}
				countRect.x = (float)Screen.width - num4;
				countRect.y = size.y * (float)num3;
				if (num2 > 0)
				{
					countRect.y += 8f;
				}
				countRect.width = num4;
				countRect.height = size.y;
				if (flag)
				{
					countRect.x -= size.x * 2f;
				}
				Sample sample = samples[log.sampleId];
				fpsRect = countRect;
				if (showFps)
				{
					tempContent.text = sample.fpsText;
					Vector2 vector2 = gUIStyle.CalcSize(tempContent);
					num4 = vector2.x + size.x;
					fpsRect.x -= num4;
					fpsRect.width = size.x;
					fpsLabelRect = fpsRect;
					fpsLabelRect.x += size.x;
					fpsLabelRect.width = num4 - size.x;
				}
				memoryRect = fpsRect;
				if (showMemory)
				{
					tempContent.text = sample.memory.ToString("0.000");
					Vector2 vector3 = gUIStyle.CalcSize(tempContent);
					num4 = vector3.x + size.x;
					memoryRect.x -= num4;
					memoryRect.width = size.x;
					memoryLabelRect = memoryRect;
					memoryLabelRect.x += size.x;
					memoryLabelRect.width = num4 - size.x;
				}
				sceneRect = memoryRect;
				if (showScene)
				{
					tempContent.text = sample.GetSceneName();
					Vector2 vector4 = gUIStyle.CalcSize(tempContent);
					num4 = vector4.x + size.x;
					sceneRect.x -= num4;
					sceneRect.width = size.x;
					sceneLabelRect = sceneRect;
					sceneLabelRect.x += size.x;
					sceneLabelRect.width = num4 - size.x;
				}
				timeRect = sceneRect;
				if (showTime)
				{
					tempContent.text = sample.time.ToString("0.000");
					Vector2 vector5 = gUIStyle.CalcSize(tempContent);
					num4 = vector5.x + size.x;
					timeRect.x -= num4;
					timeRect.width = size.x;
					timeLabelRect = timeRect;
					timeLabelRect.x += size.x;
					timeLabelRect.width = num4 - size.x;
				}
				GUILayout.BeginHorizontal(gUIStyle);
				if (log == selectedLog)
				{
					GUILayout.Box(gUIContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
					GUILayout.Label(log.condition, selectedLogFontStyle);
					if (showTime)
					{
						GUI.Box(timeRect, showTimeContent, gUIStyle);
						GUI.Label(timeLabelRect, sample.time.ToString("0.000"), gUIStyle);
					}
					if (showScene)
					{
						GUI.Box(sceneRect, showSceneContent, gUIStyle);
						GUI.Label(sceneLabelRect, sample.GetSceneName(), gUIStyle);
					}
					if (showMemory)
					{
						GUI.Box(memoryRect, showMemoryContent, gUIStyle);
						GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", gUIStyle);
					}
					if (showFps)
					{
						GUI.Box(fpsRect, showFpsContent, gUIStyle);
						GUI.Label(fpsLabelRect, sample.fpsText, gUIStyle);
					}
				}
				else
				{
					if (GUILayout.Button(gUIContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
					{
						selectedLog = log;
					}
					if (GUILayout.Button(log.condition, logButtonStyle))
					{
						selectedLog = log;
					}
					if (showTime)
					{
						GUI.Box(timeRect, showTimeContent, gUIStyle);
						GUI.Label(timeLabelRect, sample.time.ToString("0.000"), gUIStyle);
					}
					if (showScene)
					{
						GUI.Box(sceneRect, showSceneContent, gUIStyle);
						GUI.Label(sceneLabelRect, sample.GetSceneName(), gUIStyle);
					}
					if (showMemory)
					{
						GUI.Box(memoryRect, showMemoryContent, gUIStyle);
						GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", gUIStyle);
					}
					if (showFps)
					{
						GUI.Box(fpsRect, showFpsContent, gUIStyle);
						GUI.Label(fpsLabelRect, sample.fpsText, gUIStyle);
					}
				}
				if (collapse)
				{
					GUI.Label(countRect, log.count.ToString(), barStyle);
				}
				GUILayout.EndHorizontal();
				num++;
			}
			num3++;
		}
		int num5 = (int)((float)(count - (startIndex + a)) * size.y);
		if (num5 > 0)
		{
			GUILayout.BeginHorizontal(GUILayout.Height(num5));
			GUILayout.Label(" ");
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		buttomRect.x = 0f;
		buttomRect.y = (float)Screen.height - size.y;
		buttomRect.width = Screen.width;
		buttomRect.height = size.y;
		if (showGraph)
		{
			drawGraph();
		}
		else
		{
			drawStack();
		}
	}

	private void drawGraph()
	{
		graphRect = stackRect;
		graphRect.height = (float)Screen.height * 0.25f;
		GUI.skin = graphScrollerSkin;
		Vector2 drag = getDrag();
		if (graphRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
		{
			if (drag.x != 0f)
			{
				graphScrollerPos.x -= drag.x - oldDrag3;
				graphScrollerPos.x = Mathf.Max(0f, graphScrollerPos.x);
			}
			Vector2 lhs = downPos;
			if (lhs != Vector2.zero)
			{
				currentFrame = startFrame + (int)(lhs.x / graphSize);
			}
		}
		oldDrag3 = drag.x;
		GUILayout.BeginArea(graphRect, backStyle);
		graphScrollerPos = GUILayout.BeginScrollView(graphScrollerPos);
		startFrame = (int)(graphScrollerPos.x / graphSize);
		if (graphScrollerPos.x >= (float)samples.Count * graphSize - (float)Screen.width)
		{
			graphScrollerPos.x += graphSize;
		}
		GUILayout.Label(" ", GUILayout.Width((float)samples.Count * graphSize));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		maxFpsValue = 0f;
		minFpsValue = 100000f;
		maxMemoryValue = 0f;
		minMemoryValue = 100000f;
		for (int i = 0; (float)i < (float)Screen.width / graphSize; i++)
		{
			int num = startFrame + i;
			if (num >= samples.Count)
			{
				break;
			}
			Sample sample = samples[num];
			if (maxFpsValue < sample.fps)
			{
				maxFpsValue = sample.fps;
			}
			if (minFpsValue > sample.fps)
			{
				minFpsValue = sample.fps;
			}
			if (maxMemoryValue < sample.memory)
			{
				maxMemoryValue = sample.memory;
			}
			if (minMemoryValue > sample.memory)
			{
				minMemoryValue = sample.memory;
			}
		}
		if (currentFrame != -1 && currentFrame < samples.Count)
		{
			Sample sample2 = samples[currentFrame];
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.BeginHorizontal();
			GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample2.time.ToString("0.0"), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample2.GetSceneName(), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample2.memory.ToString("0.000"), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample2.fpsText, nonStyle);
			GUILayout.Space(size.x);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		graphMaxRect = stackRect;
		graphMaxRect.height = size.y;
		GUILayout.BeginArea(graphMaxRect);
		GUILayout.BeginHorizontal();
		GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Label(maxMemoryValue.ToString("0.000"), nonStyle);
		GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Label(maxFpsValue.ToString("0.000"), nonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		graphMinRect = stackRect;
		graphMinRect.y = stackRect.y + stackRect.height - size.y;
		graphMinRect.height = size.y;
		GUILayout.BeginArea(graphMinRect);
		GUILayout.BeginHorizontal();
		GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Label(minMemoryValue.ToString("0.000"), nonStyle);
		GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
		GUILayout.Label(minFpsValue.ToString("0.000"), nonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void drawStack()
	{
		if (selectedLog != null)
		{
			Vector2 drag = getDrag();
			if (drag.y != 0f && stackRect.Contains(new Vector2(downPos.x, (float)Screen.height - downPos.y)))
			{
				scrollPosition2.y += drag.y - oldDrag2;
			}
			oldDrag2 = drag.y;
			GUILayout.BeginArea(stackRect, backStyle);
			scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2);
			Sample sample = null;
			try
			{
				sample = samples[selectedLog.sampleId];
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label(selectedLog.condition, stackLabelStyle);
			GUILayout.EndHorizontal();
			GUILayout.Space(size.y * 0.25f);
			GUILayout.BeginHorizontal();
			GUILayout.Label(selectedLog.stacktrace, stackLabelStyle);
			GUILayout.EndHorizontal();
			GUILayout.Space(size.y);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.BeginHorizontal();
			GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample.time.ToString("0.000"), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample.GetSceneName(), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample.memory.ToString("0.000"), nonStyle);
			GUILayout.Space(size.x);
			GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
			GUILayout.Label(sample.fpsText, nonStyle);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(stackRect, backStyle);
			GUILayout.EndArea();
			GUILayout.BeginArea(buttomRect, backStyle);
			GUILayout.EndArea();
		}
	}

	public void OnGUIDraw()
	{
		if (show)
		{
			screenRect.x = 0f;
			screenRect.y = 0f;
			screenRect.width = Screen.width;
			screenRect.height = Screen.height;
			getDownPos();
			logsRect.x = 0f;
			logsRect.y = size.y * 2f;
			logsRect.width = Screen.width;
			logsRect.height = (float)Screen.height * 0.75f - size.y * 2f;
			stackRectTopLeft.x = 0f;
			stackRect.x = 0f;
			stackRectTopLeft.y = (float)Screen.height * 0.75f;
			stackRect.y = (float)Screen.height * 0.75f;
			stackRect.width = Screen.width;
			stackRect.height = (float)Screen.height * 0.25f - size.y;
			detailRect.x = 0f;
			detailRect.y = (float)Screen.height - size.y * 3f;
			detailRect.width = Screen.width;
			detailRect.height = size.y * 3f;
			if (currentView == ReportView.Info)
			{
				DrawInfo();
			}
			else if (currentView == ReportView.Logs)
			{
				drawToolBar();
				DrawLogs();
			}
		}
	}

	private bool isGestureDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				gestureDetector.Clear();
				gestureCount = 0;
			}
			else if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
			{
				gestureDetector.Clear();
			}
			else if (Input.touches[0].phase == TouchPhase.Moved)
			{
				Vector2 position = Input.touches[0].position;
				if (gestureDetector.Count == 0 || (position - gestureDetector[gestureDetector.Count - 1]).magnitude > 10f)
				{
					gestureDetector.Add(position);
				}
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			gestureDetector.Clear();
			gestureCount = 0;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 vector = UnityEngine.Input.mousePosition;
			float x = vector.x;
			Vector3 vector2 = UnityEngine.Input.mousePosition;
			Vector2 vector3 = new Vector2(x, vector2.y);
			if (gestureDetector.Count == 0 || (vector3 - gestureDetector[gestureDetector.Count - 1]).magnitude > 10f)
			{
				gestureDetector.Add(vector3);
			}
		}
		if (gestureDetector.Count < 10)
		{
			return false;
		}
		gestureSum = Vector2.zero;
		gestureLength = 0f;
		Vector2 rhs = Vector2.zero;
		for (int i = 0; i < gestureDetector.Count - 2; i++)
		{
			Vector2 vector4 = gestureDetector[i + 1] - gestureDetector[i];
			float magnitude = vector4.magnitude;
			gestureSum += vector4;
			gestureLength += magnitude;
			float num = Vector2.Dot(vector4, rhs);
			if (num < 0f)
			{
				gestureDetector.Clear();
				gestureCount = 0;
				return false;
			}
			rhs = vector4;
		}
		int num2 = (Screen.width + Screen.height) / 4;
		if (gestureLength > (float)num2 && gestureSum.magnitude < (float)(num2 / 2))
		{
			gestureDetector.Clear();
			gestureCount++;
			if (gestureCount >= numOfCircleToShow)
			{
				return true;
			}
		}
		return false;
	}

	private bool isDoubleClickDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				lastClickTime = -1f;
			}
			else if (Input.touches[0].phase == TouchPhase.Began)
			{
				if (lastClickTime == -1f)
				{
					lastClickTime = Time.realtimeSinceStartup;
				}
				else
				{
					if (Time.realtimeSinceStartup - lastClickTime < 0.2f)
					{
						lastClickTime = -1f;
						return true;
					}
					lastClickTime = Time.realtimeSinceStartup;
				}
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			if (lastClickTime == -1f)
			{
				lastClickTime = Time.realtimeSinceStartup;
			}
			else
			{
				if (Time.realtimeSinceStartup - lastClickTime < 0.2f)
				{
					lastClickTime = -1f;
					return true;
				}
				lastClickTime = Time.realtimeSinceStartup;
			}
		}
		return false;
	}

	private Vector2 getDownPos()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
			{
				downPos = Input.touches[0].position;
				return downPos;
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			ref Vector2 reference = ref downPos;
			Vector3 vector = UnityEngine.Input.mousePosition;
			reference.x = vector.x;
			ref Vector2 reference2 = ref downPos;
			Vector3 vector2 = UnityEngine.Input.mousePosition;
			reference2.y = vector2.y;
			return downPos;
		}
		return Vector2.zero;
	}

	private Vector2 getDrag()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				return Vector2.zero;
			}
			return Input.touches[0].position - downPos;
		}
		if (Input.GetMouseButton(0))
		{
			mousePosition = UnityEngine.Input.mousePosition;
			return mousePosition - downPos;
		}
		return Vector2.zero;
	}

	private void calculateStartIndex()
	{
		startIndex = (int)(scrollPosition.y / size.y);
		startIndex = Mathf.Clamp(startIndex, 0, currentLog.Count);
	}

	private void doShow()
	{
		show = true;
		currentView = ReportView.Logs;
		base.gameObject.AddComponent<ReporterGUI>();
		try
		{
			base.gameObject.SendMessage("OnShowReporter");
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void Update()
	{
		fpsText = fps.ToString("0.000");
		gcTotalMemory = (float)GC.GetTotalMemory(forceFullCollection: false) / 1024f / 1024f;
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex != -1 && string.IsNullOrEmpty(scenes[buildIndex]))
		{
			scenes[SceneManager.GetActiveScene().buildIndex] = SceneManager.GetActiveScene().name;
		}
		calculateStartIndex();
		if (!show && isGestureDone())
		{
			doShow();
		}
		if (threadedLogs.Count > 0)
		{
			lock (threadedLogs)
			{
				for (int i = 0; i < threadedLogs.Count; i++)
				{
					Log log = threadedLogs[i];
					AddLog(log.condition, log.stacktrace, (LogType)log.logType);
				}
				threadedLogs.Clear();
			}
		}
		if (firstTime)
		{
			firstTime = false;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
			return;
		}
		frames++;
		float num = Time.realtimeSinceStartup - lastUpdate;
		if (num > 0.25f && frames > 10)
		{
			fps = (float)frames / num;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
		}
	}

	private void CaptureLog(string condition, string stacktrace, LogType type)
	{
		AddLog(condition, stacktrace, type);
	}

	private void AddLog(string condition, string stacktrace, LogType type)
	{
		float num = 0f;
		string empty = string.Empty;
		if (cachedString.ContainsKey(condition))
		{
			empty = cachedString[condition];
		}
		else
		{
			empty = condition;
			cachedString.Add(empty, empty);
			num += (float)((!string.IsNullOrEmpty(empty)) ? (empty.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		string empty2 = string.Empty;
		if (cachedString.ContainsKey(stacktrace))
		{
			empty2 = cachedString[stacktrace];
		}
		else
		{
			empty2 = stacktrace;
			cachedString.Add(empty2, empty2);
			num += (float)((!string.IsNullOrEmpty(empty2)) ? (empty2.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		bool flag = false;
		addSample();
		Log log = new Log();
		log.logType = (_LogType)type;
		log.condition = empty;
		log.stacktrace = empty2;
		log.sampleId = samples.Count - 1;
		Log log2 = log;
		num += log2.GetMemoryUsage();
		logsMemUsage += num / 1024f / 1024f;
		if (TotalMemUsage > maxSize)
		{
			clear();
			UnityEngine.Debug.Log("Memory Usage Reach" + maxSize + " mb So It is Cleared");
			return;
		}
		bool flag2 = false;
		if (logsDic.ContainsKey(empty, stacktrace))
		{
			flag2 = false;
			logsDic[empty][stacktrace].count++;
		}
		else
		{
			flag2 = true;
			collapsedLogs.Add(log2);
			logsDic[empty][stacktrace] = log2;
			switch (type)
			{
			case LogType.Log:
				numOfCollapsedLogs++;
				break;
			case LogType.Warning:
				numOfCollapsedLogsWarning++;
				break;
			default:
				numOfCollapsedLogsError++;
				break;
			}
		}
		switch (type)
		{
		case LogType.Log:
			numOfLogs++;
			break;
		case LogType.Warning:
			numOfLogsWarning++;
			break;
		default:
			numOfLogsError++;
			break;
		}
		logs.Add(log2);
		if (!collapse || flag2)
		{
			bool flag3 = false;
			if (log2.logType == _LogType.Log && !showLog)
			{
				flag3 = true;
			}
			if (log2.logType == _LogType.Warning && !showWarning)
			{
				flag3 = true;
			}
			if (log2.logType == _LogType.Error && !showError)
			{
				flag3 = true;
			}
			if (log2.logType == _LogType.Assert && !showError)
			{
				flag3 = true;
			}
			if (log2.logType == _LogType.Exception && !showError)
			{
				flag3 = true;
			}
			if (!flag3 && (string.IsNullOrEmpty(filterText) || log2.condition.ToLower().Contains(filterText.ToLower())))
			{
				currentLog.Add(log2);
				flag = true;
			}
		}
		if (flag)
		{
			calculateStartIndex();
			int count = currentLog.Count;
			int num2 = (int)((float)Screen.height * 0.75f / size.y);
			if (startIndex >= count - num2)
			{
				scrollPosition.y += size.y;
			}
		}
		try
		{
			base.gameObject.SendMessage("OnLog", log2);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void CaptureLogThread(string condition, string stacktrace, LogType type)
	{
		Log log = new Log();
		log.condition = condition;
		log.stacktrace = stacktrace;
		log.logType = (_LogType)type;
		Log item = log;
		lock (threadedLogs)
		{
			threadedLogs.Add(item);
		}
	}

	private void OnLevelWasLoaded()
	{
		if (clearOnNewSceneLoaded)
		{
			clear();
		}
		currentScene = SceneManager.GetActiveScene().name;
		UnityEngine.Debug.Log("Scene " + SceneManager.GetActiveScene().name + " is loaded");
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("Reporter_currentView", (int)currentView);
		PlayerPrefs.SetInt("Reporter_show", show ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_collapse", collapse ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_clearOnNewSceneLoaded", clearOnNewSceneLoaded ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showTime", showTime ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showScene", showScene ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showMemory", showMemory ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showFps", showFps ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showGraph", showGraph ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showLog", showLog ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showWarning", showWarning ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showError", showError ? 1 : 0);
		PlayerPrefs.SetString("Reporter_filterText", filterText);
		PlayerPrefs.SetFloat("Reporter_size", size.x);
		PlayerPrefs.SetInt("Reporter_showClearOnNewSceneLoadedButton", showClearOnNewSceneLoadedButton ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showTimeButton", showTimeButton ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showSceneButton", showSceneButton ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showMemButton", showMemButton ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showFpsButton", showFpsButton ? 1 : 0);
		PlayerPrefs.SetInt("Reporter_showSearchText", showSearchText ? 1 : 0);
		PlayerPrefs.Save();
	}

	private IEnumerator readInfo()
	{
		string prefFile = "build_info.txt";
		string url = prefFile;
		if (prefFile.IndexOf("://") == -1)
		{
			string text = Application.streamingAssetsPath;
			if (text == string.Empty)
			{
				text = Application.dataPath + "/StreamingAssets/";
			}
			url = Path.Combine(text, prefFile);
		}
		WWW www = new WWW(url);
		yield return www;
		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.LogError(www.error);
		}
		else
		{
			buildDate = www.text;
		}
	}
}
