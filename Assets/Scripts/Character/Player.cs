using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

    public class Player : MonoBehaviour
    {

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

            if (AvatarList != null && AvatarIndex > -1)
            {
                AvatarList[AvatarIndex].SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

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
                                    enemyKeyValye.Value.State != EnemyState.DEAD)
                                {
                                    enemyKeyValye.Value.Reported = true;
                                    enemyKeyValye.Value.State = EnemyState.DYING;
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

    }

}
