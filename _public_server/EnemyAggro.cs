using UnityEngine;
using System.Collections;
using Pathfinding;
using Mirror;

public class EnemyAggro : NetworkBehaviour
{
    #region Enemy
    EnemyAttack EnemyAttack;
    EnemyControllerAI AILerpTest;
    EnemySpawnInfo EnemySpawnInfo;
    EnemyStats EnemyStats;
    EnemyTakeDamage EnemyTakeDamage;
    #endregion

    #region EnemyData  
    public GameObject player;
    public bool isAggroed;
    public bool wasAttacked;
    public Vector3 SpawnPosition;
    public float AggroRange;
    public bool vicious_attack_mode;
  
    public bool Keep_formation;
    public bool isAggroPassive;
    public GameObject last_aggroed_player;
    public bool is_network_ready;
    #endregion
    #region temp_data
    Coroutine temp_KeepAttackDistance; 
    #endregion

    private void Awake()
    {
        AILerpTest = GetComponent<EnemyControllerAI>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemySpawnInfo = GetComponent<EnemySpawnInfo>();
        EnemyStats = GetComponent<EnemyStats>();
        EnemyTakeDamage = GetComponent<EnemyTakeDamage>();
    }
    void Start()
    {
        SpawnPosition = this.transform.position;
        StartCoroutine(RoamingState());
        StartCoroutine(isEnemyTooFarFromOriginalPlace());
    }

    
    #region Networking Client
    [TargetRpc]
    public void TargetSetAggroIcon(NetworkConnection target, GameObject enemy, GameObject player)
    {

    }
    [TargetRpc]
    public void TargetUnSetAggroIcon(NetworkConnection target, GameObject enemy, GameObject player)
    {

    }
    [ClientRpc]
    public void Rpc_show_CBT(string cbt_text_data, bool critical, string text_ocasion)//el texto
    {

    }
    #endregion   

    #region Utilidades   
    Vector3 RandomDirection()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0);
        if (EnemySpawnInfo.isMobEvent)
        {
            randomDirection = new Vector3(Random.Range(-50f, 50f), Random.Range(50f, -50f), 0f);
        }

        return randomDirection;
    }
    Vector3 RandomPosition()
    {
        var target = transform.position + RandomDirection();
        return target;
    }
    IEnumerator RoamingState()
    {
        float randomin = 10f;
        float randomax = 20f;

        if (player == null)
        {
            var max_distan = 2f;
            if (EnemySpawnInfo.isMobEvent)
            {
                max_distan *= 10000f;
                randomin *= 100f;
                randomax *= 100f;
            }
            if (Keep_formation)
            {
                max_distan = 1f;
            }

            Vector3 nextPositionRoaming = RandomPosition();            
            if (AILerpTest.canMove && AILerpTest.canSearch)
            {
                if (!EnemySpawnInfo.isMobEvent)
                {
                    if (!isAggroed)
                    {
                        if (Vector2.Distance(nextPositionRoaming, SpawnPosition) > max_distan)
                        {
                            AILerpTest.destination = SpawnPosition;
                        }
                        else
                        {
                            AILerpTest.destination = nextPositionRoaming;

                        }

                    }
                }
                else
                {
                    AILerpTest.destination = nextPositionRoaming;
                }

            }

        }
        yield return new WaitForSeconds(Random.Range(randomin, randomax));
        StartCoroutine(RoamingState());
    }
    #endregion

    #region Aggro Start
    void setAggroOn()
    {
        if (!isAggroed)//if it wasnt aggroed before
        {
            EnemyAttack.reset_behavioursFlags();
            EnemyAttack.do_what_you_have_to();
        }

        ////Debug.LogError("Aggro Reset");
        EnemyStats.cancel_Quick_hp_regen();
        isAggroed = true;
    }
    void setAggroOff()
    {
        ////Debug.LogError("Aggro Reset");
        isAggroed = false;       
        EnemyStats.Quick_hp_regen();
        EnemyAttack.reset_behavioursFlags();
    }
    #endregion


    #region Aggro v2
    public void AggroedByAttack(GameObject AttackedByThisPlayer)
    {
        if (!isAggroed)
        {
            AggroNow(AttackedByThisPlayer);
        }
        else
        {
            if (Random.Range(0f, 100f) <= 30f)
            {
                AggroChange(AttackedByThisPlayer);
            }
        }
    }
    public void AggroedByPlayer(Collider2D PlayerCollider)
    {
        if (!isAggroed)
        {
            if (!isAggroPassive)
            {
                AggroNow(PlayerCollider.gameObject);
            }
        }
        else
        {
            if (Random.Range(0f, 100f) <= 10f)
            {
                AggroChange(PlayerCollider.gameObject);
            }
        }
    }
    public void AggroNow(GameObject AttackedByThisPlayer)
    {
        if (!EnemySpawnInfo.isMobEvent && is_network_ready)
        {
            player = AttackedByThisPlayer;
            TargetSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
            if (temp_KeepAttackDistance != null)
            {
                StopCoroutine(temp_KeepAttackDistance);
            }
            temp_KeepAttackDistance = StartCoroutine(KeepAttackDistance(player));

            //Lockeamos el target
            EnemyAttack.PlayerToAttack = player;
            if (AttackedByThisPlayer.tag == "Player")
            {
                TargetSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
            }
            setAggroOn();
            StartCoroutine(isTargetPlayerTooFar());
        }
    }
    IEnumerator KeepAttackDistance(GameObject player)
    {
        if (player != null)
        {
            ////Debug.LogError("Aggro Reset");
            if (Vector2.Distance(player.transform.position, gameObject.transform.position) <= 6f)
            {
                ////Debug.LogError("Aggro Reset");
                AILerpTest.destination = player.transform.position;
                TargetSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
                yield return new WaitForSeconds(0.5f);
                temp_KeepAttackDistance = StartCoroutine(KeepAttackDistance(player));
               
            }
            else
            {
                if (vicious_attack_mode)
                {
                    AILerpTest.destination = player.transform.position;
                    TargetSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
                    yield return new WaitForSeconds(0.5f);
                    temp_KeepAttackDistance = StartCoroutine(KeepAttackDistance(player));
                    
                }
                else
                {
                    ////Debug.LogError("Aggro Reset");
                    resetAggro(true);
                }
               
            }
        }
        else
        {
            ////Debug.LogError("Aggro Reset");
            resetAggro(true);
        }


    }
    public void resetAggro(bool reset_drop_tables)
    {
        if (vicious_attack_mode)
        {
            last_aggroed_player = player;
            StartCoroutine(recover_aggro());
        }
        
        if(reset_drop_tables && !vicious_attack_mode)
        {
            EnemyTakeDamage.DamageTrack.Clear();
        }
        if (temp_KeepAttackDistance != null)
        {
            StopCoroutine(temp_KeepAttackDistance);
        }
        if (player != null)
        {
            TargetUnSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
        }
        ////Debug.LogError("Aggro Reset");
        player = null;
        setAggroOff();
        EnemyAttack.PlayerToAttack = null;
        AILerpTest.destination = SpawnPosition;
    }
    IEnumerator recover_aggro()//wait till player camuflage is over and engage again
    {
        yield return new WaitForSeconds(1f);
        if (vicious_attack_mode)
        {
            if (last_aggroed_player != null)
            {
                if (!last_aggroed_player.GetComponent<NetworkProximityChecker>().forceHidden)
                {
                   // //Debug.LogError("Aggro Reset");
                    AggroedByAttack(last_aggroed_player);
                    yield break;
                }
                else
                {
                    StartCoroutine(recover_aggro());
                }

            }
        }
    }
    IEnumerator isTargetPlayerTooFar()
    {
        ////Debug.LogError("Aggro Reset");
        if (!vicious_attack_mode)//if it was not in viciouse mode: chase player no matter what
        {
            if (player != null)
            {
                if (Vector2.Distance(this.transform.position, player.transform.position) > 5)
                {
                    resetAggro(true);
                }
                yield return new WaitForSeconds(1f);
                StartCoroutine(isTargetPlayerTooFar());
            }
            else
            {
                resetAggro(true);
            }
            
        }
        ////Debug.LogError("Aggro Reset");
        if (player == null)
        {
            ////Debug.LogError("Aggro Reset");
            resetAggro(true);
        }
    }
    IEnumerator isEnemyTooFarFromOriginalPlace()
    {
        if (!vicious_attack_mode)//if it was not in viciouse mode: chase player no matter what
        {
            if (Vector2.Distance(this.transform.position, SpawnPosition) >= 10f)
            {
                resetAggro(true);
                yield return new WaitForSeconds(5f);//if distance hasnt improved yet then it is clearly being pulled
                if (Vector2.Distance(this.transform.position, SpawnPosition) >= 10f)
                {
                    EnemyTakeDamage.DestroyMob();
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(isEnemyTooFarFromOriginalPlace());
        }
    }
    public void AggroChange(GameObject new_player)
    {
        if (player != null)
        {
            TargetUnSetAggroIcon(player.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, player);
        }
        AggroNow(new_player);
    }
    #endregion

}