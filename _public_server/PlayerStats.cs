using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityWebRequest = UnityEngine.Networking.UnityWebRequest;

public class PlayerStats : NetworkBehaviour
{
    //used to refresh player stats manually on server editor
    public bool update_now;
    /* private void Update()
     {
         if (update_now)
         {
             RefreshStats();
             update_now = false;
             //Debug.LogWarning("Stats Updated");
         }
     }*/
    //cache these variables to send to detect changes and send to client
    public float[] stat_hash = new float[60];//we only sync 16 variables now, if more increase this
    byte[] temp_hash;

    //**********************Dealing with stats here    
    #region rest
    public bool in_combat;
    float combat_cd = 5f;
    Coroutine temp_in_combat;
    #endregion
    #region Stat training
    public int[] stat_training_exp;
    public int[] stat_training_levels;
    int[] stat_training_levels_temp = new int[8];//temp to know when we leveled up
    int[] temp_train_exp = new int[8];
    #endregion
    #region enchants
    public List<enchant.enchant_base> enchants_affecting = new List<enchant.enchant_base>();
    //Lazuli
    public float ench_extra_hp_from_pots;
    public float ench_extra_hp_regen_while_stationary;
    //Jade
    public float ench_extra_mp_from_pots;
    public float ench_extra_mp_regen_while_stationary;
    //Saphire
    public float ench_free_hp_potion_use_on_kill;//heal hp equivalent to equipped hp potion on hit
    public float ench_chance_to_get_free_mphp_potion_charge;
    public float ench_speed_penalty;
    //Siderite
    public float ench_free_mp_potion_use_on_kill;//heal mp equivalent to equipped mp potion on hit
    public float ench_chance_to_get_hpandmp_on_kill;
    public bool ench_potion_block;
    //Gypsum
    public float ench_chance_to_explode_deads;
    public float ench_chance_to_free_cast;
    public float ench_chance_to_fail_casting;

    #endregion
    public PlayerClass PlayerClass_now;
    public DamageType DamageType_now;
    
    //Generic base stats declaration

    public float Base_HP = 50;
    public float HP_per_STA = 5;
    public float HP_per_level = 5;
    public float HP_regen_time = 5f;
    public float HP_regen_percent = 1;

    public float Base_MP = 10;
    public float MP_per_WIS = 1;
    public float MP_per_level = 5;
    public float MP_regen_time = 1f;
    public float MP_regen_percent = 1;

    public float Damage_str = 0;
    public float Damage_int = 0;

    public float Defense_str = 0;
    public float Defense_int = 0;
    
    public float Defense_from_pdef = 0; //JWR - Added label for pdef from stats
    public float Defense_from_mdef = 0; //JWR - Added label for mdef from stats

    public float Critical_chance = 0;
    public float Critical_damage = 0;
    public float CC_soft_cap = 25f; //JWR - Added soft cap label for easier mod
    public float CC_hard_cap = 50f; //JWR - Added hard cap label for easier mod

    public float Dodge_chance = 0;
    public float Dodge_soft_cap = 25f; //JWR - Added soft cap label for easier mod
    public float Dodge_hard_cap = 50f; //JWR - Added hard cap label for easier mod

    public float AutoAtk_speed = 1f;
    public float AutoAtk_range = 1f;

    public float Walking_spd = 1f;

    public float Skill_range = 1f;
    public float Skill_mana_usage = 1f;//not used yet

    public float Casting_speed_reduction = 0f;//negative to reduce time

    public float PlayerDropChance;
    public float ExtraGoldDrop;
    public float ExtraExp;
    public float CD_reduction;

    public class PlayerSharedStats
    {
        public static float MAX_Base_HP = 150f;
        public static float MAX_HP_per_Sta = 15f;
        public static float MAX_HP_per_level = 5f;
        public static float MAX_Base_MP = MAX_Base_HP * 0.6f;
        public static float MAX_MP_per_Wis = MAX_HP_per_Sta * 0.6f;
        public static float MAX_MP_per_level = MAX_HP_per_level * 0.6f;
        public static float MAX_Defense = 4f;
        public static float MAX_Crit_Dodge = 0.12f;
        public static float MAX_Damage = MAX_Defense * 0.8f;
    }

    public enum PlayerClass
    {
        Any,
        Hunter,
        Wizard,
        Paladin,
        Warrior
    }
    public enum DamageType
    {
        magical,
        physical
    }

    #region Base class multipliers
    public class WarriorMultiplier
    {
        public static float HP_Multiplier = 1f;
        public static float MP_Multiplier = 0.6f;
        public static float PDEF_Multipplier = 1f;
        public static float MDEF_Multiplier = 0.6f;
        public static float Crit_Dodge_Multiplier = 0.6f;
        public static float DMG_Multiplier = 1f;

        public static float Base_HP = PlayerSharedStats.MAX_Base_HP * HP_Multiplier;
        public static float HP_per_STA = PlayerSharedStats.MAX_HP_per_Sta * HP_Multiplier;
        public static float HP_per_level = PlayerSharedStats.MAX_HP_per_level * HP_Multiplier;
        public static float HP_regen_time = 5f; //this is in seconds and every HP_regen_time it will regen hp
        public static float HP_regen_percent = 0.5f; //on % so 0.5%

        public static float Base_MP = PlayerSharedStats.MAX_Base_MP * MP_Multiplier;
        public static float MP_per_WIS = PlayerSharedStats.MAX_MP_per_Wis * MP_Multiplier;
        public static float MP_per_level = PlayerSharedStats.MAX_MP_per_level * MP_Multiplier;
        public static float MP_regen_time = 5f;
        public static float MP_regen_percent = 0.5f; //on % so 0.5%

        public static float Damage_per_str = PlayerSharedStats.MAX_Damage * DMG_Multiplier;
        public static float Damage_per_int = 1.0f;

        public static float Defense_per_str = PlayerSharedStats.MAX_Defense * PDEF_Multipplier;
        public static float Defense_per_int = PlayerSharedStats.MAX_Defense * MDEF_Multiplier;

        public static float Critical_chance_per_DEX = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;
        public static float Critical_damage_percent_base = 0;

        public static float Dodge_chance_per_AGI = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;

        public static float AutoAtk_speed = 0.95f;
        public static float AutoAtk_range = 1f;

        public static float Walking_spd = 1.1f;

        public static float Skill_range = 1f;
        public static float Skill_mana_usage = 1f; //not used yet

        public static float Casting_speed_reduction = 0;
    }
    public class PaladinMultiplier
    {
        public static float HP_Multiplier = 0.9f;
        public static float MP_Multiplier = 0.9f;
        public static float PDEF_Multipplier = 0.9f;
        public static float MDEF_Multiplier = 0.9f;
        public static float Crit_Dodge_Multiplier = 0.75f;
        public static float DMG_Multiplier = 0.9f;

        public static float Base_HP = PlayerSharedStats.MAX_Base_HP * HP_Multiplier;
        public static float HP_per_STA = PlayerSharedStats.MAX_HP_per_Sta * HP_Multiplier;
        public static float HP_per_level = PlayerSharedStats.MAX_HP_per_level * HP_Multiplier;
        public static float HP_regen_time = 5f;
        public static float HP_regen_percent = 0.5f;

        public static float Base_MP = PlayerSharedStats.MAX_Base_MP * MP_Multiplier;
        public static float MP_per_WIS = PlayerSharedStats.MAX_MP_per_Wis * MP_Multiplier;
        public static float MP_per_level = PlayerSharedStats.MAX_MP_per_level * MP_Multiplier;
        public static float MP_regen_time = 5f;
        public static float MP_regen_percent = 0.5f;

        public static float Damage_per_str = 1.0f;
        public static float Damage_per_int = PlayerSharedStats.MAX_Damage * DMG_Multiplier;

        public static float Defense_per_str = PlayerSharedStats.MAX_Defense * PDEF_Multipplier;
        public static float Defense_per_int = PlayerSharedStats.MAX_Defense * MDEF_Multiplier;

        public static float Critical_chance_per_DEX = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;
        public static float Critical_damage_percent_base=0;

        public static float Dodge_chance_per_AGI = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;

        public static float AutoAtk_speed = 0.9f;
        public static float AutoAtk_range = 1f;

        public static float Walking_spd = 1.1f;

        public static float Skill_range = 1f;
        public static float Skill_mana_usage = 1f; //not used yet

        public static float Casting_speed_reduction = 0;
    }
    public class HunterMultiplier
    {
        public static float HP_Multiplier = 0.75f;
        public static float MP_Multiplier = 0.75f;
        public static float PDEF_Multipplier = 0.75f;
        public static float MDEF_Multiplier = 0.75f;
        public static float Crit_Dodge_Multiplier = 1f;
        public static float DMG_Multiplier = 0.9f;

        public static float Base_HP = PlayerSharedStats.MAX_Base_HP * HP_Multiplier;
        public static float HP_per_STA = PlayerSharedStats.MAX_HP_per_Sta * HP_Multiplier;
        public static float HP_per_level = PlayerSharedStats.MAX_HP_per_level * HP_Multiplier;
        public static float HP_regen_time = 5f;
        public static float HP_regen_percent = 0.5f;

        public static float Base_MP = PlayerSharedStats.MAX_Base_MP * MP_Multiplier;
        public static float MP_per_WIS = PlayerSharedStats.MAX_MP_per_Wis * MP_Multiplier;
        public static float MP_per_level = PlayerSharedStats.MAX_MP_per_level * MP_Multiplier;
        public static float MP_regen_time = 5f;
        public static float MP_regen_percent = 0.5f;

        public static float Damage_per_str = PlayerSharedStats.MAX_Damage * DMG_Multiplier;
        public static float Damage_per_int = 1.0f;

        public static float Defense_per_str = PlayerSharedStats.MAX_Defense * PDEF_Multipplier;
        public static float Defense_per_int = PlayerSharedStats.MAX_Defense * MDEF_Multiplier;

        public static float Critical_chance_per_DEX = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;
        public static float Critical_damage_percent_base = 0;

        public static float Dodge_chance_per_AGI = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;

        public static float AutoAtk_speed = 0.8f;
        public static float AutoAtk_range = 3f;

        public static float Walking_spd = 1.2f;

        public static float Skill_range = 3f;
        public static float Skill_mana_usage = 1f; //not used yet

        public static float Casting_speed_reduction = 0;
    }
    public class WizardMultiplier
    {
        public static float HP_Multiplier = 0.6f;
        public static float MP_Multiplier = 1f;
        public static float PDEF_Multipplier = 0.6f;
        public static float MDEF_Multiplier = 1f;
        public static float Crit_Dodge_Multiplier = 0.9f;
        public static float DMG_Multiplier = 1f;

        public static float Base_HP = PlayerSharedStats.MAX_Base_HP * HP_Multiplier;
        public static float HP_per_STA = PlayerSharedStats.MAX_HP_per_Sta * HP_Multiplier;
        public static float HP_per_level = PlayerSharedStats.MAX_HP_per_level * HP_Multiplier;
        public static float HP_regen_time = 5f;
        public static float HP_regen_percent = 0.5f;

        public static float Base_MP = PlayerSharedStats.MAX_Base_MP * MP_Multiplier;
        public static float MP_per_WIS = PlayerSharedStats.MAX_MP_per_Wis * MP_Multiplier;
        public static float MP_per_level = PlayerSharedStats.MAX_MP_per_level * MP_Multiplier;
        public static float MP_regen_time = 5f;
        public static float MP_regen_percent = 0.5f;

        public static float Damage_per_str = 1.0f;
        public static float Damage_per_int = PlayerSharedStats.MAX_Damage * DMG_Multiplier;

        public static float Defense_per_str = PlayerSharedStats.MAX_Defense * PDEF_Multipplier;
        public static float Defense_per_int = PlayerSharedStats.MAX_Defense * MDEF_Multiplier;

        public static float Critical_chance_per_DEX = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;
        public static float Critical_damage_percent_base = 0;
        
        public static float Dodge_chance_per_AGI = PlayerSharedStats.MAX_Crit_Dodge * Crit_Dodge_Multiplier;

        public static float AutoAtk_speed = 1f;
        public static float AutoAtk_range = 2.7f;

        public static float Walking_spd = 1.15f;

        public static float Skill_range = 2.7f; //not used yet
        public static float Skill_mana_usage = 1f; //not used yet

        public static float Casting_speed_reduction = 0;
    }
    #endregion

    public void RefreshStats()//like this to avoid GC by assigning a new() on each call
    {
        switch (PlayerClass_now)
        {
            case PlayerClass.Hunter:
                DamageType_now = DamageType.physical;
                Base_HP = HunterMultiplier.Base_HP;
                HP_per_STA = HunterMultiplier.HP_per_STA;
                HP_per_level = HunterMultiplier.HP_per_level;
                HP_regen_time = HunterMultiplier.HP_regen_time;
                HP_regen_percent = HunterMultiplier.HP_regen_percent;
                Base_MP = HunterMultiplier.Base_MP;
                MP_per_WIS = HunterMultiplier.MP_per_WIS;
                MP_per_level = HunterMultiplier.MP_per_level;
                MP_regen_time = HunterMultiplier.MP_regen_time;
                MP_regen_percent = HunterMultiplier.MP_regen_percent;
                Damage_str = HunterMultiplier.Damage_per_str;
                Damage_int = HunterMultiplier.Damage_per_int;
                Defense_str = HunterMultiplier.Defense_per_str;
                Defense_int = HunterMultiplier.Defense_per_int;
                Critical_chance = HunterMultiplier.Critical_chance_per_DEX;
                Critical_damage = HunterMultiplier.Critical_damage_percent_base;
                Dodge_chance = HunterMultiplier.Dodge_chance_per_AGI;
                AutoAtk_speed = HunterMultiplier.AutoAtk_speed;
                AutoAtk_range = HunterMultiplier.AutoAtk_range;
                Walking_spd = HunterMultiplier.Walking_spd;
                Skill_range = HunterMultiplier.Skill_range;//not used yet
                Skill_mana_usage = HunterMultiplier.Skill_mana_usage;//not used yet
                Casting_speed_reduction = HunterMultiplier.Casting_speed_reduction;//not used yet
                break;
            case PlayerClass.Wizard:
                DamageType_now = DamageType.magical;
                Base_HP = WizardMultiplier.Base_HP;
                HP_per_STA = WizardMultiplier.HP_per_STA;
                HP_per_level = WizardMultiplier.HP_per_level;
                HP_regen_time = WizardMultiplier.HP_regen_time;
                HP_regen_percent = WizardMultiplier.HP_regen_percent;
                Base_MP = WizardMultiplier.Base_MP;
                MP_per_WIS = WizardMultiplier.MP_per_WIS;
                MP_per_level = WizardMultiplier.MP_per_level;
                MP_regen_time = WizardMultiplier.MP_regen_time;
                MP_regen_percent = WizardMultiplier.MP_regen_percent;
                Damage_str = WizardMultiplier.Damage_per_str;
                Damage_int = WizardMultiplier.Damage_per_int;
                Defense_str = WizardMultiplier.Defense_per_str;
                Defense_int = WizardMultiplier.Defense_per_int;
                Critical_chance = WizardMultiplier.Critical_chance_per_DEX;
                Critical_damage = WizardMultiplier.Critical_damage_percent_base;
                Dodge_chance = WizardMultiplier.Dodge_chance_per_AGI;
                AutoAtk_speed = WizardMultiplier.AutoAtk_speed;
                AutoAtk_range = WizardMultiplier.AutoAtk_range;
                Walking_spd = WizardMultiplier.Walking_spd;
                Skill_range = WizardMultiplier.Skill_range;//not used yet
                Skill_mana_usage = WizardMultiplier.Skill_mana_usage;//not used yet
                Casting_speed_reduction = WizardMultiplier.Casting_speed_reduction;//not used yet
                break;
            case PlayerClass.Paladin:
                DamageType_now = DamageType.magical;
                Base_HP = PaladinMultiplier.Base_HP;
                HP_per_STA = PaladinMultiplier.HP_per_STA;
                HP_per_level = PaladinMultiplier.HP_per_level;
                HP_regen_time = PaladinMultiplier.HP_regen_time;
                HP_regen_percent = PaladinMultiplier.HP_regen_percent;
                Base_MP = PaladinMultiplier.Base_MP;
                MP_per_WIS = PaladinMultiplier.MP_per_WIS;
                MP_per_level = PaladinMultiplier.MP_per_level;
                MP_regen_time = PaladinMultiplier.MP_regen_time;
                MP_regen_percent = PaladinMultiplier.MP_regen_percent;
                Damage_str = PaladinMultiplier.Damage_per_str;
                Damage_int = PaladinMultiplier.Damage_per_int;
                Defense_str = PaladinMultiplier.Defense_per_str;
                Defense_int = PaladinMultiplier.Defense_per_int;
                Critical_chance = PaladinMultiplier.Critical_chance_per_DEX;
                Critical_damage = PaladinMultiplier.Critical_damage_percent_base;
                Dodge_chance = PaladinMultiplier.Dodge_chance_per_AGI;
                AutoAtk_speed = PaladinMultiplier.AutoAtk_speed;
                AutoAtk_range = PaladinMultiplier.AutoAtk_range;
                Walking_spd = PaladinMultiplier.Walking_spd;
                Skill_range = PaladinMultiplier.Skill_range;//not used yet
                Skill_mana_usage = PaladinMultiplier.Skill_mana_usage;//not used yet
                Casting_speed_reduction = PaladinMultiplier.Casting_speed_reduction;//not used yet
                break;
            case PlayerClass.Warrior:
                DamageType_now = DamageType.physical;
                Base_HP = WarriorMultiplier.Base_HP;
                HP_per_STA = WarriorMultiplier.HP_per_STA;
                HP_per_level = WarriorMultiplier.HP_per_level;
                HP_regen_time = WarriorMultiplier.HP_regen_time;
                HP_regen_percent = WarriorMultiplier.HP_regen_percent;
                Base_MP = WarriorMultiplier.Base_MP;
                MP_per_WIS = WarriorMultiplier.MP_per_WIS;
                MP_per_level = WarriorMultiplier.MP_per_level;
                MP_regen_time = WarriorMultiplier.MP_regen_time;
                MP_regen_percent = WarriorMultiplier.MP_regen_percent;
                Damage_str = WarriorMultiplier.Damage_per_str;
                Damage_int = WarriorMultiplier.Damage_per_int;
                Defense_str = WarriorMultiplier.Defense_per_str;
                Defense_int = WarriorMultiplier.Defense_per_int;
                Critical_chance = WarriorMultiplier.Critical_chance_per_DEX;
                Critical_damage = WarriorMultiplier.Critical_damage_percent_base;
                Dodge_chance = WarriorMultiplier.Dodge_chance_per_AGI;
                AutoAtk_speed = WarriorMultiplier.AutoAtk_speed;
                AutoAtk_range = WarriorMultiplier.AutoAtk_range;
                Walking_spd = WarriorMultiplier.Walking_spd;
                Skill_range = WarriorMultiplier.Skill_range;//not used yet
                Skill_mana_usage = WarriorMultiplier.Skill_mana_usage;//not used yet
                Casting_speed_reduction = WarriorMultiplier.Casting_speed_reduction;//not used yet
                break;
            default:
                break;
        }
        //Since this is the same for all classes we can do it here
        Critical_damage = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.Crit_Multiplier].value;
        //damage
        Damage_str = (Damage_str * STR) + PlayerEquipStats[0];

        if (Conditions.increasedINT > 0f)
        {
            Damage_int = (Damage_int * (INT * (1f + (Conditions.increasedINT / 100f)))) + PlayerEquipStats[2];
        }
        else
        {
            Damage_int = (Damage_int * INT) + PlayerEquipStats[2];
        }
        if (Conditions.increasedDamage != 0f)//Concentration
        {
            //Damage_str *= (1f + (Conditions.increasedDamage / 100f));
            Damage_int *= (1f + (Conditions.increasedDamage / 100f));
        }
        if (Conditions.decreasedDamage < 0f)//Dismember
        {
            Damage_str *= (1f + (Conditions.decreasedDamage / 100f));
            //Damage_int *= (1f + (Conditions.decreasedDamage / 100f));
        }

        //defense
        Defense_from_pdef = Defense_str * DEF; //JWR - Assign pdef from stats
        Defense_from_mdef = Defense_int * MDEF; //JWR - Assign mdef from stats
        Defense_str = (Defense_str * DEF) + PlayerEquipStats[5];
        Defense_int = (Defense_int * MDEF) + PlayerEquipStats[6];
        if (Conditions.increasedDEF != 0f)
        {
            Defense_str *= (1f + (Conditions.increasedDEF / 100f));
        }
        if (Conditions.increasedMDEF != 0f)
        {
            Defense_int *= (1f + (Conditions.increasedMDEF / 100f));
        }
        if (Conditions.decreasedDEF < 0)
        {
            Defense_str *= (1f + (Conditions.decreasedDEF / 100f));
        }

        //Critical damage
        Critical_damage = Critical_damage + ((modCritDmg + passive_CritDmg) / 100f);

        //Critical chance
        Critical_chance *= DEX;
        Critical_chance += modCritChance + passive_CritChance + PlayerEquipStats[1];
        if (Critical_chance > CC_soft_cap)
        {
            Critical_chance = (Critical_chance - CC_soft_cap) / 2f;
            Critical_chance += CC_soft_cap;
        }
        if (Critical_chance > CC_hard_cap)
        {
            Critical_chance = CC_hard_cap;
        }
        
        Critical_chance += Conditions.increasedCritical;
        Critical_chance = (float)System.Math.Round((decimal)Critical_chance, 2, System.MidpointRounding.AwayFromZero);

        //Dodge
        Dodge_chance *= AGI;
        Dodge_chance += modDodge + PlayerEquipStats[7] + passive_dodge;     
        if (Dodge_chance > Dodge_soft_cap)
        {
            Dodge_chance = (Dodge_chance - Dodge_soft_cap) / 2f; 
            Dodge_chance += Dodge_soft_cap; 
        }
        if (Dodge_chance > Dodge_hard_cap)
        {
            Dodge_chance = Dodge_hard_cap;
        }
        
        Dodge_chance += Conditions.increasedDodge;

        if (Conditions.decreasedDodge < 0f)
        {
            Dodge_chance *= (1f + (Conditions.decreasedDodge / 100f));
        }

        Dodge_chance = (float)System.Math.Round((decimal)Dodge_chance, 2, System.MidpointRounding.AwayFromZero);

        //Regens
        HP_regen_percent += modHPRegen + passive_HPRegen;
        MP_regen_percent += passive_MPRegen;

        if(HP_regen_percent > 25f){
            HP_regen_percent = 25f;
        }

        if(MP_regen_percent > 25f){
            HP_regen_percent = 25f;
        }

        //Attack speed
        float totalAttkSpd = modAttkSPD + Conditions.increasedAtkSpeed + passive_atk_speed;
        float secondsToDecreaseOnAutoAtkSpeed = 0.00f;
        if(totalAttkSpd > 0f){
            for (int i = 0; totalAttkSpd >= 1; i++)
            {
                switch (i)
                {
                    case 0: //can get until 0.50sec to decrease (max: 2 attack per second / 0 to 100% attack speed total increase)
                        if (totalAttkSpd >= 100)
                        {
                            secondsToDecreaseOnAutoAtkSpeed += 0.50f;
                            totalAttkSpd -= 100f;
                        }
                        else
                        {
                            secondsToDecreaseOnAutoAtkSpeed += totalAttkSpd / 200f;
                            totalAttkSpd = 0f;
                        }
                        break;

                    case 1: //can get until +0.16sec to decrease (max: 3 attack per second / 101 to 200% attack speed total increase)
                        if (totalAttkSpd >= 100)
                        {
                            secondsToDecreaseOnAutoAtkSpeed += 0.16f;
                            totalAttkSpd -= 100f;
                        }
                        else
                        {
                            secondsToDecreaseOnAutoAtkSpeed += totalAttkSpd / 625f;
                            totalAttkSpd = 0f;
                        }
                        break;

                    case 2: //can get until +0.08sec to decrease (max: 4 attack per second / 201 to 300% attack speed total increase)
                        if (totalAttkSpd >= 100)
                        {
                            secondsToDecreaseOnAutoAtkSpeed += 0.08f;
                            totalAttkSpd -= 100f;
                        }
                        else
                        {
                            secondsToDecreaseOnAutoAtkSpeed += totalAttkSpd / 1250f;
                            totalAttkSpd = 0f;
                        }
                        break;

                    case 3: //can get until +0.05sec to decrease (max: 5 attack per second / 401 to 500% attack speed total increase)
                        if (totalAttkSpd >= 100)
                        {
                            secondsToDecreaseOnAutoAtkSpeed += 0.05f; //CAP: 0.80sec to decrease
                            totalAttkSpd = 0f; //CAP: 500% Attack Speed
                        }
                        else
                        {
                            secondsToDecreaseOnAutoAtkSpeed += totalAttkSpd / 2000f;
                            totalAttkSpd = 0f;
                        }
                        break;
                }
            }
            AutoAtk_speed = AutoAtk_speed - secondsToDecreaseOnAutoAtkSpeed;
        }

        //walking speed = base + variables              
        Walking_spd *= (1f + ((modSPD + passive_WalkingSpeed + Conditions.increasedWalkingSpeed + Conditions.decreasedWalkingSpeed) / 100f));
        if (Walking_spd >= 1.8f)
        {
            Walking_spd = 1.8f;
        }
        if (PlayerSkillsActions.is_casting)
        {
            PlayerMPSync.PlayerSpeed = Walking_spd * 0.5f;
        }
        else
        {
            PlayerMPSync.PlayerSpeed = Walking_spd;
        }



        //HP
        MaxHealth = Mathf.Round((Base_HP + PlayerEquipStats[3] + modMaxHP + (PlayerLevel * HP_per_level) + (STA * HP_per_STA)) * (1f + (passive_MaxHP / 100f)));
        if (Conditions.increasedMaxHP > 0)
        {
            MaxHealth *= (1f + (Conditions.increasedMaxHP / 100f));
        }
        MaxHealth = Mathf.Round(MaxHealth);
        //MP
        MaxMana = Mathf.Round((Base_MP + PlayerEquipStats[4] + modMaxMP + (PlayerLevel * MP_per_level) + (WIS * MP_per_WIS)) * (1f + (passive_MaxMP / 100f)));
        if (Conditions.increasedMaxMana > 0)
        {
            MaxMana *= (1f + (Conditions.increasedMaxMana / 100f));
        }

        //Event Max HP
        MaxHealth = Mathf.Round(MaxHealth * PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.extra_hp));
        //Event Max MP
        MaxMana = Mathf.Round(MaxMana * PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.extra_mp));

        //Casting
        Casting_speed_reduction = passive_Casting + modCastingSpeed + Conditions.increasedCastingSpeed;
        if (Casting_speed_reduction > 50f)
        {
            Casting_speed_reduction = 50f;
        }
        //Drop rate
        PlayerDropChance = modDropRate;

        //Extra gold drop
        ExtraGoldDrop = modGoldDrop;

        //Extra exp rate
        ExtraExp = modExpRate;

        //Cool down reduction
        CD_reduction = modCDReduction + passive_CD_redux + Conditions.increasedCooldownReduction;
        if (CD_reduction > 30f)
        {
            CD_reduction = 30f;
        }
        //Did anything change? if yes send it to client       
        DetectChangesOnStats_and_sendToClient();


    }



    private void DetectChangesOnStats_and_sendToClient()
    {


        stat_hash[0] = STR;
        stat_hash[1] = DEX;
        stat_hash[2] = INT;
        stat_hash[3] = STA;
        stat_hash[4] = WIS;
        stat_hash[5] = DEF;
        stat_hash[6] = MDEF;
        stat_hash[7] = AGI;
        stat_hash[8] = LCK;

        stat_hash[9] = HP_regen_percent;
        stat_hash[10] = HP_regen_time;
        stat_hash[11] = MP_regen_percent;
        stat_hash[12] = MP_regen_time;
        stat_hash[13] = Damage_str;
        stat_hash[14] = Critical_chance;
        stat_hash[15] = Skill_range;
        stat_hash[16] = AutoAtk_speed + 0.2f;//to allow lag
        stat_hash[17] = Damage_int;
        stat_hash[18] = Critical_damage;
        stat_hash[19] = AutoAtk_range;
        stat_hash[20] = Casting_speed_reduction;
        stat_hash[21] = Defense_str;
        stat_hash[22] = Dodge_chance;
        stat_hash[23] = Defense_int;
        stat_hash[24] = Walking_spd;

        stat_hash[25] = modSTR;
        stat_hash[26] = modDEX;
        stat_hash[27] = modINT;
        stat_hash[28] = modWIS;
        stat_hash[29] = modSTA;
        stat_hash[30] = modDEF;
        stat_hash[31] = modMDEF;
        stat_hash[32] = modAGI;
        stat_hash[33] = modMaxHP;
        stat_hash[34] = modMaxMP;
        stat_hash[35] = modDodge;
        stat_hash[36] = modReflectSTR;
        stat_hash[37] = modAttkSPD;
        stat_hash[38] = modHPRegen;
        stat_hash[39] = modHPonKill;
        stat_hash[40] = modMPonKill;
        stat_hash[41] = modCastingSpeed;
        stat_hash[42] = modCritChance;
        stat_hash[43] = modCritDmg;
        stat_hash[44] = modSPD;

        stat_hash[45] = passive_MaxHP;
        stat_hash[46] = passive_MaxMP;
        stat_hash[47] = passive_CritChance;
        stat_hash[48] = passive_CritDmg;
        stat_hash[49] = passive_Casting;
        stat_hash[50] = passive_WalkingSpeed;
        stat_hash[51] = passive_MPRegen;



        stat_hash[52] = modDropRate;
        stat_hash[53] = modGoldDrop;
        stat_hash[54] = modExpRate;
        stat_hash[55] = modCDReduction;

        stat_hash[56] = PlayerDropChance;
        stat_hash[57] = ExtraGoldDrop;
        stat_hash[58] = ExtraExp;
        stat_hash[59] = CD_reduction;


        //detect changes on any of this by using MD%
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        // create a byte array and copy the floats into it...
        var byteArray = new byte[stat_hash.Length * 4];
        Buffer.BlockCopy(stat_hash, 0, byteArray, 0, byteArray.Length);
        byte[] hash = md5.ComputeHash(byteArray);
        if (temp_hash != hash)
        {
            //if something changed send it all
            //sync new stats on client
            TargetUpdateStats_base(connectionToClient, stat_hash);
            temp_hash = new byte[hash.Length];
            temp_hash = hash;
        }
    }

    //********************************************



    #region Networking
    //[Header("Exp conf stuff")]    
    [SyncVar]
    public float CurrentHP;
    [SyncVar]
    public float CurrentMP;
    [SyncVar]
    public float MaxHealth;
    [SyncVar]
    public float MaxMana;
    [SyncVar]
    public string PlayerSelectedClass;
    [SyncVar]
    public int PlayerLevel;
    #endregion


    #region STATs   
    public float STR,
    DEX,
    INT,
    STA,
    WIS,
    DEF,
    MDEF,
    AGI,
    LCK;

    public float[] PlayerEquipStats = new float[9];
    public int[] PlayerCustomStats_lvl;
    public int[] PlayerCustomStats_reb;
    public int free_PlayerCustomStats_lvl;
    public int free_PlayerCustomStats_reb;
    //todo: set this to the right numbers
    [SerializeField]
    int customStats_per_level = 3;
    [SerializeField]
    int free_rebirthPoints_per_rebirth = 10;
    #endregion

    #region Mods
    //Normales        
    public float modSTR,
     modDEX,
     modINT,
     modWIS,
     modSTA,
     modDEF,
     modMDEF,
     modAGI,
    //Raros
     modMaxHP,
     modMaxMP,
     modDodge,
     modReflectSTR,

     modHPRegen,
     modHPonKill,
     modMPonKill,
     modCastingSpeed,
     modCritChance,
     modCritDmg,
     modSPD,
        modDropRate,
        modGoldDrop,
        modExpRate,
        modCDReduction;
    public float modAttkSPD;
    #endregion
    #region Enchants
    public List<int> enchants_equipped = new List<int>();
    #endregion
    #region Pasivos   
    public float passive_MaxHP,
     passive_MaxMP,
     passive_MPRegen,
     passive_HPRegen,
     passive_CritChance,
     passive_CritDmg,
     passive_WalkingSpeed,
     passive_Casting,
     passive_CD_redux,
     passive_exp_loss_redux,
     passive_off_hand_mastery,
     passive_atk_speed,
     passive_mana_usage_redux,
     passive_dodge;
    #endregion

    #region Player
    PlayerStatistics PlayerStatistics;
    PlayerAccountInfo PlayerAccountInfo;
    PlayerQuestInfo PlayerQuestInfo;
    PlayerGuild PlayerGuild;
    PlayerDeath PlayerDeath;
    PlayerSkills PlayerSkills;
    PlayerSkillsActions PlayerSkillsActions;
    PlayerConditions Conditions;
    PlayerInventory PlayerInventory;
    PlayerMPSync PlayerMPSync;
    PlayerGeneral PlayerGeneral;
    #endregion   

    #region EXP    
    public enum exp_source
    {
        ignore,
        grind,
        ds,
        quest,
        config,
        death
    }

    [Header("EXP configs")]
    public float baseXP = 1000;
    public float exponent = 1.65f;
    public float changeClassExpPenalty = 0.3f;

    [Header("Exp current")]
    public float CurrentEXP;
    public float thisLevelExp;
    public float Exp100;
    public float nextLevelExp;
    public int Total_rebirths = 0;

    float[] train_exp_buffer;
    float player_exp_buffer = 0f;
    bool allowed_to_save_exp;
    #endregion

    #region Others      
    int temp_PlayerLevel = 9999;
    bool StatsReady;
    bool start_level_calculated;
    #endregion

    #region Prof Exp
    [HideInInspector]
    public int mining_exp;
    [HideInInspector]
    public int herbalism_exp;
    #endregion

    #region Unity event   
    private void Awake()
    {
        train_exp_buffer = new float[8];
        PlayerQuestInfo = GetComponent<PlayerQuestInfo>();
        PlayerStatistics = GetComponent<PlayerStatistics>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerMPSync = GetComponent<PlayerMPSync>();
        PlayerInventory = GetComponent<PlayerInventory>();
        PlayerDeath = GetComponent<PlayerDeath>();
        PlayerGuild = GetComponent<PlayerGuild>();
        PlayerSkills = GetComponent<PlayerSkills>();
        PlayerSkillsActions = GetComponent<PlayerSkillsActions>();
        Conditions = GetComponent<PlayerConditions>();
        PlayerAccountInfo = GetComponent<PlayerAccountInfo>();
        PINGPONG_last_rx = Time.time;
    }
    void Start()
    {
        setCustomStats_lvl();
        setCustomStats_reb();
        HPandMPRegen();
        StartCoroutine(waitsecodsafterstart());
        StartCoroutine(save_exp_cooldown());
        StartCoroutine(PINGPONG());
    }
    private void OnDestroy()
    {
        //save exp when player logsout
        save_exp_changes(true);
    }
    #endregion
    private void setCustomStats_reb()
    {
        PlayerCustomStats_reb = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    private void setCustomStats_lvl()
    {
        PlayerCustomStats_lvl = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    #region Networking Server
    [Command]
    public void CmdPlayerCurrentSelectedClass(string PlayerSelectedClassCmd)
    {
        if (PlayerInventory.Inventory.EquippedList.Count <= PlayerInventory.inventoryFreeSpaces())
        {
            int goldpen = 5000;
            //int goldpen = 0;          
            if (PlayerLevel >= 20)
            {
                if (PlayerInventory.Gold >= goldpen)
                {

                    if (PlayerSelectedClassCmd == "Warrior" || PlayerSelectedClassCmd == "Mage" || PlayerSelectedClassCmd == "Archer" || PlayerSelectedClassCmd == "Support")
                    {
                        var ExpPayed = CurrentEXP * changeClassExpPenalty;
                        var tempExp = CurrentEXP;
                        var tempClass = PlayerSelectedClass;
                        var tempGold = PlayerInventory.Gold;

                        player_exp_change(Mathf.RoundToInt(-ExpPayed), PlayerStats.exp_source.config);
                        PlayerInventory.ChangeGold_NEGATIVE_or_POSITIVE_gold(-goldpen, string.Empty,false);
                        //StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.addGold(PlayerAccountInfo.PlayerAccount, -goldpen)));
                        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.SaveChangedClass(PlayerAccountInfo.PlayerAccount, PlayerSelectedClassCmd)));


                        PlayerSelectedClass = PlayerSelectedClassCmd;
                        ClassChange();
                        resetCustomStats_lvl();
                        //refresh player stats
                        ProcessStats();
                        var log = "Class Change " + tempClass + "->" + PlayerSelectedClassCmd + " - had EXP:" + tempExp + " now:" + CurrentEXP + " had gold:" + tempGold + " now:" + PlayerInventory.Gold;
                        StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP));
                        NudeMode();
                        TargetConfirmClassChange(connectionToClient, PlayerSelectedClassCmd);
                        //PlayerSkills.softResetTree();
                        ProcessStats();
                    }


                }

            }
            else
            {
                if (PlayerSelectedClassCmd == "Warrior" || PlayerSelectedClassCmd == "Mage" || PlayerSelectedClassCmd == "Archer" || PlayerSelectedClassCmd == "Support")
                {
                    var tempClass = PlayerSelectedClass;
                    PlayerSelectedClass = PlayerSelectedClassCmd;
                    ClassChange();
                    StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.SaveChangedClass(PlayerAccountInfo.PlayerAccount, PlayerSelectedClassCmd)));

                    resetCustomStats_lvl();
                    //refresh player stats
                    ProcessStats();
                    var log = "FREE Class Change " + tempClass + "->" + PlayerSelectedClassCmd + " - now:" + CurrentEXP + " Gold now:" + PlayerInventory.Gold;
                    StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP));
                    NudeMode();
                    //////////////.Log(PlayerSelectedClassCmd);
                    TargetConfirmClassChange(connectionToClient, PlayerSelectedClassCmd);
                    //PlayerSkills.softResetTree();
                    ProcessStats();
                }
            }
        }
    }

    private void ClassChange()
    {
        switch (PlayerSelectedClass)
        {
            case "Archer":
                PlayerClass_now = PlayerClass.Hunter;
                break;
            case "Warrior":
                PlayerClass_now = PlayerClass.Warrior;
                break;
            case "Mage":
                PlayerClass_now = PlayerClass.Wizard;
                break;
            case "Support":
                PlayerClass_now = PlayerClass.Paladin;
                break;
            default:
                break;
        }
        PlayerSkills.clear_equipped_skills();
    }

    [Command]
    public void CmdChangeClassWithGems(string PlayerSelectedClassCmd)
    {
        if (PlayerInventory.Inventory.EquippedList.Count <= PlayerInventory.inventoryFreeSpaces())
        {
            if (PlayerSelectedClassCmd == "Warrior" || PlayerSelectedClassCmd == "Mage" || PlayerSelectedClassCmd == "Archer" || PlayerSelectedClassCmd == "Support")
            {
                int tempIAPCurrency = PlayerAccountInfo.PlayerIAPcurrency;
                int price = 456;
                int precheck = tempIAPCurrency - price;
                var tempClass = PlayerSelectedClass;
                if (precheck >= 0)
                {

                    PlayerSelectedClass = PlayerSelectedClassCmd;
                    ClassChange();
                    StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.SaveChangedClass(PlayerAccountInfo.PlayerAccount, PlayerSelectedClassCmd)));

                    resetCustomStats_lvl();
                    //refresh player stats
                    ProcessStats();
                    var log = "IAP Class Change " + tempClass + "->" + PlayerSelectedClassCmd + " - now:" + CurrentEXP + " Gold now:" + PlayerInventory.Gold + " Gems:" + PlayerAccountInfo.PlayerIAPcurrency;
                    StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP));
                    NudeMode();
                    //quitar las gemas
                    PlayerAccountInfo.PlayerIAPcurrency -= price;
                    StartCoroutine(saveProductBought(PlayerGeneral.ServerDBHandler.IAP_ModifyCurrency(PlayerAccountInfo.PlayerAccount, -price), tempIAPCurrency, 0));
                    //confirmamos
                    TargetConfirmClassChange(connectionToClient, PlayerSelectedClassCmd);
                    //PlayerSkills.softResetTree();
                    ProcessStats();
                }
                else
                {
                    var log = "HACK: IAP Class Change " + tempClass + "->" + PlayerSelectedClassCmd + " - now:" + CurrentEXP + " Gold now:" + PlayerInventory.Gold + " Gems:" + PlayerAccountInfo.PlayerIAPcurrency;
                    StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP));

                }

            }
        }

    }
    /*Todo: this is only available with gems.. stablish price and set on client
     *
     * [Command]
    void CmdResetCustomLevelStats()
    {
        //resetStats();
    }*/
    [Command]
    void CmdRebirthNow(int reward)
    {
        if (PlayerLevel >= 150)
        {
            int gemsaward = 0;
            int goldrequired = 250000;

            if (PlayerInventory.Gold >= goldrequired)
            {
                switch (reward)
                {
                    case 0://Gemas random
                        SavePreRebirthLog();
                        //Quitar gold
                        PlayerInventory.ChangeGold_NEGATIVE_or_POSITIVE_gold(-goldrequired,"rebirth",true);
                        //Aumentar rebirth y borrar exp en base de datos
                        string query = PlayerGeneral.ServerDBHandler.rebirthOperations(PlayerAccountInfo.PlayerAccount, "rebirth", CurrentEXP);
                        // .LogError("Rebirth: " + query);
                        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(query, 0));

                        Total_rebirths++;
                        if (Total_rebirths >= 8)
                        {
                            PlayerGeneral.rewadTitleAndUpdateClient(26);//Inmortal
                        }

                        TargetTotalRebirths(connectionToClient, Total_rebirths);
                        //+free point and skill points
                        /*PlayerSkills.total_SP += 4;
                        PlayerSkills.SP += 4;
                        PlayerSkills.total_PSP += 1;
                        PlayerSkills.PSP += 1;*/
                        gemsaward = UnityEngine.Random.Range(100, 220 + 1);
                        var had_gems = PlayerAccountInfo.PlayerIAPcurrency;
                        PlayerAccountInfo.PlayerIAPcurrency += gemsaward;
                        StartCoroutine(PlayerGeneral.x_ObjectHelper.IAPmanager.changeIAPcurrency(had_gems, PlayerAccountInfo.PlayerIAPcurrency, gemsaward, gameObject));
                        ContinueRebirthProcess();
                        break;
                    case 1://Titulo random

                        break;
                    default:
                        break;
                }
            }

        }

    }
    [Command]
    public void CmdSaveCustomStats(int[] add_level_stats, int[] add_rebirth_stats)
    {
        //keep count of how many points are on each stat to compare with available points (never trust client)
        if (add_level_stats.Sum() <= free_PlayerCustomStats_lvl && add_rebirth_stats.Sum() <= free_PlayerCustomStats_reb)
        {
            //data sent by client matches free points so we can add array
            for (int i = 0; i < PlayerCustomStats_lvl.Length; i++)
            {
                if (add_level_stats[i] > 0)
                {
                    if (PlayerCustomStats_lvl[i] + add_level_stats[i] <= PlayerLevel)
                    {
                        PlayerCustomStats_lvl[i] += add_level_stats[i];
                        free_PlayerCustomStats_lvl -= add_level_stats[i];
                    }
                }
            }
            //data sent by client matches free points so we can add array
            for (int i = 0; i < PlayerCustomStats_reb.Length; i++)
            {
                if (add_rebirth_stats[i] > 0)
                {
                    PlayerCustomStats_reb[i] += add_rebirth_stats[i];
                    free_PlayerCustomStats_reb -= add_rebirth_stats[i];
                }
            }
            //update player stats
            ProcessStats();
            //notify client stats where saved (not 100% sure but if they are not saved by any error its not a problem) - next restart they will have free points
            sendCustomStatsToClient_all();
            //save new build to DB
            saveCustomStats_all();
        }
    }
    [Command]
    public void CmdPONG(int ping)
    {
        PINGPONG_last_rx = Time.time;
    }
    #endregion

    #region Networking Client   
    [TargetRpc]
    void TargetConfirmClassChange(NetworkConnection target, string PlayerSelectedClassCmd)
    {

    }
    [TargetRpc]
    public void TargetReceiveExpData(NetworkConnection target, float currentExpServer, float thisLevelExp, float Exp100, float nextLevelExp)
    {

    }
    [TargetRpc]
    void TargetReceiveCustomStats(NetworkConnection target, int[] lvl_stats, int lvl_free, int[] reb_stats, int reb_free)
    {

    }
    [TargetRpc]
    public void TargetTotalRebirths(NetworkConnection target, int totalRebirths)
    {

    }
    [TargetRpc]
    public void TargetUpdateSpecialMobsKilled(NetworkConnection target, int mobs) { }
    [TargetRpc]
    void TargetUpdateStats_base(NetworkConnection target, float[] detailedStats) { }
    [TargetRpc]
    public void TargetUpdateStats_base_account(NetworkConnection target, string[] detailedStats) { }
    [TargetRpc]
    public void TargetSendTraning_Exp_Data(NetworkConnection target, int[] stat_training_this_level_exp, float[] stat_effects, int[] stat_training_exp_100, int[] stat_training_levels)
    {

    }
    [TargetRpc]
    public void TargetEquippedEnchants(NetworkConnection target, byte[] dictionary)
    {

    }
    [TargetRpc]
    public void TargetPING(NetworkConnection target, int pong) { }
    #endregion

    #region Networking RPC
    [ClientRpc]
    void RpcLvlUp(GameObject player)
    {
        Animator animator = player.transform.GetChild(4).GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Effects/PixelEffects/Animation/Teleport") as RuntimeAnimatorController;
        animator.SetTrigger("Trigger");
    }
    #endregion

    #region Initializacion
    void HPandMPRegen()
    {
        StartCoroutine("HPRegen");
        StartCoroutine("MPRegen");
    }
    public void downloadPlayerCustomStats_all()
    {
        string GetPlayerCustomStatsurl = PlayerGeneral.ServerDBHandler.GetPlayerCustomStats(PlayerAccountInfo.PlayerAccount);
        StartCoroutine(downloadPlayerCustomStats(GetPlayerCustomStatsurl));
    }
    #endregion

    #region DB 
    IEnumerator saveProductBought(string urlServer, int tempIAPCurrency, int tries)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(urlServer);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {

            if (tries < 4)
            {
                yield return new WaitForSeconds(5f);
                var nextTry = tries + 1;
                StartCoroutine(saveProductBought(urlServer, tempIAPCurrency, nextTry));
            }
            else
            {
                PlayerAccountInfo.TargetIAPcommChannel(connectionToClient, "Error%0%Error#7055 - please report this error ASAP");
                StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveIAPlog("FAILED", PlayerAccountInfo.PlayerAccount, "Error#7055 while modifiying gems amount %%% " + urlServer)));
            }
        }
        else
        {
            StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveIAPlog("BOUGHT_WITH_GEMS", PlayerAccountInfo.PlayerAccount, "IAP Class Change HAD:" + tempIAPCurrency + " NOW:" + PlayerAccountInfo.PlayerIAPcurrency)));
            PlayerAccountInfo.TargetPlayerIAPcurrency(connectionToClient, PlayerAccountInfo.PlayerIAPcurrency);
        }
    }
    IEnumerator downloadPlayerCustomStats(string urlCustomStats)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(urlCustomStats);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlCustomStats));
        }
        else
        {
            string[] downloadedStats = uwr.downloadHandler.text.Split('&');
            //we get something like this
            //$STR.",".$DEX.",".$INT.",".$STA.",".$WIS.",".$DEF.",".$MDEF.",".$AGI.",".$LCK."&".$FREE."&".$STR.",".$DEX.",".$INT.",".$STA.",".$WIS.",".$DEF.",".$MDEF.",".$AGI.",".$LCK."&".$FREE
            //first set is level stats and second set is rebirth stats
            //downloadedStats[0] has level stats
            //downloadedStats[1] has free level stats
            //downloadedStats[2] has rb stats
            //downloadedStats[3] has free rb stats
            PlayerCustomStats_lvl = downloadedStats[0].Split(',').Select(int.Parse).ToArray();
            free_PlayerCustomStats_lvl = int.Parse(downloadedStats[1]);
            PlayerCustomStats_reb = downloadedStats[2].Split(',').Select(int.Parse).ToArray();
            free_PlayerCustomStats_reb = int.Parse(downloadedStats[3]);
            player_exp_change(0, PlayerStats.exp_source.config);
            //----------------USED FOR MIGRATION & checks-----------------
            //we need to make sure that players have right amount of points
            if (PlayerCustomStats_lvl.Sum() + free_PlayerCustomStats_lvl != PlayerLevel * customStats_per_level)
            {
                resetCustomStats_lvl();
            }
            for (int i = 0; i < PlayerCustomStats_lvl.Length; i++)
            {
                if (PlayerCustomStats_lvl[i] > PlayerLevel)
                {
                    resetCustomStats_lvl();
                    break;
                }
            }
            if (PlayerCustomStats_reb.Sum() + free_PlayerCustomStats_reb != Total_rebirths * 10)
            {
                resetCustomStats_reb();
            }
            //---------------------------------------------------
            //move loading bar on client
            sendCustomStatsToClient_all();
            //continue with loading process
            PlayerGeneral.B_loadFriends();
        }
    }
    void saveCustomStats_all()
    {
        //save stuff to db here
        string urlSave = PlayerGeneral.ServerDBHandler.save_all_custom_stats(PlayerAccountInfo.PlayerAccount, PlayerCustomStats_lvl, free_PlayerCustomStats_lvl, PlayerCustomStats_reb, free_PlayerCustomStats_reb);
        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlSave));

    }
    #endregion

    #region Utilidades
    public float requiredExpForLevel(float level, int rebirths)
    {
        return Mathf.Floor(baseXP * (Mathf.Pow(level, (exponent + (rebirths * 0.03f)))));
    }
    public int getLevelfromExp(float current, int rebirths)
    {
        return (int)Mathf.Floor(Mathf.Pow((current / baseXP), 1f / (exponent + (rebirths * 0.03f))));
    }
    public void NudeMode()
    {
        string playerID = PlayerAccountInfo.PlayerAccount;
        string urlServer = PlayerGeneral.ServerDBHandler.v2_inv_actions_NudeMode(playerID);
        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
        PlayerInventory.UnequipEverything();
    }

    private void sendCustomStatsToClient_all()
    {
        TargetReceiveCustomStats(connectionToClient, PlayerCustomStats_lvl, free_PlayerCustomStats_lvl, PlayerCustomStats_reb, free_PlayerCustomStats_reb);
    }
    int[] calculate_training_stats_levels(int[] total_exp_all_stats)
    {
        int[] to_return = new int[8];
        for (int i = 0; i < to_return.Length; i++)
        {
            to_return[i] = getLevelfromExp(total_exp_all_stats[i], 0);
            if (to_return[i] < 0)
            {
                to_return[i] = 0;
            }
        }
        return to_return;
    }
    #endregion

    #region PlayerLevel
    public void player_exp_change(int exp_to_change, exp_source exp_source)
    {

        if (exp_to_change > 0)
        {
            float exp_multiplier = 1f;
            switch (exp_source)
            {
                case exp_source.grind:
                    exp_multiplier = PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.grind_exp);
                    exp_to_change = Mathf.RoundToInt(exp_multiplier * exp_to_change);
                    break;
                case exp_source.ds:
                    exp_multiplier = PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.ds_exp);
                    exp_to_change = Mathf.RoundToInt(exp_multiplier * exp_to_change * 1.5f);//perma 1.5x exp on ds
                    break;
                case exp_source.quest:
                    exp_multiplier = PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.quest_exp);
                    exp_to_change = Mathf.RoundToInt(exp_multiplier * exp_to_change);
                    break;
                default:
                    break;
            }


            if (stat_training_exp_change(exp_to_change))
            {
                CurrentEXP += exp_to_change;
                PlayerGeneral.showCBT(gameObject, false, false, exp_to_change, "exp");
            }
        }
        else if (exp_to_change < 0)
        {
            CurrentEXP += exp_to_change;
            PlayerGeneral.showCBT(gameObject, false, false, exp_to_change, "exp");
            player_exp_buffer += exp_to_change;
        }


        temp_PlayerLevel = PlayerLevel;
        PlayerLevel = getLevelfromExp(CurrentEXP, Total_rebirths);

        if (PlayerLevel == 0)
        {
            Exp100 = 1000;
        }
        else
        {
            nextLevelExp = requiredExpForLevel(PlayerLevel + 1, Total_rebirths);
            Exp100 = nextLevelExp - requiredExpForLevel(PlayerLevel, Total_rebirths);
        }
        thisLevelExp = CurrentEXP - requiredExpForLevel(PlayerLevel, Total_rebirths);
        TargetReceiveExpData(connectionToClient, CurrentEXP, thisLevelExp, Exp100, nextLevelExp);

        if (temp_PlayerLevel < PlayerLevel && start_level_calculated)
        {
            RpcLvlUp(this.gameObject);
            if (PlayerGeneral.PartyID != string.Empty)
            {
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }

            ProcessStats();
            if (CurrentHP > 0)
            {
                CurrentHP = MaxHealth;
                CurrentMP = MaxMana;
            }

            if (PlayerLevel >= 150 && PlayerSelectedClass == "Mage")
            {
                PlayerGeneral.rewadTitleAndUpdateClient(31);//Necromancer
            }
            else if (PlayerLevel >= 150 && PlayerSelectedClass == "Archer")
            {
                PlayerGeneral.rewadTitleAndUpdateClient(32);//Rougue
            }
            else if (PlayerLevel >= 150 && PlayerSelectedClass == "Warrior")
            {
                PlayerGeneral.rewadTitleAndUpdateClient(33);//Cavalier
            }
            else if (PlayerLevel >= 150 && PlayerSelectedClass == "Support")
            {
                PlayerGeneral.rewadTitleAndUpdateClient(34);//Persecutor
            }


            //add free points on level up/levelup
            free_PlayerCustomStats_lvl += ((PlayerLevel - temp_PlayerLevel) * customStats_per_level);
            //save it to DB
            saveCustomStats_all();
            //update free points on client
            sendCustomStatsToClient_all();
        }
        if (PlayerGeneral.PartyID != string.Empty)
        {
            PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
        }

        start_level_calculated = true;

    }
    bool stat_training_exp_change(float WARNING_full_exp)
    {
        bool allowed_to_gain_character_exp = true;
        int total_train_exp = 0;
        Array.Copy(stat_training_exp, temp_train_exp, stat_training_exp.Length);


        for (int i = 0; i < PlayerInventory.Inventory.equipped_farm_stones.Count; i++)
        {
            var stone_upgrade = 1f + (PlayerGeneral.x_ObjectHelper.training_stones_upgrades_stat_increase(PlayerInventory.Inventory.equipped_farm_stones[i]._inventoryItem.itemUpgrade) / 100f);

            switch (PlayerInventory.Inventory.equipped_farm_stones[i]._item.ItemID)
            {
                /*0 STR
                 * 1 DEX
                 * 2 INT
                 * 3 STA
                 * 4 WIS
                 * 5 DEF
                 * 6 MDEF
                 * 7 AGI
                 * */
                case 3100:
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));//round to keep it clean
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3101:
                    stat_training_exp[4] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3102:
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3103:
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3104:
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3105:
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3106:
                    stat_training_exp[7] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3107:
                    stat_training_exp[1] += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.1f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3108:
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    stat_training_exp[4] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3109:
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3110:
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3111:
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade)); 
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade)); 
                    allowed_to_gain_character_exp = false;
                    break;
                case 3112:
                    stat_training_exp[1] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    stat_training_exp[7] += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade)); 
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.06f * stone_upgrade)); 
                    allowed_to_gain_character_exp = false;
                    break;
                case 3113:
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade)); 
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade)); 
                    allowed_to_gain_character_exp = false;
                    break;
                case 3114:
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[4] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade)); 
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade)); 
                    allowed_to_gain_character_exp = false;
                    break;
                case 3115:
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[4] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3116:
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3117:
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3118:
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3119:
                    stat_training_exp[7] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[1] += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.05f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3120:
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[5] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3121:
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[6] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3122:
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[0] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[7] += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    stat_training_exp[1] += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                case 3123:
                    stat_training_exp[3] += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    stat_training_exp[2] += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    stat_training_exp[7] += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    stat_training_exp[1] += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.03f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.08f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    total_train_exp += Mathf.RoundToInt((WARNING_full_exp * 0.02f * stone_upgrade));
                    allowed_to_gain_character_exp = false;
                    break;
                default:
                    break;
            }
        }

        stat_training_levels = calculate_training_stats_levels(stat_training_exp);
        if (stat_training_levels_temp.Sum() != stat_training_levels.Sum())
        {
            //level up
            RpcLvlUp(this.gameObject);
            stat_training_levels_temp = stat_training_levels;
        }

        //.LogError(stat_training_exp[0] + "-" + temp_train_exp[0]);
        float str_exp = stat_training_exp[0] - temp_train_exp[0];
        float dex_exp = stat_training_exp[1] - temp_train_exp[1];
        float int_exp = stat_training_exp[2] - temp_train_exp[2];
        float sta_exp = stat_training_exp[3] - temp_train_exp[3];
        float wis_exp = stat_training_exp[4] - temp_train_exp[4];
        float def_exp = stat_training_exp[5] - temp_train_exp[5];
        float mdef_exp = stat_training_exp[6] - temp_train_exp[6];
        float agi_exp = stat_training_exp[7] - temp_train_exp[7];

        if (!allowed_to_gain_character_exp)
        {
            WARNING_full_exp = 0f;
        }

        //add exp we gonna save    
        //--training
        train_exp_buffer[0] += str_exp;
        train_exp_buffer[1] += dex_exp;
        train_exp_buffer[2] += int_exp;
        train_exp_buffer[3] += sta_exp;
        train_exp_buffer[4] += wis_exp;
        train_exp_buffer[5] += def_exp;
        train_exp_buffer[6] += mdef_exp;
        train_exp_buffer[7] += agi_exp;
        //--player exp
        player_exp_buffer += WARNING_full_exp;

        save_exp_changes(false);
        refresh_stat_training_on_client();

        if (total_train_exp > 0)
        {
            PlayerGeneral.showCBT(gameObject, false, false, total_train_exp, "t_exp");
        }

        return allowed_to_gain_character_exp;
    }

    private void refresh_stat_training_on_client()
    {
        if (temp_train_exp.Sum() != stat_training_exp.Sum())
        {
            int[] stat_training_this_level_exp = new int[8];
            float[] stat_effects = new float[8];
            int[] stat_training_exp_100 = new int[8];

            for (int i = 0; i < stat_training_this_level_exp.Length; i++)
            {
                stat_training_this_level_exp[i] = stat_training_exp[i] - Mathf.RoundToInt(requiredExpForLevel(stat_training_levels[i], 0));
                stat_effects[i] = get_training_multiplier(i);
                stat_training_exp_100[i] = Mathf.RoundToInt(requiredExpForLevel(stat_training_levels[i] + 1, 0)) - Mathf.RoundToInt(requiredExpForLevel(stat_training_levels[i], 0));
            }
            TargetSendTraning_Exp_Data(connectionToClient, stat_training_this_level_exp, stat_effects, stat_training_exp_100, stat_training_levels);

        }
    }

    void save_exp_changes(bool force)
    {
        //if allowed to save now
        if (allowed_to_save_exp || force)
        {
            //if there is any change negative or positive
            if (train_exp_buffer.Sum() != 0 || player_exp_buffer != 0)
            {
                var urlServer = PlayerGeneral.ServerDBHandler.SaveExp(PlayerAccountInfo.PlayerAccount, player_exp_buffer, train_exp_buffer);
                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
                //all saved (hopefully) so clear the buffer
                Array.Clear(train_exp_buffer, 0, train_exp_buffer.Length);
                player_exp_buffer = 0f;
                allowed_to_save_exp = false;
            }
        }
    }
    IEnumerator save_exp_cooldown()
    {
        yield return new WaitForSeconds(60f);
        //please dont break, if this breaks then exp on this session wont be saved
        try
        {
            allowed_to_save_exp = true;
            StartCoroutine(save_exp_cooldown());
        }
        catch (Exception ex)
        {
            StartCoroutine(save_exp_cooldown());
            Debug.LogError(ex.ToString());
        }        
    }
    #endregion


    #region Mierda
    IEnumerator waitsecodsafterstart()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(HPandMPController());
    }
    /*IEnumerator MyCurrentLevelServer()
    {
        PlayerLevel = getLevelfromExp(CurrentEXP,Total_rebirths);

        if (startLevelCalculated)
        {
            if (temp_PlayerLevel < PlayerLevel)
            {

                RpcLvlUp(this.gameObject);
                if (PlayerGeneral.PartyID != string.Empty)
                {
                    PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
                }

                ProcessStats();

                CurrentHP = MaxHealth;
                CurrentMP = MaxMana;
                if (PlayerLevel >= 150 && PlayerSelectedClass == "Mage")
                {
                    PlayerGeneral.rewadTitleAndUpdateClient(31);//Necromancer
                }
                else if (PlayerLevel >= 150 && PlayerSelectedClass == "Archer")
                {
                    PlayerGeneral.rewadTitleAndUpdateClient(32);//Rougue
                }
                else if (PlayerLevel >= 150 && PlayerSelectedClass == "Warrior")
                {
                    PlayerGeneral.rewadTitleAndUpdateClient(33);//Cavalier
                }
                else if (PlayerLevel >= 150 && PlayerSelectedClass == "Support")
                {
                    PlayerGeneral.rewadTitleAndUpdateClient(34);//Persecutor
                }


                //add fre points on level up/levelup
                free_PlayerCustomStats_lvl += ((PlayerLevel-temp_PlayerLevel)* customStats_per_level);                
                //save it to DB
                saveCustomStats_all();
                //update free points on client
                sendCustomStatsToClient_all();
            }
        }

        //string NamePlateString = PlayerAccountInfo.PlayerNickname + " Lv." + PlayerLevel.ToString();

        if (PlayerLevel == 0)
        {
            Exp100 = 1000;
        }
        else
        {
            nextLevelExp = requiredExpForLevel(PlayerLevel + 1);
            Exp100 = nextLevelExp - requiredExpForLevel(PlayerLevel);
        }

        thisLevelExp = CurrentEXP - requiredExpForLevel(PlayerLevel);

        if (CurrentEXP != temp_CurrentExp)
        {
            TargetReceiveExpData(connectionToClient, CurrentEXP, thisLevelExp, Exp100, nextLevelExp);
            if (PlayerGeneral.PartyID != string.Empty)
            {
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }
        }

        temp_CurrentExp = CurrentEXP;
        temp_PlayerLevel = PlayerLevel;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MyCurrentLevelServer());
    }*/
    /* IEnumerator WaitForEquipent()
     {
         yield return new WaitForSeconds(0.1f);
         ProcessStats();
     }*/
    IEnumerator HPandMPController()
    {
        if (StatsReady)
        {
            if (CurrentHP > MaxHealth)
            {
                CurrentHP = MaxHealth;
            }
            if (CurrentMP > MaxMana)
            {
                CurrentMP = MaxMana;
            }
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(HPandMPController());
    }
    public IEnumerator downloadMainTableData(string statsurlServer, string hwid)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(statsurlServer);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            PlayerAccountInfo.StartCoroutine(PlayerAccountInfo.dcInSecs(5f));
        }
        else
        {
            try
            {

                string urlServerReply = uwr.downloadHandler.text;

                if (urlServerReply == "banned")
                {
                    PlayerGeneral.x_ObjectHelper.IRC_demo.submitData("Attempt:" + hwid);
                    PlayerGeneral.TargetSendToChat(connectionToClient, "This Account has been banned");
                    StartCoroutine(PlayerAccountInfo.dcInSecs(5f));
                }
                else
                {
                    char delimiter = ',';
                    string[] substrings = urlServerReply.Split(delimiter);

                    CurrentEXP = float.Parse(substrings[3]);


                    PlayerAccountInfo.PlayerNickname = substrings[0];
                    //admin
                    if (PlayerAccountInfo.PlayerNickname == "Alkanov" ||
                        PlayerAccountInfo.PlayerNickname == "Celmi" ||
                        PlayerAccountInfo.PlayerNickname == "Zelp")
                    {
                        PlayerAccountInfo.isAdmin = true;
                        this.GetComponent<NetworkProximityChecker>().forceHidden = true;

                    }
                    PlayerAccountInfo.PlayerAccount = substrings[23];

                    CurrentHP = float.Parse(substrings[1]);

                    CurrentMP = float.Parse(substrings[2]);





                    PlayerAccountInfo.PlayerNickChangesAvail = int.Parse(substrings[4]);
                    PlayerAccountInfo.TargetNickChangesAvail(connectionToClient, PlayerAccountInfo.PlayerNickChangesAvail);

                    PlayerSelectedClass = substrings[5];
                    ClassChange();
                    PlayerAccountInfo.ModifyKarma(int.Parse(substrings[6]), true);//set player karma

                    parseGuildData(substrings);

                    PlayerAccountInfo.PlayerIAPcurrency = int.Parse(substrings[12]);
                    PlayerAccountInfo.TargetPlayerIAPcurrency(connectionToClient, PlayerAccountInfo.PlayerIAPcurrency);

                    #region Premium
                    /* if (substrings[13] != string.Empty)
                     {
                         PlayerAccountInfo.Premium = true;
                     }*/
                    #endregion

                    Total_rebirths = int.Parse(substrings[14]);
                    TargetTotalRebirths(connectionToClient, Total_rebirths);

                    PlayerGeneral.loadPlayerTitles(substrings[15]);
                    PlayerGeneral.serializeAndSendOwnedTitles();
                    int equipped_title = 0;
                    if (int.TryParse(substrings[16], out equipped_title))
                    {
                        PlayerGeneral.loadEquippedTitle(equipped_title);
                    }

                    //Gold
                    PlayerInventory.Gold = int.Parse(substrings[18]);
                    if (PlayerInventory.Gold > 15000000)
                    {
                        PlayerGeneral.rewadTitleAndUpdateClient(30);//Goldilocks
                    }
                    PlayerInventory.TargetSetGoldInPlayer(connectionToClient, PlayerInventory.Gold);
                    //inv size
                    PlayerInventory.invSize = int.Parse(substrings[19]);
                    //bank size
                    PlayerInventory.bankSize = int.Parse(substrings[20]);
                    //chat pass
                    PlayerGeneral.TargetChatPass(connectionToClient, substrings[21]);
                    //online status
                    if (substrings[22] == "0" || substrings[22] == PlayerGeneral.x_ObjectHelper.IRCchat.nickName)
                    {

                        GameObject alreadyConnected = PlayerGeneral.PlayersConnected.getPlayerObject_byAccount(PlayerAccountInfo.PlayerAccount);
                        if (alreadyConnected == null)
                        {
                            string requestWWW = PlayerGeneral.ServerDBHandler.setPlayerOnlineonDB(PlayerAccountInfo.PlayerAccount, PlayerGeneral.x_ObjectHelper.IRCchat.nickName);
                            StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(requestWWW));
                            PlayerAccountInfo.online_checks_passed = true;
                            PlayerGeneral.x_ObjectHelper.PlayersConnected.allPlayers.Add(gameObject);
                        }
                        else
                        {
                            //Desconecto a los dos
                            alreadyConnected.GetComponent<PlayerMPSync>().TargetDCbyServer(alreadyConnected.GetComponent<NetworkIdentity>().connectionToClient, "0");
                            PlayerMPSync.TargetDCbyServer(connectionToClient, "0");
                            StartCoroutine(PlayerAccountInfo.dcInSecs(5f));
                            StartCoroutine(alreadyConnected.GetComponent<PlayerAccountInfo>().dcInSecs(5f));
                            yield break;
                        }
                    }
                    else
                    {

                        PlayerMPSync.TargetDCbyServer(connectionToClient, "0");
                        StartCoroutine(PlayerAccountInfo.dcInSecs(5f));
                    }


                    //Download finished
                    PlayerInventory.TargetSendInvSize(connectionToClient, PlayerInventory.invSize, PlayerInventory.bankSize);

                    ScanForNewTitles();
                    PlayerLevel = getLevelfromExp(CurrentEXP, Total_rebirths);
                    temp_PlayerLevel = PlayerLevel;

                    //stat training
                    stat_training_exp = new int[8];
                    stat_training_exp[0] = int.Parse(substrings[24]);
                    stat_training_exp[1] = int.Parse(substrings[25]);
                    stat_training_exp[2] = int.Parse(substrings[26]);
                    stat_training_exp[3] = int.Parse(substrings[27]);
                    stat_training_exp[4] = int.Parse(substrings[28]);
                    stat_training_exp[5] = int.Parse(substrings[29]);
                    stat_training_exp[6] = int.Parse(substrings[30]);
                    stat_training_exp[7] = int.Parse(substrings[31]);
                    stat_training_levels = calculate_training_stats_levels(stat_training_exp);
                    //send stats to client
                    refresh_stat_training_on_client();
                    stat_training_levels_temp = stat_training_levels;



                    downloadPlayerCustomStats_all();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                PlayerGeneral.TargetSendToChat(connectionToClient, "Error#6000 while logging in");
                StartCoroutine(PlayerAccountInfo.dcInSecs(5f));
                throw;
            }

        }
    }
    private void ScanForNewTitles()
    {
        if (Total_rebirths >= 8)
        {
            PlayerGeneral.rewadTitleAndUpdateClient(26);//Inmortal
        }
        if (PlayerAccountInfo.PlayerKarma >= 2500)
        {
            PlayerGeneral.rewadTitleAndUpdateClient(20);
        }
        if (PlayerAccountInfo.PlayerKarma <= -2500)
        {
            PlayerGeneral.rewadTitleAndUpdateClient(21);
        }
    }
    private void parseGuildData(string[] substrings)
    {
        if (substrings[7] != "-100" && substrings[8] != "-100" && substrings[11] != "0")
        {
            PlayerGuild.PlayerGuildID = int.Parse(substrings[7]);
            PlayerGuild.PlayerGuildRole = int.Parse(substrings[8]);
            PlayerGuild.PlayerGuildName = substrings[9];
            PlayerGuild.PlayerGuildAcro = substrings[10];
            PlayerGuild.PlayerGuildMembers = int.Parse(substrings[11]);
            PlayerGeneral.loadGuildTitles(substrings[17]);
            PlayerGuild.TargetGuildInfo(connectionToClient, PlayerGuild.PlayerGuildID, PlayerGuild.PlayerGuildRole, PlayerGuild.PlayerGuildMembers);

            switch (PlayerGuild.PlayerGuildRole)
            {
                case 7:
                    PlayerGeneral.PlayerOwnedTitles.Add(15);
                    break;
                case 3:
                    PlayerGeneral.PlayerOwnedTitles.Add(16);
                    break;
                case 2:
                    PlayerGeneral.PlayerOwnedTitles.Add(17);
                    break;
                default:
                    break;
            }

            StartCoroutine(PlayerGuild.GetGuildData());

        }
    }
    #endregion


    #region MP HP modifications
    IEnumerator resting_in()
    {
        yield return new WaitForSeconds(combat_cd);
        in_combat = false;
    }
    public void hpChange(int hpaffected)
    {
        if (hpaffected <= 0)
        {
            if (temp_in_combat != null)
            {
                StopCoroutine(temp_in_combat);
            }
            temp_in_combat = StartCoroutine(resting_in());
            in_combat = true;

            if (!Conditions.immortal)
            {
                if (Conditions.mana_shield && hpaffected < 0)
                {
                    if (CurrentMP + hpaffected < 0)
                    {
                        //take whatever was left from the mana
                        hpaffected += (int)CurrentMP;
                        CurrentMP = 0;
                        //remove buff
                        Conditions.clean_buff_debuff(PlayerConditions.type.buff, 13);//limpiar mana shield
                        //the rest goes to the HP
                        CurrentHP += hpaffected;
                        //show text
                        PlayerGeneral.showCBT(gameObject, false, false, hpaffected * -1, "damage");

                        if (UnityEngine.Random.Range(0f, 100f) <= 20f)
                        {
                            //spawn blood animation
                            PlayerGeneral.x_ObjectHelper.spawn_sync_object(1, 5f, transform.position);
                        }
                    }
                    else
                    {
                        CurrentMP += hpaffected;
                    }
                }
                else
                {
                    CurrentHP += hpaffected;
                    if (UnityEngine.Random.Range(0f, 100f) <= 20f)
                    {
                        //spawn blood animation
                        PlayerGeneral.x_ObjectHelper.spawn_sync_object(1, 5f, transform.position);
                    }

                }

                //wake him up
                if (hpaffected < 0)
                {
                    Conditions.clean_buff_debuff(PlayerConditions.type.debuff, 6);
                }
            }
        }
        else
        {
            CurrentHP += hpaffected;
            if (CurrentHP > MaxHealth)
            {
                CurrentHP = MaxHealth;
            }
        }
    }
    IEnumerator MPRegen()
    {
        if (CurrentMP <= MaxMana && StatsReady && !PlayerDeath.isPlayerDead)
        {
            if (PlayerMPSync.stationary)
            {
                CurrentMP += Mathf.Ceil(MaxMana * (MP_regen_percent * (1f + (ench_extra_mp_regen_while_stationary / 100f)) / 100f));
            }
            else
            {
                CurrentMP += Mathf.Ceil(MaxMana * (MP_regen_percent / 100f));
            }


            if (CurrentMP > MaxMana)
            {
                CurrentMP = MaxMana;
            }
            if (PlayerGeneral.PartyID != string.Empty)
            {
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }
        }
        if (in_combat)
        {
            yield return new WaitForSeconds(MP_regen_time);
        }
        else
        {
            if (CurrentMP < MaxMana)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(MP_regen_time);
            }
        }

        StartCoroutine("MPRegen");
    }
    IEnumerator HPRegen()
    {
        if (CurrentHP < MaxHealth && StatsReady && !PlayerDeath.isPlayerDead)
        {
            if (PlayerMPSync.stationary)
            {
                CurrentHP += Mathf.Ceil(MaxHealth * (HP_regen_percent * (1f + (ench_extra_hp_regen_while_stationary / 100f)) / 100f));
                //.LogError("CurrentHP stationary:" + Mathf.Ceil(MaxHealth * (HP_regen_percent * (1f + (ench_extra_hp_regen_while_stationary / 100f)) / 100f)));
                //1000*(0.05*(1+(0/100)/100)
            }
            else
            {
                CurrentHP += Mathf.Ceil(MaxHealth * (HP_regen_percent / 100f));
                //.LogError("CurrentHP normal:" + Mathf.Ceil(MaxHealth * (HP_regen_percent / 100f)));
            }

            if (CurrentHP > MaxHealth)
            {
                CurrentHP = MaxHealth;
            }
            if (PlayerGeneral.PartyID != string.Empty)
            {
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }
        }
        if (in_combat)
        {
            yield return new WaitForSeconds(HP_regen_time * 1.5f);
        }
        else
        {
            if (CurrentHP < MaxHealth)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(HP_regen_time);
            }

        }
        StartCoroutine("HPRegen");
    }
    #endregion

    #region Update Current Stats
    public void ProcessStats()
    {
        var starter_min_str = 1;
        var starter_min_int = 1;
        switch (PlayerClass_now)
        {
            case PlayerClass.Hunter:
                starter_min_str = 2;
                break;
            case PlayerClass.Wizard:
                starter_min_int = 3;
                break;
            case PlayerClass.Paladin:
                starter_min_int = 2;
                break;
            case PlayerClass.Warrior:
                starter_min_str = 3;
                break;
            default:
                break;
        }

        STR = Mathf.Round(((starter_min_str + PlayerCustomStats_lvl[0] + PlayerCustomStats_reb[0]) * get_training_multiplier(0)) + modSTR);
        DEX = ((1 + PlayerCustomStats_lvl[1] + PlayerCustomStats_reb[1]) * get_training_multiplier(1)) + modDEX;
        INT = Mathf.Round(((starter_min_int + PlayerCustomStats_lvl[2] + PlayerCustomStats_reb[2]) * get_training_multiplier(2)) + modINT);
        STA = Mathf.Round(((1 + PlayerCustomStats_lvl[3] + PlayerCustomStats_reb[3]) * get_training_multiplier(3)) + modSTA);
        WIS = Mathf.Round(((1 + PlayerCustomStats_lvl[4] + PlayerCustomStats_reb[4]) * get_training_multiplier(4)) + modWIS);
        DEF = Mathf.Round(((1 + PlayerCustomStats_lvl[5] + PlayerCustomStats_reb[5]) * get_training_multiplier(5)) + modDEF);
        MDEF = Mathf.Round(((1 + PlayerCustomStats_lvl[6] + PlayerCustomStats_reb[6]) * get_training_multiplier(6)) + modMDEF);
        AGI = ((1 + PlayerCustomStats_lvl[7] + PlayerCustomStats_reb[7]) * get_training_multiplier(7)) + modAGI;
        LCK = (1 + PlayerEquipStats[8] + PlayerCustomStats_lvl[8] + PlayerCustomStats_reb[8]);


        DEX = (float)(Math.Round((Decimal)DEX, 2, MidpointRounding.AwayFromZero)); //round to 2 decimal places
        AGI = (float)(Math.Round((Decimal)AGI, 2, MidpointRounding.AwayFromZero)); //round to 2 decimal places
        LCK = (float)(Math.Round((Decimal)LCK, 2, MidpointRounding.AwayFromZero)); //round to 2 decimal places

        RefreshStats();

        StatsReady = true;


    }

    float get_training_multiplier(int stat_index)
    {
        for (int i = 0; i < PlayerGeneral.x_ObjectHelper.stat_training_multipliers_list.Count; i++)
        {
            if (stat_training_levels[stat_index] >= PlayerGeneral.x_ObjectHelper.stat_training_multipliers_list[i].level_min && stat_training_levels[stat_index] <= PlayerGeneral.x_ObjectHelper.stat_training_multipliers_list[i].level_max)
            {
                return 1f + (stat_training_levels[stat_index] * PlayerGeneral.x_ObjectHelper.stat_training_multipliers_list[i].multiplier / 100f);
            }
        }
        return 1f;
    }
    #endregion

    #region Statistics
    public IEnumerator safeGetAccountStatistics(string requestWWW)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(requestWWW);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(safeGetAccountStatistics(requestWWW));
        }
        else
        {
            string[] data = uwr.downloadHandler.text.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> crap_temp = new List<string>();
            crap_temp.Add(data[20]);
            crap_temp.Add(data[0]);
            crap_temp.Add(data[1]);
            crap_temp.Add(data[2]);
            crap_temp.Add(data[3]);
            crap_temp.Add(data[4]);
            crap_temp.Add(data[5]);
            crap_temp.Add(data[6]);
            crap_temp.Add(data[7]);
            crap_temp.Add(data[8]);
            crap_temp.Add(data[9]);
            crap_temp.Add(data[10]);
            crap_temp.Add(data[11]);
            crap_temp.Add(data[12]);
            crap_temp.Add(data[13]);
            crap_temp.Add(data[14]);
            crap_temp.Add(data[15]);
            crap_temp.Add(data[16]);
            crap_temp.Add(data[17]);
            crap_temp.Add(data[18]);

            TargetUpdateStats_base_account(connectionToClient, crap_temp.ToArray());
            StartCoroutine(PlayerStatistics.updateOnClient());

            PlayerStatistics.mobskilled_account = int.Parse(data[0]);
            PlayerStatistics.boss_killed_account = int.Parse(data[1]);
            PlayerStatistics.deaths_account = int.Parse(data[2]);
            PlayerStatistics.hpPotions_used_account = int.Parse(data[3]);
            PlayerStatistics.mpPotions_used_account = int.Parse(data[4]);
            PlayerStatistics.potionRefills_account = int.Parse(data[5]);
            PlayerStatistics.quests_done_account = int.Parse(data[6]);
            PlayerStatistics.quests_abandonned_account = int.Parse(data[7]);
            PlayerStatistics.items_bought_account = int.Parse(data[8]);
            PlayerStatistics.items_sold_account = int.Parse(data[9]);
            PlayerStatistics.items_collected_account = int.Parse(data[10]);
            PlayerStatistics.arenakills_account = int.Parse(data[11]);
            PlayerStatistics.openpvpkills_account = int.Parse(data[12]);
            PlayerStatistics.dsjoined_account = int.Parse(data[13]);
            PlayerStatistics.dspvpkills_account = int.Parse(data[14]);
            PlayerStatistics.trades_successful_account = int.Parse(data[15]);
            PlayerStatistics.trades_cancelled_account = int.Parse(data[16]);
            PlayerStatistics.damage_done_account = int.Parse(data[17]);
            PlayerStatistics.damage_taken_account = int.Parse(data[18]);
            PlayerStatistics.healed_account = int.Parse(data[19]);
            PlayerStatistics.minutes_played = int.Parse(data[20]);
            PlayerStatistics.event_mobs_account = int.Parse(data[21]);
            TargetUpdateSpecialMobsKilled(connectionToClient, PlayerStatistics.event_mobs_account);
            PlayerGeneral.F_loadLocation();
        }
    }
    #endregion



    #region Stats Reset
    public void resetCustomStats_lvl()
    {
        //IMPORTANT: call this after new level has been calculated or we could end up with players who change class to delevel and get more free points (based on level before deleveling)

        //free points are based on player level, so no need to take PlayerCustomStats_lvl into account
        free_PlayerCustomStats_lvl = PlayerLevel * customStats_per_level;
        //reset level custom stats
        setCustomStats_lvl();
        //refresh on client
        sendCustomStatsToClient_all();
        //save it to DB
        saveCustomStats_all();

    }
    public void resetCustomStats_reb()
    {
        //sum all current custom stats and add them to free
        free_PlayerCustomStats_reb = Total_rebirths * 10;
        //reset level custom stats
        setCustomStats_reb();
        //refresh on client
        sendCustomStatsToClient_all();
        //save it to DB
        saveCustomStats_all();
    }
    #endregion

    #region Rebirths
    private void ContinueRebirthProcess()
    {
        //increase free rebirth points
        free_PlayerCustomStats_reb += free_rebirthPoints_per_rebirth;
        //set level points to 0
        setCustomStats_lvl();
        //saves and sends current skill points and prestige points
        // PlayerSkills.SaveAndSendSkillData();
        //nude
        NudeMode();
        //teleport
        PlayerMPSync.Unstuck();
        //level 1 = 1001 exp because 1000 triggers the tutorial
        CurrentEXP = 1001f;
        player_exp_change(0, PlayerStats.exp_source.config);
        PlayerLevel = 1;
        //reset level custom stats (this will also save all custom stats level and rebirth including free points)
        resetCustomStats_lvl();
        //refresh player stats
        ProcessStats();
        //abandon all current quest       
        for (int i = 0; i < PlayerGeneral.QuestCenter.QuestDatabase.Count; i++)
        {
            //PlayerQuestInfo.AbandonQuest(PlayerGeneral.QuestsDB.QuestDatabase[i].QuestID);
        }
        //send new stats to client
        sendCustomStatsToClient_all();
        //refresh stats
        ProcessStats();
    }
    private void SavePreRebirthLog()
    {
        int allstats = 0;
        for (int i = 0; i < PlayerCustomStats_lvl.Length; i++)
        {
            allstats += PlayerCustomStats_lvl[i];
        }
        var log = "Rebirth level " + PlayerLevel + " Stats_Total " + allstats + " Gold " + PlayerInventory.Gold;
        StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP));
    }
    #endregion

    #region Stats
    public void sendStatisticsToClient()
    {
        TargetUpdateSpecialMobsKilled(connectionToClient, PlayerStatistics.event_mobs_account + PlayerStatistics.event_mobs_session);
    }
    #endregion

    #region Enchants
    void ZeroEnchats()
    {
        enchants_affecting = new List<enchant.enchant_base>();
        ench_extra_hp_from_pots = 0f;
        ench_extra_hp_regen_while_stationary = 0f;
        ench_extra_mp_from_pots = 0f;
        ench_extra_mp_regen_while_stationary = 0f;
        //Saphire
        ench_free_hp_potion_use_on_kill = 0f;
        ench_chance_to_get_free_mphp_potion_charge = 0f;
        ench_speed_penalty = 0f;
        //Siderite
        ench_free_mp_potion_use_on_kill = 0f;
        ench_chance_to_get_hpandmp_on_kill = 0f;
        ench_potion_block = false;
        //Gypsum
        ench_chance_to_explode_deads = 0f;
        ench_chance_to_free_cast = 0f;
        ench_chance_to_fail_casting = 0f;
    }
    public void applyEnchants()
    {
        ZeroEnchats();
        //the key is the enchant and the value is how many we have equipped
        Dictionary<int, int> enchants_grouped = new Dictionary<int, int>();
        for (int i = 0; i < enchants_equipped.Count; i++)//to count how many of each enchantments we have equipped
        {
            if (!enchants_grouped.ContainsKey(enchants_equipped[i]))//new?
            {
                enchants_grouped.Add(enchants_equipped[i], 1);

            }
            else//more than 1 equipped already
            {
                if (enchants_grouped[enchants_equipped[i]] < 4)//max sets allowed
                {
                    enchants_grouped[enchants_equipped[i]]++;
                }

            }
        }

        //iterate all the dictionary to 
        foreach (KeyValuePair<int, int> enchants in enchants_grouped)
        {
            //.LogError("ID: " + enchants.Key + " has:" + enchants.Value);
            //if we have more than 1 on this set
            if (enchants.Value > 1)
            {
                //get the enchant based on its ID
                var enchant_found = PlayerGeneral.x_ObjectHelper.enchantsDB.FetchEnchantBase(enchants.Key);
                if (enchant_found != null)
                {
                    switch (enchant_found.ench_base)
                    {
                        case enchant.enchant_base.lazuli:
                            //.LogError("ID: " + enchants.Key + " has:" + enchants.Value + " enchant_found.set_bonuses[" + enchant_found.IDs.IndexOf(enchants.Key) + "][" + (enchants.Value - 1) + "];");
                            ench_extra_hp_from_pots += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            enchants_affecting.Add(enchant.enchant_base.lazuli);
                            break;
                        case enchant.enchant_base.corrupted_lazuli:
                            if (!enchants_affecting.Contains(enchant.enchant_base.corrupted_lazuli))
                            {
                                ench_extra_hp_from_pots += -50f;//-50% hp from pots
                                enchants_affecting.Add(enchant.enchant_base.corrupted_lazuli);
                            }
                            ench_extra_hp_regen_while_stationary += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            break;
                        case enchant.enchant_base.jade:
                            ench_extra_mp_from_pots += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            enchants_affecting.Add(enchant.enchant_base.jade);
                            break;
                        case enchant.enchant_base.corrupted_jade:
                            if (!enchants_affecting.Contains(enchant.enchant_base.corrupted_jade))
                            {
                                ench_extra_mp_from_pots += -50f;//-50% hp from pots
                                enchants_affecting.Add(enchant.enchant_base.corrupted_jade);
                            }
                            ench_extra_mp_regen_while_stationary += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            break;
                        case enchant.enchant_base.saphire:
                            ench_free_hp_potion_use_on_kill += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            enchants_affecting.Add(enchant.enchant_base.saphire);
                            break;
                        case enchant.enchant_base.corrupted_saphire:
                            ench_chance_to_get_free_mphp_potion_charge += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            if (!enchants_affecting.Contains(enchant.enchant_base.corrupted_saphire))
                            {
                                ench_speed_penalty = -25f;
                                Conditions.decreasedWalkingSpeed = ench_speed_penalty;
                                RefreshStats();
                                enchants_affecting.Add(enchant.enchant_base.corrupted_saphire);
                            }
                            break;
                        case enchant.enchant_base.siderite:
                            ench_free_mp_potion_use_on_kill += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            enchants_affecting.Add(enchant.enchant_base.siderite);
                            break;
                        case enchant.enchant_base.corrupted_siderite:
                            ench_chance_to_get_hpandmp_on_kill += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            if (!enchants_affecting.Contains(enchant.enchant_base.corrupted_siderite))
                            {
                                ench_potion_block = true;
                                enchants_affecting.Add(enchant.enchant_base.corrupted_siderite);
                            }
                            break;
                        case enchant.enchant_base.gypsum:
                            ench_chance_to_explode_deads += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            enchants_affecting.Add(enchant.enchant_base.gypsum);
                            break;
                        case enchant.enchant_base.corrupted_gypsum:
                            ench_chance_to_free_cast += enchant_found.set_bonuses[enchant_found.IDs.IndexOf(enchants.Key)][enchants.Value - 2];
                            if (!enchants_affecting.Contains(enchant.enchant_base.corrupted_gypsum))
                            {
                                ench_chance_to_fail_casting += 5f;
                                enchants_affecting.Add(enchant.enchant_base.corrupted_gypsum);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

        }

        //serialize and sent dictionary of equipped enchants ID and amount
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, enchants_grouped);

        TargetEquippedEnchants(connectionToClient, mStream.ToArray());

    }
    private MemoryStream SerializeItem(Dictionary<int, int> equippedEnchants)
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, equippedEnchants);
        return mStream;
    }
    #endregion

    #region PINGPONG    
    float PINGPONG_last_rx = 0f;
    float PINGPONG_max_timeout = 15f;//more than 3 pings without answer
    IEnumerator PINGPONG()
    {
        yield return new WaitForSeconds(3f);
        TargetPING(connectionToClient, 12345);
        yield return new WaitForSeconds(1f);//just extra time to allow for RTT
        if (Time.time - PINGPONG_last_rx > PINGPONG_max_timeout)//this should usually be around if(1>15)
        {
            //timeout
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }
        StartCoroutine(PINGPONG());
    }
    #endregion

}
