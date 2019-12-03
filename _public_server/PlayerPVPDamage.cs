using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerPVPDamage : NetworkBehaviour
{
    #region Networking
    [HideInInspector]
    [SyncVar]
    public bool isPVPenabled;
    [HideInInspector]
    [SyncVar]
    public bool isInArena;
    #endregion

    #region PVP Data
    float pvp_damage_factor = 1f;
    [HideInInspector]
    public string PVPmodeOn;
    [HideInInspector]
    public bool[] takingAutoAttackDamage = new bool[100];
    [HideInInspector]
    public List<GameObject> AttackersList = new List<GameObject>();//limpiar cuando muere
    List<float> PlayerDamageDone = new List<float>();//limpiar cuando muere
    int PlayerinFight;
    [HideInInspector]
    public float DEFvalue;
    [HideInInspector]
    public bool PlayerFrozen_byAdmin;
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    #endregion

    #region Player
    PlayerStats PlayerStats;
    PlayerDeath PlayerDeath;
    PlayerAccountInfo PlayerAccountInfo;
    PlayerSkills PlayerSkills;
    PlayerConditions PlayerConditions;
    PlayerInventory PlayerInventory;
    PlayerGeneral PlayerGeneral;
    PlayerStatistics PlayerStatistics;
    PlayerMPSync PlayerMPSync;
    PlayerSkillsActions PlayerSkillsActions;
    PlayerGuild PlayerGuild;
    #endregion

    [SerializeField]
    public GameObject droppedItemPrefab;


    private void Awake()
    {
        PVPmodeOn = "HuntMode";
        PlayerStatistics = GetComponent<PlayerStatistics>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerInventory = GetComponent<PlayerInventory>();
        PlayerAccountInfo = GetComponent<PlayerAccountInfo>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerDeath = GetComponent<PlayerDeath>();
        PlayerSkills = GetComponent<PlayerSkills>();
        PlayerConditions = GetComponent<PlayerConditions>();
        PlayerMPSync = GetComponent<PlayerMPSync>();
        PlayerSkillsActions = GetComponent<PlayerSkillsActions>();
        PlayerGuild = GetComponent<PlayerGuild>();
        PlayerFrozen_byAdmin = false;
        DEFvalue = 0.8f;
    }


    #region Networking Client
    [TargetRpc]
    public void TargetsetPVPtoggle(NetworkConnection target, bool mode)
    {

    }
    #endregion

    #region Networking Server
    [Command]
    public void CmdsetToggleModeOnServer(string mode)
    {
        PVPmodeOn = mode;
    }
    [Command]
    public void CmdPVPTageDamageinServer(GameObject AutoAttackThisEnemy)
    {
        if (AutoAttackThisEnemy.GetComponent<PlayerPVPDamage>().isPVPenabled && isPVPenabled && PlayerStats.CurrentHP > 0 && !PlayerGeneral.using_teleport && !PlayerSkillsActions.is_casting)
        {
            if (isInArena)
            {
                if (PlayerSkillsActions.arena_should_affect_target(gameObject, AutoAttackThisEnemy, PlayerSkillsActions.target_modes.outside_my_team_only))
                {
                    AutoAttackThisEnemy.GetComponent<PlayerPVPDamage>().TakeAutoAttackDamage(this.gameObject);
                    TriggerSkillOnAttack();
                }
            }
            else
            {
                if (PlayerGeneral.PartyID == string.Empty && AutoAttackThisEnemy.GetComponent<PlayerGeneral>().PartyID == string.Empty)
                {
                    AutoAttackThisEnemy.GetComponent<PlayerPVPDamage>().TakeAutoAttackDamage(this.gameObject);
                    TriggerSkillOnAttack();
                }
                else if (AutoAttackThisEnemy.GetComponent<PlayerGeneral>().PartyID != PlayerGeneral.PartyID)
                {
                    AutoAttackThisEnemy.GetComponent<PlayerPVPDamage>().TakeAutoAttackDamage(this.gameObject);
                    TriggerSkillOnAttack();
                }
            }

            PlayerConditions.RevealPlayer();
        }
    }
    #endregion


    #region Utilidades
    public void setPVPtoggle(bool mode)
    {

        TargetsetPVPtoggle(connectionToClient, mode);

    }   
    string randomString()
    {
        int minCharAmount = 10;
        int maxCharAmount = 12;
        string myString = "";
        int charAmount = Random.Range(minCharAmount, maxCharAmount + 1); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        return myString;
    }
    #endregion


    #region Auto Attack
    public void TakeAutoAttackDamage(GameObject fromPlayer)
    {
        //if Player attacking is alive and player receiving is alive
        if (fromPlayer.GetComponent<PlayerStats>().CurrentHP > 0 && PlayerStats.CurrentHP > 0)
        {
            //if attacking player is not in the list of attackers add him
            if (!AttackersList.Contains(fromPlayer))
            {
                AttackersList.Add(fromPlayer);
                PlayerDamageDone.Add(0);
            }
            //get the index of the player who attacked you, this is used to know when the auto attack cooldown is over
            PlayerinFight = AttackersList.IndexOf(fromPlayer);

            //Raycast to avoid being hit over walls
            if (PlayerGeneral.x_ObjectHelper.Raycast_didItHit(fromPlayer, gameObject, Vector2.Distance(transform.position, fromPlayer.transform.position), LayerMask.GetMask("Player", "Coliders")))
            {
                //flag this attacker as "attacking"
                takingAutoAttackDamage[PlayerinFight] = true;
                //continue here
                StartCoroutine(TakingAutoAttackDamage(fromPlayer, PlayerinFight, fromPlayer.GetComponent<PlayerStats>().AutoAtk_speed));
            }

        }

    }
    IEnumerator TakingAutoAttackDamage(GameObject fromPlayer, int PlayerinFight, float PlayerAutoAttackSpeed)
    {
        //if both players are in PVP area and attacker is alive and not frozen by adming (not in use)
        if (isPVPenabled && fromPlayer.GetComponent<PlayerPVPDamage>().isPVPenabled && fromPlayer.GetComponent<PlayerStats>().CurrentHP > 0 && !PlayerFrozen_byAdmin)
        {
            bool Critico = false;
            bool dodged = false;

            //get incomming damage from attacker player
            float DamageRX = CalculateDamageRx(fromPlayer);
            
            float playerCriticalChance = fromPlayer.GetComponent<PlayerStats>().Critical_chance;
            if(playerCriticalChance > 50f){
                playerCriticalChance = 50f;
            }

            //critical chance lottery
            if (Random.Range(0f, 100f) <= playerCriticalChance)
            {
                Critico = true;
                float critMultiplier = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Crit_Multiplier].value;
                DamageRX = DamageRX * (critMultiplier + fromPlayer.GetComponent<PlayerStats>().Critical_damage);
            }        

	    	float playerDodgeChance = PlayerStats.Dodge_chance;
            if(playerDodgeChance > 50f){
                playerDodgeChance = 50f;
            }
            //dodge chance lottery
            if (Random.Range(0f, 100f) <= playerDodgeChance)
            {
                DamageRX = 0f;
                dodged = true;
            }

            //if damage was not dodged and damage is above 0 (means DEF/MDEF was not higher than damage)
            if (DamageRX > 0f && !dodged)
            {
                //Calculate reflect if player has any reflect mode on him
                var reflectSTR = (float) PlayerStats.modReflectSTR * DamageRX / 100f;
                if (reflectSTR > 0f)
                {
                    int toReflect = Mathf.RoundToInt(reflectSTR);
                    //reflect the damage straight away - player attacking cannot dodge/defend himself from this damage
                    fromPlayer.GetComponent<PlayerStats>().CurrentHP -= toReflect;
                    //send animation to client (damage numbers)
                    PlayerGeneral.showCBT(fromPlayer, false, false, toReflect, "reflect");
                }
                //--------------------skill/passives checks
                //if attacker has frozen hands
                if (fromPlayer.GetComponent<PlayerConditions>().has_buff_debuff(PlayerConditions.type.buff, 17))
                {
                    PlayerConditions.decreasedWalkingSpeed = -20f;//-20% walking speed
                    PlayerStats.RefreshStats();
                    PlayerConditions.add_buff_debuff(2, null, false, 3f, fromPlayer, PlayerConditions.type.debuff, true);
                }
                //Linked hearts
                var buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.buff, 20);
                if (buff_info != null)
                {
                    if (buff_info.skill_owner != null && buff_info.skill_owner.GetComponent<PlayerStats>().CurrentHP > 0)
                    {
                        //damage to paladin
                        var linked_damage = Mathf.RoundToInt(DamageRX * buff_info.skill_requested.multipliers[1] / 100f);
                        buff_info.skill_owner.GetComponent<PlayerStats>().hpChange(-linked_damage);
                        buff_info.skill_owner.GetComponent<PlayerGeneral>().showCBT(buff_info.skill_owner, false, false, linked_damage, "damage");
                        //damage portion
                        DamageRX -= linked_damage;
                    }
                    else
                    {
                        PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 20);
                    }
                }
                //Burn on touch
                buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.buff, 21);
                if (buff_info != null)
                {
                    //PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 21);
                    fromPlayer.GetComponent<PlayerConditions>().handle_effect(DOT_effect.effect_type.fire, buff_info.skill_requested.multipliers[0], buff_info.skill_owner, 0);
                }
            }

            int damageToTake = Mathf.RoundToInt(DamageRX);
            //take the damage
            takeDamageinPVPNow(damageToTake, fromPlayer, Critico);


            //send auto attack animation to client
            PlayerGeneral.send_autoATK_animation(fromPlayer, gameObject);
            //send damage number animation to client
            PlayerGeneral.showCBT(gameObject, Critico, dodged, damageToTake, "damage");
            //add damage this player did to list (dont remember using this anywhere)
            PlayerDamageDone[PlayerinFight] = PlayerDamageDone[PlayerinFight] + damageToTake;

            //once the auto attack cool down finishes tag this player as free to attack again
            yield return new WaitForSeconds(PlayerAutoAttackSpeed);
            takingAutoAttackDamage[PlayerinFight] = false;
        }

    }
    float CalculateDamageRx(GameObject fromPlayer)
    {
        float DamageRX = 0f;
        PlayerStats fromPlayerStats = fromPlayer.GetComponent<PlayerStats>();
        float playerTotalDef = 0f;
        float fromPlayerDamage = 0f;
        
        bool USE_NEW_FORMULA = Mathf.RoundToInt(PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.Use_New_PVP_Formula].value) == 1;
        float NERF_FINAL = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_FinalDmg_Nerf].value;
        float NERF_DEFENSE = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Defense_Nerf].value;
        float NERF_DAMAGE = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Damage_Nerf].value;

        switch (fromPlayerStats.DamageType_now)
        {
            case PlayerStats.DamageType.magical:
                playerTotalDef = PlayerStats.Defense_int;
                fromPlayerDamage = fromPlayerStats.Damage_int;
                break;
            case PlayerStats.DamageType.physical:
                playerTotalDef = PlayerStats.Defense_str;
                fromPlayerDamage = fromPlayerStats.Damage_str;
                break;
            default:
                break;
        }

        if(USE_NEW_FORMULA)
        {
            DamageRX = (Mathf.Pow(fromPlayerDamage, NERF_DAMAGE) - Mathf.Pow(playerTotalDef, NERF_DEFENSE)) * NERF_FINAL;
        }
        else{
            DamageRX = (fromPlayerDamage * NERF_DAMAGE - playerTotalDef * NERF_DEFENSE) * NERF_FINAL;
        }

        //if damage is below 0 make sure to return 0, a negative number here would heal the player instead (100HP-(-100 damage)=200)
        if (DamageRX < 0f)
        {
            DamageRX = 0f;
        }

        //random a number between -10%/+10% from damage received to keep number aleatory
        DamageRX = Random.Range(DamageRX * 0.9f, DamageRX * 1.1f);

        //arrow deflect
        if (PlayerConditions.buffs.Contains(15) && fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Hunter)//arrow deflect buff
        {
            DamageRX = DamageRX * (1f - (PlayerConditions.get_buff_information(PlayerConditions.type.buff, 15).skill_requested.multipliers[0] / 100f));
        }
        //shields up
        if (PlayerConditions.buffs.Contains(16) && (fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Hunter || fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Warrior))
        {
            PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 16);
            DamageRX = 0f;
        }

        return DamageRX;
    }
    #endregion

    #region Skills
    public void TakeSkillDamage(GameObject fromPlayer, skill thisSkill)
    {
        afterPartyChecks(fromPlayer, thisSkill);
    }
    private void afterPartyChecks(GameObject fromPlayer, skill skillRequested)
    {
        //////////.LogError("DamageSkill");
        if (fromPlayer.GetComponent<PlayerStats>().CurrentHP > 0 && !PlayerFrozen_byAdmin)
        {
            //////////.LogError("DamageSkill");
            bool Critico = false;
            bool dodged = false;
            int finalNumber = 0;
            string damageType = "null";

            if (PlayerStats.CurrentHP > 0)
            {
                if (skillRequested.type == skill.Stype.AOE_heal)// || skill.type == skill.Stype.AOEheal
                {
                    /*damageType = "heal";
                    float healRX = fromPlayer.GetComponent<PlayerStats>().Damage_int * skillRequested.multipliers[0];
                    float playerCriticalChance = fromPlayer.GetComponent<PlayerStats>().DEX * 0.05f;
                    playerCriticalChance = playerCriticalChance + fromPlayer.GetComponent<PlayerStats>().modCritChance + fromPlayer.GetComponent<PlayerStats>().passive_CritChance;
                    if (playerCriticalChance >= 45f)
                    {
                        playerCriticalChance = 45f;
                    }
                    playerCriticalChance = playerCriticalChance + PlayerConditions.increasedCritical;
                    if (Random.Range(0f, 100f) <= playerCriticalChance)
                    {
                        Critico = true;
                        healRX = Mathf.RoundToInt((healRX * 1.35f) * (1f + ((fromPlayer.GetComponent<PlayerStats>().modCritDmg + fromPlayer.GetComponent<PlayerStats>().passive_CritDmg) / 100f)));
                    }

                    finalNumber = Mathf.RoundToInt(healRX);
                    PlayerStats.CurrentHP += finalNumber;*/
                    //////////.LogError("DamageSkill");
                }
                else if (skillRequested.type == skill.Stype.selfHeal_over_time)
                {
                    if (skillRequested.SkillID == 64007)//self heal
                    {
                        PlayerConditions.handle_effect(DOT_effect.effect_type.heal, skillRequested.multipliers[0], fromPlayer, 0);
                    }

                }
                else if (skillRequested.type == skill.Stype.AOE_cleanse)
                {
                    ///manejar cleanse
                    damageType = "clean";
                    finalNumber = PlayerConditions.cleanDebuffs((int)skillRequested.multipliers[0]);
                }
                else if (skillRequested.type == skill.Stype.AOE_buff)
                {
                    PlayerConditions.handle_buffs_debuffs(skillRequested, fromPlayer);
                }
                else if (skillRequested.type == skill.Stype.target_action)
                {
                    //if this is a debuff we only send animation - no damage                   
                }
                else if (skillRequested.type != skill.Stype.AOE_revive)
                {
                    if (skillRequested.type == skill.Stype.target_DOT)
                    {
                        damageType = "DOT";
                        switch (skillRequested.SkillID)
                        {
                            case 62003://burn - wizard
                                PlayerConditions.handle_effect(DOT_effect.effect_type.fire, skillRequested.multipliers[0], fromPlayer, 0);
                                break;
                            case 61004://bleed - warrior
                                PlayerConditions.handle_effect(DOT_effect.effect_type.bleed, skillRequested.multipliers[0], fromPlayer, 0);
                                break;
                            case 63006://poison arrow - hunter
                                PlayerConditions.handle_effect(DOT_effect.effect_type.poison, skillRequested.multipliers[0], fromPlayer, 0);
                                break;
                            default:
                                break;
                        }
                        takeDamageinPVPNow(0, fromPlayer, false);
                    }
                    else if (skillRequested.type == skill.Stype.target_debuff)
                    {
                        damageType = "debuff";
                        takeDamageinPVPNow(0, fromPlayer, false);
                    }
                    else if (skillRequested.type == skill.Stype.AOE_damage || skillRequested.type == skill.Stype.target_damage)
                    {
                        //////////.LogError("DamageSkill");
                        damageType = "damage";
                        float DamageRX = CalculateSkillDamageRx(fromPlayer, skillRequested.multipliers[0]);
                        
                        float playerCriticalChance = fromPlayer.GetComponent<PlayerStats>().Critical_chance;
                        if(playerCriticalChance > 50f){
                            playerCriticalChance = 50f;
                        }

                        if (Random.Range(0f, 100f) <= playerCriticalChance)
                        {
                            Critico = true;
                        }

                        if (skillRequested.SkillID == 61016)//execution
                        {
                            if (PlayerStats.CurrentHP / PlayerStats.MaxHealth <= skillRequested.multipliers[1] / 100f)
                            {
                                Critico = true;
                            }
                        }
                        if (Critico)
                        {  
                            float critMultiplier = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Crit_Multiplier].value;
                            DamageRX = DamageRX * (critMultiplier + fromPlayer.GetComponent<PlayerStats>().Critical_damage);
                        }

	    			    float playerDodgeChance = PlayerStats.Dodge_chance;
                        if(playerDodgeChance > 50f){
                            playerDodgeChance = 50f;
                        }

	                    if (Random.Range(0f, 100f) <= playerDodgeChance) //JWR - Use adjusted dodge chance
                        {
                            DamageRX = 0f;
                            dodged = true;
                        }

                        if (DamageRX < 0f)
                        {
                            DamageRX = 0f;
                        }

                        if (!PlayerConditions.immortal && !dodged)
                        {
                            //soul cravings
                            var buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.debuff, 1);
                            if (skillRequested.SkillID == 61010 && buff_info != null)//stunned
                            {
                                PlayerConditions.remove_buff_debuff(PlayerConditions.type.debuff, 1);

                                int to_heal = Mathf.RoundToInt(DamageRX * skillRequested.multipliers[1] / 100f);
                                fromPlayer.GetComponent<PlayerStats>().hpChange(to_heal);
                                fromPlayer.GetComponent<PlayerGeneral>().showCBT(fromPlayer, false, false, to_heal, "heal");
                            }
                            //flame missile
                            if (skillRequested.SkillID == 62002)
                            {
                                if (Random.Range(0f, 100f) <= skillRequested.multipliers[1])
                                {
                                    PlayerConditions.handle_effect(DOT_effect.effect_type.fire, skillRequested.multipliers[0], fromPlayer, 0);
                                }
                            }
                            //posion arrow
                            if (skillRequested.SkillID == 63006)
                            {
                                if (Random.Range(0f, 100f) <= skillRequested.multipliers[1])
                                {
                                    PlayerConditions.handle_effect(DOT_effect.effect_type.poison, skillRequested.multipliers[0], fromPlayer, 0);
                                }
                            }
                            /*buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.debuff, 14);
                            if (buff_info != null)//if has Hunter's Mark
                            {
                                if ((float)PlayerStats.CurrentHP / (float)PlayerStats.MaxHealth <= buff_info.skill_requested.multipliers[1] / 100f)
                                {
                                    DamageRX = DamageRX * (1f + (buff_info.skill_requested.multipliers[2] / 100f));
                                }
                            }*/
                            //Linked hearts
                            buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.debuff, 20);
                            if (buff_info != null)
                            {
                                if (buff_info.skill_owner != null && buff_info.skill_owner.GetComponent<PlayerStats>().CurrentHP > 0)
                                {
                                    //damage to paladin
                                    var linked_damage = Mathf.RoundToInt(DamageRX * buff_info.skill_requested.multipliers[1] / 100f);
                                    buff_info.skill_owner.GetComponent<PlayerStats>().hpChange(-linked_damage);
                                    buff_info.skill_owner.GetComponent<PlayerGeneral>().showCBT(buff_info.skill_owner, false, false, linked_damage, "damage");
                                    //damage portion
                                    DamageRX -= linked_damage;

                                }
                                else
                                {
                                    PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 20);
                                }
                            }
                            //Burn on touch
                            buff_info = PlayerConditions.get_buff_information(PlayerConditions.type.buff, 21);
                            if (buff_info != null)
                            {
                                //PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 21);
                                fromPlayer.GetComponent<PlayerConditions>().handle_effect(DOT_effect.effect_type.fire, buff_info.skill_requested.multipliers[0], buff_info.skill_owner, 0);
                            }
                            if (skillRequested.SkillID == 61008)//provoke
                            {
                                if (Random.Range(0f, 100f) <= skillRequested.multipliers[1])
                                {
                                    damageType = "Provoked!";
                                    PlayerGeneral.TargetUntargetMe(connectionToClient, fromPlayer, 0);//time not used
                                }
                                else
                                {
                                    damageType = "Ignored";
                                }
                            }


                        }

                        int damageToTake = Mathf.RoundToInt(DamageRX);
                        takeDamageinPVPNow(damageToTake, fromPlayer, Critico);
                        finalNumber = damageToTake;
                    }
                }
            }
            else
            {
                if (skillRequested.type == skill.Stype.AOE_revive)
                {
                    ///manejar revival               
                    if (PlayerStats.CurrentHP <= 0)
                    {
                        damageType = "Revived";
                        var key = randomString();
                        PlayerDeath.TargetSendRevival(connectionToClient, key, (int)skillRequested.multipliers[0]);
                        PlayerDeath.RevivalKeys.Add(key);
                    }

                }
            }


            if (damageType != "null")
            {
                PlayerConditions.handle_buffs_debuffs(skillRequested, fromPlayer);
                PlayerGeneral.showCBT(gameObject, Critico, dodged, finalNumber, damageType);
                PlayerGeneral.show_skill_damage_animation(fromPlayer, gameObject, skillRequested);
            }
        }


    }
    public float CalculateSkillDamageRx(GameObject fromPlayer, float power_multiplier)
    {
        float DamageRxAcc = 0f;
        PlayerStats fromPlayerStats = fromPlayer.GetComponent<PlayerStats>();
        float playerTotalDef = 0f;
        float fromPlayerDamage = 0f;
        
        bool USE_NEW_FORMULA = Mathf.RoundToInt(PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.Use_New_PVP_Formula].value) == 1;
        float NERF_FINAL = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_FinalDmg_Nerf].value;
        float NERF_DEFENSE = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Defense_Nerf].value;
        float NERF_DAMAGE = PlayerGeneral.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVP_Damage_Nerf].value;

        switch (fromPlayerStats.DamageType_now)
        {
            case PlayerStats.DamageType.magical: 
                playerTotalDef = PlayerStats.Defense_int;
                fromPlayerDamage = fromPlayerStats.Damage_int * power_multiplier;
                break;
            case PlayerStats.DamageType.physical:         
                playerTotalDef = PlayerStats.Defense_str;
                fromPlayerDamage = fromPlayerStats.Damage_str * power_multiplier;
                break;
            default:
                break;
        }

        if(USE_NEW_FORMULA)
        {
            DamageRxAcc = (Mathf.Pow(fromPlayerDamage, NERF_DAMAGE) - Mathf.Pow(playerTotalDef, NERF_DEFENSE)) * NERF_FINAL;
        }
        else{
            DamageRxAcc = (fromPlayerDamage * NERF_DAMAGE - playerTotalDef * NERF_DEFENSE) * NERF_FINAL;
        }

        //if damage is below 0 make sure to return 0, a negative number here would heal the player instead (100HP-(-100 damage)=200)
        if (DamageRxAcc < 0f)
        {
            DamageRxAcc = 0f;
        }

        //random a number between -10%/+10% from damage received to keep number aleatory
        DamageRxAcc = Random.Range(DamageRxAcc * 0.9f, DamageRxAcc * 1.1f);

        //arrow deflect
        if (PlayerConditions.buffs.Contains(15) && fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Hunter)//arrow deflect buff
        {
            DamageRxAcc = DamageRxAcc * PlayerConditions.get_buff_information(PlayerConditions.type.buff, 15).skill_requested.multipliers[1] / 100f;
        }
        //shields up
        if (PlayerConditions.buffs.Contains(16) && (fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Hunter || fromPlayer.GetComponent<PlayerStats>().PlayerClass_now == PlayerStats.PlayerClass.Warrior))
        {
            PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 16);
            DamageRxAcc = 0f;
        }
        return DamageRxAcc;
    }
    #endregion

    #region Damage
    public void takeDamageinPVPNow(int DamageToTake, GameObject fromPlayer, bool critical)
    {
        //if player is not immortal - currently given by paladin and while teleporting
        if (!PlayerConditions.immortal)
        {
            //assume hit was missed 
            bool hit = false;
            //add damage done to statistics (damaging player)
            fromPlayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.damage_done_session, DamageToTake);
            //damage taken statistics to damaged player
            PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.damage_taken_session, DamageToTake);


            //if we are in arena
            if (isInArena)
            {
                //is the attacker in arena too?
                if (fromPlayer.GetComponent<PlayerPVPDamage>().isInArena)
                {
                    //save how much hp player had before hit
                    var temp_hp = PlayerStats.CurrentHP;
                    //take the hp now
                    PlayerStats.hpChange(-DamageToTake);

                    //did player die?
                    if (PlayerStats.CurrentHP <= 0)
                    {
                        //apply death penalty based on Arena parameter
                        PlayerDeath.DeathExpPenalty("Arena", fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname, fromPlayer);
                        //add kill in arena to player who attacked
                        fromPlayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.arenakills_session, 1);
                        //calculate overkill for server to send it to system chat
                        var overkill = temp_hp - DamageToTake;
                        overkill = overkill * -1;
                        //send msg to chat
                        PlayerGeneral.x_ObjectHelper.IRC_demo.Arena_Death(fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname, PlayerAccountInfo.PlayerNickname, DamageToTake, critical, overkill);
                    }
                }

            }
            else//not in arena = open PVP
            {
                //is player taking damage alive?
                if (PlayerStats.CurrentHP > 0)
                {
                    //take the damage from hp
                    PlayerStats.hpChange(-DamageToTake);
                    //there was a hit
                    hit = true;

                    //Did player hit die?
                    if (PlayerStats.CurrentHP <= 0)
                    {
                        //yeap he ded
                        PlayerDeath.isPlayerDead = true;
                        //apply PVP penalties
                        PlayerDeath.DeathExpPenalty("PVP", fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname, fromPlayer);
                        //add a kill to attacker player statistics
                        fromPlayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.openpvpkills_session, 1);
                        //was this in devil square?
                        if (PlayerGeneral.in_devilSquare)
                        {
                            //inform devil square of one player dead
                            PlayerGeneral.DieNow();
                            //add one DS pvp kills to attacker 
                            fromPlayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.dspvpkills_session, 1);
                        }

                        //Karma checks to know who becomes PK/Hero and what to do with Karma
                        //is player being killed PK?
                        if (PlayerAccountInfo.isPlayerPK) // PK<---PK+CM
                        {
                            //calculate gold to drop 5% current gold
                            //int goldToDrop = Mathf.RoundToInt(PlayerInventory.Gold * 0.05f);

                            //Another PK killed this player PK <---PK 
                            if (fromPlayer.GetComponent<PlayerAccountInfo>().isPlayerPK)
                            {
                                //DropGold, take it from inventory and save it to DB                   
                                //moved to PlayerDeath

                                //save logs to DB to track crybabies
                                //StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, "%web-log%1%" + fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname + "%killed%" + PlayerAccountInfo.PlayerNickname + "%Gold dropped: " + goldToDrop, this.GetComponent<PlayerAccountInfo>().PlayerIP)));
                                //StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, "PK->PK gold dropped: " + goldToDrop + " had: " + PlayerInventory.Gold, this.GetComponent<PlayerAccountInfo>().PlayerIP)));

                            }
                            else//Player dead was PK and killed by a white player or CM=commoner PK <---CM
                            {
                                //DropGold, take it from inventory and save it to DB      
                                //moved to PlayerDeath

                                //save logs to DB to track crybabies
                                //StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, "%web-log%1%" + fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname + "%hunted%" + PlayerAccountInfo.PlayerNickname + "%Gold dropped: " + goldToDrop, this.GetComponent<PlayerAccountInfo>().PlayerIP)));
                                //StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, "Hunt->PK gold dropped: " + goldToDrop + " had: " + PlayerInventory.Gold, this.GetComponent<PlayerAccountInfo>().PlayerIP)));

                                //are we sure it we didnt kill ourselves with a trap?
                                //**I think this check is not necesary because to be ourselves we would need to be commoner or hero, but above we check if we are PK...***
                                if (fromPlayer != gameObject)
                                {
                                    //amount to add to PK-killer and amount to take from PK karma
                                    var heroKarmatemp = 50;
                                    //add to pk-killer
                                    fromPlayer.GetComponent<PlayerAccountInfo>().ModifyKarma(heroKarmatemp, false);
                                    //reduce PK karma by 50
                                    var futureKarma = PlayerAccountInfo.PlayerKarma + 50;
                                    //if when adding 50 karma goes above 0 (hero) then we make it 0 because we dont want a PK becomming Hero, if not then we reduce karma by 50 normally
                                    if (futureKarma <= 0)
                                    {
                                        PlayerAccountInfo.ModifyKarma(50, false);
                                    }
                                    else
                                    {
                                        PlayerAccountInfo.ModifyKarma(0, true);
                                    }
                                }
                            }
                        }
                        else//If player dead is not PK and was attacked by PK,Commoner or Hero CM<---PK+CM
                        {
                            //is player attacking PK?
                            if (fromPlayer.GetComponent<PlayerAccountInfo>().isPlayerPK)// CM<---PK
                            {
                                //save logs for crybabies
                                StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, "%web-log%1%" + fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname + "%murdered%" + PlayerAccountInfo.PlayerNickname, PlayerAccountInfo.PlayerIP)));

                                //Im not killing myself? (not sure if this check is necesary)
                                if (fromPlayer != gameObject)
                                {
                                    string previous_stage = fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage.ToString();

                                    //-300 or more karma to the already PK who killed this player
                                    fromPlayer.GetComponent<PlayerAccountInfo>().ModifyKarma(get_karma_after_calculations(fromPlayer), false);

                                    //announce it if 3rd stage
                                    if (fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage == PlayerAccountInfo.karma_stages.pk_3 && previous_stage!= fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage.ToString())
                                    {
                                        PlayerGeneral.x_ObjectHelper.IRC_demo.submitChat(string.Format("{0} became 3rd stage PK near x:{1} y:{2}", fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname, fromPlayer.transform.position.x, fromPlayer.transform.position.y));
                                    }
                                    
                                }
                            }
                            else// (No one is PK) Player attacking is not a PK CM<---CM
                            {
                                //Im not killing myself? (not sure if this check is necesary)
                                if (fromPlayer != gameObject)
                                {
                                    //if player attacking me was not flagged (purple) - if this player was flagged we know the attacker player was defending himself
                                    if (!PlayerAccountInfo.PKflaggedNow)
                                    {
                                        //save logs
                                        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, "%web-log%1%" + fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname + "%murdered%" + PlayerAccountInfo.PlayerNickname, PlayerAccountInfo.PlayerIP)));
                                        //temp previous pk stage to compare later
                                        string previous_stage = fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage.ToString();
                                        //So.. we are not flagged, none of us is PK, that only means this player was PKed by a commoner/hero player
                                        //attacker now gets -300 or more karma even if he was hero
                                        fromPlayer.GetComponent<PlayerAccountInfo>().ModifyKarma(get_karma_after_calculations(fromPlayer), true);                                       
                                        //announce it if 3rd stage
                                        if (fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage == PlayerAccountInfo.karma_stages.pk_3 && previous_stage != fromPlayer.GetComponent<PlayerAccountInfo>().PlayerKarmaStage.ToString())
                                        {
                                            PlayerGeneral.x_ObjectHelper.IRC_demo.submitChat(string.Format("{0} became 3rd stage PK near x:{1} y:{2}", fromPlayer.GetComponent<PlayerAccountInfo>().PlayerNickname, fromPlayer.transform.position.x, fromPlayer.transform.position.y));
                                        }

                                    }
                                }
                            }

                        }


                    }
                    else//this player took a hit and is still alive
                    {
                        if (hit)//this is always true so im not sure wtf is it doing here
                        {
                            //if player gets damaged by his own trap dont flag him
                            if (fromPlayer != gameObject)
                            {
                                //if this player is not flagged nor is PK
                                if (!PlayerAccountInfo.PKflaggedNow && !PlayerAccountInfo.isPlayerPK)
                                {
                                    //if the attacker is not PK either
                                    if (!fromPlayer.GetComponent<PlayerAccountInfo>().isPlayerPK)
                                    {
                                        //flag attacker player
                                        fromPlayer.GetComponent<PlayerAccountInfo>().PKflagged(this.gameObject);
                                    }

                                }
                            }

                        }

                    }
                }
            }
            //if damage was done or not and this player is on party
            if (PlayerGeneral.PartyID != string.Empty)
            {
                //send an update with new HP/MP/exp... to all party members UI
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }
        }
        //trigger passive skills
        after_damage_skill_triggers();

    }

    private int get_karma_after_calculations(GameObject fromPlayer)
    {
        int karma_calculations = -300;
        if (PlayerGeneral.x_ObjectHelper.game_event_manager.is_event_on(game_event_manager.game_event.guild_wars) == 1f) //guildwars active
        {
            //guild checks
            if (PlayerGuild.PlayerGuildID != 0)//if player killed is in a guild
            {
                if(fromPlayer.GetComponent<PlayerGuild>().PlayerGuildID != 0)//if killer is in a guild too
                {
                    return 0;
                }                
            }          
           
        }
        float level_difference = fromPlayer.GetComponent<PlayerStats>().PlayerLevel / PlayerStats.PlayerLevel;

        if (level_difference > 1f)
        {
            karma_calculations = Mathf.RoundToInt(karma_calculations * level_difference);
        }
        if (Mathf.Abs(karma_calculations) > 3000)
        {
            karma_calculations = -3000;//karma cap to -3000
        }

        return karma_calculations;
    }
    #endregion

    #region skill triggers
    public void after_damage_skill_triggers()
    {
        /*if (PlayerStats.PlayerClass_now == PlayerStats.PlayerClass.Warrior)
        {           
            foreach (var knownskill in PlayerSkills.PlayerKnownSkills)
            {
                if (knownskill.SkillID == 1030)
                { //SKILL-Final Frenzy      
                    if (PlayerStats.CurrentHP / PlayerStats.MaxHealth <= 0.4f)
                    {
                        if (Random.Range(0f, 100f) <= knownskill.multipliers[0])//15% chance
                        {
                            PlayerConditions.handle_buffs_debuffs(knownskill, gameObject);
                            PlayerGeneral.showCBT(gameObject, false, false, 0, "F.Frenzy!");
                        }
                    }
                }
                if (knownskill.SkillID == 1100)
                {  //SKILL-Enraged 
                    if (Random.Range(0f, 100f) <= knownskill.multipliers[0])//15% chance
                    {
                        //stun = no
                        PlayerConditions.handle_buffs_debuffs(knownskill, gameObject);
                        PlayerGeneral.showCBT(gameObject, false, false, 0, "Enraged!");
                    }
                    break;
                }
            }

        }*/


    }
    public void TriggerSkillOnAttack()
    {
        //SKILL-QUICKSHOT
        /*if (PlayerStats.PlayerSelectedClass == "Archer")
        {
            if (Random.Range(0f, 100f) <= 15f)//15% chance
            {
                foreach (var knownskill in PlayerSkills.PlayerKnownSkills)
                {
                    if (knownskill.SkillID == 2080)
                    {
                        PlayerConditions.handle_buffs_debuffs(knownskill, gameObject);
                        PlayerGeneral.showCBT(gameObject, false, false, 0, "Q.Shot!");
                    }
                }
            }
        }*/
    }
    #endregion

    #region Party_Arena_Checker
    /*public bool Party_Arena_Checker(GameObject AgainstPlayer, skill skillRequested)
    {
        if (isPVPenabled && AgainstPlayer.GetComponent<PlayerPVPDamage>().isPVPenabled)
        {
            if (isInArena && AgainstPlayer.GetComponent<PlayerPVPDamage>().isInArena)
            {
                if (AgainstPlayer.GetComponent<PlayerAccountInfo>().arena_team != "targetable")
                {
                    if (AgainstPlayer.GetComponent<PlayerAccountInfo>().arena_team != PlayerAccountInfo.arena_team)
                    {
                        if (AgainstPlayer.GetComponent<PlayerAccountInfo>().arena_team != "spectator")
                        {
                            return true;
                        }
                    }
                    else if ((skillRequested.type == skill.Stype.AOE_heal || skillRequested.type == skill.Stype.AOE_cleanse || skillRequested.type == skill.Stype.AOE_revive) && AgainstPlayer.GetComponent<PlayerAccountInfo>().arena_team == PlayerAccountInfo.arena_team)
                    {
                        if (AgainstPlayer.GetComponent<PlayerAccountInfo>().arena_team != "spectator")
                        {
                            return true;
                        }                        
                    }                    
                }                
                    return false;
                
            }
            else
            {
                if (PlayerGeneral.PartyID == string.Empty && AgainstPlayer.GetComponent<PlayerGeneral>().PartyID == string.Empty)
                {
                    return true;//nadie en party
                }
                else if (AgainstPlayer.GetComponent<PlayerGeneral>().PartyID != PlayerGeneral.PartyID && skillRequested.type != skill.Stype.AOE_heal && skillRequested.type != skill.Stype.AOE_cleanse && skillRequested.type != skill.Stype.AOE_revive)
                {
                    return true;//distintos party's y no es heal
                }
                else if ((skillRequested.type == skill.Stype.AOE_heal || skillRequested.type == skill.Stype.AOE_cleanse || skillRequested.type == skill.Stype.AOE_revive) && AgainstPlayer.GetComponent<PlayerGeneral>().PartyID == PlayerGeneral.PartyID)
                {
                    return true;//es heal y mismo party
                }
                else if (PlayerGeneral.PartyID == string.Empty && AgainstPlayer.GetComponent<PlayerGeneral>().PartyID != string.Empty && (skillRequested.type == skill.Stype.AOE_heal || skillRequested.type == skill.Stype.AOE_cleanse || skillRequested.type == skill.Stype.AOE_revive))
                {
                    return true;//no estoy en party pero el si y es heal
                }                
                    return false;
            }
        }
        else
        {
            return false;
        }
        
    }*/
    #endregion












}
