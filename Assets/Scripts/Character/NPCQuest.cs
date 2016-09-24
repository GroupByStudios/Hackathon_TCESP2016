using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class NPCQuest : MonoBehaviour {

		public UIObject[] UIObjects;
		private UIObject uiObjectsTree;
		GameObject uiManagerGO;
		UIManager uiManager;

		// Use this for initialization
		void Start () {


		}
		
		// Update is called once per frame
		void Update () {


		}

		public void ActivateQuest()
		{
			uiManagerGO = GameObject.FindGameObjectWithTag("UIManager");
			if (uiManagerGO != null)
			{
				uiManager = uiManagerGO.GetComponent<UIManager>();
			}

			if (UIObjects != null && UIObjects.Length > 0 && uiManager != null)
			{
				// Cria a lista encadeada
				uiObjectsTree = UIObjects[0];
				uiObjectsTree.Speaker = transform;
				UIObject uiObjectPtr = uiObjectsTree;
				for (int i = 1; i < UIObjects.Length; i++)
				{				
					uiObjectPtr.NextUIObject = UIObjects[i];
					uiObjectPtr = uiObjectPtr.NextUIObject;
				}

				if (!uiObjectsTree.IsShowing)
				{
					uiManager.UIObjectList.Add(uiObjectsTree);
				}
			}
		}
	}

}
