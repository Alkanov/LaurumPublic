using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTakeDamage : NetworkBehaviour
{
    #region Enemy
    EnemyStats EnemyStats;
    ItemDatabase database;
    EnemyAggro EnemyAggro;
    EnemyLoot EnemyLoot;
    EnemyConditions EnemyConditions;
    EnemySpawnInfo EnemySpawnInfo;
    EnemyAttack EnemyAttack;
    #endregion
    #region Data   
    GameObject currentlyTakingDamageFrom;
    bool[] takingAutoAttackDamage = new bool[10];
    List<GameObject> AttackersList = new List<GameObject>();
    public Dictionary<GameObject, float> DamageTrack = new Dictionary<GameObject, float>();
    int PlayerinFight;
    float DEFvalue;
    bool respawning;
    bool dying_now;
    #endregion
    #region Networking

    #endregion
    #region Managers   
    ServerDBHandler ServerDBHandler;
    #endregion
    void Start()
    {
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemySpawnInfo = GetComponent<EnemySpawnInfo>();
        DEFvalue = 0.8f;
        database = GameObject.Find("LootManager").GetComponent<ItemDatabase>();
        EnemyLoot = GetComponent<EnemyLoot>();
        EnemyStats = GetComponent<EnemyStats>();
        //AILerpTest = GetComponent<EnemyControllerAI>();
        EnemyAggro = GetComponent<EnemyAggro>();
        //StartCoroutine("CheckForDestroy");
        ServerDBHandler = GameObject.Find("ServerDBreflector").GetComponent<ServerDBHandler>();
        EnemyConditions = GetComponent<EnemyConditions>();
    }

    #region Networking Client   
    [ClientRpc]
    public void RpcDestroyIt()
    {

    }
    #endregion

    #region Utiliaddes
    bool karma_change_allowed(int playerLVL)
    {
        try
        {
            bool allowed = false;
            int levelDiff = playerLVL - EnemyStats.Level;
            var cap = 10;
            if (levelDiff >= -20 && levelDiff <= cap)
            {
                allowed = true;
            }
            //for current cap monster level 110
            if (EnemyStats.Level >= 110)
            {
                allowed = true;
            }
            return allowed;
        }
        catch (System.Exception)
        {
            return false;
        }

    }
    bool levelToExpAllowed(int playerLVL, int rebirths)
    {
        try
        {
            bool allowed = false;
            int levelDiff = playerLVL - EnemyStats.Level;
            var cap = 60 + (rebirths * 10);
            if (levelDiff >= -20 && levelDiff <= cap)
            {
                allowed = true;
            }
            return allowed;
        }
        catch (System.Exception)
        {
            return false;
        }

    }
    bool levelToDropAllowed(int playerLVL, int rebirths)
    {
        try
        {
            bool allowed = false;
            int levelDiff = playerLVL - EnemyStats.Level;
            var cap = 60 + (rebirths * 10);
            if (levelDiff <= cap)
            {
                allowed = true;
            }
            return allowed;
        }
        catch (System.Exception)
        {
            return false;
        }


    }
    bool isAllowedToDamage(GameObject fromPlayer)
    {
        if (EnemyStats.CurrentHP > 0 && fromPlayer.GetComponent<PlayerStats>().CurrentHP > 0 && !fromPlayer.GetComponent<PlayerConditions>().stunned)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool isCritical(GameObject fromPlayer)
    {
        //Critical lottery
        var roll = Random.Range(0f, 100f);
        if (roll <= fromPlayer.GetComponent<PlayerStats>().Critical_chance)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    bool did_I_Dodged()
    {
        if (Random.Range(0f, 100f) <= EnemyStats.Dodge_percent_dex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Auto attack process
    public void TakeAutoAttackDamage(GameObject fromPlayer)
    {
        //If enemy is alive and player is alive and not stunned
        if (isAllowedToDamage(fromPlayer))
        {
            //if player is not on list add him
            if (!AttackersList.Contains(fromPlayer))
            {
                AttackersList.Add(fromPlayer);
            }
            //get the index of that player on the list to use it later to avoid speed hack attacks 
            PlayerinFight = AttackersList.IndexOf(fromPlayer);


            //Raycast to avoid damage above walls       
            if (EnemySpawnInfo.x_ObjectHelper.Raycast_didItHit(fromPlayer, gameObject, Vector2.Distance(fromPlayer.transform.position, transform.position), LayerMask.GetMask("Enemy", "Coliders")))
            {
                //if this player auto attack is allowed (this is false once attack speed time has completed)
                if (!takingAutoAttackDamage[PlayerinFight])
                {
                    takingAutoAttackDamage[PlayerinFight] = true;
                    //where the magic happens after all the checks
                    StartCoroutine(TakingAutoAttackDamage(fromPlayer, PlayerinFight));
                }
            }

        }
    }
    IEnumerator TakingAutoAttackDamage(GameObject fromPlayer, int PlayerinFight)
    {
        //this keeps the player from attacking quicker than what he should, but we should probably handle this on the auto attack method instead of on the enemy
        //because right now, you your auto attack cd is per enemy, if we had an infinite amount of enemies lined up, you could attack each of them one by one without taking auto attack speed into account

        TakeHit(fromPlayer, 1f);
        //wait for that player auto attack speed
        yield return new WaitForSeconds(fromPlayer.GetComponent<PlayerStats>().AutoAtk_speed);
        //flag this as false so player can auto attack again 
        takingAutoAttackDamage[PlayerinFight] = false;

    }

    private void TakeHit(GameObject fromPlayer, float damage_multiplier)
    {
        //used on aggro
        EnemyAggro.wasAttacked = true;
        //damage 
        float DamageRX = CalculateDamageRx(fromPlayer, damage_multiplier);

        //used for animations
        bool Critico = isCritical(fromPlayer);
        bool dodged = did_I_Dodged();

        //Critical lottery      
        if (Critico)
        {
            float critMultiplier = EnemySpawnInfo.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVE_Crit_Multiplier].value;
            DamageRX = DamageRX * (critMultiplier + fromPlayer.GetComponent<PlayerStats>().Critical_damage);
        }

        //Enemy dodge lottery       
        if (dodged)
        {
            DamageRX = 0f;
        }

        if (DamageRX > 0f)
        {
            DamageRX = Random.Range(DamageRX * 0.95f, DamageRX * 1.05f);
        }


        //take the hit
        DamageRX = CentralEnemyTakeDamage(DamageRX, fromPlayer);

        //skill/passives checks
        if (fromPlayer.GetComponent<PlayerConditions>().has_buff_debuff(PlayerConditions.type.buff, 17))//if attacker has frozen hands
        {
            EnemyConditions.slowed = true;
            EnemyConditions.EnemyControllerAI.maxSpeed *= 0.75f;
            EnemyConditions.add_buff_debuff(2, null, false, 2.5f, fromPlayer, EnemyConditions.type.debuff, true);
        }

        //Show hit numbers on top of this enemy
        fromPlayer.GetComponent<PlayerGeneral>().showCBT(gameObject, Critico, dodged, Mathf.RoundToInt(DamageRX), "damage");
        //show auto attack animation
        fromPlayer.GetComponent<PlayerGeneral>().send_autoATK_animation(fromPlayer, gameObject);
        //some skills are triggered on auto attack - we do it here
        fromPlayer.GetComponent<PlayerPVPDamage>().TriggerSkillOnAttack();

        //If enemy hp is 0 or below we kill it
        if (EnemyStats.CurrentHP <= 0)
        {
            dieNow();
        }
    }
    #endregion

    #region Skill attack process
    public void TakeSkillDamage(GameObject fromPlayer, skill skillRequested)
    {
        //if player is alive and enemy is alive and player is not stunned
        if (isAllowedToDamage(fromPlayer))
        {
            bool Critico = isCritical(fromPlayer);

            if (skillRequested.SkillID == 61016)//execution
            {
                if (EnemyStats.CurrentHP / EnemyStats.MaxHP <= skillRequested.multipliers[1] / 100f)
                {
                    Critico = true;
                }
            }



            bool dodged = did_I_Dodged();

            int finalNumber = 0;
            //this is used to differentiate damage type (critical, dot, healing...) animation on client
            string damageType = "null";

            if (skillRequested.type == skill.Stype.target_DOT)
            {//if damage is DOT we handle it in handle_effect
                damageType = "DOT";
                switch (skillRequested.SkillID)
                {
                    case 62003://burn - wizard
                        EnemyConditions.handle_effect(DOT_effect.effect_type.fire, skillRequested.multipliers[0], fromPlayer);
                        break;
                    case 61004://bleed - warrior
                        EnemyConditions.handle_effect(DOT_effect.effect_type.bleed, skillRequested.multipliers[0], fromPlayer);
                        break;
                    default:
                        break;
                }

            }
            else if (skillRequested.type == skill.Stype.target_debuff)
            {//if this is a debuff we only send animation - no damage
                damageType = "debuff";
            }
            else if (skillRequested.type == skill.Stype.target_action)
            {//if this is a debuff we only send animation - no damage

            }
            else if (skillRequested.type == skill.Stype.AOE_damage || skillRequested.type == skill.Stype.target_damage)
            {
                //if skill causes damage (currently only AOE_damage and target_damage)

                //tag as damage (red) on animation
                damageType = "damage";

                //add player to list of attackers
                if (!AttackersList.Contains(fromPlayer))
                {
                    AttackersList.Add(fromPlayer);
                }

                //flag enemy as attacked (used for aggro change)
                EnemyAggro.wasAttacked = true;

                //calculate skill damage
                float DamageRX = CalculateSkillDamageRx(fromPlayer, skillRequested.multipliers[0]);

                //critical lottery
                if (Critico)
                {
                    float critMultiplier = EnemySpawnInfo.x_ObjectHelper.ServerUniversalSettings.dict_vars[ServerUniversalSettings.var_names.PVE_Crit_Multiplier].value;
                    DamageRX = DamageRX * (critMultiplier + fromPlayer.GetComponent<PlayerStats>().Critical_damage);
                }
                //dodge lottery
                if (dodged)
                {
                    DamageRX = 0;
                }


                if (!EnemyConditions.immortal)
                {
                    var buff_info = EnemyConditions.get_buff_information(EnemyConditions.type.debuff, 1);
                    //soul cravings
                    if (skillRequested.SkillID == 61010 && buff_info != null)//stunned
                    {
                        EnemyConditions.remove_buff_debuff(EnemyConditions.type.debuff, 1);
                        int to_heal = Mathf.RoundToInt(DamageRX * skillRequested.multipliers[1] / 100f);
                        fromPlayer.GetComponent<PlayerStats>().hpChange(to_heal);
                        fromPlayer.GetComponent<PlayerGeneral>().showCBT(fromPlayer, false, false, to_heal, "heal");
                    }
                    //flame missile
                    if (skillRequested.SkillID == 62002)
                    {
                        if (Random.Range(0f, 100f) <= (skillRequested.multipliers[1]))
                        {
                            EnemyConditions.handle_effect(DOT_effect.effect_type.fire, skillRequested.multipliers[0], fromPlayer);
                        }
                    }
                    //posion arrow
                    if (skillRequested.SkillID == 63006)
                    {
                        if (Random.Range(0f, 100f) <= skillRequested.multipliers[1])
                        {
                            EnemyConditions.handle_effect(DOT_effect.effect_type.poison, skillRequested.multipliers[0], fromPlayer);
                        }
                    }
                    /*buff_info = EnemyConditions.get_buff_information(EnemyConditions.type.debuff, 14);//hunter's mark
                    if (buff_info != null)
                    {
                        if (EnemyStats.CurrentHP / EnemyStats.MaxHP <= buff_info.skill_requested.multipliers[1] / 100f)
                        {
                            DamageRX *= 1f + (buff_info.skill_requested.multipliers[2] / 100f);
                        }
                    }*/
                    if (skillRequested.SkillID == 61008)//provoke
                    {
                        if (Random.Range(0f, 100f) < skillRequested.multipliers[1])
                        {
                            damageType = "Provoked!";
                            EnemyAggro.AggroChange(fromPlayer);
                        }
                        else
                        {
                            damageType = "Ignored";
                        }
                    }

                }


                //take the damage
                finalNumber = Mathf.RoundToInt(CentralEnemyTakeDamage(DamageRX, fromPlayer));
            }



            //Used for animations
            if (damageType != "null")
            {
                //skills above are the only ones that can affect enemies, so if the skill that managed to get here is not any of those we dont show any animation nor take damage (before you could damage enemies by using AOE heal because enemies where taking the heal as damage)
                fromPlayer.GetComponent<PlayerGeneral>().showCBT(gameObject, Critico, dodged, finalNumber, damageType);
                fromPlayer.GetComponent<PlayerGeneral>().show_skill_damage_animation(fromPlayer, gameObject, skillRequested);
                EnemyConditions.handle_buffs_debuffs(skillRequested, fromPlayer);
            }

            //if enemy now has 0 or less hp: he ded
            if (EnemyStats.CurrentHP <= 0)
            {
                dieNow();
            }
        }


    }
    public float CalculateSkillDamageRx(GameObject fromPlayer, float skillDamage_multiplier)
    {
        float DamageRxAcc;

        //skill damage is calculated based on players autoattackdamage and a skill multiplier e.g: skill "fire arrow" just multiplies auto attack by 1.4 times... 
        var DamageTx = skillDamage_multiplier;
        switch (fromPlayer.GetComponent<PlayerStats>().DamageType_now)
        {
            case PlayerStats.DamageType.magical:
                DamageTx *= fromPlayer.GetComponent<PlayerStats>().Damage_int;
                DamageTx -= EnemyStats.Defense_int;
                break;
            case PlayerStats.DamageType.physical:
                DamageTx *= fromPlayer.GetComponent<PlayerStats>().Damage_str;
                DamageTx -= EnemyStats.Defense_str;
                break;
            default:
                break;
        }

        //if player-enemy level diff is above 10 lvls then we cut divide by 5 - this is to create a quick balance must be fixed by stats
        DamageRxAcc = LevelModifierDamage(fromPlayer, DamageTx);

        if (DamageRxAcc <= 0)
        {
            DamageRxAcc = 1;
        }
        return DamageRxAcc;

    }
    #endregion

    #region Damage related
    float CalculateDamageRx(GameObject fromPlayer, float extra_dmg_multiplier)
    {
        //this is the final damage to return       
        float DamageRxAcc = 0;

        //monster defense
        switch (fromPlayer.GetComponent<PlayerStats>().DamageType_now)
        {
            case PlayerStats.DamageType.magical:
                DamageRxAcc = fromPlayer.GetComponent<PlayerStats>().Damage_int;
                DamageRxAcc -= EnemyStats.Defense_int;
                break;
            case PlayerStats.DamageType.physical:
                DamageRxAcc = fromPlayer.GetComponent<PlayerStats>().Damage_str;
                DamageRxAcc -= EnemyStats.Defense_str;
                break;
            default:
                break;
        }
        //.LogError("Damage Player --> Enemy  " + DamageRxAcc);
        //if player-enemy level diff is above 10 lvls then we cut divide by 5 - this is to create a quick balance must be fixed by stats
        DamageRxAcc = LevelModifierDamage(fromPlayer, DamageRxAcc);

        //multiplie
        DamageRxAcc *= extra_dmg_multiplier;
        //.LogError("Damage Player --> Enemy  " + DamageRxAcc);
        //if player damage is below enemies DEF/MDEF it will end up negative thus healing enemy
        if (DamageRxAcc <= 0)
        {
            DamageRxAcc = 2f;
        }
        return DamageRxAcc;

    }
    public float CentralEnemyTakeDamage(float DamageToTake, GameObject fromPlayer)
    {
        //used for looting
        float damage_to_track = 0f;
        //if enemy is already aggroed by someone else there is a 30% chance to pull that aggro to avoid super tanking
        if (EnemyAggro.isAggroed)
        {
            if (Random.Range(0f, 100f) <= 30f)
            {
                EnemyAggro.AggroedByAttack(fromPlayer);
            }
        }
        else
        {
            //if not, then aggro now
            EnemyAggro.AggroedByAttack(fromPlayer);
        }
        //track damage done to enemies
        fromPlayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.damage_done_session, (int)DamageToTake);

        //absorbing damage
        if (EnemyConditions.converting_dmg_to_hp)
        {
            EnemyStats.CurrentHP = EnemyStats.CurrentHP + DamageToTake;
            fromPlayer.GetComponent<PlayerGeneral>().showCBT(gameObject, false, false, Mathf.RoundToInt(DamageToTake), "heal");
        }
        else
        {
            //take damage from HP           
            if (EnemyStats.CurrentHP - DamageToTake < 0)
            {
                DamageToTake = EnemyStats.CurrentHP;                
            }
            //add damage to the tracker
            damage_to_track += DamageToTake;           
            //take mob hp
            EnemyStats.CurrentHP -= DamageToTake;
        }

        //when enemy has reflect
        if (EnemyConditions.reflect && DamageToTake > 0)
        {
            var dmng_reflected = Mathf.RoundToInt(DamageToTake * 0.5f); //50%
            fromPlayer.GetComponent<PlayerStats>().hpChange(-dmng_reflected);
            fromPlayer.GetComponent<PlayerGeneral>().showCBT(fromPlayer, false, false, dmng_reflected, "reflect");

            if (Mathf.Round(fromPlayer.GetComponent<PlayerStats>().CurrentHP) <= 0)
            {
                //dead body animation
                EnemySpawnInfo.x_ObjectHelper.spawn_sync_object(2, 15f, transform.position);

                if (fromPlayer.GetComponent<PlayerGeneral>().in_devilSquare)
                {
                    fromPlayer.GetComponent<PlayerGeneral>().DieNow();
                }
                fromPlayer.GetComponent<PlayerDeath>().DeathExpPenalty("PVE", EnemyStats.MobName + "-reflect", null);
            }

        }

        //if this enemy was(or not) already damaged by player then add up the new amount
        if (DamageTrack.ContainsKey(fromPlayer))
        {
            DamageTrack[fromPlayer] += damage_to_track;
        }
        else
        {
            DamageTrack.Add(fromPlayer, damage_to_track);
        }

        //if enemy is deadeded
        if (EnemyStats.CurrentHP <= 0)
        {
            //dead body animation
            EnemySpawnInfo.x_ObjectHelper.spawn_sync_object(2, 15f, transform.position);

        }
        else
        {
            //spawn blood animation
            EnemySpawnInfo.x_ObjectHelper.spawn_sync_object(1, 5f, transform.position);
        }
        //used to to show players how much damage was done
        return DamageToTake;
    }

    private static void karma_changes(GameObject player, GameObject maxDmgPlayer)
    {
        //did last hit come from player? - wtf.. it always comes from player, possible use for NPC vs Enemy 
        if (player.tag == "Player")
        {
            //If enemy is elite add statistics
            /*if (EnemySpawnInfo.isElite)
            {
                fromThisObject.GetComponent<PlayerStatistics>().rewadTitleAndUpdateClient(3);
            }*/

            //Karma related - self explanatory, if player was hero take 3 karma, if it was pk add 3 - this number can be tweaked per hero/pk stage
            PlayerAccountInfo KarmaPlayerAccountInfo = player.GetComponent<PlayerAccountInfo>();
            var tempKarma = 0;
            if (KarmaPlayerAccountInfo.PlayerKarmaStage == PlayerAccountInfo.karma_stages.hero)//Hero
            {
                //only take karma away from hero if the player that killed the mob (maxDmgPlayer) is the one we are checking right now (player)
                if (maxDmgPlayer == player)
                {
                    tempKarma = KarmaPlayerAccountInfo.PlayerKarma - 1;
                    if (tempKarma >= 0)
                    {
                        KarmaPlayerAccountInfo.ModifyKarma(-1, false);
                    }
                    else
                    {
                        KarmaPlayerAccountInfo.ModifyKarma(0, true);
                    }
                }


            }
            else if (KarmaPlayerAccountInfo.PlayerKarmaStage == PlayerAccountInfo.karma_stages.pk_1)//1st STAGE OUTLAW
            {
                tempKarma = KarmaPlayerAccountInfo.PlayerKarma + 3;
                if (tempKarma <= 0)
                {
                    KarmaPlayerAccountInfo.ModifyKarma(3, false);
                }
                else
                {
                    KarmaPlayerAccountInfo.ModifyKarma(0, true);
                }
            }
            else if (KarmaPlayerAccountInfo.PlayerKarmaStage == PlayerAccountInfo.karma_stages.pk_2 || KarmaPlayerAccountInfo.PlayerKarmaStage == PlayerAccountInfo.karma_stages.pk_3)//2nd STAGE OUTLAW and MURDERER
            {
                KarmaPlayerAccountInfo.ModifyKarma(3, false);
            }
        }
    }
    #endregion

    #region Die process
    public void dieNow()
    {
        try
        {
            if (!dying_now)
            {
                dying_now = true;
                GameObject maxDMGplayer = DamageTrack.OrderByDescending(y => y.Value).FirstOrDefault().Key;

                EnemyConditions.clean_buff_debuff(EnemyConditions.type.debuff, 4);
                if (maxDMGplayer != null)
                {
                    exp_and_karmar_changes(maxDMGplayer);
                    OnKillMods(maxDMGplayer);
                    Statistics(maxDMGplayer);
                    Addquestprogression(maxDMGplayer);


                    if (levelToDropAllowed(maxDMGplayer.GetComponent<PlayerStats>().PlayerLevel, maxDMGplayer.GetComponent<PlayerStats>().Total_rebirths))
                    {
                        Drops(maxDMGplayer);
                    }
                    else
                    {
                        DestroyMob();
                    }

                }
                else
                {
                    DestroyMob();
                }
            }
        }
        catch (System.Exception)
        {
            //Debug.LogError(ex.ToString());
            DestroyMob();
            throw;
        }


    }
    public float calculateEXP(GameObject thisPlayer)
    {
        float expMod = (float)EnemyStats.Level / thisPlayer.GetComponent<PlayerStats>().PlayerLevel;
        if (expMod > 2f)
        {
            expMod = 2f;
        }
        return expMod;
    }
    private void exp_and_karmar_changes(GameObject maxDMGplayer)
    {
        try
        {
            if (levelToExpAllowed(maxDMGplayer.GetComponent<PlayerStats>().PlayerLevel, maxDMGplayer.GetComponent<PlayerStats>().Total_rebirths))
            {
                if (maxDMGplayer.GetComponent<PlayerGeneral>().PartyID != string.Empty)
                {

                    var partygroup = EnemySpawnInfo.x_ObjectHelper.PartyController.getPartyMembers_go(maxDMGplayer.GetComponent<PlayerGeneral>().PartyID);
                    for (int i = 0; i < partygroup.Count; i++)
                    {

                        if (levelToExpAllowed(partygroup[i].GetComponent<PlayerStats>().PlayerLevel, maxDMGplayer.GetComponent<PlayerStats>().Total_rebirths) && partygroup[i].GetComponent<PlayerStats>().CurrentHP > 0)
                        {

                            if (Vector2.Distance(partygroup[i].transform.position, gameObject.transform.position) <= 6f)
                            {
                                bool bonus = false;
                                //only activate exp bonus if partygroup[i] has anyone from his party around
                                for (int p = 0; p < partygroup.Count; p++)
                                {
                                    if (partygroup[p] != partygroup[i])//if partygroup[p] is not himself
                                    {
                                        if (Vector2.Distance(partygroup[i].transform.position, partygroup[p].transform.position) <= 8f) //if the party memeber we are looking at is close to another one
                                        {
                                            bonus = true;
                                            break;
                                        }
                                    }

                                }

                                float expMod = calculateEXP(partygroup[i]);
                                var ExpAwarded = EnemyStats.Exp * expMod;
                                if (bonus)
                                {
                                    ExpAwarded *= 1.1f;
                                }

                                if (partygroup[i].GetComponent<PlayerStats>().ExtraExp > 0)
                                {
                                    ExpAwarded = ExpAwarded * (1f + (partygroup[i].GetComponent<PlayerStats>().ExtraExp / 100f));
                                }
                                grant_exp(partygroup[i], ExpAwarded);
                                if (karma_change_allowed(partygroup[i].GetComponent<PlayerStats>().PlayerLevel))
                                {
                                    karma_changes(partygroup[i], maxDMGplayer);
                                }


                                EnemyAggro.TargetUnSetAggroIcon(partygroup[i].GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, partygroup[i]);
                            }
                        }


                    }
                }
                else
                {
                    float expMod = calculateEXP(maxDMGplayer);
                    var ExpAwarded = EnemyStats.Exp * expMod;

                    if (maxDMGplayer.GetComponent<PlayerStats>().ExtraExp > 0)
                    {
                        ExpAwarded = ExpAwarded * (1f + (maxDMGplayer.GetComponent<PlayerStats>().ExtraExp / 100f));
                    }
                    grant_exp(maxDMGplayer, ExpAwarded);
                    if (karma_change_allowed(maxDMGplayer.GetComponent<PlayerStats>().PlayerLevel))
                    {
                        karma_changes(maxDMGplayer, maxDMGplayer);
                    }


                }


            }
        }
        catch (System.Exception)
        {
            DestroyMob();
            throw;
        }


    }
    void grant_exp(GameObject player, float ExpAwarded)
    {
        if (EnemySpawnInfo.isInDevilSquare)
        {
            player.GetComponent<PlayerStats>().player_exp_change(Mathf.RoundToInt(ExpAwarded), PlayerStats.exp_source.ds);
        }
        else
        {
            player.GetComponent<PlayerStats>().player_exp_change(Mathf.RoundToInt(ExpAwarded), PlayerStats.exp_source.grind);
        }
        player.GetComponent<PlayerStatistics>().exp_session += Mathf.RoundToInt(ExpAwarded);

    }
    private void OnKillMods(GameObject maxDMGplayer)
    {
        try
        {
            if (maxDMGplayer != null)
            {
                PlayerStats maxDMGplayer_PlayerStats = maxDMGplayer.GetComponent<PlayerStats>();
                PlayerInventory maxDMGplayer_PlayerInventory = maxDMGplayer.GetComponent<PlayerInventory>();

                if (maxDMGplayer_PlayerStats.CurrentHP > 0)
                {
                    //modHPonKill
                    maxDMGplayer_PlayerStats.CurrentHP += (maxDMGplayer_PlayerStats.modHPonKill / 100f * maxDMGplayer_PlayerStats.MaxHealth);
                    //mod
                    //modMPonKill
                    maxDMGplayer_PlayerStats.CurrentMP += (maxDMGplayer_PlayerStats.modMPonKill / 100f * maxDMGplayer_PlayerStats.MaxMana);
                    //enchant
                    if (maxDMGplayer_PlayerStats.ench_free_hp_potion_use_on_kill > 0f)
                    {
                        if (Random.Range(0f, 100f) <= maxDMGplayer_PlayerStats.ench_free_hp_potion_use_on_kill)
                        {
                            var item = maxDMGplayer_PlayerInventory.FetchEquippedItemByType(Item.UseAs.HPPotion);
                            if (item != null)
                            {
                                var itemData = maxDMGplayer.GetComponent<PlayerGeneral>().ItemDatabase.FetchItemByID(item.itemID);
                                var to_gain = Mathf.RoundToInt((itemData.misc_data[0] + (itemData.misc_data[0] * 0.15f * item.itemUpgrade)) * (1f + (maxDMGplayer_PlayerStats.ench_extra_hp_from_pots / 100f)));

                                maxDMGplayer_PlayerStats.CurrentHP += to_gain;
                                maxDMGplayer.GetComponent<PlayerGeneral>().showCBT(maxDMGplayer, true, false, 0, "+Saphire");
                            }
                        }
                    }
                    if (maxDMGplayer_PlayerStats.ench_chance_to_get_free_mphp_potion_charge > 0)
                    {
                        if (Random.Range(0f, 100f) <= maxDMGplayer_PlayerStats.ench_chance_to_get_free_mphp_potion_charge)
                        {
                            var hp_potion = maxDMGplayer_PlayerInventory.FetchEquippedItemByType(Item.UseAs.HPPotion);
                            var mp_potion = maxDMGplayer_PlayerInventory.FetchEquippedItemByType(Item.UseAs.MPPotion);
                            if (hp_potion != null)
                            {
                                hp_potion.itemDurability++;
                                var newdur = hp_potion.itemDurability;
                                maxDMGplayer_PlayerInventory.Change_durabil(newdur, hp_potion);
                            }
                            if (mp_potion != null)
                            {
                                mp_potion.itemDurability++;
                                var newdur = mp_potion.itemDurability;
                                maxDMGplayer_PlayerInventory.Change_durabil(newdur, mp_potion);
                            }

                            maxDMGplayer_PlayerInventory.SendWholeEquipmentToClient();
                            maxDMGplayer.GetComponent<PlayerGeneral>().showCBT(maxDMGplayer, true, false, 0, "+Corrupted Saphire");
                        }
                    }
                    if (maxDMGplayer_PlayerStats.ench_free_mp_potion_use_on_kill > 0)
                    {
                        if (Random.Range(0f, 100f) <= maxDMGplayer_PlayerStats.ench_free_mp_potion_use_on_kill)
                        {
                            var item = maxDMGplayer_PlayerInventory.FetchEquippedItemByType(Item.UseAs.MPPotion);
                            if (item != null)
                            {
                                var itemData = maxDMGplayer.GetComponent<PlayerGeneral>().ItemDatabase.FetchItemByID(item.itemID);
                                var to_gain = Mathf.RoundToInt((itemData.misc_data[0] + (itemData.misc_data[0] * 0.15f * item.itemUpgrade)) * (1f + (maxDMGplayer_PlayerStats.ench_extra_mp_from_pots / 100f)));

                                maxDMGplayer_PlayerStats.CurrentMP += to_gain;
                                maxDMGplayer.GetComponent<PlayerGeneral>().showCBT(maxDMGplayer, true, false, 0, "+Siderite");
                            }
                        }
                    }
                    if (maxDMGplayer_PlayerStats.ench_chance_to_get_hpandmp_on_kill > 0)
                    {
                        if (Random.Range(0f, 100f) <= maxDMGplayer_PlayerStats.ench_chance_to_get_hpandmp_on_kill)
                        {
                            maxDMGplayer_PlayerStats.CurrentHP = maxDMGplayer_PlayerStats.CurrentHP + (maxDMGplayer_PlayerStats.MaxHealth * 0.01f);
                            maxDMGplayer_PlayerStats.CurrentMP = maxDMGplayer_PlayerStats.CurrentMP + (maxDMGplayer_PlayerStats.MaxMana * 0.01f);
                            maxDMGplayer.GetComponent<PlayerGeneral>().showCBT(maxDMGplayer, true, false, 0, "+Corrupted Siderite");
                        }
                    }
                    if (maxDMGplayer_PlayerStats.ench_chance_to_explode_deads > 0)
                    {
                        if (Random.Range(0f, 100f) <= maxDMGplayer_PlayerStats.ench_chance_to_explode_deads)
                        {
                            var enemies_around = EnemySpawnInfo.x_ObjectHelper.get_AOE_LOS_targets(gameObject, 1f, LayerMask.GetMask("Enemy"), false);
                            for (int i = 0; i < enemies_around.Count; i++)
                            {
                                if (enemies_around[i].GetComponent<EnemyStats>().CurrentHP > 0)
                                {
                                    enemies_around[i].GetComponent<EnemyTakeDamage>().TakeHit(maxDMGplayer, 3f);
                                }

                            }
                            maxDMGplayer.GetComponent<PlayerGeneral>().showCBT(maxDMGplayer, true, false, 0, "+Gypsum");
                        }
                    }

                    if (maxDMGplayer_PlayerStats.CurrentHP > maxDMGplayer_PlayerStats.MaxHealth)
                    {
                        maxDMGplayer_PlayerStats.CurrentHP = maxDMGplayer_PlayerStats.MaxHealth;
                    }
                    if (maxDMGplayer_PlayerStats.CurrentMP > maxDMGplayer_PlayerStats.MaxMana)
                    {
                        maxDMGplayer_PlayerStats.CurrentMP = maxDMGplayer_PlayerStats.MaxMana;
                    }
                }
            }
        }
        catch (System.Exception)
        {
            DestroyMob();
            throw;
        }

    }
    private void Statistics(GameObject maxDMGplayer)
    {
        try
        {
            //statistics
            if (EnemyStats.MonsterType_now == EnemyStats.MonsterType.boss)
            {
                maxDMGplayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.boss_killed, 1);
            }
            else if (EnemySpawnInfo.isMobEvent)
            {
                maxDMGplayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.event_mobs_session, 1);
            }
            else
            {
                maxDMGplayer.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.mobskilled_session, 1);
            }

            // }
        }
        catch (System.Exception)
        {
            DestroyMob();
            throw;
        }

    }
    private void Drops(GameObject maxDMGplayer)
    {
        try
        {
            if (EnemySpawnInfo.isInDevilSquare)
            {
                EnemyLoot.roll_loot(maxDMGplayer, EnemyLoot.loot_source.ds);
            }
            else
            {
                EnemyLoot.roll_loot(maxDMGplayer, EnemyLoot.loot_source.grind);
            }
        }
        catch (System.Exception ex)
        {
            //Debug.LogError(ex.ToString());
            DestroyMob();
            throw;
        }
    }
    private void Addquestprogression(GameObject maxDMGplayer)
    {
        try
        {
            if (maxDMGplayer.GetComponent<PlayerGeneral>().PartyID != string.Empty)
            {
                var partygroup = EnemySpawnInfo.x_ObjectHelper.PartyController.getPartyMembers_go(maxDMGplayer.GetComponent<PlayerGeneral>().PartyID);
                for (int i = 0; i < partygroup.Count; i++)
                {
                    if (Vector2.Distance(partygroup[i].transform.position, gameObject.transform.position) <= 10f)
                    {
                        if (EnemySpawnInfo.isInDevilSquare)
                        {
                            //quest track
                            partygroup[i].GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.ds_pve_kill, EnemySpawnInfo.x_ObjectHelper.DevilSquare_reg.DS_min_level, maxDMGplayer, 1);
                        }
                        else
                        {
                            //quest track
                            partygroup[i].GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.pve_kill_x_y_times, EnemyStats.MobID, maxDMGplayer, 1);
                        }

                    }

                }
            }
            else
            {
                if (EnemySpawnInfo.isInDevilSquare)
                {
                    //quest track
                    maxDMGplayer.GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.ds_pve_kill, EnemySpawnInfo.x_ObjectHelper.DevilSquare_reg.DS_min_level, maxDMGplayer, 1);
                }
                else
                {
                    //quest track
                    maxDMGplayer.GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.pve_kill_x_y_times, EnemyStats.MobID, maxDMGplayer, 1);
                }

            }
        }
        catch (System.Exception)
        {
            DestroyMob();
            throw;
        }

    }
    #endregion





    public void DestroyMob()
    {
        //.LogError("Traceme DestroyMob");
        try
        {
            //.LogError("Traceme DestroyMob");
            if (!respawning)
            {
                respawning = true;

                if (!EnemySpawnInfo.isInDevilSquare)
                {
                    EnemySpawnInfo.MasterSpawner.StartCoroutine(EnemySpawnInfo.MasterSpawner.RespawnMobIn(EnemySpawnInfo.MyHouse, Random.Range(EnemySpawnInfo.respawnInTime, EnemySpawnInfo.respawnInTime * 2f + 1)));
                }

                RpcDestroyIt();
                Destroy(this.gameObject);
            }
        }
        catch (System.Exception)
        {
            ////Debug.LogError("Traceme DestroyMob");
            RpcDestroyIt();
            Destroy(this.gameObject);
            throw;
        }

    }
    private float LevelModifierDamage(GameObject fromPlayer, float DamageRxAcc)
    {
        int levelDiff = EnemyStats.Level - fromPlayer.GetComponent<PlayerStats>().PlayerLevel;
        if (levelDiff > 5)
        {
            ////Debug.LogError("PRE LevelModifierDamage Player --> Enemy: PlayerDamage=" + DamageRxAcc);
            //when monster level is 6 or higher than your level, decrease 2% damage by each level dif
            float damageLevelPenalty = (100f - (levelDiff * 2f)) / 100f;
            if (damageLevelPenalty < 0.05f)
                damageLevelPenalty = 0.05f;

            // //Debug.LogError(DamageRxAcc + "*" + damageLevelPenalty);
            DamageRxAcc = DamageRxAcc * damageLevelPenalty;

        }
        ////Debug.LogError("AFTER LevelModifierDamage Player --> Enemy: PlayerDamage=" + DamageRxAcc);
        return DamageRxAcc;
    }
}
