using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    NavMeshAgent enemyAgent;

    [SerializeField, Range(0, 10)]
    public float visionRadius;

    private float distancia;

    public bool isWatchingPlayer = false;

    public Transform target;
    public Vector2 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        target = GameObject.Find("Jugador2DVC").transform;

        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector2.Distance(target.position, transform.position);

        if (distancia < visionRadius)
        {
            isWatchingPlayer = true;
        }
        else
        {
            isWatchingPlayer = false;
        }
    }

    private void FixedUpdate()
    {
        if (isWatchingPlayer)
        {
            enemyAgent.SetDestination(target.position);
        }
        else
        {
            enemyAgent.SetDestination(initialPosition);
        }
    }
}
