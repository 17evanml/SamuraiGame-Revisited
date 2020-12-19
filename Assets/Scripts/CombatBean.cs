using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBean
{
    public enum Attacks { slice, parry, feint, fail }
    delegate int AttackMethod(CombatBean other);
    List<AttackMethod> AttackList = new List<AttackMethod>();  
    private Attacks attack;
    AttackMoves self;

    public Attacks Attack
    {
        get { return attack; }
    }
    public Vector2 PlayerPosition
    {
        get { return playerPosition; }
    }
    public Vector2 Position
    {
        get { return position; }
    }
    public AttackMoves Self
    {
        get { return self; }
    }
    private Vector2 position;
    private Vector2 playerPosition;
    public CombatBean(Attacks _attack, Vector2 _position, Vector2 _playerPosition, AttackMoves _self) {
        attack = _attack;
        position = _position;
        playerPosition = _playerPosition;
        self = _self;
        Initialize();
    }


    public int Battle(CombatBean other)
    {
        if(other == null)
        {
            Debug.Log("No player attack detected");
            return AttackList[(int)Attack](null);
        }
        if ((this.Position - other.PlayerPosition).magnitude > 1f)
        {
            Debug.Log("Cursor Position:" + this.position);
            Debug.Log("Target Position:" + other.playerPosition);
            Debug.Log("Position Failed");
            Fail();
        }
        if((other.Position - this.PlayerPosition).magnitude > 1f)
        {
            //other.Fail();
        }
            return AttackList[(int)Attack](other);
    }

    public void Fail()
    {
        this.attack = Attacks.fail;
    }
        

    private int Slice(CombatBean other)
    {
        if(other == null)
        {
            return -1;
        } else if (other.Attack == Attacks.slice)
        {
            return 0;
        }
        else if (other.Attack == Attacks.parry)
        {
            return -1;
        }
        else if (other.Attack == Attacks.feint)
        {
            return 1;
        }
        else
        {
            return 1;
        }
    }

    private int Parry(CombatBean other)
    {
        if (other == null)
        {
            return -1;
        }
        else if (other.Attack == Attacks.slice)
        {
            return 1;
        }
        else if (other.Attack == Attacks.parry)
        {
            return 0;
        }
        else if (other.Attack == Attacks.feint)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private int Feint(CombatBean other)
    {
        if (other == null)
        {
            return -1;
        }
        else if(other.Attack == Attacks.slice)
        {
            return -1;
        }
        else if (other.Attack == Attacks.parry)
        {
            return 1;
        }
        else if (other.Attack == Attacks.feint)
        {
            return 0;
        } else
        {
            return 1;
        }
    }

    private int Fail(CombatBean other)
    {
        if (other == null)
        {
            return -1;
        }
        else if(other.Attack == Attacks.slice)
        {
            return -1;
        }
        else if (other.Attack == Attacks.parry)
        {
            return 0;
        }
        else if (other.Attack == Attacks.feint)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    public void Initialize()
    {
        AttackList.Add(Slice);
        AttackList.Add(Parry);
        AttackList.Add(Feint);
        AttackList.Add(Fail);
    }
}
