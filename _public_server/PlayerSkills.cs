using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : NetworkBehaviour
{
    #region Player
    PlayerAccountInfo PlayerAccountInfo;
    PlayerSkillsActions PlayerSkillsActions;
    PlayerStats PlayerStats;
    PlayerConditions PlayerConditions;
    PlayerGeneral PlayerGeneral;
    #endregion

    #region Skill Data      
    public List<skill> equipped_skills = new List<skill>();
    [HideInInspector]
    public NetworkIdentity NetworkIdentity;
    #endregion


    private void Awake()
    {
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerAccountInfo = GetComponent<PlayerAccountInfo>();
        PlayerSkillsActions = GetComponent<PlayerSkillsActions>();
        NetworkIdentity = GetComponent<NetworkIdentity>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerConditions = GetComponent<PlayerConditions>();
        skill null_skill = null;
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
    }




    #region Networking Client
    [TargetRpc]
    public void TargetLoadEquippedSkills(NetworkConnection target, int[] equ_skills) { }
    #endregion

    #region Networking Client RPC    
    [ClientRpc]
    public void RpcIsCasting(float time){}
    [ClientRpc]
    public void Rpc_castingDone(){}
    #endregion

    #region Networking Server  
    [Command]
    public void CmdUseSkill(int skillpos, GameObject target)
    {
        if (PlayerStats.CurrentHP > 0 && PlayerStats.CurrentMP > 0 && !PlayerConditions.stunned && !PlayerSkillsActions.is_casting && !PlayerConditions.silence)
        {
            PlayerConditions.RevealPlayer();
            var skillid = equipped_skills[skillpos].SkillID;
            var skillrequested = equipped_skills[skillpos];
            if (checkSkillRequisites(skillrequested))
            {
                if (PlayerSkillsActions.Skills_in_CoolDown.ContainsKey(skillpos))
                {
                    //necesito saber hace cuanto uso este skill
                    var time_since = Time.time - PlayerSkillsActions.Skills_in_CoolDown[skillpos];
                    var cdtocompare = skillrequested.cd * (1f - (PlayerStats.CD_reduction / 100));
                    cdtocompare = cdtocompare / 1.2f;//20% less to compensate for lag
                    if (time_since >= cdtocompare)
                    {
                        ////Debug.LogError("skill_cd:" + skillrequested.cd + " time_since:" + time_since + " Time.time:" + Time.time + " Skills_in_CoolDown[skillid]:" + PlayerSkillsActions.Skills_in_CoolDown[skillid]);
                        PlayerSkillsActions.Skills_in_CoolDown.Remove(skillpos);
                        notInCD_continueWithSkill(target, skillrequested, skillpos);
                    }
                    /*else
                    {
                        //todavia en cd
                        // //Debug.LogError("skill_cd:"+ skillrequested .cd+ " time_since:" + time_since + " Time.time:" + Time.time + " Skills_in_CoolDown[skillid]:" + PlayerSkillsActions.Skills_in_CoolDown[skillid]);
                    }*/
                }
                else
                {
                    /*foreach (var item in PlayerSkillsActions.Skills_in_CoolDown)
                    {
                        // //Debug.LogError("skill_cd:" + skillrequested.cd + " skillid:" + skillid + " PlayerSkillsActions.Skills_in_CoolDown.Count:" + item.Key.ToString());
                    }*/
                    notInCD_continueWithSkill(target, skillrequested, skillpos);
                }
            }

        }
    }
    #endregion

    #region Utilidades  
    bool checkSkillRequisites(skill skillrequested)
    {
        bool allowed;
        switch (skillrequested.SkillID)
        {
            case 1030://Final Frenzy - vida menor a 30%
                var hpleftPercent = PlayerStats.CurrentHP * 100 / PlayerStats.MaxHealth;
                if (hpleftPercent <= 40f)
                {
                    allowed = true;
                }
                else
                {
                    allowed = false;
                }
                break;
            default:
                allowed = true;
                break;
        }

        return allowed;
    }
    public void sendEquippedSkillstoClient()
    {
        int[] skills_equipped = new int[equipped_skills.Count];
        for (int i = 0; i < equipped_skills.Count; i++)
        {
            if (equipped_skills[i] != null)
            {
                skills_equipped[i] = equipped_skills[i].SkillID;
            }
            else
            {
                skills_equipped[i] = 0;
            }
        }
        TargetLoadEquippedSkills(connectionToClient, skills_equipped);
    }
    public void clear_equipped_skills()
    {
        equipped_skills.Clear();
        skill null_skill = null;
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        equipped_skills.Add(null_skill);
        sendEquippedSkillstoClient();
    }
    #endregion

    #region Skill Related
    private void notInCD_continueWithSkill(GameObject target, skill skillrequested, int skillpos)
    {
        switch (skillrequested.type)
        {
            case skill.Stype.target_damage:
                if (target != null)
                {
                    PlayerSkillsActions.CheckSkillClass(target, skillrequested, skillpos);
                }
                break;
            case skill.Stype.target_DOT:
                if (target != null)
                {
                    PlayerSkillsActions.CheckSkillClass(target, skillrequested, skillpos);
                }
                break;
            case skill.Stype.target_debuff:
                if (target != null)
                {
                    PlayerSkillsActions.CheckSkillClass(target, skillrequested, skillpos);
                }
                break;
            case skill.Stype.target_action:
                if (target != null)
                {
                    PlayerSkillsActions.CheckSkillClass(target, skillrequested, skillpos);
                }
                break;
            default:
                PlayerSkillsActions.CheckSkillClass(null, skillrequested, skillpos);
                break;
        }
    }
    void clear_passives()
    {
        PlayerStats.passive_MaxHP = 0;
        PlayerStats.passive_MaxMP = 0;
        PlayerStats.passive_MPRegen = 0;
        PlayerStats.passive_HPRegen = 0;
        PlayerStats.passive_CritChance = 0;
        PlayerStats.passive_CritDmg = 0;
        PlayerStats.passive_WalkingSpeed = 0;
        PlayerStats.passive_Casting = 0;
        PlayerStats.passive_CD_redux = 0;
        PlayerStats.passive_exp_loss_redux = 0;
        PlayerStats.passive_off_hand_mastery = 0;
        PlayerStats.passive_atk_speed = 0;
        PlayerStats.passive_mana_usage_redux = 0;
        PlayerStats.passive_dodge = 0;
        PlayerStats.ProcessStats();
    }
    #endregion




}
