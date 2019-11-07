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
        Skills.Add(new skill(61001, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Warrior, 4f, 0.3f, 2.19f, 30));//value
        //Bleed
        Skills.Add(new skill(61004, skill.SAction.active, skill.Stype.target_DOT, PlayerStats.PlayerClass.Warrior, 9f, 0.3f, new float[2] { 2.0f, 6f }, 15));//value//time(not in use)
        //Execution
        Skills.Add(new skill(61016, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 5f, 0.3f, new float[2] { 2.1f, 40f }, 20));//damage/% requirement
        //Armor Crusher
        Skills.Add(new skill(61024, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 9f, 0.2f, new float[3] { -30f, 90f, 6f }, 12));//decreased def/chance/time
        //Dismember
        Skills.Add(new skill(61025, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 6f, 0.5f, new float[3] { 1.8f, 4f, -50f }, 18));//damage/time/debuff
        //---------------Utility
        //Charge                                                                                                                                                     
        Skills.Add(new skill(61006, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 12f, 0.8f, new float[2] { 50f, 5f }, 14));//buff/time
        //Provoke
        Skills.Add(new skill(61008, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Warrior, 2f, 0.3f, new float[2] { 1f, 90f }, 11));//damage/chance/
        //Battle Shout
        Skills.Add(new skill(61013, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Warrior, 30f, 0.8f, new float[2] { 35f, 15f }, 13));//buff/time
        //Slow down!
        Skills.Add(new skill(61026, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 7f, 0.2f, new float[2] { 40f, 3f }, 10));//debuff/time
        //On your knees!
        Skills.Add(new skill(61027, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Warrior, 4f, 0.2f, new float[2] { 70f, 50f }, 12));//chance/below hp        
        //---------------Champion     
        //Shield Stun
        Skills.Add(new skill(61003, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 5f, 0.3f, new float[2] { 1.8f, 51f }, 19));//damage/chance
        //Soul Cravings
        Skills.Add(new skill(61010, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Warrior, 4f, 0.3f, new float[2] { 1.95f, 30f }, 21));//damage/heal
        //Ultimate Defense
        Skills.Add(new skill(61020, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 16f, 0.2f, new float[2] { 40f, 10f }, 10));//buff/time
        //Arrow Deflect
        Skills.Add(new skill(61028, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 20f, 0.2f, new float[2] { 40f, 6f }, 11));//buff/time
        //Shields Up
        Skills.Add(new skill(61029, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Warrior, 7f, 0.2f, new float[1] { 9f }, 18));//time

        //-----Wizard            
        //---------------Fire
        //Fireball
        Skills.Add(new skill(62001, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 5f, 2f, 2.5f, 25));
        //Flame missile
        Skills.Add(new skill(62002, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 7f, 2f, new float[2] { 2.3f, 80f }, 28));//damage/chance
        //Burn
        Skills.Add(new skill(62003, skill.SAction.active, skill.Stype.target_DOT, PlayerStats.PlayerClass.Wizard, 4f, 0.5f, 2.5f, 22));
        //Meteor Rain
        Skills.Add(new skill(62004, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Wizard, 12f, 5f, 3.0f, 30));
        //Lava Barrier
        Skills.Add(new skill(62005, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Wizard, 16f, 4f, 2.0f, 20));
        //---------------Ice
        //Ice Spear
        Skills.Add(new skill(62006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 1f, 0.6f, 1.8f, 23));
        //Frost Blade
        Skills.Add(new skill(62007, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 7f, 2f, new float[2] { 2.15f, 70f }, 24));//damage/chance
        //Frozen Hands
        Skills.Add(new skill(62008, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 30f, 1f, new float[1] { 16f }, 19));//time
        //Blizzard
        Skills.Add(new skill(62009, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Wizard, 18f, 1f, new float[3] { 1.5f, 50f, 90f }, 32));//damage/chance to freeze/chance to slow
        //Frost Bomb
        Skills.Add(new skill(62010, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Wizard, 5f, 1f, new float[2] { 2.2f, 70f }, 24));//damage//chance
        //---------------Util
        //Corpse Life Drain
        Skills.Add(new skill(62011, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Wizard, 3f, 0.5f, new float[2] { 20f, 30f }, 20));//effect/time
        //Mana shield
        Skills.Add(new skill(62012, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 60f, 0.3f, 45f, 18));//time
        //Expanded Mana
        Skills.Add(new skill(62013, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 60f, 0.5f, new float[2] { 60f, 45f }, 22));//effect/time
        //Caster Contract
        Skills.Add(new skill(62014, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 30f, 0.8f, new float[1] { 6f }, 27));//time
        //Concentration
        Skills.Add(new skill(62015, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Wizard, 12f, 0.3f, new float[2] { 35f, 9f }, 25));//effect/time

        //-----Hunter            
        //---------------Marskman
        //Snipe
        Skills.Add(new skill(63001, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 3f, 0.2f, 1.5f, 20));
        //Hawkeye
        Skills.Add(new skill(63002, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 16f, 0.2f, new float[2] { 30f, 10f }, 12));//buff/time
        //Multishot
        Skills.Add(new skill(63003, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Hunter, 8f, 2f, new float[2] { 1.4f, 5f }, 30));//damage//targets
        //Steady shot
        Skills.Add(new skill(63004, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 0.9f, 0.2f, 1.3f, 25));
        //Hamstring shot
        Skills.Add(new skill(63005, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 4f, 0.2f, new float[3] { 1.4f, 100f, 20f }, 15));//damage/chance/effect
        //---------------Survival
        //Poison Arrow
        Skills.Add(new skill(63006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Hunter, 4f, 0.4f, new float[2] { 1.6f, 80f }, 13));//damage/chance
        //Multi trap
        Skills.Add(new skill(63007, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 9f, 1f, 2f, 20));
        //Posion Trap
        Skills.Add(new skill(63008, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 6f, 0.3f, 2.4f, 18));//damage
        //Steel trap
        Skills.Add(new skill(63009, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 9f, 0.5f, 1.5f, 20));
        //Booby Trap
        Skills.Add(new skill(63010, skill.SAction.active, skill.Stype.trap, PlayerStats.PlayerClass.Hunter, 6f, 0.2f, 1.8f, 19));
        //---------------Util
        //Hunters Mark
        Skills.Add(new skill(63011, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Hunter, 10f, 0.2f, new float[3] { 6f, 50f, 30f }, 15));//time//threshold/extra damage
        //Hunters Ritual
        Skills.Add(new skill(63012, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Hunter, 12f, 0.2f, new float[2] { 50f, 5f }, 17));//buff/time
        //Camouflage
        Skills.Add(new skill(63013, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 20f, 1f, 5f, 20));
        //Soul Sacrifice
        Skills.Add(new skill(63014, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 10f, 0.3f, new float[2] { 40f, 90f }, 2));//mp required//% to recover from mp burned
        //Acrobatics
        Skills.Add(new skill(63015, skill.SAction.active, skill.Stype.selfBuff, PlayerStats.PlayerClass.Hunter, 20f, 0.4f, new float[2] { 100f, 3f }, 20));//effect/time

        //-----Paladin            
        //---------------Protection
        //Final Protection
        Skills.Add(new skill(64001, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 20f, 1f, 35f, 20));
        //Magic Protection totem
        Skills.Add(new skill(64002, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 3f, 0.2f, 35f, 15));
        //Physical Protection totem
        Skills.Add(new skill(64003, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 3f, 0.2f, 35f, 15));
        //Linked Hearts
        Skills.Add(new skill(64004, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 14f, 1f, new float[2] { 10f, 70f }, 13));//time/absorb
        //burn on touch buff
        Skills.Add(new skill(64005, skill.SAction.active, skill.Stype.AOE_buff, PlayerStats.PlayerClass.Paladin, 4f, 1.5f, new float[2] { 1.5f, 12f }, 20));//damage/time
        //Holy Light
        Skills.Add(new skill(64006, skill.SAction.active, skill.Stype.target_damage, PlayerStats.PlayerClass.Paladin, 3f, 0.5f, 2.0f, 12));
        //Self Heal
        Skills.Add(new skill(64007, skill.SAction.active, skill.Stype.selfHeal_over_time, PlayerStats.PlayerClass.Paladin, 5f, 0.8f, 0.5f, 13));
        //Heal totem
        Skills.Add(new skill(64008, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 10f, 0.5f, 70f, 16));
        //Resurrection
        Skills.Add(new skill(64009, skill.SAction.active, skill.Stype.AOE_revive, PlayerStats.PlayerClass.Paladin, 30f, 5f, 50f, 22));
        //Cleanse
        Skills.Add(new skill(64010, skill.SAction.active, skill.Stype.AOE_cleanse, PlayerStats.PlayerClass.Paladin, 8f, 1f, 2f, 13));
        //Mana totem
        Skills.Add(new skill(64011, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 15f, 1f, new float[1] { 30f }, 21));//value
        //Silence
        Skills.Add(new skill(64012, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Paladin, 15f, 1f, new float[2] { 80f, 5f }, 18));//chance/time
        //Speed totem
        Skills.Add(new skill(64013, skill.SAction.active, skill.Stype.totem_spawn, PlayerStats.PlayerClass.Paladin, 20f, 2f, new float[2] { 40f, 10f }, 12));//effect/time
        //Buff remover
        Skills.Add(new skill(64014, skill.SAction.active, skill.Stype.target_debuff, PlayerStats.PlayerClass.Paladin, 8f, 0.5f, 70f, 12));//chance
        //Remember me->changed to Consecration
        Skills.Add(new skill(64015, skill.SAction.active, skill.Stype.AOE_damage, PlayerStats.PlayerClass.Paladin, 5f, 0.3f, 1.8f, 25));

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
