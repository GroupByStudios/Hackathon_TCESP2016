﻿using UnityEngine;
using System.Collections;


namespace Hackatoon_TCE
{

	public enum EnemyState
	{
		IDLE,
		MOVING,
		GOING_COLLECT,
		COLLECTING,
		GOING_DELIVERY,
		DELIVERING,
		ENJOYING,
		DYING,
		DEAD
	}

	[RequireComponent(typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour {

		public EnemyState State;

		public int AvatarIndex;
		public GameObject[] AvatarList;
		public Transform SpawnPoint;

		public GameObject DeliveryIcon;
		public GameObject RunIcon;
		public GameObject SettingsIcon;
		public GameObject EnjoyingIcon;

		public Coin CoinPrefabs;

		public bool AnimationStatic = false;
		public Animator AnimatorController;

		public float Speed = 5f;
		public float AnimationSpeedMultiplier = 1;


		const string ANIM_SPEED = "Speed_f";
		const string ANIM_SPEED_MULTIPLIER = "SpeedMultiplier_f";
		const string ANIM_JUMP = "Jump_b";
		const string ANIM_STATIC = "Static_b";

		void Awke()
		{
			if (SpawnPoint != null)
			{
				transform.position = SpawnPoint.position;
				transform.rotation = SpawnPoint.rotation;
			}

			IdleCooldown = Random.Range(1, 5);

		}


		// Use this for initialization
		void Start () {
		
			if (AvatarList != null)
			{				
				AvatarIndex = Random.Range(0, AvatarList.Length);

				// Palhaco esta bugado
				while(AvatarIndex == 5 || AvatarIndex == 10)
				{
					AvatarIndex = Random.Range(0, AvatarList.Length);
				}
				
				AvatarList[AvatarIndex].SetActive(true);
			}

			navmeshAgent = GetComponent<NavMeshAgent>();
			collectPoints = GameObject.FindObjectsOfType<CollectPoint>();
			deliveryPoints = GameObject.FindObjectsOfType<DeliveryPoint>();
			player = GameObject.FindObjectOfType<Player>();

			ClearIcons();
		}


		NavMeshAgent navmeshAgent;

		CollectPoint[] collectPoints;
		CollectPoint collectPointTarget;

		DeliveryPoint[] deliveryPoints;
		DeliveryPoint deliveryPointTarget;

		Player player;

		bool isRunning = false;

		public float RunningMinDistance = 6f;
		public int MovingChance = 70;
		public int CollectingChance = 40;
		public int IdleCooldown = 0;
		public float CollectingCooldown = 2f;
		public float DeliveryCoolDown = 3f;
		public float RunningCooldown = 4f;
		public float DeadCooldown = 3f;

		float IdleCurrentCooldown = 0f;
		float CollectingCurrentCooldown = 0f;
		float currentDeliveryCooldown = 0f;
		float currentRunningCooldown = 0f;
		float currentDeadCooldown = 0f;

		// Update is called once per frame
		void Update () {
		

			int movingDice = Random.Range(0, 101);
			int collectingDice = Random.Range(0, 101);


			switch(State)
			{

			case EnemyState.IDLE:

				IdleCurrentCooldown += Time.deltaTime;

				if (IdleCurrentCooldown >= IdleCooldown)
				{
					
					/*if (movingDice >= MovingChance)
					{
						
					}*/

					if (collectingDice >= CollectingChance)
					{
						collectPointTarget = collectPoints[Random.Range(0, collectPoints.Length)];
						Vector2 circleRandom = Random.insideUnitCircle * collectPointTarget.Range;

						ClearIcons();
						State = EnemyState.GOING_COLLECT;
						navmeshAgent.SetDestination(collectPointTarget.transform.position + new Vector3(circleRandom.x, collectPointTarget.transform.position.y, circleRandom.y));
					}

					IdleCurrentCooldown = 0f;
				}

				// Chances de se Mover, define o ponto de movimento e troca o estado para moving
				// Chances de iniciar Coleta, define o ponto de coleta e troca o estado para going_collect

				break;
			case EnemyState.MOVING:

				// Se Move, ao chegar no destino troca para o idle

				break;
			case EnemyState.GOING_COLLECT:

				RunFromPlayer();

				// Quando chegar no ponto de coleta, troca o estado para collecting
				if (navmeshAgent.enabled && navmeshAgent.remainingDistance < 1)
				{
					ClearIcons();
					State =  EnemyState.COLLECTING;
					navmeshAgent.ResetPath();
				}

				break;
			case EnemyState.COLLECTING:

				if (SettingsIcon != null && !SettingsIcon.activeInHierarchy)
					SettingsIcon.SetActive(true);

				RunFromPlayer();

				// Animacao de coleta
				// Depois de um tempo escolhe um ponto de entrega, troca o estado para Going_Delivery
				CollectingCurrentCooldown += Time.deltaTime;

				if (CollectingCurrentCooldown >= CollectingCooldown)
				{
					// Recupera os pontos de Entrega
					deliveryPointTarget = deliveryPoints[Random.Range(0,  deliveryPoints.Length)];
					State = EnemyState.GOING_DELIVERY;
					navmeshAgent.SetDestination(deliveryPointTarget.transform.position);

					ClearIcons();
					CollectingCurrentCooldown = 0f;
				}



				break;
			case EnemyState.GOING_DELIVERY:

				if (DeliveryIcon != null && !DeliveryIcon.activeInHierarchy)
					DeliveryIcon.SetActive(true);

				RunFromPlayer();

				// SE move ate o ponto de entrega selecionado, Ao chegar troca o estado para Delivery
				if (navmeshAgent.enabled && navmeshAgent.remainingDistance < 1)
				{
					ClearIcons();
					State = EnemyState.DELIVERING;
					navmeshAgent.ResetPath();

				}

				break;
			case EnemyState.DELIVERING:

				// Animacao de entrega
				// Depois de um tempo faz a entrega e troca o estado para Enjoying
				RunFromPlayer();


				currentDeliveryCooldown += Time.deltaTime;
				if (currentDeliveryCooldown >= DeliveryCoolDown)
				{
					ClearIcons();
					State = EnemyState.IDLE;
				}

				break;
			case EnemyState.ENJOYING:

				// Curte o sorvete
				if (EnjoyingIcon != null && !EnjoyingIcon.activeInHierarchy)
					EnjoyingIcon.SetActive(true);

				break;
			case EnemyState.DYING:

				ClearIcons();

				if (RunIcon != null && RunIcon.activeInHierarchy)
					RunIcon.SetActive(false);

				player.target = null;
				navmeshAgent.Stop();
				navmeshAgent.velocity = Vector3.zero;

				// Spawn prefabs
				if (CoinPrefabs != null)
				{
					int coinsQuantity = Random.Range(1, 6);

					for (int i = 0; i < coinsQuantity; i++)
					{
						Coin coinPrefab = Instantiate(CoinPrefabs) as Coin;
						coinPrefab.transform.position = transform.position;

						switch(i)
						{
						case 0:
							coinPrefab.transform.LookAt( transform.position + transform.forward);	
							break;
						case 1:
							coinPrefab.transform.LookAt(transform.position + transform.right);	
							break;
						case 2:
							coinPrefab.transform.LookAt(transform.position + transform.forward * -1);	
							break;
						case  3:
							coinPrefab.transform.LookAt(transform.position + transform.right * -1);	
							break;
						}
					}
				}

				State = EnemyState.DEAD;

				break;
			case EnemyState.DEAD:

				if (navmeshAgent.enabled)
				{
					navmeshAgent.enabled = false;
					//transform.Rotate(0, 0, 90);

					currentDeadCooldown += Time.deltaTime;

					if (currentDeadCooldown > DeadCooldown)
					{
						transform.position = transform.position + Vector3.down * Time.deltaTime;

						if (transform.position.y < -3)
							Destroy(gameObject);						
					}
				}

				break;
			}
		}

		void RunFromPlayer()
		{
			if (player != null)
			{
				Vector3 distance = player.transform.position - transform.position;

				if (distance.magnitude < RunningMinDistance)
				{
					if (distance.magnitude < 2)
					{
						if (player.target == null)
							player.target = this;
					}
					else
					{
						if (player.target == this)
							player.target = null;
					}

					navmeshAgent.transform.LookAt(navmeshAgent.transform.position + distance * -1);

					if (!isRunning)
					{
						navmeshAgent.Stop();
						navmeshAgent.velocity = Vector3.zero;
						isRunning = true;
					}
				}
				else
				{
					currentRunningCooldown += Time.deltaTime;

					if (currentRunningCooldown >= RunningCooldown)
					{
						if (RunIcon != null && RunIcon.activeInHierarchy)
							RunIcon.SetActive(false);
						
						isRunning = false;
						navmeshAgent.Resume();
						currentRunningCooldown = 0;
					}
				}

				if (isRunning)
				{
					ClearIcons();

					if (RunIcon != null && !RunIcon.activeInHierarchy)
						RunIcon.SetActive(true);

					navmeshAgent.Move(navmeshAgent.transform.forward * navmeshAgent.speed * Time.deltaTime);
				}
			}
		}

		void ClearIcons()
		{
			if (DeliveryIcon != null)
				DeliveryIcon.SetActive(false);

			if(SettingsIcon != null)
				SettingsIcon.SetActive(false);

			if (EnjoyingIcon != null)
				SettingsIcon.SetActive(false);
			
		}


	}

}
