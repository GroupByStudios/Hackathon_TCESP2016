using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{


    public class Officer : MonoBehaviour
    {
        public string[] Frases =
       {
        "ao encontrar um lápis, devolva ao professor.",
        "nunca pegue o que não é seu.",
        "respeitar, professores e pessoas mais velhas.",
        "respeite seus pais.",
        "ao pegar um brinquedo, devolva para seu amigo.",
        "saber perder, se divertir acima de tudo!",
        "o bem sempre vence o mal.",
        "seja solidário, ajude o próximo!",
        "sempre atravesse na faixa de pedestres.",
        "fale sempre a verdade.",
        "você é o futuro do Brasil!",
        "todos são iguais e merecem respeito.",
        "corrupção, diga não!",
        "não cole estude",
        "não jogue lixo no chão"
    };

        private NPCQuest _npcQuest = null;

        public OfficerState State;
        public Vector3? Target;
        public float MinDistance;

        public Animator animatorController;
        public NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            State = OfficerState.DontHaveTarget;


            // Adiciona uma frase ao policial
            _npcQuest = gameObject.AddComponent<NPCQuest>();
            _npcQuest.UIObjects = new UIObject[1];
            UIObject _uiTextObject = new UIObject();
            _uiTextObject.Time = 5f;
            _uiTextObject.Speaker = this.transform;
            _uiTextObject.Text = "Obrigado por ajudar! Nunca se esqueça, " + Frases[Random.Range(0, Frases.Length)];
            _npcQuest.UIObjects[0] = _uiTextObject;
        }

        void Update()
        {
            if (animatorController != null)
                animatorController.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);

            switch (State)
            {
                case OfficerState.DontHaveTarget:

                    if (Target.HasValue)
                        State = OfficerState.HasTarget;

                    break;

                case OfficerState.HasTarget:


                    State = OfficerState.Moving;
                    navMeshAgent.SetDestination(Target.Value);

                    break;
                case OfficerState.Moving:

                    Vector3 distanceFromTarget = Target.Value - transform.position;

                    if (distanceFromTarget.magnitude <= MinDistance)
                    {
                        State = OfficerState.FinishMove;
                        navMeshAgent.Stop();
                        navMeshAgent.velocity = Vector3.zero;
                    }

                    break;
                case OfficerState.FinishMove:

                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    _npcQuest.ActivateQuest();

                    State = OfficerState.Finished;
                    
                    break;
                case OfficerState.Finished:
                    

                    break;
            }

        }

    }

    public enum OfficerState
    {
        DontHaveTarget,
        HasTarget,
        Moving,
        FinishMove,
        Finished
    }

}

