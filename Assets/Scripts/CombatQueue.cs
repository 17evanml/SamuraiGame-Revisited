using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatQueue
{

    private Queue<CombatBean> playerAttacks;
    private Queue<CombatBean> enemyAttacks;

    int playerAttackCount = 0;
    int enemyAttackCount = 0;
    // Start is called before the first frame update

    public CombatQueue()
    {
        playerAttacks = new Queue<CombatBean>();
        enemyAttacks = new Queue<CombatBean>();
    }

    public int Dequeue()
    {
        if (playerAttackCount > 0 && enemyAttackCount > 0)
        {
            playerAttackCount--;
            enemyAttackCount--;
            return playerAttacks.Dequeue().Battle(enemyAttacks.Dequeue());
        }
        else if (enemyAttackCount > 0)
        {
            enemyAttackCount--;
            return enemyAttacks.Dequeue().Battle(null);
        }
        else if (playerAttackCount > 0)
        {
            playerAttacks.Dequeue();
            playerAttackCount--;
            return -2;
        } else
        {
            return -2;
        }
    }

    public CombatBean[] Peek()
    {
        CombatBean[] peek = new CombatBean[2];
        if (playerAttacks.Count != 0)
        {
            peek[0] = playerAttacks.Peek();
        }
        if (enemyAttacks.Count != 0)
        {
            peek[1] = enemyAttacks.Peek();
        }
        return peek;
    }

    public void enqueuePlayerAttack(CombatBean attack)
    {
        playerAttacks.Enqueue(attack);
        playerAttackCount++;
    }

    public void enqueueEnemyAttack(CombatBean attack)
    {
        enemyAttacks.Enqueue(attack);
        enemyAttackCount++;
    }

    public bool IsEmpty()
    {
        if (enemyAttacks.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
