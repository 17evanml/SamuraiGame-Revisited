using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public float VISIONRADIUS = 5;
    public bool looking = true;
    public CombatBean.Attacks attack;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position -player.transform.position).magnitude < VISIONRADIUS && looking == true)
        {
            Debug.Log("saw you");
            looking = false;
            GameManager.instance.AddEnemyAttack(new CombatBean(attack, GameManager.instance.player.transform.position, HelperScripts.Clone(transform.position), gameObject.GetComponent<AttackMoves>())) ;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, VISIONRADIUS);
    }

}
