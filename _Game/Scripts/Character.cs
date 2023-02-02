using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator amin;
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] protected CombatText CombatTextPrefab;
    


    private float hp;
    private string currentAminName;


    public bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100 , transform);
    }

    public virtual void OnDespawn()
    {

    }
    public virtual void OnDead()
    {
        ChangeAmin("dead");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAmin(string aminName)
    {
        if (currentAminName != aminName)
        {
            amin.ResetTrigger(aminName);
            currentAminName = aminName;
            amin.SetTrigger(currentAminName);
        }
    }

    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if (IsDead)
            {
                OnDead();
            }
            healthBar.setNewHp(hp);
            CombatText combatText = Instantiate(CombatTextPrefab, transform.position + Vector3.up, Quaternion.identity);
            combatText.OnInit(damage);
            combatText.transform.position = transform.position;
        }
    }

   
}
