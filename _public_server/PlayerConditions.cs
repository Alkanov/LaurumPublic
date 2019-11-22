using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Pathfinding;
using System.Linq;

public class PlayerConditions : NetworkBehaviour
{
    #region TEST
    public bool refresh_buffs;
    public int buff_to_add;
    public int debuff_to_add;
    #endregion
    #region Networking   
    [HideInInspector]
    public SyncListInt buffs = new SyncListInt();
    [HideInInspector]
    public SyncListInt de_buffs = new SyncListInt();
    [SyncVar]
    public bool silence = false;
    #endregion

    #region Player
    PlayerMPSync PlayerMPSync;
    PlayerStats PlayerStats;
    PlayerPVPDamage PlayerPVPDamage;
    PlayerGeneral PlayerGeneral;
    PlayerSkillsActions PlayerSkillsActions;
    #endregion

    #region Buff and Debuff data
    [System.Serializable]
    public class buff_debuff_data
    {
        public List<int> buff_debuff_ID;
        public float time;

        public buff_debuff_data()
        {

        }

        public buff_debuff_data(List<int> buff_debuff_ID, float time)
        {
            this.buff_debuff_ID = buff_debuff_ID;
            this.time = time;
        }

    }
    #endregion

    #region Condition Data   
    //trackers
    public enum type
    {
        buff,
        debuff
    }
    [System.Serializable]
    public class track_buff_debuffs
    {
        public int buff_debuff_ID;
        public skill skill_requested;
        public float expiresOn;
        public GameObject skill_owner;
        public bool canStack;
        public type type;
        public bool can_be_cleaned;

        public track_buff_debuffs(int buff_debuff_ID, skill skill_requested, float expiresOn, GameObject skill_owner, bool canStack, type type, bool can_be_cleaned)
        {
            this.buff_debuff_ID = buff_debuff_ID;
            this.skill_requested = skill_requested;
            this.expiresOn = expiresOn;
            this.skill_owner = skill_owner;
            this.canStack = canStack;
            this.type = type;
            this.can_be_cleaned = can_be_cleaned;
        }
    }

    public List<track_buff_debuffs> tracker_list = new List<track_buff_debuffs>();
    //conditions   
    [HideInInspector]
    public bool stunned = false;   
    [HideInInspector]
    public bool concentrated = false;
    [HideInInspector]
    public bool immortal = false;
    [HideInInspector]
    public bool mana_shield = false;
    [HideInInspector]
    public bool potion_blocked = false;   
    //buffs
    [HideInInspector]
    public float increasedDamage;
    [HideInInspector]
    public float increasedAtkSpeed;
    [HideInInspector]
    public float increasedCritical;
    [HideInInspector]
    public float increasedINT;
    [HideInInspector]
    public float increasedSTR;
    [HideInInspector]
    public float increasedDEX;
    [HideInInspector]
    public float increasedAGI;
    [HideInInspector]
    public float increasedDEF;
    [HideInInspector]
    public float increasedMDEF;
    [HideInInspector]
    public float increasedDodge;
    [HideInInspector]
    public float increasedMaxHP;
    [HideInInspector]
    public float increasedMaxMana;
    [HideInInspector]
    public float increasedWalkingSpeed;
    [HideInInspector]
    public float increasedCooldownReduction;
    [HideInInspector]
    public float increasedCastingSpeed;

    //debuffs
    [HideInInspector]
    public float decreasedDEF;
    [HideInInspector]
    public float decreasedMDEF;
    [HideInInspector]
    public float decreasedDamage;  
    [HideInInspector]
    public float decreasedWalkingSpeed;
    #endregion

    #region Effects Data
    [System.Serializable]
    public class effects_timers
    {
        public Coroutine effect_timer;
        public int effectID;
        public effects_timers(Coroutine effect_timer, int effectID)
        {
            this.effect_timer = effect_timer;
            this.effectID = effectID;
        }
    }
    [HideInInspector]
    public List<effects_timers> effects_running = new List<effects_timers>();
    //public int TEST_EMERGENCY_STOP;
    #endregion

    private void Awake()
    {
        PlayerMPSync = GetComponent<PlayerMPSync>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerPVPDamage = GetComponent<PlayerPVPDamage>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerSkillsActions = GetComponent<PlayerSkillsActions>();
        increasedAtkSpeed = 1;
    }
    private void Start()
    {
        StartCoroutine(checkBuffTimers());
    }

    #region TEST
    private void Update()
    {
        if (refresh_buffs)
        {
            refresh_buffs = false;
            //buff
            if (buff_to_add != 0)
            {
                buffs.Add(buff_to_add);
                tracker_list.Add(new track_buff_debuffs(buff_to_add, null, Time.time + 30f, null, false, type.buff, true));
                buff_to_add = 0;
            }
            //debuff
            if (debuff_to_add != 0)
            {
                de_buffs.Add(debuff_to_add);
                tracker_list.Add(new track_buff_debuffs(debuff_to_add, null, Time.time + 30f, null, false, type.debuff, true));
                debuff_to_add = 0;
            }
            //Debug.LogError("Buff/Debuff Added");
        }
    }
    #endregion

    #region Networking Client
    [ClientRpc]
    public void RpccleanDeBuffs(int conditionID)
    {
    }
    [ClientRpc]
    public void RpccleanBuffs(int conditionID)
    {
    }
    #endregion

    #region Utilidades
    public buff_debuff_data find_debuff(skill skillRequested)
    {
        var debuff_data = new buff_debuff_data();
        debuff_data.buff_debuff_ID = new List<int>();
        debuff_data.time = 2f;
        var chance = 0;
        switch (skillRequested.SkillID)
        {
            // de_buffid[0] = 1; stun
            // de_buffid[0] = 2; slow
            // de_buffid[0] = 3; explosive
            // de_buffid[0] = 4; life drain on death
            // de_buffid[0] = 5; Confused (client side)
            // de_buffid[0] = 6; Sleep 
            // de_buffid[0] = 7; (can't drink potions) 
            // de_buffid[0] = 8; (damage decrease)
            // de_buffid[0] = 9; (defense decrease)
            // de_buffid[0] = 10; Silence 
            // de_buffid[0] = 11; Armor crusher
            //de_buffid[0] = 12; Unarmed
            //de_buffid[0] = 13; Frozen
            //14 Skills hit harder if under x hp
            #region Warrior
            case 61003://shield stun              
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(1);
                    stunned = true;
                    PlayerMPSync.PlayerCanMove = false;
                }
                break;
            case 61010: //soul cravings               
                //handled on PlayerPVP and EnemyTakeDamage
                break;
            case 61020://Ultimate Defense
                chance = 50;
                if (Random.Range(1, 100) <= chance)
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    debuff_data.time = skillRequested.multipliers[1];                  
                    decreasedWalkingSpeed = -20f;
                }
                break;
            case 61024://Armor Crusher
                chance = (int)skillRequested.multipliers[1];
                if (Random.Range(1, 100) <= chance)
                {
                    debuff_data.buff_debuff_ID.Add(11);
                    debuff_data.time = skillRequested.multipliers[2];
                    decreasedDEF = skillRequested.multipliers[0];

                }
                break;
            case 61025://dismember                
                if (has_buff_debuff(type.debuff, 11))//armor crushed
                {
                    debuff_data.buff_debuff_ID.Add(12);
                    debuff_data.time = skillRequested.multipliers[1];
                    remove_buff_debuff(type.debuff, 11);
                    decreasedDamage = skillRequested.multipliers[2];
                }
                break;
            case 61026:
                debuff_data.buff_debuff_ID.Add(2);
                debuff_data.time = skillRequested.multipliers[1];
                decreasedWalkingSpeed = -skillRequested.multipliers[0];
                break;
            case 61027://on your knees
                if (PlayerStats.CurrentHP / PlayerStats.MaxHealth <= skillRequested.multipliers[1])
                {
                    chance = (int)skillRequested.multipliers[0];
                    if (Random.Range(1, 100) <= chance)
                    {
                        debuff_data.buff_debuff_ID.Add(1);
                        stunned = true;
                        PlayerMPSync.PlayerCanMove = false;
                    }
                }
                break;
            #endregion

            #region Wizard
            case 62007:  //frost blade           
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(13);
                    stunned = true;
                    PlayerMPSync.PlayerCanMove = false;
                }
                break;
            case 62009:
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(13);
                    debuff_data.time = 3f;
                    stunned = true;
                    PlayerMPSync.PlayerCanMove = false;
                }
                else if (Random.Range(1, 100) <= skillRequested.multipliers[2])
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    debuff_data.time = 5f;
                    decreasedWalkingSpeed = -20f;                    
                }
                break;
            case 62010:
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    decreasedWalkingSpeed = -20f;
                }
                break;
            case 62011://Corpse Life Drain
                debuff_data.buff_debuff_ID.Add(4);
                debuff_data.time = skillRequested.multipliers[1];
                break;
            #endregion

            #region Hunter
            case 63005://Hamstring Shot                
                if (Random.Range(1, 100) <= skillRequested.multipliers[1] )
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    decreasedWalkingSpeed = -skillRequested.multipliers[2];
                }
                break;
            case 63011:                
                    debuff_data.buff_debuff_ID.Add(14);
                debuff_data.time = skillRequested.multipliers[0];              
                break;
            #endregion
            #region Paladin
            case 64012:
                chance = (int)skillRequested.multipliers[0];
                if (Random.Range(1, 100) <= chance)
                {
                    debuff_data.buff_debuff_ID.Add(10);
                    debuff_data.time = skillRequested.multipliers[1];
                    silence = true;
                }
                break;
            case 64014://buff remover
                chance = (int)skillRequested.multipliers[0];
                if (Random.Range(1, 100) <= chance && buffs.Count > 0)
                {
                    reset_buffs_or_debuffs(type.buff);
                }
                break;
            #endregion


            /* case 2020://Hamstring Shot
                  chance = 60;
                  if (Random.Range(1, 100) <= chance)
                  {
                      de_buffid[0] = 2;
                      slowed = true;
                      PlayerMPSync.PlayerSpeed *= 0.5f;
                  }
                  break;
              case 1040://Fierce Blow               
                  de_buffid[0] = 2;
                  slowed = true;
                  PlayerMPSync.PlayerSpeed *= 0.8f;
                  break;
              case 2040://Explosive Arrow
                  de_buffid[0] = 2;
                  de_buffid[1] = 3;
                  slowed = true;
                  PlayerMPSync.PlayerSpeed *= 0.5f;
                  break;
              case 3100://Corpse Life Drain
                  de_buffid[0] = 4;
                  break;*/


            default:
                break;
        }
        if (debuff_data.buff_debuff_ID.Count > 0)
        {
            PlayerStats.ProcessStats();
        }
        return debuff_data;
    }
    public buff_debuff_data find_buff(skill skill)
    {
        var buff_data = new buff_debuff_data();
        buff_data.buff_debuff_ID = new List<int>();
        buff_data.time = 2f;
        var chance = 0;

        // buffid[0] = 1; speed
        // buffid[0] = 2; increased damage
        // buffid[0] = 3; increased int
        // buffid[0] = 4; increasedCritical
        // buffid[0] = 5; max hp
        // buffid[0] = 6;  max mp
        // buffid[0] = 7; increasedDEF
        // buffid[0] = 8; increasedMDEF
        // buffid[0] = 9; increasedAtkSpeed
        // buffid[0] = 10; immortal
        // buffid[0] = 11; increasedDodge
        // buffid[0] = 12; increasedDEF - Ultimate Defense
        // buffid[0] = 13; mana_shield
        // buffid[0] = 14; invisible
        // buffid[0] = 15; arrow deflect - % of damage vs ranged physical is absorbed
        // buffid[0] = 16; shields up - next hit does 0 and takes the buff off
        // buffid[0] = 17; Frozen hands speed and debuff speed
        // buffid[0] = 18; Next spell is instant
        //buffid[0] = 19; Remember me--->deprecated
        //buffid[0] = 20; linked heart
        //buffid[0] = 21; burn when damaged
        //buffid[0] = 22; concentration
       //buffid[0] = 23; speed banner

        switch (skill.SkillID)
        {

            #region Warrior           
            case 61006:
                buff_data.buff_debuff_ID.Add(1);
                buff_data.time = skill.multipliers[1];
                increasedWalkingSpeed = skill.multipliers[0];                                   
                break;
            case 61013://battle shout
                buff_data.buff_debuff_ID.Add(5);
                buff_data.time = skill.multipliers[1];
                increasedMaxHP = skill.multipliers[0];
                break;
            case 61020://Ultimate Defense
                buff_data.buff_debuff_ID.Add(12);
                buff_data.time = skill.multipliers[1];
                increasedDEF = skill.multipliers[0];
                break;
            case 61028://arrow deflect
                buff_data.buff_debuff_ID.Add(15);
                buff_data.time = skill.multipliers[1];
                break;
            case 61029://shields up
                buff_data.buff_debuff_ID.Add(16);
                buff_data.time = skill.multipliers[0];
                break;
            #endregion
            #region Wizard
            case 62008://frozen hands               
                buff_data.buff_debuff_ID.Add(17);
                buff_data.time = skill.multipliers[0];
                increasedWalkingSpeed = skill.multipliers[1]; 
                break;
            case 62012://mana shield
                mana_shield = true;
                buff_data.buff_debuff_ID.Add(13);
                buff_data.time = skill.multipliers[0];
                break;
            case 62013://expanded mana
                increasedMaxMana = skill.multipliers[0];
                buff_data.time = skill.multipliers[1];
                buff_data.buff_debuff_ID.Add(6);
                break;
            case 62014://caster contract
                buff_data.time = skill.multipliers[0];
                buff_data.buff_debuff_ID.Add(18);
                break;
            case 62015://concentration
                increasedDamage = skill.multipliers[0];
                buff_data.time = skill.multipliers[1];
                buff_data.buff_debuff_ID.Add(2);
                buff_data.buff_debuff_ID.Add(22);
                concentrated = true;
                break;
            #endregion
            #region Hunter
            case 63002://hawkeye  
                increasedCritical = skill.multipliers[0];
                buff_data.time = skill.multipliers[1];
                buff_data.buff_debuff_ID.Add(4);
                break;
            case 63012:
                buff_data.buff_debuff_ID.Add(1);
                buff_data.time = skill.multipliers[1];
                increasedWalkingSpeed = skill.multipliers[0];        
                break;
            case 63013:
                buff_data.buff_debuff_ID.Add(14);
                buff_data.time = skill.multipliers[0];
                GetComponent<NetworkProximityChecker>().forceHidden = true;//
                PlayerSkillsActions.Reset_enemies_players_aggro();
                break;
            case 63014://soul sacririce -> no buff but we handle it here
                var to_regen = PlayerStats.MaxMana * skill.multipliers[1] / 100f;
                PlayerStats.CurrentHP += to_regen;
                PlayerGeneral.showCBT(gameObject, false, false, (int)to_regen, "heal");
                break;
            case 63015:
                buff_data.buff_debuff_ID.Add(11);
                buff_data.time = skill.multipliers[1];
                increasedDodge = skill.multipliers[0];
                break;
            #endregion
            #region Paladin
            case 64001://Final Protection (dodge de los players al rededor)
                buff_data.buff_debuff_ID.Add(11);
                increasedDodge = skill.multipliers[0];
                break;
            case 64002:
                increasedMDEF = skill.multipliers[1];
                buff_data.buff_debuff_ID.Add(8);
                buff_data.time = skill.multipliers[0];
                break;
            case 64003:
                increasedDEF = skill.multipliers[1];
                buff_data.buff_debuff_ID.Add(7);
                buff_data.time = skill.multipliers[0];
                break;
            case 64004://linked heart
                buff_data.buff_debuff_ID.Add(20);
                buff_data.time = skill.multipliers[0];
                break;
            case 64005://burn on touch
                 buff_data.buff_debuff_ID.Add(21);
                 buff_data.time = skill.multipliers[1];
                 break;
            case 64013: //speed banner
                buff_data.buff_debuff_ID.Add(23);
                buff_data.time = skill.multipliers[4];
                increasedWalkingSpeed = skill.multipliers[0];
                increasedAtkSpeed = skill.multipliers[1];
                increasedCastingSpeed = skill.multipliers[2];
                increasedCooldownReduction = skill.multipliers[3];
                break;          
                    
                
            #endregion
            /*            
             
             case 1100://Enraged
                if (!buffs.Contains(1))
                {
                    debuff_data.buff_debuff_ID.Add(1);
                    speedy = true;
                    PlayerMPSync.PlayerSpeed *= 1.20f;                   
                }
                break; 

              case 1030://Final Frenzy              
                            debuff_data.buff_debuff_ID.Add(2);
                            increasedDamage = 20f;//1.35 o 1.X              
                            break;
                        case 4020://speedy
                            if (!buffs.Contains(1))
                            {
                                chance = 40;
                                if (Random.Range(1, 100) <= chance)
                                {
                                    buffid[0] = 1;
                                    PlayerMPSync.PlayerSpeed *= 1.20f;
                                    speedy = true;
                                }
                            }
                            break;
                        case 3030://concentration
                            increasedINT = skill.multipliers[0];
                            buffid[0] = 3;
                            concentrated = true;
                            break;
                        case 2030://hawkeye  
                            increasedCritical = skill.multipliers[0];
                            buffid[0] = 4;
                            break;
                        case 4030://Arcane Protection
                            increasedDEF = skill.multipliers[0];
                            increasedMDEF = skill.multipliers[0];
                            buffid[0] = 7;
                            buffid[1] = 8;
                            break;
                        case 2080://quickshot
                            buffid[0] = 9;
                            increasedAtkSpeed = 1 - (skill.multipliers[0] / 100);
                            break;
                        case 4060://Final Protection (dodge de los players al rededor)
                            buffid[0] = 11;
                            increasedDodge = skill.multipliers[0];
                            break;

                        case 3060:
                            mana_shield = true;
                            buffid[0] = 13;
                            break;
                        case 2100:
                            buffid[0] = 14;
                            GetComponent<NetworkProximityChecker>().forceHidden = true;
                            PlayerSkillsActions.Reset_enemies_players_aggro();
                            break;

                            */

            default:
                break;
        }

        if (buff_data.buff_debuff_ID.Count > 0)
        {
            PlayerStats.ProcessStats();
        }
        return buff_data;
    }
    public void RevealPlayer()
    {
        if (GetComponent<NetworkProximityChecker>().forceHidden)
        {
            GetComponent<NetworkProximityChecker>().forceHidden = false;
            clean_buff_debuff(type.buff, 14);
        }

    }
    #endregion

    #region Conditions related
    public void handle_buffs_debuffs(skill skill, GameObject Player_executing_skill)
    {
        ////////.LogError("handle_buffs_debuffs");
        var debuffs = find_debuff(skill);
        var buffs = find_buff(skill);
        bool stackable = false;//deprecated and always false
        bool can_be_cleaned = true;//deprecated and always false;

        for (int i = 0; i < debuffs.buff_debuff_ID.Count; i++)
        {
            add_buff_debuff(debuffs.buff_debuff_ID[i], skill, stackable, debuffs.time, Player_executing_skill, type.debuff, can_be_cleaned);
            PlayerGeneral.Rpc_skill_damage_animation(gameObject, gameObject, skill.SkillID);
        }
        for (int i = 0; i < buffs.buff_debuff_ID.Count; i++)
        {
            add_buff_debuff(buffs.buff_debuff_ID[i], skill, stackable, buffs.time, Player_executing_skill, type.buff, can_be_cleaned);
            PlayerGeneral.Rpc_skill_damage_animation(gameObject, gameObject, skill.SkillID);
        }

    }
    public void add_buff_debuff(int buff_debuff_ID, skill skill, bool stackable, float time, GameObject Player_executing_skill, type type, bool can_be_cleaned)
    {
        ////////.LogError("add_buff_debuff " + buff_debuff_ID);
        //buffs
        switch (type)
        {
            case type.buff:
                if (buffs.Contains(buff_debuff_ID) && !stackable)
                {
                    //renovar buff
                    for (int i = 0; i < tracker_list.Count; i++)
                    {
                        if (tracker_list[i].buff_debuff_ID == buff_debuff_ID)
                        {
                            tracker_list[i].expiresOn = Time.time + time;
                            break;
                        }
                    }

                }
                else
                {
                    //nuevo buff                
                    buffs.Add(buff_debuff_ID);
                    tracker_list.Add(new track_buff_debuffs(buff_debuff_ID, skill, Time.time + time, Player_executing_skill, stackable, type, can_be_cleaned));
                }
                break;
            case type.debuff:
                if (de_buffs.Contains(buff_debuff_ID) && !stackable)
                {
                    //renovar buff
                    for (int i = 0; i < tracker_list.Count; i++)
                    {
                        if (tracker_list[i].buff_debuff_ID == buff_debuff_ID)
                        {
                            tracker_list[i].expiresOn = Time.time + time;
                            break;
                        }
                    }

                }
                else
                {
                    //nuevo buff                
                    de_buffs.Add(buff_debuff_ID);
                    tracker_list.Add(new track_buff_debuffs(buff_debuff_ID, skill, Time.time + time, Player_executing_skill, stackable, type, can_be_cleaned));
                }
                break;
            default:
                break;
        }

        //debuffs


    }
    IEnumerator checkBuffTimers()
    {
        if (buffs.Count > 0 || de_buffs.Count > 0)
        {
            for (int i = 0; i < tracker_list.Count; i++)
            {
                if (tracker_list[i].expiresOn <= Time.time)
                {
                    if (tracker_list[i].type == type.buff)
                    {
                        clean_buffs(tracker_list[i]);
                    }
                    else if (tracker_list[i].type == type.debuff)
                    {
                        for (int d = 0; d < effects_running.Count; d++)
                        {
                            if (effects_running[d].effectID == tracker_list[i].buff_debuff_ID)
                            {
                                if (effects_running[d].effect_timer != null)
                                {
                                    StopCoroutine(effects_running[d].effect_timer);
                                    effects_running.RemoveAt(d);
                                }
                                else
                                {
                                    effects_running.RemoveAt(d);
                                }
                            }
                        }
                        clean_debuffs(tracker_list[i]);
                    }

                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(checkBuffTimers());

    }
    void clean_debuffs(track_buff_debuffs track_buff_debuffs)
    {
        ////////.Log("Cleaning debuff: " + conditionID);
        switch (track_buff_debuffs.buff_debuff_ID)
        {
            case 1://stun
                PlayerMPSync.PlayerCanMove = true;
                stunned = false;
                break;
            case 2://slow               
                decreasedWalkingSpeed = 0f;
                break;
            case 3://Explosive Arrow
                   //explotar bomba
                var Approved_targets_around = PlayerGeneral.x_ObjectHelper.get_AOE_LOS_targets(gameObject, 1.5f, LayerMask.GetMask("Player", "Enemy", "Coliders", "decoy"), true);
                for (int i = 0; i < Approved_targets_around.Count; i++)
                {
                    if (Approved_targets_around[i].tag == "Player")
                    {
                        if (track_buff_debuffs.skill_owner!=null && track_buff_debuffs.skill_owner.GetComponent<PlayerSkillsActions>().PVP_Arena_Party_checks(track_buff_debuffs.skill_requested, Approved_targets_around[i])){

                            Approved_targets_around[i].GetComponent<PlayerConditions>().handle_effect(DOT_effect.effect_type.fire, track_buff_debuffs.skill_requested.multipliers[0], track_buff_debuffs.skill_owner, 0f);

                        }

                    }
                    else if (Approved_targets_around[i].tag == "decoy")
                    {
                        Approved_targets_around[i].GetComponent<DecoyGeneral>().hit_decoy();
                    }
                    else if (Approved_targets_around[i].tag == "Enemy")
                    {
                        Approved_targets_around[i].GetComponent<EnemyConditions>().handle_effect(DOT_effect.effect_type.fire, track_buff_debuffs.skill_requested.multipliers[0], track_buff_debuffs.skill_owner);
                    }
                }
                PlayerGeneral.show_skill_casted_animation(track_buff_debuffs.skill_requested, Vector2.zero);
                break;
            case 4://Corpse Life Drain
                if (PlayerStats.CurrentHP <= 0 && track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP > 0)
                {
                    var hpdrained = PlayerStats.MaxHealth * track_buff_debuffs.skill_requested.multipliers[0] / 100;
                    if (track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP + hpdrained > track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth)
                    {
                        var hpdiff = track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth - track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP;
                        track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP = track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth;
                        track_buff_debuffs.skill_owner.GetComponent<PlayerGeneral>().showCBT(track_buff_debuffs.skill_owner, false, false, (int)hpdiff, "heal");
                    }
                    else
                    {
                        track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP += hpdrained;
                        track_buff_debuffs.skill_owner.GetComponent<PlayerGeneral>().showCBT(track_buff_debuffs.skill_owner, false, false, (int)hpdrained, "heal");
                    }

                }
                break;
            case 5://Confused (client side)
                break;
            case 6://Sleep
                PlayerMPSync.PlayerCanMove = true;
                stunned = false;
                break;
            case 7://(can't drink potions) 
                potion_blocked = false;
                break;
            case 8://(damage decrease)
                decreasedDamage = 0f;
                break;
            case 9://(defense decrease)
                decreasedDEF = 0f;
                decreasedMDEF = 0f;
                break;
            case 10://Silence 
                silence = false;
                break;
            //-----------------------V2-----------------
            case 11://Armor Crusher
                decreasedDEF = 0f;
                break;
            case 12://dissarmed
                decreasedDamage = 0;
                break;
            case 13://frozen
                PlayerMPSync.PlayerCanMove = true;
                stunned = false;
                break;
            default:
                break;
        }
        PlayerStats.ProcessStats();
        de_buffs.RemoveAt(de_buffs.IndexOf(track_buff_debuffs.buff_debuff_ID));
        RpccleanDeBuffs(track_buff_debuffs.buff_debuff_ID);
        tracker_list.Remove(track_buff_debuffs);
    }
    void clean_buffs(track_buff_debuffs track_buff_debuffs)
    {
        ////////.Log("Cleaning buff: " + conditionID);
        switch (track_buff_debuffs.buff_debuff_ID)
        {
            case 1://speedy
                increasedWalkingSpeed = 0f;
                break;
            case 2://Final Frenzy
                increasedDamage = 0f;
                break;   
            case 4://Hawkeye
                increasedCritical = 0;
                break;
            case 5://max hp               
                increasedMaxHP = 0;
                break;
            case 6://max mp               
                increasedMaxMana = 0f;
                break;
            case 7://Physical Protection               
                increasedDEF = 0;
                break;
            case 8://MAgical Protection               
                increasedMDEF = 0;
                break;
            case 9://quickshot
                increasedAtkSpeed = 1;
                break;
            case 10:
                immortal = false;
                break;
            case 11:
                increasedDodge = 0;
                break;
            case 12://ultimate defense
                increasedDEF = 0;
                break;
            case 13://manashield
                mana_shield = false;
                break;
            case 14://Camouflage
                GetComponent<NetworkProximityChecker>().forceHidden = false;
                break;
            case 15://arrow block

                break;
            case 16://shields up

                break;
            case 17://frozen hands
                increasedWalkingSpeed = 0f;
                break;
            case 19://remember me - when player dies all buffs are removed too---->deprecated
                if (PlayerStats.CurrentHP <= 0)
                {
                    PlayerGeneral.showCBT(gameObject, true, false, 0, "Remeber me!!!");
                    var party_members = PlayerGeneral.x_ObjectHelper.PartyController.getPartyMembers_go(PlayerGeneral.PartyID, gameObject, 7f);
                    for (int i = 0; i < party_members.Count; i++)
                    {
                        if (party_members[i] != null && party_members[i].GetComponent<PlayerStats>().CurrentHP > 0)
                        {
                            party_members[i].GetComponent<PlayerStats>().CurrentHP = party_members[i].GetComponent<PlayerStats>().MaxHealth;
                            PlayerGeneral.showCBT(party_members[i], true, false, Mathf.RoundToInt(party_members[i].GetComponent<PlayerStats>().MaxHealth), "heal");

                            party_members[i].GetComponent<PlayerStats>().CurrentMP = party_members[i].GetComponent<PlayerStats>().MaxMana;
                            PlayerGeneral.showCBT(party_members[i], true, false, Mathf.RoundToInt(party_members[i].GetComponent<PlayerStats>().MaxHealth), "heal_mana");
                        }
                    }
                }
                break;
            case 22://Concentration              
                concentrated = false;
                break;
            case 23:
                increasedWalkingSpeed = 0f:
                increasedAtkSpeed = 0f;
                increasedCastingSpeed = 0f;
                increasedCooldownReduction = 0f;
                break;
            default:
                break;
        }
        PlayerStats.ProcessStats();
        buffs.RemoveAt(buffs.IndexOf(track_buff_debuffs.buff_debuff_ID));
        RpccleanBuffs(track_buff_debuffs.buff_debuff_ID);
        tracker_list.Remove(track_buff_debuffs);
    }
    #endregion


    #region Effects
    /*Effects IDs
     * 9000 - efectos
     * 9010 - poison
     * 9020 - fire
     */
   
    public void handle_effect(DOT_effect.effect_type effect, float effect_power, GameObject effect_dealer, float pve_damage)
    {
        var total_time = 6f;
        var effectid = -1;
        var effect_every = 1f;
        switch (effect)
        {
            case DOT_effect.effect_type.poison:
                effectid = 9010;
                effect_every = 1f;//cada segundo se hace dano 
                total_time = 5f;
                break;
            case DOT_effect.effect_type.fire:
                effectid = 9020;
                effect_every = 1f;
                total_time = 4f;
                break;
            case DOT_effect.effect_type.bleed:
                effectid = 9030;
                effect_every = 1f;//cada segundo se hace dano 
                total_time = 4f;
                break;
            case DOT_effect.effect_type.heal:
                effectid = 9040;
                effect_every = 1f;//cada segundo 
                total_time = 4f;
                break;
            case DOT_effect.effect_type.true_damage_poison:
                effectid = 9011;
                effect_every = 1f;//cada segundo se hace dano 
                total_time = 29f;
                //effect_power in this case has a % of the maxhealth to take since its true damage, example: 2f = 2% in total_time
                break;
            case DOT_effect.effect_type.true_damage_poison_lesser:
                effectid = 9012;
                effect_every = 1f;//cada segundo se hace dano 
                total_time = 9f;
                //effect_power in this case has a % of the maxhealth to take since its true damage, example: 2f = 2% in total_time
                break;
            default:
                break;
        }
        add_effect(effectid, total_time, effect_every, effect_dealer, effect_power, pve_damage, effect);
    }
    public void add_effect(int debuffID, float total_time, float effect_every, GameObject effect_dealer, float effect_power, float pve_damage, DOT_effect.effect_type effect)
    {
        bool found = false;
        for (int i = 0; i < tracker_list.Count; i++)
        {
            if (tracker_list[i].buff_debuff_ID == debuffID)
            {
                tracker_list[i].expiresOn = Time.time + total_time;
                suffer_effect(debuffID, total_time, effect_every, effect_dealer, effect_power, pve_damage);
                found = true;
                break;
            }
        }
        if (!found)
        {
            switch (effect)
            {                
                case DOT_effect.effect_type.true_damage_poison:                    
                    tracker_list.Add(new track_buff_debuffs(debuffID, null, Time.time + total_time, effect_dealer, false, type.debuff, false));
                    break;
                default:
                    tracker_list.Add(new track_buff_debuffs(debuffID, null, Time.time + total_time, effect_dealer, false, type.debuff, true));
                    break;
            }
            
            de_buffs.Add(debuffID);
            suffer_effect(debuffID, total_time, effect_every, effect_dealer, effect_power, pve_damage);
        }
    }
    void suffer_effect(int effectID, float total_time, float effect_every, GameObject effect_dealer, float effect_power, float pve_damage)
    {
        for (int i = 0; i < effects_running.Count; i++)
        {
            if (effects_running[i].effectID == effectID)
            {
                if (effects_running[i].effect_timer != null)
                {
                    StopCoroutine(effects_running[i].effect_timer);
                    effects_running.RemoveAt(i);
                }
            }
        }
        //TEST_EMERGENCY_STOP++;
        var co_timer = StartCoroutine(start_effect_timer(effectID, total_time, effect_every, effect_dealer, effect_power, pve_damage));
        effects_running.Add(new effects_timers(co_timer, effectID));
    }
    public IEnumerator start_effect_timer(int effectID, float total_time, float effect_every, GameObject effect_dealer, float effect_power, float pve_damage)
    {
        //if effect_dealer is a player we use it, if not we use the damage of the effect        
        float total_damage_or_heal;
        int damage_or_heal_over_time;
        if (pve_damage == 0 && effect_dealer != null)
        {
            if (effectID == 9040)//heal
            {
                total_damage_or_heal = effect_dealer.GetComponent<PlayerStats>().Damage_int * effect_power;

            }else if (effectID == 9011 || effectID == 9012)//true damage
            {
                total_damage_or_heal = PlayerStats.MaxHealth * effect_power/100f;
            }
            else//damage
            {
                total_damage_or_heal = PlayerPVPDamage.CalculateSkillDamageRx(effect_dealer, effect_power);
            }

            damage_or_heal_over_time = Mathf.RoundToInt(total_damage_or_heal / (total_time / effect_every));
        }
        else
        {
            if (effectID == 9011 || effectID == 9012)//true damage
            {
                total_damage_or_heal = PlayerStats.MaxHealth * effect_power / 100f;
            }
            else
            {
                total_damage_or_heal = PlayerGeneral.x_ObjectHelper.CalculateEnemyToPlayerDamage(PlayerStats.Defense_int, pve_damage);
            }
            
            damage_or_heal_over_time = Mathf.RoundToInt(total_damage_or_heal / (total_time / effect_every));
        }

        //Debug.LogError("Damage:" + damage_or_heal_over_time + " effectID:" + effectID);

        if (PlayerStats.CurrentHP > 0)
        {
            if (effect_dealer!=null && effect_dealer.tag == "Player")
            {
                if (effectID == 9040)//heal
                {
                    PlayerStats.hpChange(damage_or_heal_over_time);

                }
                else
                {
                    PlayerPVPDamage.takeDamageinPVPNow((int)damage_or_heal_over_time, effect_dealer, false);
                }

            }
            else
            {
                PlayerGeneral.x_ObjectHelper.DamagePlayerNow(gameObject, damage_or_heal_over_time, "DOT", null);
            }

            if (effectID != 9040)//if damage
            {
                PlayerGeneral.showCBT(gameObject, false, false, damage_or_heal_over_time, "damage");
            }
            else
            {
                PlayerGeneral.showCBT(gameObject, false, false, damage_or_heal_over_time, "heal");
            }

            yield return new WaitForSeconds(effect_every);
            
                for (int i = 0; i < effects_running.Count; i++)
                {
                    if (effects_running[i].effectID == effectID)
                    {
                        /*TEST_EMERGENCY_STOP++;
                        if (TEST_EMERGENCY_STOP < 20)
                        {*/
                        effects_running[i].effect_timer = StartCoroutine(start_effect_timer(effectID, total_time, effect_every, effect_dealer, effect_power, pve_damage));
                        break;
                        //}

                    }
                }
            
        }

    }
    public int cleanDebuffs(int amount)
    {
        int confirmed_cleaned = 0;
        List<track_buff_debuffs> debuffs_selected = new List<track_buff_debuffs>();
        for (int i = 0; i < amount; i++)
        {
            for (int d = 0; d < tracker_list.Count; d++)
            {
                if (tracker_list[d].type == type.debuff)
                {
                    debuffs_selected.Add(tracker_list[d]);
                    confirmed_cleaned++;
                    break;
                }

            }
        }
        for (int i = 0; i < debuffs_selected.Count; i++)
        {
            tracker_list[tracker_list.IndexOf(debuffs_selected[i])].expiresOn = 0;
        }
        return confirmed_cleaned;
    }
    #endregion

    #region Effectos AOE

    #endregion

    #region Reset
    public void reset_buffs_or_debuffs(type buff_or_debuff)
    {
        List<track_buff_debuffs> to_remove_list = new List<track_buff_debuffs>();
        for (int i = 0; i < tracker_list.Count; i++)
        {
            if (buff_or_debuff == tracker_list[i].type)
            {
                to_remove_list.Add(tracker_list[i]);
            }
        }
        for (int i = 0; i < to_remove_list.Count; i++)
        {
            tracker_list[tracker_list.IndexOf(to_remove_list[i])].expiresOn = 0;
        }
    }
    public void clean_buff_debuff(type buff_or_debuff, int ID)
    {
        List<track_buff_debuffs> to_be_cleaned = new List<track_buff_debuffs>();
        for (int i = 0; i < tracker_list.Count; i++)
        {

            if (tracker_list[i].type == buff_or_debuff && tracker_list[i].buff_debuff_ID == ID)
            {
                to_be_cleaned.Add(tracker_list[i]);
            }


        }
        for (int i = 0; i < to_be_cleaned.Count; i++)
        {
            tracker_list[tracker_list.IndexOf(to_be_cleaned[i])].expiresOn = 0;
        }
    }
    #endregion

    #region Util
    public bool has_buff_debuff(type type, int buffID)
    {
        switch (type)
        {
            case type.buff:
                if (buffs.Contains(buffID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case type.debuff:
                if (de_buffs.Contains(buffID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
    public bool remove_buff_debuff(type type, int buffID)
    {
        for (int d = 0; d < tracker_list.Count; d++)
        {
            if (tracker_list[d].type == type && tracker_list[d].buff_debuff_ID == buffID)
            {
                tracker_list[d].expiresOn = 0;
                return true;
            }

        }
        return false;
    }
    public track_buff_debuffs get_buff_information(type type, int buffID)
    {
        for (int i = 0; i < tracker_list.Count; i++)
        {
            if (tracker_list[i].type == type && tracker_list[i].buff_debuff_ID == buffID)
            {
                return tracker_list[i];
            }
        }
        return null;
    }
    #endregion
}
