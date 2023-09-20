using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
    }

    private void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
    }
}
