using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class MonoBehaviorEvent : MonoBehaviour
	{
		[Header("Activation")]
		[SerializeField]
		private OrderedEventDispatcher onAwake = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onEnable = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onStart = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onDisable = new OrderedEventDispatcher();

		[Header("Touch input")]
		[SerializeField]
		private OrderedEventDispatcher onMouseDown = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onMouseUp = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onMouseUpAsButton = new OrderedEventDispatcher();

		[Header("Rendering")]
		[SerializeField]
		private OrderedEventDispatcher onVisible = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onInvisible = new OrderedEventDispatcher();

		[Header("Others")]
		[SerializeField]
		private OrderedEventDispatcher onSceneLoaded = new OrderedEventDispatcher();

		[Header("Customized")]
		[SerializeField]
		private OrderedEventDispatcher onOneTimeFixedUpdate = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onOneTimeUpdate = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onOneTimeLateUpdate = new OrderedEventDispatcher();

		private bool isOneTimeFixedUpdateCalled;

		private bool isOneTimeUpdateCalled;

		private bool isOneTimeLateUpdateCalled;

		public void Awake()
		{
			onAwake.Dispatch();
		}

		public void OnEnable()
		{
			onEnable.Dispatch();
		}

		public void Start()
		{
			onStart.Dispatch();
		}

		public void OnDisable()
		{
			onDisable.Dispatch();
		}

		public void OnMouseDown()
		{
			onMouseDown.Dispatch();
		}

		public void OnMouseUp()
		{
			onMouseUp.Dispatch();
		}

		public void OnMouseUpAsButton()
		{
			onMouseUpAsButton.Dispatch();
		}

		public void OnBecameInvisible()
		{
			onInvisible.Dispatch();
		}

		public void OnBecameVisible()
		{
			onVisible.Dispatch();
		}

		public void OnLevelWasLoaded(int level)
		{
			onSceneLoaded.Dispatch();
		}

		public void LateUpdate()
		{
			if (!isOneTimeLateUpdateCalled)
			{
				isOneTimeLateUpdateCalled = true;
				onOneTimeLateUpdate.Dispatch();
			}
		}

		public void Update()
		{
			if (!isOneTimeUpdateCalled)
			{
				isOneTimeUpdateCalled = true;
				onOneTimeUpdate.Dispatch();
			}
		}

		public void FixedUpdate()
		{
			if (!isOneTimeFixedUpdateCalled)
			{
				isOneTimeFixedUpdateCalled = true;
				onOneTimeFixedUpdate.Dispatch();
			}
		}
	}
}
