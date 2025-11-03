using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.NPG.Codebase.Utils
{
	public class FpsCount : MonoBehaviour
	{
		[SerializeField] TMP_Text TMP_Text;

		private void Update()
		{
			TMP_Text.text = Mathf.Ceil(GetFps()).ToString();
			Debug.Log(Mathf.Ceil(GetFps()).ToString());
		}

		private float GetFps()
		{
			return 1f / Time.unscaledDeltaTime;
		}
	}
}
