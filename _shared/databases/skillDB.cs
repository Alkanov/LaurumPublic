using System.Collections.Generic;
using UnityEngine;

public class skillDB : MonoBehaviour
{

    public List<skill> Skills = new List<skill>();


    void Start()
    {
        //-------V2----------------
        //Passives
        /* Skills.Add(new skill(901, 1, "HP", "max hp", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(902, 1, "MP", "max mp", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(903, 1, "HP regen", "hp regen", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(904, 1, "MP regen", "MP regen", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(905, 1, "Crit chance", "Crit chance", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(906, 1, "Crit damage", "Crit damage", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(907, 1, "Walking speed", "Walking speed", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(908, 1, "Casting Redux", "Casting Redux", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(909, 1, "CD redux", "CD redux", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(910, 1, "Less Exp loss on death", "AAAA", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(911, 1, "Off Hand Mastery", "AAAA", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(912, 1, "Atk Speed", "AAAA", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(913, 1, "Mana Usage", "AAAA", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value
         Skills.Add(new skill(914, 1, "Dodge", "AAAA", 0, 1, skill.Stype.statBuff, new float[1] { 5f }, ""));//value*/

        //-----Warrior
        //---------------Berserker
        //Whirlwind
        Skills.Add(new skill(61001, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Warrior, 4f, 0.4f, 1.9f, 25));//value
        //Bleed
        Skills.Add(new skill(61004, skill.SAction.active, skill.Stype.target_DOT, PlayerStats.PlayerClass.Warrior, 9f, 0.3f, new float[2] { 1.8f, 5f }, 18));//value//time(not in use)
        //Execution
        Skills.Add(new skill(61016, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 5f, 0.5f, new float[2] { 2f, 40f }, 20));//damage/% requirement
        //Armor Crusher
        Skills.Add(new skill(61024, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 15f, 0.2f, new float[3] { -40f, 100f, 5f }, 12));//decreased def/chance/time
        //Dismember
        Skills.Add(new skill(61025, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 5f, 0.3f, new float[3] { 1.7f, 5f, -50f }, 15));//damage/time/debuff
        //---------------Utility
        //Charge                                                                                                                                                     
        Skills.Add(new skill(61006, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 15f, 0.8f, new float[2] { 45f, 5f }, 10));//buff/time
        //Provoke
        Skills.Add(new skill(61008, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Warrior, 2f, 0.2f, new float[2] { 1f, 90f }, 12));//damage/chance/
        //Battle Shout
        Skills.Add(new skill(61013, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Warrior, 30f, 0.8f, new float[2] { 30f, 20f }, 10));//buff/time
        //Slow down!
        Skills.Add(new skill(61026, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 9f, 0.3f, new float[2] { 40f, 3f }, 12));//debuff/time
        //On your knees!
        Skills.Add(new skill(61027, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 6f, 0.2f, new float[2] { 75f, 50f }, 12));//chance/below hp        
        //---------------Champion     
        //Shield Stun
        Skills.Add(new skill(61003, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 6f, 0.4f, new float[2] { 1.6f, 50f }, 15));//damage/chance
        //Soul Cravings
        Skills.Add(new skill(61010, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 5f, 0.3f, new float[2] { 1.7f, 30f }, 15));//damage/heal
        //Ultimate Defense
        Skills.Add(new skill(61020, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 15f, 0.4f, new float[2] { 40f, 10f }, 10));//buff/time
        //Arrow Deflect
        Skills.Add(new skill(61028, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 15f, 0.2f, new float[2] { 40f, 5f }, 12));//buff/time
        //Shields Up
        Skills.Add(new skill(61029, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 5f, 0.2f, new float[1] { 10f }, 12));//time

        //-----Wizard            
        //---------------Fire
        //Fireball
        Skills.Add(new skill(62001, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 5f, 1f, 2f, 35));
        //Flame missile
        Skills.Add(new skill(62002, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 6f, 0.8f, new float[2] { 1.7f, 75f }, 30));//damage/chance
        //Burn
        Skills.Add(new skill(62003, skill.SAction.active, skill.Stype.target_DOT, PlayerStats.PlayerClass.Wizard, 10f, 0.6f, 1.8f, 32));
        //Meteor Rain
        Skills.Add(new skill(62004, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Wizard, 12f, 4f, 1.9f, 40));
        //Lava Barrier
        Skills.Add(new skill(62005, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Wizard, 10f, 2f, 1.8f, 38));
        //---------------Ice
        //Ice Spear
        Skills.Add(new skill(62006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 1.5f, 0.4f, 1.4f, 28));
        //Frost Blade
        Skills.Add(new skill(62007, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 7f, 0.8f, new float[2] { 1.6f, 50f }, 30));//damage/chance
        //Frozen Hands
        Skills.Add(new skill(62008, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 15f, 0.8f, new float[1] { 5f, 45f }, 25));//time
        //Blizzard
        Skills.Add(new skill(62009, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Wizard, 15f, 1f, new float[3] { 1.5f, 50f, 75f }, 30));//damage/chance to freeze/chance to slow
        //Frost Bomb
        Skills.Add(new skill(62010, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 6f, 0.6f, new float[2] { 1.7f, 75f }, 30));//damage//chance
        //---------------Util
        //Corpse Life Drain
        Skills.Add(new skill(62011, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Wizard, 2.5f, 0.2f, new float[2] { 30f, 30f }, 28));//effect/time
        //Mana shield
        Skills.Add(new skill(62012, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 60f, 0.4f, 45f, 25));//time
        //Expanded Mana
        Skills.Add(new skill(62013, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 60f, 0.4f, new float[2] { 50f, 45f }, 25));//effect/time
        //Caster Contract
        Skills.Add(new skill(62014, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 30f, 0.8f, new float[1] { 10f }, 28));//time
        //Concentration
        Skills.Add(new skill(62015, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 15f, 0.8f, new float[2] { 30f, 10f }, 28));//effect/time

        //-----Hunter            
        //---------------Marskman
        //Snipe
        Skills.Add(new skill(63001, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 5f, 0.8f, 1.8f, 25));
        //Hawkeye
        Skills.Add(new skill(63002, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 15f, 0.2f, new float[2] { 25f, 10f }, 15));//buff/time
        //Multishot
        Skills.Add(new skill(63003, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Hunter, 8f, 2f, new float[2] { 1.7f, 5f }, 30));//damage//targets
        //Steady shot
        Skills.Add(new skill(63004, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 1.25f, 0.3f, 1.2f, 18));
        //Hamstring shot
        Skills.Add(new skill(63005, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 6f, 0.45f, new float[3] { 1.5f, 100f, 20f }, 20));//damage/chance/effect
        //---------------Survival
        //Poison Arrow
        Skills.Add(new skill(63006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 6f, 0.6f, new float[2] { 1.5f, 75f }, 20));//damage/chance
        //Multi trap
        Skills.Add(new skill(63007, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 8f, 1f, 1.6f, 28));
        //Posion Trap
        Skills.Add(new skill(63008, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 4f, 0.2f, 1.6f, 22));//damage
        //Steel trap
        Skills.Add(new skill(63009, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 5f, 0.2f, 1.5f, 20));
        //Booby Trap
        Skills.Add(new skill(63010, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 4f, 0.2f, 1.6f, 22));
        //---------------Util
        //Hunters Mark
        Skills.Add(new skill(63011, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Hunter, 15f, 0.2f, new float[3] { 5f, 100f, 30f }, 18));//time//threshold/extra damage
        //Hunters Ritual
        Skills.Add(new skill(63012, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Hunter, 12f, 0.2f, new float[2] { 50f, 5f }, 15));//buff/time
        //Camouflage
        Skills.Add(new skill(63013, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 15f, 0.8f, 5f, 15));
        //Soul Sacrifice
        Skills.Add(new skill(63014, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 10f, 0.4f, new float[2] { 25f, 75f }, 1));//mp required//% to recover from mp burned
        //Acrobatics
        Skills.Add(new skill(63015, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 15f, 0.4f, new float[2] { 25f, 10f }, 15));//buff/time

        //-----Paladin            
        //---------------Protection
        //Final Protection
        Skills.Add(new skill(64001, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 15f, 0.8f, 25f, 18));
        //Magic Protection totem
        Skills.Add(new skill(64002, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 15f, 0.4f, new float[2] { 40f, 15f }, 18));  //def/time
        //Physical Protection totem
        Skills.Add(new skill(64003, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 15f, 0.4f, new float[2] { 40f, 15f }, 18)); //def/time
        //Linked Hearts
        Skills.Add(new skill(64004, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 15f, 0.8f, new float[2] { 10f, 75f }, 18));//time/absorb
        //burn on touch buff
        Skills.Add(new skill(64005, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 15f, 1f, new float[2] { 1.6f, 10f }, 28));//damage/time
        //Holy Light
        Skills.Add(new skill(64006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Paladin, 3f, 0.5f, 1.8f, 30));
        //Self Heal
        Skills.Add(new skill(64007, skill.SAction.active, skill.Stype.selfHeal_over_time, PlayerStats.PlayerClass.Paladin, 5f, 0.4f, 0.5f, 18));
        //Heal totem
        Skills.Add(new skill(64008, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 10f, 0.4f, 75f, 18));
        //Resurrection
        Skills.Add(new skill(64009, skill.SAction.active, skill.Stype.AOE_revive, PlayerStats.PlayerClass.Paladin, 30f, 2f, 75f, 18));
        //Cleanse
        Skills.Add(new skill(64010, skill.SAction.active, skill.Stype.AOE_cleanse, PlayerStats.PlayerClass.Paladin, 5f, 0.2f, 5f, 18));
        //Mana totem
        Skills.Add(new skill(64011, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 15f, 0.4f, new float[1] { 45f }, 18));//value
        //Silence
        Skills.Add(new skill(64012, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Paladin, 15f, 0.2f, new float[2] { 100f, 5f }, 20));//chance/time
        //Speed totem
        Skills.Add(new skill(64013, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 15f, 0.8f, new float[5] { 45f, 35f, 25f, 15f, 10f }, 20));//mov speed, attack speed, casting speed, cooldown reduction, time
        //Buff remover
        Skills.Add(new skill(64014, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Paladin, 5f, 0.2f, 100f, 18));//chance
        //Remember me->changed to Consecration
        Skills.Add(new skill(64015, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Paladin, 4f, 0.4f, 1.7f, 35));

    }

    public skill FetchSkillByID(int ID)
    {
        for (int i = 0; i < Skills.Count; i++)
            if (Skills[i].SkillID == ID)
                return Skills[i];
        return null;
    }

    /* 
     * Deprecated
     * 
     * public skill FetchNextSkillUpgrade(int currentSkillID, int nextLevel)
     {
         for (int i = 0; i < Skills.Count; i++)
             if (Skills[i].SkillID == currentSkillID && Skills[i].level == nextLevel)
                 return Skills[i];
         return null;
     }

     public skill FetchSkillByParentAndLevel(int currentSkillID, int nextLevel)
     {
         for (int i = 0; i < Skills.Count; i++)
             if (Skills[i].SkillID == currentSkillID && Skills[i].level == nextLevel)
                 return Skills[i];
         return null;
     }*/

    public List<skill> FetchSkillsAndUpgrades(int ID)
    {
        List<skill> toReturn = new List<skill>();

        for (int i = 0; i < Skills.Count; i++)
        {
            if (Skills[i].SkillID == ID)
            {
                toReturn.Add(Skills[i]);
            }

        }
        return toReturn;
    }
}
