using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float ENEMYVISIONTIMER = 0.5f;
    const float PLAYERATTACKTIMER = 2f;


    public static GameManager instance;
    public CombatQueue combatQueue;
    public GameObject player;
    public GameObject enemyPrefab;
    public List<AttackMoves> attackAnimators = new List<AttackMoves>();
    public HeroKnight playerController;
    public List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    public Queue<CombatBean> enemyTempQueue = new Queue<CombatBean>();
    public int outstandingAttacks = 0;
    private bool timer = false;


    public GameObject Timer;
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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
       // StartTimer();
        combatQueue = new CombatQueue();
        attackAnimators.Add(player.GetComponent<AttackMoves>());
        playerController = player.GetComponent<HeroKnight>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            int type = Random.Range(0, 3);
            if(type == 0)
            {
                SpawnEnemy(CombatBean.Attacks.slice, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            } else if (type == 1)
            {
                SpawnEnemy(CombatBean.Attacks.feint, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            } else
            {
                SpawnEnemy(CombatBean.Attacks.parry, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }


    public void AddEnemyAttack(CombatBean attack)
    {
        attack.Self.StopTime();
        player.GetComponent<AttackMoves>().StopTime();
        playerController.StopTime();
        attackAnimators.Add(attack.Self);
        combatQueue.enqueueEnemyAttack(attack);
        enemyTempQueue.Enqueue(attack);
        if(!timer)
        {
            Debug.Log("Timer on");
            timer = true;
            StartCoroutine(DequeueEnemyAttacks());
        }
    }

    IEnumerator DequeueEnemyAttacks()
    {
        CombatBean e = enemyTempQueue.Dequeue();
        EnableEnemyLineRenderer(e);
        yield return new WaitForSeconds(ENEMYVISIONTIMER);
        if(enemyTempQueue.Count > 0)
        {
            Debug.Log($"Queue not empty: {enemyTempQueue.Count}");
            StartCoroutine(DequeueEnemyAttacks());
        } else
        {
            Debug.Log($"Queue empty: Starting attacks");
            DisableEnemyLooking();
            StopTime();
            StartCoroutine(EnableAttackBack());
        }
    }

    IEnumerator EnableAttackBack()
    {
        Debug.Log("Player can now attack");
        playerController.ToggleAttacking();
        yield return new WaitForSeconds(PLAYERATTACKTIMER);
        playerController.ToggleAttacking();
        DisableEnemyLineRenderers();
        AttackBack();
    }

    public void EnableEnemyLineRenderer(CombatBean attack)
    {
        attack.Self.LR.gameObject.GetComponent<LineRenderer>();
        attack.Self.LR.enabled = true;
        attack.Self.LR.SetPosition(0, attack.PlayerPosition);
        attack.Self.LR.SetPosition(1, attack.Position);
    }

    public void DisableEnemyLineRenderers()
    {
        foreach(AttackMoves a in attackAnimators)
        {
            if(a.LR != null)
            {
                a.LR.enabled = false;
            }
        }
    }


    public void AddPlayerAttack(CombatBean attack)
    {
        combatQueue.enqueuePlayerAttack(attack);
    }


    IEnumerator StopTimeCountdown()
    {
        StopTime();
        yield return new WaitForSeconds(0.5f);
    }

    void DisableEnemyLooking()
    {
        foreach (EnemyBehavior e in enemies)
        {
            e.looking = false;
        }
    }

    void StopTime()
    {
        foreach(AttackMoves a in attackAnimators)
        {
            a.StopTime();
        }
    }

    void StartTime()
    {
        combatQueue.Clear();
        timer = false;
        playerController.StartTime();
        List<AttackMoves> toDie = new List<AttackMoves>();
        foreach (AttackMoves a in attackAnimators)
        {
            AttackMoves current = a.StartTime();
            if(current != null)
            {
                toDie.Add(current);
            }
        }

        foreach(AttackMoves a in toDie)
        {
            attackAnimators.Remove(a);
            Destroy(a.gameObject);
        }

        foreach (EnemyBehavior e in enemies)
        {
            e.looking = true;
        }
    }

    void AttackBack()
    {
        CombatBean playerAttack = combatQueue.Peek()[0];
        CombatBean enemyAttack = combatQueue.Peek()[1];
        if (enemyAttack != null)
        {
            int winner = combatQueue.Dequeue();
            Debug.Log($"Winner is: {winner}");
            if(playerAttack != null)
            {
                playerAttack.Self.Attack(playerAttack, enemyAttack.Self.gameObject);
                outstandingAttacks += 1;
            } else
            {
                Debug.Log("No Attack Recorded");
            }
            if(winner == 1)
            {
                enemyAttack.Self.Lose();
            } else if (winner == -1)
            {
                //End screen;
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
        outstandingAttacks--;
    }

    void SpawnEnemy(CombatBean.Attacks attack, Vector2 position) {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.GetComponent<EnemyBehavior>().attack = attack;
       
    }

    void StartTimer()
    {
        Timer.GetComponent<TimerScript>().StartTimer(2);
    }
}
