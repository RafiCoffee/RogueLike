using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public float attackCooldown = 1;

    private GameObject attackCollider;

    private EnemyBehaviour enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        attackCollider = GameObject.Find("AttackCollider");
        enemyScript = GetComponent<EnemyBehaviour>();

        enemyScript.isBoss = true;

        attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyScript.bossAnim.Stop();
            enemyScript.bossAnim.Play("Attack");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        enemyScript.invencible = true;
        enemyScript.isMoving = false;
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackCollider.SetActive(false);
        enemyScript.invencible = false;
        enemyScript.isMoving = true;
    }
}
