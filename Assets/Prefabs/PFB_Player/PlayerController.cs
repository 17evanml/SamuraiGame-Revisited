using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private KeyCode LEFT = KeyCode.A;
    private KeyCode RIGHT = KeyCode.D;
    private KeyCode UP = KeyCode.W;
    private KeyCode DOWN = KeyCode.S;
    private KeyCode JUMP = KeyCode.Space;


    

    //Control Booleans
    private bool left = false;
    private bool right = false;
    private bool up = false;
    private bool down = false;

    //Movement Variables
    private Rigidbody2D rb;
    private float SPEED = 2;
    private float JUMPHEIGHT = 10;
    private Collider2D boxCollider;
    private float GROUNDEDOFFSET = 0.1f;
    public bool grounded = true;
    bool jumpCooldown = false;

    //Attacking Variables
    bool attacking = false;


    //Animator
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Control Booleans
        if (Input.GetKeyDown(LEFT))
        {
            if (attacking)
            {
                Slice();
            }
            else
            {
                animator.SetInteger("AnimState", 1);
                left = true;
            }
        }
        if (Input.GetKeyUp(LEFT))
        {
            left = false;
        }

        if (Input.GetKeyDown(RIGHT))
        {
            if (attacking)
            {
                Feint();
            } else
            {
                animator.SetInteger("AnimState", 1);
                right = true;
            }
        }
        if (Input.GetKeyUp(RIGHT))
        {
            right = false;
        }

        if (Input.GetKeyDown(DOWN))
        {
            if(attacking)
            {
                Parry();
            } else
            {
                down = true;
            }
        }
        if (Input.GetKeyUp(DOWN))
        {
            down = false;
        }

        if (Input.GetKeyDown(UP))
        {
            up = true;
        }
        if (Input.GetKeyUp(UP))
        {
            up = false;
        }

        if (Input.GetKeyDown(JUMP))
        {
            Jump();
        }
        #endregion

        if (left)
        {
            MoveLeft();
        }
        if (right)
        {
            MoveRight();
        }
        if(down)
        {
        }
        if (Physics2D.Raycast(transform.position, -transform.up, boxCollider.bounds.extents.y+GROUNDEDOFFSET, 1 << LayerMask.NameToLayer("Stage")))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void Jump()
    {
        if (grounded && !jumpCooldown && !attacking)
        {
            rb.AddForce(transform.up * JUMPHEIGHT, ForceMode2D.Impulse);
        }
    }

    void MoveLeft()
    {
        rb.velocity = new Vector2(-SPEED, rb.velocity.y);
    }

    void MoveRight()
    {
        rb.velocity = new Vector2(SPEED, rb.velocity.y);
    }

    void Slice()
    {
        GameManager.instance.AddPlayerAttack(new CombatBean(CombatBean.Attacks.slice, HelperScripts.Clone(MouseWorldPoint()), HelperScripts.Clone(this.transform.position), GetComponent<AttackMoves>()));
    }

    void Feint()
    {
        GameManager.instance.AddPlayerAttack(new CombatBean(CombatBean.Attacks.feint, HelperScripts.Clone(MouseWorldPoint()), HelperScripts.Clone(this.transform.position), GetComponent<AttackMoves>()));
    }

    void Parry()
    {
        GameManager.instance.AddPlayerAttack(new CombatBean(CombatBean.Attacks.parry, HelperScripts.Clone(MouseWorldPoint()), HelperScripts.Clone(this.transform.position), GetComponent<AttackMoves>()));
    }

    public void ToggleAttacking()
    {
        attacking = !attacking;
    }

    public Vector2 MouseWorldPoint()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
