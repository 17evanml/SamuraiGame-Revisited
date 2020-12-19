using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoves : MonoBehaviour
{
    private float SLICESPEED = 3;
    private float SLICEDIST = 2;
    public LineRenderer lr;
    private Animator m_animator;
    Rigidbody2D rb;
    Collider2D collider2D;
    private bool WillDie = false;


    public LineRenderer LR
    {
        get {
            if (tag.Equals("Enemy")) {
                return lr;
            } else {
                return null;
            }; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(tag.Equals("Enemy"))
        {
            lr = GetComponent<LineRenderer>();
        }
        collider2D = GetComponent<Collider2D >();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Slice(GameObject other)
    {
        Vector2 target = new Vector2(other.transform.position.x, other.transform.position.y);
        target = target + (target - (Vector2)this.transform.position).normalized * 3f;
        StartCoroutine(SliceAnimation(this.transform.position, target));
    }
    public void Feint(GameObject other)
    {
        Vector2 target = new Vector2(other.transform.position.x, other.transform.position.y);
        target = target - (target - (Vector2)this.transform.position).normalized * 1f;
        StartCoroutine(FeintAnimation(this.transform.position, target));
    }
    public void Parry(GameObject other)
    {
        Vector2 target = new Vector2(other.transform.position.x, other.transform.position.y);
        StartCoroutine(ParryAnimation(this.transform.position, target));
    }

    public IEnumerator SliceAnimation(Vector2 self, Vector2 other)
    {
        m_animator.SetTrigger("Attack" + 1);
        Vector2 dir = (other - self).normalized * (other - self).magnitude / 0.2f;
        if (dir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

        } else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        rb.velocity = dir;
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        GameManager.instance.AttackFinish();
    }

    public IEnumerator FeintAnimation(Vector2 self, Vector2 other)
    {

        Vector2 dir = (other - self).normalized * (other - self).magnitude / 0.2f;
        if (dir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        rb.velocity = dir;
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        //yield return new WaitForSeconds(0f);
        m_animator.SetTrigger("Attack" + 1);
        GameManager.instance.AttackFinish();
    }

    public IEnumerator ParryAnimation(Vector2 self, Vector2 other)
    {

        Vector2 dir = (other - self).normalized;
        if (dir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        yield return new WaitForSeconds(0.08f);
        m_animator.SetTrigger("Block");
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.AttackFinish();
    }

    public void Attack(CombatBean attack, GameObject other)
    {
        rb.constraints = (RigidbodyConstraints2D.FreezeRotation);
        if(attack.Attack == CombatBean.Attacks.slice)
        {
            Slice(other);
        } else if (attack.Attack == CombatBean.Attacks.feint)
        {
            Feint(other);
        } else if (attack.Attack == CombatBean.Attacks.parry)
        {
            Parry(other);
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

    public AttackMoves StartTime()
    {
        Debug.Log(gameObject.name);
        rb.gravityScale = 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        if(WillDie)
        {
            return this;
        } else
        {
            return null;
        }
        //rb.simulated = true;
       // collider2D.enabled = true;
    }

    public void Lose()
    {
        WillDie = true;
    }
}
