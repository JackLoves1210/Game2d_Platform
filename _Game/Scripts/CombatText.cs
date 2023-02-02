using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] Text hpText;


    public void OnInit(float damge)
    {
        hpText.text = damge.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }
}
