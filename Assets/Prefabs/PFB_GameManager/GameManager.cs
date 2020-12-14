using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CombatQueue combatQueue;
    public GameObject player;
    public GameObject enemyPrefab;
    public List<AttackMoves> attackAnimators = new List<AttackMoves>();
    public PlayerController playerController;
    public List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    public int outstandingAttacks = 0;
    private bool timer = false;

    void Awake() { 
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"));
        combatQueue = new CombatQueue();
        attackAnimators.Add(player.GetComponent<AttackMoves>());
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy(CombatBean.Attacks.slice, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }


    public void AddEnemyAttack(CombatBean attack)
    {
        attack.Self.StopTime();
        attackAnimators.Add(attack.Self);
        combatQueue.enqueueEnemyAttack(attack);
        if(timer == false)
        {
            timer = true;
            StartCoroutine(StopTimeCountdown(2));
        }
    }

    public void AddPlayerAttack(CombatBean attack)
    {
        combatQueue.enqueuePlayerAttack(attack);
    }


    IEnumerator StopTimeCountdown(int time)
    {
        yield return new WaitForSeconds(0);
        StopTime();
        StartCoroutine(AttackBackCountdown(time));
    }

    IEnumerator AttackBackCountdown(int time)
    {
        playerController.ToggleAttacking();
        yield return new WaitForSeconds(time);
        foreach (EnemyBehavior e in enemies)
        {
            e.looking = false;
        }
        playerController.ToggleAttacking();
        AttackBack();
    }

    void StopTime()
    {
        player.GetComponent<AttackMoves>().StopTime();
    }

    void StartTime()
    {
        timer = false;
        foreach (AttackMoves a in attackAnimators)
        {
            a.StartTime();
        }
    }

    void AttackBack()
    {
        Debug.Log("attackback");
        CombatBean playerAttack = combatQueue.Peek()[0];
        CombatBean enemyAttack = combatQueue.Peek()[1];
        if (enemyAttack != null)
        {
            int winner = combatQueue.Dequeue();
            if(playerAttack != null)
            {
                playerAttack.Self.Attack(playerAttack, enemyAttack.Self.gameObject);
                outstandingAttacks += 1;
            }
            enemyAttack.Self.Attack(enemyAttack, player);
            outstandingAttacks += 1;
            StartCoroutine(WaitForAttacks());
        } else
        {
            StartTime();
        }
    }

    IEnumerator WaitForAttacks()
    {
        yield return new WaitUntil(()=> outstandingAttacks == 0);
        AttackBack();

    }

    public void AttackFinish()
    {
        Debug.Log("finished");
        outstandingAttacks--;
    }
    
    void SpawnEnemy(CombatBean.Attacks attack, Vector2 position) {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.GetComponent<EnemyBehavior>().attack = attack;
        attackAnimators.Add(enemy.GetComponent<AttackMoves>());
    }
}
