using UnityEngine;
using UnityEngine.EventSystems;

namespace Funzilla
{
	internal class Joystick : MonoBehaviour
	{
		private float Horizontal => snapX ? SnapFloat(_input.x, AxisOptions.Horizontal) : _input.x;

		private float Vertical => snapY ? SnapFloat(_input.y, AxisOptions.Vertical) : _input.y;

		internal Vector2 Direction => new Vector2(Horizontal, Vertical);

		private float HandleRange
		{
			set => handleRange = Mathf.Abs(value);
		}

		private float DeadZone
		{
			set => deadZone = Mathf.Abs(value);
		}

		[SerializeField] private float handleRange = 1;
		[SerializeField] private float deadZone;
		[SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
		[SerializeField] private bool snapX;
		[SerializeField] private bool snapY;

		[SerializeField] protected RectTransform background;
		[SerializeField] private RectTransform handle;
		private RectTransform _baseRect;

		private Canvas _canvas;
		private Camera _cam;

		private Vector2 _input = Vector2.zero;

		protected virtual void Start()
		{
			HandleRange = handleRange;
			DeadZone = deadZone;
			_baseRect = GetComponent<RectTransform>();
			_canvas = GetComponentInParent<Canvas>();
			if (_canvas == null)
				Debug.LogError("The Joystick is not placed inside a canvas");

			var center = new Vector2(0.5f, 0.5f);
			background.pivot = center;
			handle.anchorMin = center;
			handle.anchorMax = center;
			handle.pivot = center;
			handle.anchoredPosition = Vector2.zero;
		}

		internal virtual void OnPointerDown(Vector2 touchPosition)
		{
			OnDrag(touchPosition);
		}

		private void OnDrag(Vector2 touchPosition)
		{
			_cam = null;
			if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
				_cam = _canvas.worldCamera;

			var position = RectTransformUtility.WorldToScreenPoint(_cam, background.position);
			var radius = background.sizeDelta / 2;
			_input = (touchPosition - position) / (radius * _canvas.scaleFactor);
			FormatInput();
			HandleInput(_input.magnitude, _input.normalized, radius);
			handle.anchoredPosition = _input * radius * handleRange;
		}

		protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius)
		{
			if (magnitude > deadZone)
			{
				if (magnitude > 1)
					_input = normalised;
			}
			else
				_input = Vector2.zero;
		}

		private void FormatInput()
		{
			_input = axisOptions switch
			{
				AxisOptions.Horizontal => new Vector2(_input.x, 0f),
				AxisOptions.Vertical => new Vector2(0f, _input.y),
				_ => _input
			};
		}

		private float SnapFloat(float value, AxisOptions snapAxis)
		{
			if (value == 0)
				return value;

			if (axisOptions == AxisOptions.Both)
			{
				var angle = Vector2.Angle(_input, Vector2.up);
				return snapAxis switch
				{
					AxisOptions.Horizontal when angle < 22.5f || angle > 157.5f => 0,
					AxisOptions.Horizontal => (value > 0) ? 1 : -1,
					AxisOptions.Vertical when angle > 67.5f && angle < 112.5f => 0,
					AxisOptions.Vertical => (value > 0) ? 1 : -1,
					_ => value
				};
			}
			else
			{
				if (value > 0)
					return 1;
				if (value < 0)
					return -1;
			}

			return 0;
		}

		internal virtual void OnPointerUp()
		{
			_input = Vector2.zero;
			handle.anchoredPosition = Vector2.zero;
		}

		protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
		{
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition,
				_cam, out var localPoint)) return Vector2.zero;
			var sizeDelta = _baseRect.sizeDelta;
			var pivotOffset = _baseRect.pivot * sizeDelta;
			return localPoint - background.anchorMax * sizeDelta + pivotOffset;

		}

		private bool _touched;

		public Joystick()
		{
			snapY = false;
		}

#if !UNITY_EDITOR
		private void Update()
		{
			if (Input.touches.Length <= 0) return;
			switch (Input.touches[0].phase)
			{
				case TouchPhase.Began:
					if (!EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
					{
						_touched = true;
						OnPointerDown(Input.touches[0].position);
					}
					break;
				case TouchPhase.Moved:
					break;
				case TouchPhase.Stationary:
					if (_touched)
					{
						OnDrag(Input.touches[0].position);
					}
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					if (_touched)
					{
						ReleaseTouch();
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
#else
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				_touched = true;
				OnPointerDown(Input.mousePosition);
			}

			if (Input.GetMouseButtonUp(0))
			{
				ReleaseTouch();
			}

			if (_touched)
			{
				OnDrag(Input.mousePosition);
			}
		}
#endif

		private void ReleaseTouch()
		{
			_touched = false;
			OnPointerUp();
		}
	}

	internal enum AxisOptions
	{
		Both,
		Horizontal,
		Vertical
	}
}