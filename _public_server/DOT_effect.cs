using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT_effect : NetworkBehaviour
{
    #region Collision detection
    public ContactFilter2D ContactFilter2D;
    public float scan_time;
    #endregion

    #region Networking
    [SyncVar]
    public int trap_sprite;
    #endregion

    #region Trap details
    public enum spawn_shape
    {
        trap,
        barrier,
        circular
    }
    public spawn_shape spawnShape;
    public float vanish_timer;
    public GameObject owner;
    public effect_type trap_effect;
    public float trap_effect_power;
    public enum effect_type
    {
        poison,
        fire,
        bleed,
        heal,
        apply_slow_debuff,
        apply_stun_debuff,
        apply_bomb_debuff,
        true_damage_poison,
        true_damage_poison_lesser

    }
    public string trap_pvp_status;
    public bool trap_destroy_on_trigger;
    public skill skillRequested;
    #endregion

    #region Data
    public bool IsReady;
    public List<Collider2D> objects_where_here = new List<Collider2D>();
    Collider2D trap_collider;
    bool triggered;
    #endregion

    // Use this for initialization
    private void Awake()
    {
        switch (spawnShape)
        {
            case spawn_shape.trap:
                trap_collider = GetComponent<PolygonCollider2D>();
                break;
            case spawn_shape.barrier:
                trap_collider = GetComponent<PolygonCollider2D>();
                break;
            case spawn_shape.circular:
                trap_collider = GetComponent<CircleCollider2D>();
                break;
            default:
                trap_collider = GetComponent<PolygonCollider2D>();
                break;
        }

    }
    void Start()
    {
        try
        {
            StartCoroutine(deployTrap());
            StartCoroutine(expire_trap());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("DOT exeception \n " + ex.ToString());
            RpcDestroyMe();
            Destroy(gameObject);
            throw;
        }
        
    }


    IEnumerator deployTrap()
    {
        yield return new WaitForSeconds(0.1f);
        IsReady = true;
        check_existng_objects();       
        StartCoroutine(keep_checking_object());
    }
    IEnumerator expire_trap()
    {
        if (vanish_timer != 0)
        {
            yield return new WaitForSeconds(vanish_timer);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }        
        RpcDestroyMe();
        Destroy(gameObject);
    }
    IEnumerator keep_checking_object()
    {
        yield return new WaitForSeconds(scan_time);
        check_existng_objects();
        StartCoroutine(keep_checking_object());
    }

    #region Triggers
    private void OnTriggerEnter2D(Collider2D triggeredBy)
    {
        if (IsReady)
        {
            triggerTrap(triggeredBy);
            if (!objects_where_here.Contains(triggeredBy))
            {
                objects_where_here.Add(triggeredBy);
            }
        }
        else
        {
            if (!objects_where_here.Contains(triggeredBy))
            {
                objects_where_here.Add(triggeredBy);
            }
        }
    }
    /* private void OnTriggerStay2D(Collider2D triggeredBy)
     {
         ////////.LogError("Capturado");
         if (IsReady && !triggered)
         {
             triggerTrap(triggeredBy);
         }
         else
         {
             if (!objects_where_here.Contains(triggeredBy))
             {
                 objects_where_here.Add(triggeredBy);
             }
         }
     }*/
    #endregion


    private void triggerTrap(Collider2D triggeredBy)
    {
        try
        {
            if (triggeredBy != null && !triggered)
            {
                if (triggeredBy.tag == "Player")
                {
                    if (triggeredBy.gameObject.GetComponent<PlayerPVPDamage>().isPVPenabled)
                    {
                        if (owner.GetComponent<PlayerPVPDamage>().isInArena)
                        {
                            if (triggeredBy.GetComponent<PlayerSkillsActions>().o_pvp_should_affect_target(owner, triggeredBy.gameObject, PlayerSkillsActions.target_modes.outside_my_team_only))
                            {
                                pvp_trap_triggered(triggeredBy);
                            }

                        }
                        else
                        {

                            switch (trap_pvp_status)
                            {

                                case "HuntMode"://si la trampa fue colocada en este modo
                                    if (triggeredBy.gameObject.GetComponent<PlayerAccountInfo>().isPlayerPK && triggeredBy.GetComponent<PlayerSkillsActions>().o_pvp_should_affect_target(owner, triggeredBy.gameObject, PlayerSkillsActions.target_modes.outside_my_team_only))
                                    {
                                        pvp_trap_triggered(triggeredBy);
                                    }
                                    break;
                                case "PKMode":
                                    if (triggeredBy.GetComponent<PlayerSkillsActions>().o_pvp_should_affect_target(owner, triggeredBy.gameObject, PlayerSkillsActions.target_modes.outside_my_team_only))
                                    {
                                        pvp_trap_triggered(triggeredBy);
                                    }
                                    break;
                                case "PassiveMode":

                                    break;
                                default:
                                    break;
                            }
                        }


                    }
                }
                else if (triggeredBy.tag == "Enemy")
                {
                    if (trap_effect == effect_type.apply_slow_debuff)
                    {
                        var chance = 100;
                        if (Random.Range(1, 100) <= chance)
                        {
                            triggeredBy.gameObject.GetComponent<EnemyConditions>().add_buff_debuff(2, skillRequested, false, 2f, owner, EnemyConditions.type.debuff, true);
                            triggeredBy.gameObject.GetComponent<EnemyConditions>().slowed = true;
                            triggeredBy.gameObject.GetComponent<Pathfinding.EnemyControllerAI>().maxSpeed *= 0.80f;
                           
                        }
                        else
                        {
                            owner.GetComponent<PlayerGeneral>().showCBT(triggeredBy.gameObject, false, false, 0, "Trap fail");
                        }
                    }
                    else if (trap_effect == effect_type.apply_stun_debuff)
                    {
                        var chance = 100;
                        if (Random.Range(1, 100) <= chance)
                        {
                            triggeredBy.gameObject.GetComponent<EnemyConditions>().add_buff_debuff(1, skillRequested, false, 2f, owner, EnemyConditions.type.debuff, true);
                            triggeredBy.gameObject.GetComponent<EnemyConditions>().stunned = true;
                            triggeredBy.gameObject.GetComponent<Pathfinding.EnemyControllerAI>().canMove = false;
                        }
                        else
                        {
                            owner.GetComponent<PlayerGeneral>().showCBT(triggeredBy.gameObject, false, false, 0, "Trap fail");
                        }

                    }
                    else if (trap_effect == effect_type.apply_bomb_debuff)
                    {
                        var chance = 100;
                        if (Random.Range(1, 100) <= chance)
                        {
                            triggeredBy.gameObject.GetComponent<EnemyConditions>().add_buff_debuff(3, skillRequested, false, 2f, owner, EnemyConditions.type.debuff, true);
                        }
                        else
                        {
                            owner.GetComponent<PlayerGeneral>().showCBT(triggeredBy.gameObject, false, false, 0, "Trap fail");
                        }

                    }
                    else
                    {
                        triggeredBy.gameObject.GetComponent<EnemyConditions>().handle_effect(trap_effect, trap_effect_power, owner);

                    }
                    
                    if (trap_destroy_on_trigger)
                    {
                        RpcDestroyMe();
                        Destroy(gameObject);
                        triggered = true;
                    }
                }
                

                
            }
        }
        catch (System.Exception)
        {
            return;
        }

    }

   
    private void pvp_trap_triggered(Collider2D triggeredBy)
    {
        bool trap_fail = false;
        if (trap_effect == effect_type.apply_slow_debuff)
        {
            var chance = 100;
            if (Random.Range(1, 100) <= chance)
            {
                triggeredBy.gameObject.GetComponent<PlayerConditions>().add_buff_debuff(2, skillRequested, false, 2f, owner, PlayerConditions.type.debuff, true);
                triggeredBy.gameObject.GetComponent<PlayerConditions>().decreasedWalkingSpeed = -20f;
                triggeredBy.gameObject.GetComponent<PlayerStats>().RefreshStats();
            }
            else
            {
                trap_fail = true;
            }

        }
        else if (trap_effect == effect_type.apply_stun_debuff)
        {
            var chance = 100;
            if (Random.Range(1, 100) <= chance)
            {
                triggeredBy.gameObject.GetComponent<PlayerConditions>().add_buff_debuff(1, skillRequested, false, 2f, owner, PlayerConditions.type.debuff, true);
                triggeredBy.gameObject.GetComponent<PlayerConditions>().stunned = true;
                triggeredBy.gameObject.GetComponent<PlayerMPSync>().PlayerCanMove = false;
            }
            else
            {
                trap_fail = true;
            }

        }
        else if (trap_effect == effect_type.apply_bomb_debuff)
        {
            var chance = 100;
            if (Random.Range(1, 100) <= chance)
            {
                triggeredBy.gameObject.GetComponent<PlayerConditions>().add_buff_debuff(3, skillRequested, false, 2f, owner, PlayerConditions.type.debuff, true);
            }
            else
            {
                trap_fail = true;
            }

        }
        else
        {
            triggeredBy.gameObject.GetComponent<PlayerConditions>().handle_effect(trap_effect, trap_effect_power, owner, 0);
        }
        if (trap_fail)
        {
            triggeredBy.gameObject.GetComponent<PlayerGeneral>().showCBT(triggeredBy.gameObject, false, false, 0, "Trap fail");
        }
        if (trap_destroy_on_trigger)
        {
            triggered = true;
            RpcDestroyMe();
            Destroy(gameObject);
        }

    }

    #region Networking RPC
    [ClientRpc]
    public void RpcDestroyMe()
    {

    }
    #endregion


    #region Utilidades
    void check_existng_objects()
    {
        Collider2D[] found = new Collider2D[20];
        var colliderFound = new Collider2D[trap_collider.OverlapCollider(ContactFilter2D, found)];

        for (int i = 0; i < colliderFound.Length; i++)
        {
            if (objects_where_here.Contains(found[i]))
            {
                if (IsReady)
                {
                    triggerTrap(objects_where_here[i]);
                }
            }
        }

    }
    #endregion
}
