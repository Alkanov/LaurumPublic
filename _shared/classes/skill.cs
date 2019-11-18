
[System.Serializable]
public class skill
{
    public int SkillID;  
    public SAction action;
    public Stype type;
    public PlayerStats.PlayerClass classRequired;
    public float cd;
    public float casting;
    public float[] multipliers;
    public int baseMana;  

    public enum Stype
    {
        target_damage,
        AOE_buff,
        selfBuff,
        AOE_damage,
        trap,
        selfHeal_over_time,
        statBuff,
        AOE_heal,
        AOE_cleanse,
        target_DOT,
        AOE_revive,
        target_debuff,
        decoy,
        target_action,//like cancel target's target
        totem_spawn
    }
    public enum SAction
    {
        active,
        passive
    }

    /// <summary>
    /// Skills with ONE multiplier
    /// </summary>
    public skill(int skillID, SAction action, Stype type, PlayerStats.PlayerClass classRequired, float cd, float casting, float dmgModifier, int baseMana)
    {
        SkillID = skillID;
        this.action = action;
        this.type = type;
        this.classRequired = classRequired;
        this.cd = cd;
        this.casting = casting;
        this.multipliers = new float[1] { dmgModifier };
        this.baseMana = baseMana;       
    }
    /// <summary>
    /// Skills with TWO OR MORE multipliers
    /// </summary>
    public skill(int skillID,  SAction action, Stype type, PlayerStats.PlayerClass classRequired, float cd, float casting, float[] dmgModifier, int baseMana)
    {
        SkillID = skillID;       
        this.action = action;
        this.type = type;
        this.classRequired = classRequired;
        this.cd = cd;
        this.casting = casting;
        this.multipliers = dmgModifier;
        this.baseMana = baseMana;       
    }
    /// <summary>
    /// Passive Skills
    /// </summary>
    public skill(int skillID, Stype type, float[] dmgModifier)
    {
        SkillID = skillID;       
        this.action = SAction.passive;
        this.type = type;
        this.classRequired = PlayerStats.PlayerClass.Any;
        this.multipliers = dmgModifier;       
    }

    public skill()
    {
        SkillID = -1;
    }
}