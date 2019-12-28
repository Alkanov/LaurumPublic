using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class EnemyStats : NetworkBehaviour
{
    public enum MonsterType
    {
        normal,        
        elite,
        boss       
    }
    public enum AttackType
    {
        melee,
        ranged,
        ignore
    }
    public enum DamageType
    {       
        physical,
        magical,
        ignore
    }

    #region Networking
    [SyncVar]
    public int MaxHP = 100;
    [SyncVar]
    public float CurrentHP;
    [SyncVar]
    public int Level;
    [SyncVar]
    public string MobName;
    [SyncVar]
    public int MobID;
    [SyncVar]
    public Color NameColor;
    #endregion

    #region Enemy
    EnemyTakeDamage EnemyTakeDamage;
    EnemyConditions Conditions;
    #endregion

    #region temp data
    float temp_hpregen;
    public float temp_def;
    public float temp_dmg_int;
    public float temp_dmg_str;
    public float temp_dodge;
    #endregion

    #region Stats
    public MonsterType MonsterType_now;
    public AttackType AttackType_now;
    public DamageType DamageType_now;
    
   
    public float HP_regen = 0.025f;//2.5% regen
    public float hp_regen_time = 10f;
    public float MP_regen = 0.025f;//2.5% regen

    public float Damage_str;
    public float Damage_int;

    public int MaxMP;

    public float Defense_str;
    public float Defense_int;

    public float Dodge_percent_dex;
    public float Critical_percent_agi;

    public float AttackSpeed;
    public float AttackRange;

    public float Exp;

    public float WalkingSpeed = 0.9f;

    #endregion

    private void Awake()
    {
        HP_regen = 0.025f;
        temp_hpregen = hp_regen_time;
        EnemyTakeDamage = GetComponent<EnemyTakeDamage>();
        Conditions = GetComponent<EnemyConditions>();
    }
    void Start()
    {
        CurrentHP = MaxHP;
        StartCoroutine(HPMPRegen());
        StartCoroutine(HPwatchdog());
    }
    public void Quick_hp_regen()
    {
        //used for quickly regenerating hp when aggro is off and to set it to normal when aggro is back on
        temp_hpregen = 0.5f;      
    }
    public void cancel_Quick_hp_regen()
    {
        temp_hpregen = hp_regen_time;
    }
  
    public void ProcessStats()
    {
        if (Conditions.decreasedDEF < 0f)
        {
            Defense_str = Defense_str * (1f + (Conditions.decreasedDEF / 100f));
        }
        if (Conditions.decreasedDamage < 0f)
        {
            Damage_str = Damage_str *(1f + (Conditions.decreasedDamage / 100f));
            //Damage_int = Damage_int *(1f + (Conditions.decreasedDamage / 100f));
        }
        if (Conditions.decreasedDodge < 0f)
        {
            Dodge_percent_dex = Dodge_percent_dex * (1f + (Conditions.decreasedDodge / 100f));
        }
    }

    IEnumerator HPMPRegen()
    {
        yield return new WaitForSeconds(temp_hpregen);
        if (CurrentHP > 0f)
        {
            var hp_to_regen = MaxHP * HP_regen;
            CurrentHP += hp_to_regen;
            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
                cancel_Quick_hp_regen();
            }                               
            StartCoroutine(HPMPRegen());
        }
    }

    IEnumerator HPwatchdog()
    {
        yield return new WaitForSeconds(2f);
        if (CurrentHP <= 0)
        {
            EnemyTakeDamage.dieNow();
        }
        else
        {
            StartCoroutine(HPwatchdog());
        }
    }

    #region Network client
    [ClientRpc]
    public void RpcMakeSound(string sound, Vector2 pos)
    {

    }
    #endregion

}