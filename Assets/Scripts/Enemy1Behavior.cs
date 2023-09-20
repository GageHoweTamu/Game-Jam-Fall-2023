using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behavior : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {

    }

    private void Enemy1TakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
    }

    private void Enemy1Heal(int healing)
    {
        GameManager.gameManager._enemy1Health.HealUnit(healing);
    }
}
