using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public Canvas Canvas;
	public Text TextPrefab;
	public List<UIObject> UIObjectList;

	public float UIOffsetY;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		for (int i = 0; i < UIObjectList.Count; i++)
		{
			if (TextPrefab != null)
			{
				string textMessage;
				float textTime = UIObjectList[i].GetText(Time.deltaTime, out textMessage);

				if (textMessage != null)
				{
					if (textTime > 0)					
					{
						Text textComponent = Instantiate(TextPrefab).GetComponent<Text>();
						textComponent.transform.SetParent(Canvas.transform);
						textComponent.text = textMessage;
						Destroy(textComponent.gameObject, textTime);
						UIObjectList[i].SetUIText(textComponent);
					}
				}
			}
		}

		for (int i = 0; i < UIObjectList.Count; i++)
		{
			if (i >= UIObjectList.Count)
			{
				i = UIObjectList.Count -1;
			}

			if (UIObjectList[i].AllStepsWerePlayed())
			{
				UIObjectList[i].Reset();
				UIObjectList.RemoveAt(i);
			}
			else
			{
				Text uiTextComponent = UIObjectList[i].GetTextComponentInstance();

				if (uiTextComponent != null)
				{
					Vector3 textPosition = Camera.main.WorldToScreenPoint(UIObjectList[i].Speaker.position);
					textPosition.z = 0;
					textPosition.y +=  UIOffsetY * Screen.height / 720;
					uiTextComponent.rectTransform.position = textPosition;
				}
			}
		}
	}
}

[Serializable]
public class UIObject
{
	[HideInInspector]
	public Transform Speaker;
	[TextArea]
	public string Text;
	public float Time;
	[HideInInspector]
	public bool IsShowing;

	[HideInInspector]
	public bool MovedToNext;
	public UIObject NextUIObject;
	public Action onStartShowing;
	public Action onFinishShowing;
	public Action onMoveToNext;

	[HideInInspector]
	public Text uiTextPointer;

	private float currentTime;
	private float currentTransitionTime;

	public float GetText(float deltaTime, out string Text)
	{
		UIObject ptr = this;

		while(ptr != null && ptr.MovedToNext)
		{
			ptr = ptr.NextUIObject;
		}

		if (ptr == null)
		{
			Text = null;
			return 0;
		}

		if (!ptr.MovedToNext)
		{
			ptr.currentTime += deltaTime;

			if (ptr.currentTime > ptr.Time)
			{
				ptr.currentTransitionTime += deltaTime;
				if (ptr.currentTransitionTime < 1.1f)
				{
					Text = string.Empty;
					return 0;
				}
				else
				{
					ptr.MovedToNext = true;
					Text = string.Empty;
					return 0;
				}
			}
			else
			{
				if(!ptr.IsShowing)
				{
					ptr.IsShowing = true;
					Text = ptr.Text;
					return ptr.Time;
				}
				else
				{
					Text = string.Empty;
					return 0;					
				}
			}
		}
		else
		{
			Text = string.Empty;
			return 0;
		}
	}

	public bool AllStepsWerePlayed()
	{
		bool ret = true;
		UIObject ptr = this;

		while(ptr != null)
		{
			ret = ret && ptr.MovedToNext;
			ptr = ptr.NextUIObject;

			if (!ret)
				return ret;
		}

		return ret;
	}

	public void SetUIText(Text uiText)
	{
		UIObject ptr = this;

		while(ptr != null && ptr.MovedToNext)
		{
			ptr = ptr.NextUIObject;
		}

		ptr.uiTextPointer = uiText;
	}

	public void Reset()
	{
		UIObject ptr = this;

		while(ptr != null)
		{
			ptr.IsShowing = false;
			ptr.MovedToNext = false;
			ptr.currentTime = 0;
			ptr.currentTransitionTime = 0;
			ptr = ptr.NextUIObject;
		}
	}

	public Text GetTextComponentInstance()
	{
		UIObject ptr = this;

		while(ptr != null)
		{
			if (ptr.uiTextPointer != null)
				return ptr.uiTextPointer;
			else
				ptr = ptr.NextUIObject;
		}

		return null;
	}
}
