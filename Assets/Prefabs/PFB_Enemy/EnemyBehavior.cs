using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public GameObject eyes;
    public float VISIONRADIUS = 5;
    public bool looking = true;
    public float speed = 2;
    public bool walking = false;
    public CombatBean.Attacks attack;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.instance.player;
        GameManager.instance.attackAnimators.Add(GetComponent<AttackMoves>());
        GameManager.instance.enemies.Add(GetComponent<EnemyBehavior>());
        if (attack == CombatBean.Attacks.slice)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (attack == CombatBean.Attacks.parry)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (attack == CombatBean.Attacks.feint)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        transform.GetChild(1).transform.localScale = new Vector3(transform.GetChild(1).localScale.x*VISIONRADIUS, transform.GetChild(1).localScale.y*VISIONRADIUS, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - player.transform.position).magnitude < VISIONRADIUS && looking == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (player.transform.GetChild(5).position - eyes.transform.position).normalized, Mathf.Infinity,  ~(1 <<LayerMask.NameToLayer("Enemy")));
            Debug.DrawLine(player.transform.GetChild(5).position, eyes.transform.position);
            Debug.DrawLine(transform.position, Vector2.up, Color.blue);
            Debug.DrawLine(player.transform.position, Vector2.up, Color.red);
            Debug.Log(hit.collider);
            if (hit.collider.gameObject.tag.Equals("Player"))
            {
                Debug.Log("Player seen, attacking");
                looking = false;
                GameManager.instance.AddEnemyAttack(new CombatBean(attack, HelperScripts.Clone(GameManager.instance.player.transform.position), HelperScripts.Clone(transform.position), gameObject.GetComponent<AttackMoves>()));
            }
        }
        if(walking)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, VISIONRADIUS);
    }

}
