using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillsActions : MonoBehaviour
{
    #region Player
    PlayerSkills PlayerSkills;
    PlayerStats PlayerStats;
    PlayerPVPDamage PlayerPVPDamage;
    PlayerMPSync PlayerMPSync;
    PlayerConditions PlayerConditions;
    PlayerAccountInfo PlayerAccountInfo;
    PlayerGeneral PlayerGeneral;
    PlayerAnimatorC PlayerAnimatorC;
    PlayerGuild PlayerGuild;
    #endregion

    #region Skill Data
    [HideInInspector]
    public Dictionary<int, float> Skills_in_CoolDown = new Dictionary<int, float>();
    [HideInInspector]
    public bool is_casting;
    [HideInInspector]
    public Coroutine casting_coroutine;
    [SerializeField]
    GameObject trap_prefab;
    [SerializeField]
    GameObject barrier_prefab;
    [SerializeField]
    GameObject decoy_prefab;
    [SerializeField]
    GameObject totem_prefab;

    public Dictionary<int, GameObject> totems_out = new Dictionary<int, GameObject>();
    public float casting_speed_reduction;
    #endregion

    private void Awake()
    {
        PlayerConditions = GetComponent<PlayerConditions>();
        PlayerSkills = GetComponent<PlayerSkills>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerPVPDamage = GetComponent<PlayerPVPDamage>();
        PlayerMPSync = GetComponent<PlayerMPSync>();
        PlayerAccountInfo = GetComponent<PlayerAccountInfo>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerAnimatorC = GetComponent<PlayerAnimatorC>();
        PlayerGuild = GetComponent<PlayerGuild>();
    }

    #region Steps - ordered
    //1 - desde PlayerSkills
    public void CheckSkillClass(GameObject target, skill skillRequested, int skillpos)
    {
        if (skillRequested.classRequired == PlayerStats.PlayerClass.Any || skillRequested.classRequired == PlayerStats.PlayerClass_now)
        {
            if (!is_casting)
            {
                exec_skill(target, skillRequested, skillpos);
            }
        }
    }
    //2
    public void exec_skill(GameObject target, skill skillRequested, int skillpos)
    {
        var mana_usage = skillRequested.baseMana + (PlayerStats.PlayerLevel / 3);

        if (PlayerConditions.concentrated)
        {
            mana_usage *= 2;
        }
        if (skillRequested.SkillID == 63014)//soul sacrifice
        {
            mana_usage = Mathf.RoundToInt(PlayerStats.MaxMana * skillRequested.multipliers[0] / 100f);
        }

        if (checkMana(mana_usage))
        {
            float castingtime = skillRequested.casting;

            if (PlayerStats.Casting_speed_reduction > 0)
            {
                castingtime *= (1f - (PlayerStats.Casting_speed_reduction / 100f));
            }
            if (castingtime < 0)
            {
                castingtime = 0;
            }
            if (PlayerConditions.has_buff_debuff(PlayerConditions.type.buff, 18))
            {
                castingtime = 0;
                PlayerConditions.remove_buff_debuff(PlayerConditions.type.buff, 18);
            }
            casting_coroutine = StartCoroutine(casting(castingtime, target, skillRequested, mana_usage, skillpos));

        }
    }
    //3
    IEnumerator casting(float time, GameObject target, skill skillRequested, int mana_usage, int skillpos)
    {
        //burn mana
        if (PlayerStats.ench_chance_to_free_cast > 0)
        {
            if (Random.Range(1, 100) <= PlayerStats.ench_chance_to_free_cast)
            {
                PlayerGeneral.showCBT(gameObject, false, false, 0, "+Corrupted Gypsum");
                PlayerStats.CurrentMP -= mana_usage / 2f;
                time /= 2f;
            }
            else
            {
                PlayerStats.CurrentMP -= mana_usage;
            }
        }
        else
        {
            PlayerStats.CurrentMP -= mana_usage;
        }

        if (PlayerGeneral.PartyID != string.Empty)
        {
            PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
        }
        //start cast
        is_casting = true;
        //in cd now
        if (Skills_in_CoolDown.ContainsKey(skillpos))
        {
            Skills_in_CoolDown[skillpos] = Time.time;
        }
        else
        {
            Skills_in_CoolDown.Add(skillpos, Time.time);
        }
        if (time > 0)
        {
            casting_speed_reduction = -50f;
            PlayerSkills.RpcIsCasting(time);
        }
        PlayerStats.RefreshStats();
        yield return new WaitForSecondsRealtime(time);
        PlayerGeneral.RpcMakeSound(skillRequested.SkillID + "_skill", transform.position);
        PlayerSkills.Rpc_castingDone();
        is_casting = false;
        casting_speed_reduction = 0f;
        PlayerStats.RefreshStats();

        if (PlayerStats.CurrentHP > 0 && !PlayerConditions.stunned)
        {
            if (PlayerStats.ench_chance_to_fail_casting > 0)
            {
                if (Random.Range(1, 100) <= PlayerStats.ench_chance_to_fail_casting)
                {
                    PlayerGeneral.showCBT(gameObject, false, false, 0, "Failed");
                    yield break;
                }
            }
            if (target != null)
            {
                if (skillRequested.SkillID != 2040)//explosive arrow animation is shown when the bomb explodes
                {
                    PlayerGeneral.show_skill_projectile(skillRequested, target);
                }
            }
            else
            {
                PlayerGeneral.show_skill_casted_animation(skillRequested, Vector2.zero);
            }

            switch (skillRequested.type)
            {
                case skill.Stype.target_damage:
                    if (target != null)
                    {
                        process_skill(target, skillRequested);
                    }
                    break;
                case skill.Stype.target_DOT:
                    if (target != null)
                    {
                        process_skill(target, skillRequested);
                    }
                    break;
                case skill.Stype.AOE_buff:
                    if (skillRequested.SkillID == 64001)//final protection
                    {   //stun = no
                        PlayerConditions.stunned = true;
                        PlayerConditions.immortal = true;
                        PlayerMPSync.PlayerCanMove = false;
                        PlayerConditions.add_buff_debuff(1, skillRequested, false, 5f, gameObject, PlayerConditions.type.debuff, false);
                        PlayerConditions.add_buff_debuff(10, skillRequested, false, 5f, gameObject, PlayerConditions.type.buff, true);
                    }
                    if (skillRequested.SkillID == 64004)//linked hearts - party only
                    {
                        if (PlayerGeneral.PartyID != string.Empty)
                        {
                            var members_affected = PlayerGeneral.x_ObjectHelper.PartyController.getPartyMembers_go(PlayerGeneral.PartyID, gameObject, 8);
                            for (int i = 0; i < members_affected.Count; i++)
                            {
                                members_affected[i].GetComponent<PlayerPVPDamage>().TakeSkillDamage(this.gameObject, skillRequested);
                            }
                        }
                    }
                    else
                    {
                        get_aoe_targets(skillRequested, true);
                    }

                    break;
                case skill.Stype.selfBuff:
                    PlayerConditions.handle_buffs_debuffs(skillRequested, gameObject);
                    break;
                case skill.Stype.AOE_damage:
                    if (skillRequested.SkillID == 63003)
                    {

                        var targets = get_aoe_targets(skillRequested, false, (int)skillRequested.multipliers[1]);
                        /*for (int i = 0; i < targets.Count; i++)
                        {
                            if (targets[i].tag == "Player" || targets[i].tag == "Enemy")
                            {
                                
                            } 
                        }*/
                    }
                    else
                    {
                        get_aoe_targets(skillRequested, false);
                    }
                    break;
                case skill.Stype.selfHeal_over_time:
                    PlayerPVPDamage.TakeSkillDamage(gameObject, skillRequested);
                    break;
                case skill.Stype.AOE_heal:
                    get_aoe_targets(skillRequested, false);
                    break;
                case skill.Stype.AOE_cleanse:
                    get_aoe_targets(skillRequested, false);
                    break;
                case skill.Stype.trap:
                    dropTrap(skillRequested);
                    break;
                case skill.Stype.AOE_revive:
                    get_aoe_targets(skillRequested, false);
                    break;
                case skill.Stype.target_debuff:
                    if (target != null)
                    {
                        process_skill(target, skillRequested);
                    }
                    break;
                case skill.Stype.decoy:
                    //dropDecoys(skillRequested);
                    break;
                case skill.Stype.target_action:
                    if (target != null)
                    {
                        process_skill(target, skillRequested);
                    }
                    break;
                case skill.Stype.totem_spawn:
                    dropTotem(skillRequested);
                    break;
                default:
                    break;
            }
        }
    }
    //4
    /// <summary>
    /// All targets around
    /// </summary>
    public void get_aoe_targets(skill skillRequested, bool include_myself)
    {
        var possible_targets = PlayerGeneral.x_ObjectHelper.get_AOE_LOS_targets(gameObject, PlayerStats.Skill_range, LayerMask.GetMask("Enemy", "Player", "decoy", "Coliders"), include_myself);

        for (int i = 0; i < possible_targets.Count; i++)
        {
            process_skill(possible_targets[i], skillRequested);
        }
    }
    /// <summary>
    /// X amount of targets
    /// </summary>
    public List<GameObject> get_aoe_targets(skill skillRequested, bool include_myself, int number_of_targets)
    {
        var possible_targets = PlayerGeneral.x_ObjectHelper.get_AOE_LOS_targets(gameObject, PlayerStats.Skill_range, LayerMask.GetMask("Enemy", "Player", "decoy", "Coliders"), include_myself);
        possible_targets = possible_targets.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();

        if (possible_targets.Count > 0)//if targets arround
        {
            for (int i = 0; i < number_of_targets; i++)
            {
                process_skill(possible_targets[i], skillRequested);
            }
        }
        return possible_targets;
    }



    //5
    public void process_skill(GameObject target, skill skillRequested)
    {
        var distance = PlayerStats.Skill_range;
        if (skillRequested.SkillID == 61026 || skillRequested.SkillID == 64012) // slow down or silence
        {
            distance = 15f;
        }
        var hit = PlayerGeneral.x_ObjectHelper.Raycast_didItHit(gameObject, target, distance, LayerMask.GetMask("Enemy", "Player", "decoy", "Coliders"));


        if (hit)
        {

            if (target.tag == "Player")
            {
                if (target.GetComponent<PlayerStats>().CurrentHP > 0)
                {
                    continueWithPVPchecks(target, skillRequested, hit);
                }
                else
                {
                    if (skillRequested.type == skill.Stype.AOE_revive)
                    {
                        target.GetComponent<PlayerPVPDamage>().TakeSkillDamage(this.gameObject, skillRequested);
                    }
                }
            }
            else
            {
                if (skillRequested.type != skill.Stype.selfHeal_over_time && skillRequested.type != skill.Stype.AOE_buff && skillRequested.type != skill.Stype.AOE_heal && skillRequested.type != skill.Stype.AOE_revive && skillRequested.type != skill.Stype.AOE_cleanse)
                {
                    if (target.tag == "decoy")
                    {
                        if (target != null && target.GetComponent<DecoyGeneral>())
                        {
                            target.GetComponent<DecoyGeneral>().hit_decoy();
                        }

                    }
                    else if (target.tag == "Enemy")
                    {
                        if (skillRequested.SkillID == 63003)
                        {
                            PlayerGeneral.show_skill_projectile(skillRequested, target);
                        }
                        target.GetComponent<EnemyTakeDamage>().TakeSkillDamage(this.gameObject, skillRequested);
                    }
                }
            }
        }
        else
        {
            //////.Log("MISS process_skill " + target.gameObject.transform.position);
            if (target.tag == "decoy" || target.tag == "Enemy")
            {
                PlayerGeneral.showCBT(target, false, false, 0, "miss");
            }
        }



    }
    //6
    private void continueWithPVPchecks(GameObject target, skill skillRequested, bool hit)
    {
        if (PVP_Arena_Party_checks(skillRequested, target))
        {
            if (hit)
            {//Hacer dano en PVP 
                target.GetComponent<PlayerPVPDamage>().TakeSkillDamage(this.gameObject, skillRequested);
                if (skillRequested.SkillID == 63003)
                {
                    PlayerGeneral.show_skill_projectile(skillRequested, target);
                }
            }
            else
            {
                //miss
                PlayerGeneral.showCBT(target, false, false, 0, "miss");
            }
        }
    }
    #endregion

    #region Traps & Decoys and spawns
    void dropTotem(skill skillRequested)
    {        
        if (totems_out.ContainsKey(skillRequested.SkillID))
        {
            Destroy(totems_out[skillRequested.SkillID]);
            totems_out.Remove(skillRequested.SkillID);
        }
        GameObject totem_deployed = Instantiate(totem_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        totems_out.Add(skillRequested.SkillID, totem_deployed);
        totem_effect totem_effect = totem_deployed.GetComponent<totem_effect>();
        totem_effect.owner = gameObject;
        totem_effect.skillRequested = skillRequested;
        totem_effect.x_ObjectHelper = PlayerGeneral.x_ObjectHelper;
        //totem color on client
        switch (skillRequested.SkillID)
        {
            case 64002://magic protection
                totem_effect.sprite_id = 5;
                break;
            case 64003://Physical protection
                totem_effect.sprite_id = 8;
                break;
            case 64008://Heal
                totem_effect.sprite_id = 16;
                break;
            case 64011://Mana
                totem_effect.sprite_id = 1;
                break;
            case 64013://Speed
                totem_effect.sprite_id = 12;
                break;
            default:
                break;
        }
        Mirror.NetworkServer.Spawn(totem_deployed);

    }
    void dropTrap(skill skillRequested)
    {
        if (skillRequested.SkillID == 63007)//multi trap
        {
            float offset = 0.35f;
            for (int i = 0; i < 4; i++)
            {
                //trap 1
                GameObject trap_deployed = Instantiate(trap_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
                if (i == 0)
                {
                    trap_deployed.transform.position = new Vector3(transform.position.x + offset, transform.position.y, 0);
                }
                else if (i == 1)
                {
                    trap_deployed.transform.position = new Vector3(transform.position.x - offset, transform.position.y, 0);
                }
                else if (i == 2)
                {
                    trap_deployed.transform.position = new Vector3(transform.position.x, transform.position.y + offset, 0);
                }
                else
                {
                    trap_deployed.transform.position = new Vector3(transform.position.x, transform.position.y - offset, 0);
                }
                trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visRange = 1;
                trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visUpdateInterval = 1f;
                DOT_effect trap_details = trap_deployed.GetComponent<DOT_effect>();
                trap_details.vanish_timer = 15f;
                trap_details.owner = gameObject;
                trap_details.trap_effect_power = (int)skillRequested.multipliers[0];
                trap_details.trap_pvp_status = PlayerPVPDamage.PVPmodeOn;
                trap_details.skillRequested = skillRequested;
                trap_details.trap_effect = DOT_effect.effect_type.bleed_and_apply_slow_debuff;
                trap_details.trap_sprite = 1;
                trap_details.spawnShape = DOT_effect.spawn_shape.trap;
                trap_details.trap_destroy_on_trigger = true;

                Mirror.NetworkServer.Spawn(trap_deployed);
            }
        }
        else if (skillRequested.SkillID == 63008)//posion trap
        {
            GameObject trap_deployed = Instantiate(trap_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visRange = 1;
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visUpdateInterval = 1f;
            DOT_effect trap_details = trap_deployed.GetComponent<DOT_effect>();
            trap_details.vanish_timer = 15f;
            trap_details.owner = gameObject;
            trap_details.trap_effect_power = (int)skillRequested.multipliers[0];
            trap_details.trap_pvp_status = PlayerPVPDamage.PVPmodeOn;
            trap_details.skillRequested = skillRequested;
            trap_details.trap_effect = DOT_effect.effect_type.poison;
            trap_details.trap_sprite = 1;
            trap_details.spawnShape = DOT_effect.spawn_shape.trap;
            trap_details.trap_destroy_on_trigger = true;

            Mirror.NetworkServer.Spawn(trap_deployed);
        }
        else if (skillRequested.SkillID == 63009)//steel trap
        {
            GameObject trap_deployed = Instantiate(trap_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visRange = 1;
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visUpdateInterval = 1f;
            DOT_effect trap_details = trap_deployed.GetComponent<DOT_effect>();
            trap_details.vanish_timer = 15f;
            trap_details.owner = gameObject;
            trap_details.trap_effect_power = (int)skillRequested.multipliers[0];
            trap_details.trap_pvp_status = PlayerPVPDamage.PVPmodeOn;
            trap_details.skillRequested = skillRequested;
            trap_details.trap_effect = DOT_effect.effect_type.apply_stun_debuff;
            trap_details.trap_sprite = 1;
            trap_details.spawnShape = DOT_effect.spawn_shape.trap;
            trap_details.trap_destroy_on_trigger = true;

            Mirror.NetworkServer.Spawn(trap_deployed);

        }
        else if (skillRequested.SkillID == 63010)//bomb trap
        {
            GameObject trap_deployed = Instantiate(trap_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visRange = 1;
            trap_deployed.GetComponent<Mirror.NetworkProximityChecker>().visUpdateInterval = 1f;
            DOT_effect trap_details = trap_deployed.GetComponent<DOT_effect>();
            trap_details.vanish_timer = 15f;
            trap_details.owner = gameObject;
            trap_details.trap_effect_power = (int)skillRequested.multipliers[0];
            trap_details.trap_pvp_status = PlayerPVPDamage.PVPmodeOn;
            trap_details.skillRequested = skillRequested;
            trap_details.trap_effect = DOT_effect.effect_type.apply_bomb_debuff;
            trap_details.trap_sprite = 1;
            trap_details.spawnShape = DOT_effect.spawn_shape.trap;
            trap_details.trap_destroy_on_trigger = true;

            Mirror.NetworkServer.Spawn(trap_deployed);
        }

        else if (skillRequested.SkillID == 62005)//lava barrier
        {
            GameObject trap_deployed = Instantiate(barrier_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
            DOT_effect trap_details = trap_deployed.GetComponent<DOT_effect>();
            trap_details.vanish_timer = 10f;
            trap_details.owner = gameObject;
            trap_details.trap_effect_power = (int)skillRequested.multipliers[0];
            trap_details.trap_pvp_status = PlayerPVPDamage.PVPmodeOn;
            trap_details.trap_effect = DOT_effect.effect_type.fire;
            trap_details.trap_sprite = 2;
            trap_details.spawnShape = DOT_effect.spawn_shape.barrier;
            trap_details.trap_destroy_on_trigger = false;
            Mirror.NetworkServer.Spawn(trap_deployed);
        }



    }
    /* void dropDecoys(skill skillRequested)
     {
         Reset_enemies_players_aggro();
         var nameplate = "";
         if (PlayerGuild.PlayerGuildAcro != string.Empty)
         {
             nameplate = "<color=#" + ColorUtility.ToHtmlStringRGBA(PlayerAccountInfo.PlayerNickColor) + ">[" + PlayerGuild.PlayerGuildAcro + "]" + PlayerAccountInfo.PlayerNickname + " Lv." + PlayerStats.PlayerLevel + "</color>";
         }
         else
         {
             nameplate = "<color=#" + ColorUtility.ToHtmlStringRGBA(PlayerAccountInfo.PlayerNickColor) + ">" + PlayerAccountInfo.PlayerNickname + " Lv." + PlayerStats.PlayerLevel + "</color>";
         }
         var limit = skillRequested.multipliers[0] + 1;
         for (int i = 1; i < limit; i++)
         {

             GameObject decoy_deployed = Instantiate(decoy_prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
             DecoyGeneral Decoy_details = decoy_deployed.GetComponent<DecoyGeneral>();

             Decoy_details.decoy_canvas = nameplate;

             Decoy_details.decoy_armor_sprite = PlayerAnimatorC.PlayerArmorSkin;
             Decoy_details.decoy_weapon_sprite = PlayerAnimatorC.PlayerWeaponSkin;
             Decoy_details.decoy_class = PlayerStats.PlayerClass_now.ToString();
             Decoy_details.decoy_title = PlayerGeneral.PlayerTitleInUse;
             Decoy_details.Auto_Destroy_in = 5f;
             Mirror.NetworkServer.Spawn(decoy_deployed);
             if (i == 5)
             {
                 decoy_deployed.GetComponent<EnemyControllerAI>().destination = transform.position;
             }
             else
             {
                 decoy_deployed.GetComponent<EnemyControllerAI>().destination = RandomPosition(i);
             }

             decoy_deployed.GetComponent<EnemyControllerAI>().maxSpeed = PlayerMPSync.PlayerSpeed;
         }

     }*/
    public void Reset_enemies_players_aggro()
    {
        PlayerGeneral.RpcUnTargetMe(gameObject);
        var enemies_around = PlayerGeneral.x_ObjectHelper.get_AOE_LOS_targets(gameObject, PlayerStats.Skill_range, LayerMask.GetMask("Enemy"), false);
        foreach (var enemy in enemies_around)
        {
            if (enemy.GetComponent<EnemyAttack>().PlayerToAttack == gameObject)
            {
                enemy.GetComponent<EnemyAggro>().resetAggro(false);
            }

        }
    }
    #endregion

    #region Utilidades
    public void stopcasting()
    {
        if (casting_coroutine != null)
        {
            StopCoroutine(casting_coroutine);            
        }
        is_casting = false;
        casting_speed_reduction = 0f;
        PlayerStats.RefreshStats();
    }
    public bool checkMana(int mana_usage)
    {
        if (PlayerStats.CurrentMP >= mana_usage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector3 RandomDirection(int variable)
    {
        Vector3 randomDirection = new Vector3(Random.Range(-5.0f * variable * Random.Range(-1, 1), 5.0f * variable * Random.Range(-1, 1)), Random.Range(-5.0f * variable * Random.Range(-1, 1), 5.0f * variable * Random.Range(-1, 1)), 0);
        return randomDirection;
    }
    Vector3 RandomPosition(int variable)
    {
        var target = transform.position + RandomDirection(variable);
        return target;
    }
    #endregion

    #region Open PVP/Party & Arena checks
    public enum target_modes
    {
        everyone,
        my_team_only,
        outside_my_team_only,
        cancel

    }
    public bool PVP_Arena_Party_checks(skill skillRequested, GameObject targetPlayer)
    {
        if (PlayerPVPDamage.isPVPenabled && targetPlayer.GetComponent<PlayerPVPDamage>().isPVPenabled)
        {
            target_modes mode = target_modes.cancel;
            if (PlayerPVPDamage.isInArena)
            {
                #region affect based on skill in arena
                switch (skillRequested.type)
                {
                    case skill.Stype.target_damage:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.AOE_buff:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.selfBuff:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.AOE_damage:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.trap:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.selfHeal_over_time:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.statBuff:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.AOE_heal:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.AOE_cleanse:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.target_DOT:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.AOE_revive:
                        mode = target_modes.my_team_only;
                        break;
                    case skill.Stype.target_debuff:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.decoy:
                        mode = target_modes.outside_my_team_only;
                        break;
                    case skill.Stype.totem_spawn:
                        mode = target_modes.my_team_only;
                        break;
                    default:
                        break;
                }
                #endregion

                if (arena_should_affect_target(gameObject, targetPlayer, mode))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                switch (PlayerPVPDamage.PVPmodeOn)
                {

                    case "HuntMode"://si yo esto en modo Hunt                  
                        if (targetPlayer.GetComponent<PlayerAccountInfo>().isPlayerPK)//y el target es PK
                        {

                            switch (skillRequested.type)
                            {
                                case skill.Stype.target_damage:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.AOE_buff:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.selfBuff:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_damage:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.trap:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.selfHeal_over_time:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.statBuff:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_heal:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_cleanse:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.target_DOT:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.AOE_revive:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.target_debuff:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.decoy:
                                    mode = target_modes.outside_my_team_only;
                                    break;
                                case skill.Stype.totem_spawn:
                                    mode = target_modes.my_team_only;
                                    break;
                                default:
                                    break;
                            }
                            if (o_pvp_should_affect_target(gameObject, targetPlayer, mode))
                            {//no atacar pk en mi party a menos que sea un buff
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {//target no es pk pero puede no estar en el mismo party
                            #region affect based on pvp mode
                            switch (skillRequested.type)
                            {
                                case skill.Stype.AOE_buff:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_heal:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_cleanse:
                                    mode = target_modes.my_team_only;
                                    break;
                                case skill.Stype.AOE_revive:
                                    mode = target_modes.everyone;
                                    break;
                                case skill.Stype.totem_spawn:
                                    mode = target_modes.my_team_only;
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            if (o_pvp_should_affect_target(gameObject, targetPlayer, mode))
                            {//no atacar pk en mi party
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    case "PKMode"://si yo estoy en modo PK
                        #region affect based on pvp mode
                        switch (skillRequested.type)
                        {
                            case skill.Stype.target_damage:
                                mode = target_modes.outside_my_team_only;
                                break;
                            case skill.Stype.target_DOT:
                                mode = target_modes.outside_my_team_only;
                                break;
                            case skill.Stype.target_debuff:
                                mode = target_modes.outside_my_team_only;
                                break;
                            case skill.Stype.AOE_buff:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_damage:
                                mode = target_modes.outside_my_team_only;
                                break;
                            case skill.Stype.AOE_heal:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_cleanse:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_revive:
                                mode = target_modes.everyone;
                                break;
                            case skill.Stype.totem_spawn:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.trap:
                                mode = target_modes.outside_my_team_only;
                                break;
                            default:
                                break;
                        }
                        #endregion
                        if (o_pvp_should_affect_target(gameObject, targetPlayer, mode))
                        {//atacar
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "PassiveMode"://Si yo estoy en pasivo  
                        #region affect based on pvp mode
                        switch (skillRequested.type)
                        {
                            case skill.Stype.AOE_buff:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_heal:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_cleanse:
                                mode = target_modes.my_team_only;
                                break;
                            case skill.Stype.AOE_revive:
                                mode = target_modes.everyone;
                                break;
                            case skill.Stype.totem_spawn:
                                mode = target_modes.my_team_only;
                                break;
                            default:
                                break;
                        }
                        #endregion
                        if (o_pvp_should_affect_target(gameObject, targetPlayer, mode))
                        {//no atacar pk en mi party
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
        }
        else
        {
            bool approved = false;
            #region skills soporte
            switch (skillRequested.type)
            {
                case skill.Stype.AOE_buff:
                    approved = true;
                    break;
                case skill.Stype.AOE_heal:
                    approved = true;
                    break;
                case skill.Stype.AOE_cleanse:
                    approved = true;
                    break;
                case skill.Stype.AOE_revive:
                    approved = true;
                    break;
                case skill.Stype.totem_spawn:
                    approved = true;
                    break;
                default:
                    approved = false;
                    break;
            }
            #endregion
            if (approved)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool arena_should_affect_target(GameObject PlayerOne, GameObject PlayerTwo, target_modes mode)
    {
        if (mode != target_modes.cancel)
        {
            if (PlayerOne.GetComponent<PlayerPVPDamage>().isPVPenabled && PlayerTwo.GetComponent<PlayerPVPDamage>().isPVPenabled)//los dos en pvp
            {
                if (PlayerOne.GetComponent<PlayerPVPDamage>().isInArena && PlayerTwo.GetComponent<PlayerPVPDamage>().isInArena)//los dos en arena
                {
                    if (PlayerOne.GetComponent<PlayerAccountInfo>().arena_team != "targetable" && PlayerTwo.GetComponent<PlayerAccountInfo>().arena_team != "targetable")//los dos en algun equipo
                    {
                        if (PlayerOne.GetComponent<PlayerAccountInfo>().arena_team != "spectator" && PlayerTwo.GetComponent<PlayerAccountInfo>().arena_team != "spectator")//nadie es spectator
                        {
                            if (mode == target_modes.everyone)
                            {
                                return true;
                            }
                            else
                            {
                                if (mode == target_modes.my_team_only)//heals, clean, buffs... etc
                                {
                                    if (PlayerOne.GetComponent<PlayerAccountInfo>().arena_team == PlayerTwo.GetComponent<PlayerAccountInfo>().arena_team)//distintos equipos
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (PlayerOne.GetComponent<PlayerAccountInfo>().arena_team != PlayerTwo.GetComponent<PlayerAccountInfo>().arena_team)//distintos equipos
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }


                        }
                    }
                }
            }
        }
        return false;
    }
    public bool o_pvp_should_affect_target(GameObject PlayerOne, GameObject PlayerTwo, target_modes mode)
    {
        if (PlayerOne == PlayerTwo)
        {
            switch (mode)
            {
                case target_modes.everyone:
                    return true;
                case target_modes.my_team_only:
                    return true;
                case target_modes.outside_my_team_only:
                    return false;
                case target_modes.cancel:
                    return false;
                default:
                    break;
            }
        }
        else
        {

            if (mode != target_modes.cancel)
            {
                if (mode == target_modes.everyone)
                {
                    return true;
                }
                else
                {
                    if (PlayerOne.GetComponent<PlayerGeneral>().PartyID != PlayerTwo.GetComponent<PlayerGeneral>().PartyID)
                    {//distintos partys (incluyendo uno en party y otro no)
                        if (mode == target_modes.my_team_only)
                        {
                            return false;
                        }
                        else if (mode == target_modes.outside_my_team_only)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        if (PlayerOne.GetComponent<PlayerGeneral>().PartyID == string.Empty && PlayerTwo.GetComponent<PlayerGeneral>().PartyID == string.Empty)
                        {//nadie en party
                            if (mode == target_modes.my_team_only)
                            {
                                return false;
                            }
                            else if (mode == target_modes.outside_my_team_only)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (mode == target_modes.my_team_only)
                            {
                                return true;
                            }
                            else if (mode == target_modes.outside_my_team_only)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    #endregion

}
