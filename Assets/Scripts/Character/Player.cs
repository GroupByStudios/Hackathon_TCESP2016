using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class Player : MonoBehaviour {

		public GameObject SpawnPoint;

		public int AvatarIndex;
		public GameObject[] AvatarList;

		public bool AnimationStatic = false;
		public Animator AnimatorController;

		public float AnimationSpeedMultiplier = 1;

		[HideInInspector]
		public Enemy target;

		const string ANIM_SPEED = "Speed_f";
		const string ANIM_SPEED_MULTIPLIER = "SpeedMultiplier_f";
		const string ANIM_JUMP = "Jump_b";
		const string ANIM_STATIC = "Static_b";

		void Awake()
		{
			if (SpawnPoint != null)
			{
				transform.position = SpawnPoint.transform.position;
				transform.rotation = SpawnPoint.transform.rotation;
			}
		}

		// Use this for initialization
		void Start () {
		
			if (AvatarList != null && AvatarIndex > -1)
			{
				AvatarList[AvatarIndex].SetActive(true);
			}
		}
		
		// Update is called once per frame
		void Update () {

			float horizontalInput = Input.GetAxisRaw("Horizontal");
			float verticalInput = Input.GetAxisRaw("Vertical");

			transform.LookAt(transform.position + new Vector3(horizontalInput, 0, verticalInput));
			if (AnimatorController != null)
			{
				AnimatorController.SetFloat(ANIM_SPEED_MULTIPLIER, AnimationSpeedMultiplier);
				AnimatorController.SetBool(ANIM_STATIC, AnimationStatic);
				AnimatorController.SetFloat(ANIM_SPEED, Mathf.Abs(horizontalInput) +  Mathf.Abs(verticalInput));
			}


			if (Input.GetKeyDown(KeyCode.E))
			{
				if (target != null)
				{
					target.State = EnemyState.DYING;
				}

				GameObject[] npcQuestGO = GameObject.FindGameObjectsWithTag("NPCQuest");

				if (npcQuestGO != null)
				{
					int index = 0;
					float distance = float.MaxValue;
					for (int i = 0; i < npcQuestGO.Length; i++)
					{
						float _distance = Mathf.Abs(Vector3.Distance(npcQuestGO[i].transform.position, transform.position));

						if (_distance < distance)
						{
							index = i;
							distance = _distance;
						}
					}

					NPCQuest npcQuestComp = npcQuestGO[index].GetComponent<NPCQuest>();
					npcQuestComp.ActivateQuest();

				}
			}
		}
	}

}
