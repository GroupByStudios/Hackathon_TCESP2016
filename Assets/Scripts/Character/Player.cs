using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hackatoon_TCE
{

    public class Player : MonoBehaviour
    {
        public float defaultY;


        public float MaxConeEnergy = 100f;
        public float ConeEnergy = 100f;
        public float ConeEnergyRecoveryRate = 15f;
        public float ConeConsumeEnergy = 20f;

        public Cone ConeGameObject;
        public Renderer[] ConeMaterial;
        public Renderer[] ConeBorderMaterial;

        public float InsideConeTime = 2f;
        private float currentInsideConeTime = 0f;

        public float ConeShrinkVelocity;
        public float ConeGrowVelocity;
        private float currentConeGrow;
        public float MaxConeGrow;

        public Gradient ConeGradientColor;
        public Gradient ConeBorderColor;

        public GameObject SpawnPoint;

        public int AvatarIndex;
        public GameObject[] AvatarList;

        public bool AnimationStatic = false;
        public Animator AnimatorController;

        public float AnimationSpeedMultiplier = 1;

        public Text HUD_Star_Text;
        public Slider HUD_Energia_Slider;
        private float coinAmmount = 0f;

        private Rigidbody currentRigidBody;

		private AudioSource starAudioSource;
		public AudioClip StarGiveCoinAudioClip;
		public GameObject StarParticle;

		private AudioSource quedaAudioSource;
		public AudioClip QuedaAudioClip;

		public GameManager gameManager;
		public SpawnerManager spawnerManager;
		public float CompassDegreeSpeed = 15f;
		public GameObject Compass;
		public bool CompassEnemy = false;
		public float CompassMinDistance = 99999f;

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
        void Start()
        {
            
            if (AvatarList != null)
            {
                AvatarList[GameManager.StaticSelectedAvatar].SetActive(true);
                AnimatorController = GetComponent<Animator>();
            }

            defaultY = transform.position.y;

            currentRigidBody = GetComponent<Rigidbody>();

			starAudioSource = gameObject.AddComponent<AudioSource>();
			starAudioSource.playOnAwake = false;
			starAudioSource.clip = StarGiveCoinAudioClip;

			quedaAudioSource = gameObject.AddComponent<AudioSource>();
			quedaAudioSource.playOnAwake = false;
			quedaAudioSource.clip = QuedaAudioClip;
        }

        // Update is called once per frame
        void Update()
        {
			CompassEnemy = false;
			CompassMinDistance = 99999f;

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            transform.LookAt(transform.position + new Vector3(horizontalInput, 0, verticalInput));
            if (AnimatorController != null)
            {
                AnimatorController.SetFloat(ANIM_SPEED_MULTIPLIER, AnimationSpeedMultiplier);
                AnimatorController.SetBool(ANIM_STATIC, AnimationStatic);
                AnimatorController.SetFloat(ANIM_SPEED, Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            }


            if (Input.GetKeyDown(KeyCode.E))
            {


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

            if (ConeGameObject != null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    ConeEnergy -= ConeConsumeEnergy * Time.deltaTime;
                    if (ConeEnergy < 0)
                        ConeEnergy = 0;

                    // Verifica se pode aumentar e tem energia
                    if (currentConeGrow <= MaxConeGrow && ConeEnergy > 0)
                    {
                        ConeGameObject.transform.localScale += Vector3.one * ConeGrowVelocity * Time.deltaTime;
                        currentConeGrow += ConeGrowVelocity * Time.deltaTime;
                    }

                    // Se nao tiver amis energia diminui o cone
                    if (ConeEnergy <= 0)
                    {
                        ConeGameObject.transform.localScale = Vector3.Lerp(ConeGameObject.transform.localScale, Vector3.zero, ConeShrinkVelocity);
                    }

                    if (ConeGameObject != null && ConeGameObject.Reporting)
                    {
                        currentInsideConeTime += Time.deltaTime;
                        if (currentInsideConeTime > InsideConeTime)
                            currentInsideConeTime = InsideConeTime;

                        float currentConeTime = currentInsideConeTime / InsideConeTime;

                        UpdateConeColor(currentConeTime);

                        if (currentConeTime >= 1)
                        {
                            // Marca os inimigos no Cone como reportados
                            foreach (var enemyKeyValye in ConeGameObject.EnemiesDictionary)
                            {
                                if (enemyKeyValye.Value.State != EnemyState.DYING &&
									enemyKeyValye.Value.State != EnemyState.DEAD && 
									enemyKeyValye.Value.IsCorrupt)
                                {
									gameManager.SpawnOfficer(enemyKeyValye.Value.transform.position);
                                    enemyKeyValye.Value.Reported = true;
                                    enemyKeyValye.Value.State = EnemyState.DYING;

									// Faz nascer a particula no inimigo e toca o auido de quedra
									quedaAudioSource.Play();
									GameObject particle =  Instantiate(StarParticle);
									particle.transform.position = new Vector3(enemyKeyValye.Value.transform.position.x, enemyKeyValye.Value.transform.position.y + 0.5f, enemyKeyValye.Value.transform.position.z);
                                }
                            }
                        }
                    }
                    else
                    {
                        currentInsideConeTime -= Time.deltaTime;

                        if (currentInsideConeTime < 0)
                            currentInsideConeTime = 0f;

                        float currentConeTime = currentInsideConeTime / InsideConeTime;

                        UpdateConeColor(currentConeTime);
                    }

                }
                else
                {
                    ConeEnergy += ConeEnergyRecoveryRate * Time.deltaTime;

                    if (ConeEnergy > MaxConeEnergy)
                        ConeEnergy = MaxConeEnergy;

                    currentConeGrow = 0;
                    currentInsideConeTime = 0;
                    ConeGameObject.transform.localScale = Vector3.zero;
                    UpdateConeColor(0);
                }
            }

            AtualizarHUD();

            if (transform.position.y > 1)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                currentRigidBody.velocity = Vector3.zero;
            }

			Vector3 compassTarget = Vector3.zero;
			float compassDistance = 99999f;
			Enemy _enemy;
			// Calcula a posicao da Seta
			for(int i = 0; i < spawnerManager.Enemies.Length; i++)
			{
				_enemy = spawnerManager.Enemies[i];

				if (_enemy != null && _enemy.IsCorrupt)
				{
					if (compassDistance >= (_enemy.transform.position - transform.position).magnitude)
					{
						compassDistance = (_enemy.transform.position - transform.position).magnitude;
						compassTarget = _enemy.transform.position;
						CompassEnemy = true;
					}
				}
			}

			if (CompassEnemy)
			{
				Compass.SetActive(true);
				Compass.transform.LookAt(new Vector3(compassTarget.x, Compass.transform.position.y, compassTarget.z));
			}
			else
			{
				Compass.SetActive(false);
			}
        }

        private void UpdateConeColor(float colorTime)
        {
            if (ConeMaterial != null)
            {
                for (int i = 0; i < ConeMaterial.Length; i++)
                {
                    ConeMaterial[i].material.color = ConeGradientColor.Evaluate(colorTime);
                }
            }

            if (ConeBorderMaterial != null)
            {
                for (int i = 0; i < ConeBorderMaterial.Length; i++)
                {
                    ConeBorderMaterial[i].material.color = ConeBorderColor.Evaluate(colorTime);
                }
            }
        }

        public void GiveCoin()
        {
			gameManager.AumtentarCidadania();
            this.coinAmmount++;
			starAudioSource.Play();
        }

        public void RemoveCoin(float quantidade)
        {
            this.coinAmmount -= quantidade;

            if (coinAmmount < 0)
                coinAmmount = 0;

        }

        public void AtualizarHUD()
        {
            if (HUD_Star_Text != null)
            {
                HUD_Star_Text.text = coinAmmount.ToString();
            }

            if (HUD_Energia_Slider != null)
            {
                HUD_Energia_Slider.maxValue = MaxConeEnergy;
                HUD_Energia_Slider.value = ConeEnergy;
            }

        }

    }

}
