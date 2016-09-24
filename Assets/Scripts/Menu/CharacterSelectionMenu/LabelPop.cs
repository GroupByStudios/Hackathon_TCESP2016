using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hackatoon_TCE
{

	public class LabelPop : MonoBehaviour {

		public float FontPopVelocity = 1;
		public float FontPopPercent = 0.1f;
		public Text FontText;

		private float DefaultFontSize;
		private float MaxFontSize;
		private float MinFontSize;
		private bool Growing = true;
		private float BufferFontSize;

		// Use this for initialization
		void Start () {
		
			if (FontText != null)
			{
				DefaultFontSize = FontText.fontSize;
				BufferFontSize = DefaultFontSize;
			}

		}
		
		// Update is called once per frame
		void Update () {
		
			if (FontText != null)
			{
				MinFontSize = DefaultFontSize - ( DefaultFontSize * FontPopPercent );
				MaxFontSize = DefaultFontSize + ( DefaultFontSize * FontPopPercent );

				if (Growing)
				{
					if (FontText.fontSize < MaxFontSize)
					{
						BufferFontSize += FontPopVelocity * Time.deltaTime;
					}
					else
					{
						Growing = false;
					}
				}
				else
				{
					if (FontText.fontSize > MinFontSize)
					{
						BufferFontSize += -1 * FontPopVelocity * Time.deltaTime;
					}
					else
					{
						Growing = true;
					}
				}

				FontText.fontSize = (int)BufferFontSize;
			}
		}
	}

}
