using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Pathfinding;

public class EnemyConditions : NetworkBehaviour
{

    #region Networking
    public SyncListInt buffs = new SyncListInt();
    public SyncListInt de_buffs = new SyncListInt();
    #endregion
    #region Enemy
    public EnemyControllerAI EnemyControllerAI;
    EnemyStats EnemyStats;
    EnemyTakeDamage EnemyTakeDamage;
    EnemySpawnInfo EnemySpawnInfo;
    EnemyAttack EnemyAttack;
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
    #region Conditions Data
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

        public track_buff_debuffs(int buff_debuff_ID, skill parent_skill, float expiresOn, GameObject skill_owner, bool canStack, type type, bool can_be_cleaned)
        {
            this.buff_debuff_ID = buff_debuff_ID;
            this.skill_requested = parent_skill;
            this.expiresOn = expiresOn;
            this.skill_owner = skill_owner;
            this.canStack = canStack;
            this.type = type;
            this.can_be_cleaned = can_be_cleaned;
        }
    }
    public List<track_buff_debuffs> tracker_list = new List<track_buff_debuffs>();
    //conditions
    public bool slowed = false;
    public bool stunned = false;
    public bool speedy = false;
    public bool concentrated = false;
    public bool immortal = false;
    public bool mana_shield = false;
    public bool reflect = false;
    public bool converting_dmg_to_hp = false;
    public bool silence = false;
    //buffs  
    //debuffs
    //debuffs
   
    public float decreasedDEF;
   
    public float decreasedDamage;
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
    public List<effects_timers> effects_running = new List<effects_timers>();
    //public int TEST_EMERGENCY_STOP;
    #endregion

    private void Awake()
    {

        EnemyStats = GetComponent<EnemyStats>();
        EnemyControllerAI = GetComponent<EnemyControllerAI>();
        EnemyTakeDamage = GetComponent<EnemyTakeDamage>();
        EnemySpawnInfo = GetComponent<EnemySpawnInfo>();
        EnemyAttack = GetComponent<EnemyAttack>();
        ////Debug.LogError("Spawn-3213213");
        //random buff

    }
    private void Start()
    {
        StartCoroutine(checkBuffTimers());
        ////Debug.LogError("Spawn-00000000");

        if (EnemyStats.Level > 10)
        {
            float buff_chance = 10f;
            if ((int)EnemyAttack.EnemyBehaviour > 0)//if mob is anything but a normal one increase chance of buff
            {
                buff_chance = 40f;
            }
            applyRandomBuff(buff_chance);
            if (Random.Range(1f, 100f) <= 5f)//5% chance to have double buff
            {
                applyRandomBuff(buff_chance);
            }
        }
    }

    #region Utilidades
    public buff_debuff_data find_debuff(skill skillRequested)
    {
        var debuff_data = new buff_debuff_data();
        debuff_data.buff_debuff_ID = new List<int>();
        debuff_data.time = 2f;
        var chance = 0f;
        switch (skillRequested.SkillID)
        {
            #region Warrior
            case 61003://shield stun
                chance = 51;
                if (Random.Range(1, 100) <= chance)
                {
                    debuff_data.buff_debuff_ID.Add(1);
                    stunned = true;
                    EnemyControllerAI.canMove = false;
                }
                break;
            case 61024://Armor Crusher
                chance = skillRequested.multipliers[1];
                if (Random.Range(1, 100) <= chance)
                {
                    
                    debuff_data.buff_debuff_ID.Add(11);
                    debuff_data.time = skillRequested.multipliers[2];
                    EnemyStats.temp_def = EnemyStats.Defense_str;
                    decreasedDEF = skillRequested.multipliers[0];

                }               
                break;
            case 61025://dismember                
                if (has_buff_debuff(type.debuff, 11))
                {
                    //remove armor crusher debuff and add dissarmed debuff
                    remove_buff_debuff(type.debuff, 11);
                    decreasedDamage = skillRequested.multipliers[2]; //usually a big negative number like -90
                    debuff_data.buff_debuff_ID.Add(12);
                    debuff_data.time = skillRequested.multipliers[1];
                    EnemyStats.temp_dmg_int = EnemyStats.Damage_int;
                    EnemyStats.temp_dmg_str = EnemyStats.Damage_str;                   
                }
                break;
            case 61026://slow down
                debuff_data.buff_debuff_ID.Add(2);
                slowed = true;
                EnemyControllerAI.maxSpeed *= (1f-(skillRequested.multipliers[0] / 100f));
                break;
            case 61027://on your kneess
                if (EnemyStats.CurrentHP / EnemyStats.MaxHP <= (skillRequested.multipliers[1]/100f))
                {
                    chance = (int)skillRequested.multipliers[0];
                    if (Random.Range(1, 100) <= chance)
                    {
                        debuff_data.buff_debuff_ID.Add(1);
                        stunned = true;
                        EnemyControllerAI.canMove = false;
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
                    EnemyControllerAI.canMove = false;
                }
                break;
            case 62009://blizzard
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(13);
                    stunned = true;
                    EnemyControllerAI.canMove = false;
                }
                else if (Random.Range(1, 100) <= skillRequested.multipliers[2])
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    slowed = true;
                    EnemyControllerAI.maxSpeed *= 0.5f;
                }
                break;
            case 62010://frostbomb
                if (Random.Range(1, 100) <= skillRequested.multipliers[1])
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    slowed = true;
                    EnemyControllerAI.maxSpeed *= 0.5f;
                }
                break;
            case 62011://Corpse Life Drain
                debuff_data.buff_debuff_ID.Add(4);
                debuff_data.time = skillRequested.multipliers[1];
                break;
            #endregion
            #region Hunter
            case 63005://Hamstring Shot
                chance = 60;
                if (Random.Range(1, 100) <= chance)
                {
                    debuff_data.buff_debuff_ID.Add(2);
                    slowed = true;
                    EnemyControllerAI.maxSpeed = EnemyControllerAI.maxSpeed * 0.5f;
                }
                break;
            case 63011:   //hunters mark             
                    debuff_data.buff_debuff_ID.Add(14);
                    debuff_data.time = 10f;
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

            /*case 1040://Fierce Blow               
                debuff_data.buff_debuff_ID.Add(2);
                slowed = true;
                EnemyControllerAI.maxSpeed = EnemyControllerAI.maxSpeed * 0.8f;
                break;
            case 2040://Explosive Arrow
                debuff_data.buff_debuff_ID.Add(2);
                debuff_data.buff_debuff_ID.Add(3);
                slowed = true;
                EnemyControllerAI.maxSpeed = EnemyControllerAI.maxSpeed * 0.5f;
                break;*/

            //--------------------------V2----------------------



            #region Wizard

            #endregion
            default:
                break;
        }

        if (debuff_data.buff_debuff_ID.Count > 0)
        {
            EnemyStats.ProcessStats();
        }
        return debuff_data;
    }
    void applyRandomBuff(float buff_chance)
    {
        ////Debug.LogError("RandomBuff");
        if (Random.Range(1f, 100f) <= buff_chance)//10% chance of getting a buffed mob with better drop rate
        {
            ////Debug.LogError("RandomBuff YAS");
            int winner_buff = Random.Range(1, 11);
            if (winner_buff == 10)//we dont want immortal mobs here
            {
                winner_buff = 11;
            }
            buffs.Add(winner_buff);
            tracker_list.Add(new track_buff_debuffs(winner_buff, null, Time.time+ 3600f, null, false, type.buff, true));
            EnemySpawnInfo.LootExtraChance = 5f;
            switch (winner_buff)
            {
                case 1://speedy
                    speedy = true;
                    EnemyControllerAI.maxSpeed *= 1.3f;
                    break;
                case 2://Final Frenzy
                    EnemyStats.Damage_int = Mathf.RoundToInt(EnemyStats.Damage_int * 1.3f);
                    EnemyStats.Damage_str = Mathf.RoundToInt(EnemyStats.Damage_str * 1.3f);
                    break;
                case 3://Concentration
                    EnemyStats.Damage_int = Mathf.RoundToInt(EnemyStats.Damage_int * 1.2f);
                    EnemyStats.Damage_str = Mathf.RoundToInt(EnemyStats.Damage_str * 1.2f);
                    break;
                case 4://Hawkeye
                    EnemyStats.Critical_percent_agi = 50f;
                    break;
                case 5://extra hp            
                    EnemyStats.MaxHP *= 2;
                    EnemyStats.CurrentHP = EnemyStats.MaxHP;
                    break;
                case 6://extra hp regen
                    EnemyStats.HP_regen = 0.05f;
                    break;
                case 7://Arcane Protection               
                    EnemyStats.Defense_str = Mathf.RoundToInt(EnemyStats.Defense_str * 1.5f);
                    break;
                case 8://Arcane Protection               
                    EnemyStats.Defense_int = Mathf.RoundToInt(EnemyStats.Defense_int * 1.5f);
                    break;
                case 9://quickshot
                    EnemyStats.AttackSpeed *= 0.5f;
                    break;
                case 10:
                    immortal = false;
                    break;
                case 11:
                    EnemyStats.Dodge_percent_dex = 50f;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region Conditions related
    public void handle_buffs_debuffs(skill skill, GameObject Player_executing_skill)
    {
        var debuffs = find_debuff(skill);
        bool stackable = false;//deprecated and always false
        bool can_be_cleaned = true;//deprecated and always false;
        if (debuffs != null && debuffs.buff_debuff_ID!=null)
        {
            for (int i = 0; i < debuffs.buff_debuff_ID.Count; i++)
            {
                add_buff_debuff(debuffs.buff_debuff_ID[i], skill, stackable, debuffs.time, Player_executing_skill, type.debuff, can_be_cleaned);
            }
        }
    }
    public void add_buff_debuff(int buff_debuff_ID, skill skill, bool stackable, float time, GameObject Player_executing_skill, type type, bool can_be_cleaned)
    {
        //buffs
        switch (type)
        {
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
                    if (tracker_list[i].type == type.debuff)
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
                    else
                    {
                        clean_buffs(tracker_list[i]);
                    }

                }
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(checkBuffTimers());

    }
    public void clean_buffs(track_buff_debuffs track_buff_debuffs)
    {
        try
        {
            switch (track_buff_debuffs.buff_debuff_ID)
            {
                case 1://speedy
                    speedy = false;
                    EnemyControllerAI.maxSpeed *= 0.8f;
                    break;
                case 2://Final Frenzy
                    EnemyStats.Damage_int = Mathf.RoundToInt(EnemyStats.Damage_int * 0.8f);
                    EnemyStats.Damage_str = Mathf.RoundToInt(EnemyStats.Damage_str * 0.8f);
                    break;
                case 3://Concentration
                    EnemyStats.Damage_int = Mathf.RoundToInt(EnemyStats.Damage_int * 0.8f);
                    EnemyStats.Damage_str = Mathf.RoundToInt(EnemyStats.Damage_str * 0.8f);
                    break;
                case 4://Hawkeye
                    EnemyStats.Critical_percent_agi = 10f;
                    break;
                case 5://extra hp            
                    EnemyStats.MaxHP = EnemyStats.MaxHP/2;
                    if (EnemyStats.CurrentHP > EnemyStats.MaxHP)
                    {
                        EnemyStats.CurrentHP = EnemyStats.MaxHP;
                    }                    
                    break;
                case 6://extra hp regen
                    EnemyStats.HP_regen = 0.02f;
                    break;
                case 7://Arcane Protection               
                    EnemyStats.Defense_str = Mathf.RoundToInt(EnemyStats.Defense_str * 0.8f);
                    break;
                case 8://Arcane Protection               
                    EnemyStats.Defense_int = Mathf.RoundToInt(EnemyStats.Defense_int * 0.8f);
                    break;
                case 9://quickshot
                    EnemyStats.AttackSpeed *= 2f;
                    break;
                case 10:
                    immortal = false;
                    break;
                case 11:
                    EnemyStats.Dodge_percent_dex = 5f;
                    break;
                case 12://reflect
                    reflect = false;
                    break;
                case 13:
                    converting_dmg_to_hp = false;
                    break;
                default:
                    break;
            }
            buffs.RemoveAt(buffs.IndexOf(track_buff_debuffs.buff_debuff_ID));
            tracker_list.Remove(track_buff_debuffs);
        }
        catch (System.Exception)
        {
            return;
        }

    }
    public void clean_debuffs(track_buff_debuffs track_buff_debuffs)
    {
        try
        {
            ////////.Log("Cleaning debuff: " + conditionID);
            switch (track_buff_debuffs.buff_debuff_ID)
            {
                case 1://stun              
                    stunned = false;
                    EnemyControllerAI.canMove = true;
                    break;
                case 2://slow              
                    slowed = false;
                    EnemyControllerAI.maxSpeed = EnemyStats.WalkingSpeed;
                    break;
                case 3://Explosive Arrow
                       //explotar bomba
                    var Approved_targets_around = EnemySpawnInfo.x_ObjectHelper.get_AOE_LOS_targets(gameObject, 1.5f, LayerMask.GetMask("Player", "Enemy", "Coliders", "decoy"), true);
                    for (int i = 0; i < Approved_targets_around.Count; i++)
                    {
                        if (Approved_targets_around[i].tag == "Player")
                        {
                            //Approved_targets_around[i].GetComponent<PlayerConditions>().handle_effect(DOT_effect.dot_type.fire, track_buff_debuffs.parent_skill.dmgModifier, track_buff_debuffs.skill_owner,0);
                        }
                        /*else if (Approved_targets_around[i].tag == "decoy")
                        {
                            //Approved_targets_around[i].GetComponent<DecoyGeneral>().hit_decoy();
                        }*/
                        else if (Approved_targets_around[i].tag == "Enemy")
                        {
                            Approved_targets_around[i].GetComponent<EnemyConditions>().handle_effect(DOT_effect.effect_type.fire, track_buff_debuffs.skill_requested.multipliers[0], track_buff_debuffs.skill_owner);
                        }
                    }
                    track_buff_debuffs.skill_owner.GetComponent<PlayerGeneral>().show_skill_casted_animation(track_buff_debuffs.skill_requested, transform.position);
                    break;
                case 4://Corpse Life Drain
                    if (EnemyStats.CurrentHP <= 0 && track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP > 0)
                    {
                        var hpdrained = EnemyStats.MaxHP * track_buff_debuffs.skill_requested.multipliers[0] / 100f;
                        if (track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP + hpdrained > track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth)
                        {
                            var hpdiff = track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth - track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP;
                            track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP = track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().MaxHealth;
                            track_buff_debuffs.skill_owner.GetComponent<PlayerGeneral>().showCBT(track_buff_debuffs.skill_owner, false, false, (int)hpdiff, "heal");
                            //////.Log("Healed");
                        }
                        else
                        {
                            //////.Log("Healed");
                            track_buff_debuffs.skill_owner.GetComponent<PlayerStats>().CurrentHP += hpdrained;
                            track_buff_debuffs.skill_owner.GetComponent<PlayerGeneral>().showCBT(track_buff_debuffs.skill_owner, false, false, (int)hpdrained, "heal");
                        }
                    }
                    break;
                //-----------------------V2-----------------
                case 10://silence
                    silence = false;
                    break;
                case 11://Armor Crusher
                    decreasedDEF = 0f;
                    EnemyStats.Defense_str = (int)EnemyStats.temp_def;
                    EnemyStats.temp_def = 0;

                    break;
                case 12://dissarmed
                    decreasedDamage = 0;
                    EnemyStats.Damage_int = (int)EnemyStats.temp_dmg_int;
                    EnemyStats.Damage_str = (int)EnemyStats.temp_dmg_str;
                    EnemyStats.temp_dmg_int = 0;
                    EnemyStats.temp_dmg_str = 0;
                    break;
                case 13:
                    stunned = false;
                    EnemyControllerAI.canMove = true;
                    break;
                default:
                    break;

            }
            de_buffs.RemoveAt(de_buffs.IndexOf(track_buff_debuffs.buff_debuff_ID));
            tracker_list.Remove(track_buff_debuffs);
        }
        catch (System.Exception)
        {
            return;
        }

    }
    #endregion


    #region Effects
    /*Effects IDs
     * 9000 - efectos
     * 9010 - poison
     * 9020 - fire
     */
    public void handle_effect(DOT_effect.effect_type effect, float effect_power, GameObject effect_dealer)
    {
        var total_time = 8f;
        var effectid = -1;
        var effect_every = 1f;
        switch (effect)
        {
            case DOT_effect.effect_type.poison:
                effectid = 9010;
                effect_every = 1f;//cada segundo se hace dano 
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
            default:
                break;
        }
        add_effect(effectid, total_time, effect_every, effect_dealer, effect_power);
    }
    public void add_effect(int debuffID, float total_time, float effect_every, GameObject effect_dealer, float effect_power)
    {
        bool found = false;
        for (int i = 0; i < tracker_list.Count; i++)
        {
            if (tracker_list[i].buff_debuff_ID == debuffID)
            {
                tracker_list[i].expiresOn = Time.time + total_time;
                suffer_effect(debuffID, total_time, effect_every, effect_dealer, effect_power);
                found = true;
                break;
            }
        }
        if (!found)
        {
            tracker_list.Add(new track_buff_debuffs(debuffID, null, Time.time + total_time, effect_dealer, false, type.debuff, true));
            de_buffs.Add(debuffID);
            suffer_effect(debuffID, total_time, effect_every, effect_dealer, effect_power);
        }
    }
    void suffer_effect(int effectID, float total_time, float effect_every, GameObject effect_dealer, float effect_power)
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
        var co_timer = StartCoroutine(start_effect_timer(effectID, total_time, effect_every, effect_dealer, effect_power));
        effects_running.Add(new effects_timers(co_timer, effectID));
    }
    public IEnumerator start_effect_timer(int effectID, float total_time, float effect_every, GameObject effect_dealer, float effect_power)
    {
        var hp_modifier = -(int)EnemyTakeDamage.CalculateSkillDamageRx(effect_dealer, effect_power);
        var hpaffected = hp_modifier / (total_time / effect_every);
        if (EnemyStats.CurrentHP > 0)
        {
            hpaffected=EnemyTakeDamage.CentralEnemyTakeDamage((int)hpaffected * -1, effect_dealer);
            effect_dealer.GetComponent<PlayerGeneral>().showCBT(gameObject, false, false, (int)hpaffected * -1, "damage");
            yield return new WaitForSeconds(effect_every);
            for (int i = 0; i < effects_running.Count; i++)
            {
                if (effects_running[i].effectID == effectID)
                {
                    /*TEST_EMERGENCY_STOP++;
                    if (TEST_EMERGENCY_STOP < 20)
                    {*/
                    effects_running[i].effect_timer = StartCoroutine(start_effect_timer(effectID, total_time, effect_every, effect_dealer, effect_power));
                    break;
                    //}

                }
            }

        }
        else
        {
            EnemyTakeDamage.dieNow();
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
            clean_debuffs(debuffs_selected[i]);
            //tracker_list[tracker_list.IndexOf(debuffs_selected[i])].expiresOn = 0;
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
            if (buff_or_debuff == tracker_list[i].type && tracker_list[i].can_be_cleaned)
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
        List<track_buff_debuffs> debuffs_selected = new List<track_buff_debuffs>();
        for (int i = 0; i < tracker_list.Count; i++)
        {
            for (int d = 0; d < tracker_list.Count; d++)
            {
                if (tracker_list[i].type == buff_or_debuff && tracker_list[i].buff_debuff_ID == ID && tracker_list[i].can_be_cleaned)
                {
                    debuffs_selected.Add(tracker_list[d]);
                }

            }
        }
        for (int i = 0; i < debuffs_selected.Count; i++)
        {
            clean_debuffs(debuffs_selected[i]);
            //tracker_list[tracker_list.IndexOf(to_be_cleaned[i])].expiresOn = 0;
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
