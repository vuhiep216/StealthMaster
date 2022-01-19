using UnityEngine;

namespace Funzilla
{
	internal class FloatingJoystick : Joystick
	{
		protected override void Start()
		{
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
	}
}