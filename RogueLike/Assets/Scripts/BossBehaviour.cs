using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class BossBehaviour : MonoBehaviour
{
    public float attackCooldown = 1;

    private GameObject attackCollider;

    private Stopwatch timerAttack = new Stopwatch();

    private EnemyBehaviour enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        attackCollider = GameObject.Find("AttackCollider");
        enemyScript = GetComponent<EnemyBehaviour>();

        enemyScript.isBoss = true;

        timerAttack.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timerAttack.ElapsedMilliseconds / 1000 < attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        enemyScript.invencible = true;
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackCollider.SetActive(false);
        enemyScript.invencible = false;
        timerAttack.Restart();
    }
}
