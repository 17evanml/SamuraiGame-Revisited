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
    private float SPEED = 1;
    private float JUMPHEIGHT = 10;
    private Collider2D boxCollider;
    private float GROUNDEDOFFSET = 0.1f;
    bool grounded = true;
    bool jumpCooldown = false;

    //Attacking Variables
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Control Booleans
        if (Input.GetKeyDown(LEFT))
        {
            left = true;
        }
        if (Input.GetKeyUp(LEFT))
        {
            left = false;
        }

        if (Input.GetKeyDown(RIGHT))
        {
            right = true;
        }
        if (Input.GetKeyUp(RIGHT))
        {
            right = false;
        }

        if (Input.GetKeyDown(DOWN))
        {
            down = true;
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
            if(attacking)
            {
                Slice();
            } else
            {
                MoveLeft();
            }
        }
        if (right)
        {
            if(attacking)
            {
                Feint();
            } else
            {
                MoveRight();
            }
        }
        if(down)
        {
            Parry();
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
        rb.AddForce(-transform.right * SPEED);
    }

    void MoveRight()
    {
        rb.AddForce(transform.right * SPEED);
    }

    void Slice()
    {
        GameManager.instance.AddPlayerAttack(new CombatBean(CombatBean.Attacks.slice, MouseWorldPoint(), this.transform.position, GetComponent<AttackMoves>()));
    }

    void Feint()
    {

    }

    void Parry()
    {

    }

    public void ToggleAttacking()
    {
        attacking = !attacking;
    }

    public Vector2 MouseWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
