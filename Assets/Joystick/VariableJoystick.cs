using UnityEngine;

namespace Funzilla
{
	internal class VariableJoystick : Joystick
	{
		[SerializeField] private float moveThreshold = 1;
		[SerializeField] private JoystickType joystickType = JoystickType.Fixed;

		private Vector2 _fixedPosition = Vector2.zero;

		private void SetMode(JoystickType type)
		{
			joystickType = type;
			if (type == JoystickType.Fixed)
			{
				background.anchoredPosition = _fixedPosition;
				background.gameObject.SetActive(true);
			}
			else
				background.gameObject.SetActive(false);
		}

		protected override void Start()
		{
			base.Start();
			_fixedPosition = background.anchoredPosition;
			SetMode(joystickType);
		}

		internal override void OnPointerDown(Vector2 touchPosition)
		{
			if (joystickType != JoystickType.Fixed)
			{
				background.anchoredPosition = ScreenPointToAnchoredPosition(touchPosition);
				background.gameObject.SetActive(true);
			}

			base.OnPointerDown(touchPosition);
		}

		internal override void OnPointerUp()
		{
			if (joystickType != JoystickType.Fixed)
				background.gameObject.SetActive(false);

			base.OnPointerUp();
		}

		protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius)
		{
			if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
			{
				var difference = normalised * (magnitude - moveThreshold) * radius;
				background.anchoredPosition += difference;
			}

			base.HandleInput(magnitude, normalised, radius);
		}
	}

	internal enum JoystickType
	{
		Fixed,
		Floating,
		Dynamic
	}
}