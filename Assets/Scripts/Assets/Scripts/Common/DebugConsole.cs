using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class DebugConsole : MonoBehaviour
	{
		public Rect ButtonRect = new Rect(0f, 0f, 200f, 80f);

		public Rect PosRect = new Rect(0f, 80f, 1000f, 1000f);

		public Rect ViewRect = new Rect(0f, 0f, 1000f, 1000f);

		public bool Show;

		public bool StackTrace;

		private bool _expand;

		private Vector2 _scrollPos;

		private int _count;

		private readonly StringBuilder _log = new StringBuilder();

		private readonly StringBuilder _fullLog = new StringBuilder();

		private bool _subscribed;

		public void Awake()
		{
			Subscribe();
		}

		public void Subscribe()
		{
			if (!_subscribed)
			{
				Application.logMessageReceived += HandleLog;
				_subscribed = true;
			}
		}

		public void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.BackQuote))
			{
				_expand = !_expand;
			}
		}

		public void HandleLog(string message, string stackTrace, LogType type)
		{
			_log.AppendLine(message);
			if (StackTrace)
			{
				_log.AppendLine(stackTrace);
			}
			_fullLog.AppendLine(message);
			_fullLog.AppendLine(stackTrace);
		}

		public void OnGUI()
		{
			if (Show)
			{
				if (GUI.Button(ButtonRect, "Console"))
				{
					_expand = !_expand;
				}
				if (_expand)
				{
					_scrollPos = GUI.BeginScrollView(PosRect, _scrollPos, ViewRect);
					GUI.TextArea(new Rect(0f, 0f, ViewRect.width - 50f, ViewRect.height), _log.ToString());
					GUI.EndScrollView();
				}
			}
		}

		public string GetLogs()
		{
			return _fullLog.ToString();
		}
	}
}
