using UnityEngine;

namespace Funzilla
{
	internal class DynamicJoystick : Joystick
	{
		private float MoveThreshold
		{
			set => moveThreshold = Mathf.Abs(value);
		}

		[SerializeField] private float moveThreshold = 1;

		protected override void Start()
		{
			MoveThreshold = moveThreshold;
			base.Start();
			background.gameObject.SetActive(false);
		}

		internal override void OnPointerDown(Vector2 touchPosition)
		{
			background.anchoredPosition = ScreenPointToAnchoredPosition(touchPosition);
			background.gameObject.SetActive(true);
			base.OnPointerDown(touchPosition);
		}

		internal override void OnPointerUp()
		{
			background.gameObject.SetActive(false);
			base.OnPointerUp();
		}

		protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius)
		{
			if (magnitude > moveThreshold)
			{
				var difference = normalised * (magnitude - moveThreshold) * radius;
				background.anchoredPosition += difference;
			}

			base.HandleInput(magnitude, normalised, radius);
		}
	}
}
