using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    // Fields
    private int _health;
    private int _maxHealth;

    // Properties
    public int Health
    {
        get 
        {
            return _health;
        }
        set
        { 
            _health = value;
        }

    }

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set 
        {
            _maxHealth = value;
        }
    }

    // Constructor
    public UnitHealth(int health, int maxhealth)
    {
        _maxHealth = maxhealth;
        _health = health;
    }

    // Methods
    public void DmgUnit(int dmgAmount)
    {
        if (_health > 0)
        {
            _health -= dmgAmount;
        }

    }

    public void HealUnit(int healAmount)
    {
        if (_health < _maxHealth)
        {
            _health += healAmount;
        }
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

}
