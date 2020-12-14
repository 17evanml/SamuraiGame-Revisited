using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoves : MonoBehaviour
{
    private float SLICESPEED = 3;
    private float SLICEDIST = 2;
    public GameObject other;
    Rigidbody2D rb;
    Collider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D >();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliceMethod(GameObject other)
    {
        Vector2 target = new Vector2(other.transform.position.x, other.transform.position.y);
        target = target + (target - (Vector2)this.transform.position).normalized * 1.5f;
        StartCoroutine(Slice(this.transform.position, target));
    }

    public IEnumerator Slice(Vector2 self, Vector2 other)
    {
        rb.velocity = (other - self).normalized*(other-self).magnitude/0.5f;
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.AttackFinish();
    }

    public void Attack(CombatBean attack, GameObject other)
    {
        rb.constraints = (RigidbodyConstraints2D.FreezeRotation);
        Debug.Log("AttackMoves: Attack()" + attack.Attack);
        if(attack.Attack == CombatBean.Attacks.slice)
        {
            Debug.Log("Attack: SliceMethod()");
            SliceMethod(other);
        } else if (attack.Attack == CombatBean.Attacks.feint)
        {

        } else if (attack.Attack == CombatBean.Attacks.parry)
        {

        } else
        {
            GameManager.instance.AttackFinish();
        }
    }


    public void StopTime()
    {
        rb.constraints = (RigidbodyConstraints2D.FreezeAll);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        //collider2D.enabled = false;
    }

    public void StartTime()
    {
        rb.gravityScale = 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        //rb.simulated = true;
       // collider2D.enabled = true;
    }
}
