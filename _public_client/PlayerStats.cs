using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    #region Player
    PlayerGeneral PlayerGeneral;
    PlayerSkills PlayerSkills;
    PlayerConditions PlayerConditions;
    PlayerInventory PlayerInventory;
    PlayerAnimatorC PlayerAnimatorC;
    PlayerTargetController PlayerTargetController;
    PlayerDeath PlayerDeath;
    #endregion

    #region Networking
    [SyncVar(hook = "UpdateCurrentHP")]
    public float CurrentHP;
    [SyncVar(hook = "UpdateCurrentMP")]
    public float CurrentMP;
    [SyncVar(hook = "UpdateCurrentMaxHP")]
    public float MaxHealth;
    [SyncVar(hook = "UpdateCurrentMaxMP")]
    public float MaxMana;
    [SyncVar(hook = "UpdateClass")]
    public string PlayerSelectedClass = "Archer";
    [SyncVar(hook = "UpdateLevel")]
    public int PlayerLevel;
    #endregion

    #region Data
    public Dictionary<int, int> enchants_grouped = new Dictionary<int, int>();
    [Header("EXP configs")]
    public float baseXP = 1000;
    public float exponent = 1.65f;
    bool in_tutorial;
    [Header("UI stuff")]
    public Image playerHealthBar;
    public PlayerClass PlayerSelectedClass_now;
    public enum PlayerClass
    {
        Any,
        Hunter,
        Wizard,
        Paladin,
        Warrior

    }

    [Header("Statistics")]
    public int special_mobs_killed;
    public int Total_rebirths;
    public float CurrentEXP;
    public float Exp100;
    public float thisLevelExp;
    public float nextLevelExp;
    public float AutoAttackSpeed;
    public float AutoAttackRange;
    public float SkillRange;
    public float PlayerEnemySelectionRange = 5f;
    public float PlayerDropChance;
    public float ExtraGoldDrop;
    public float ExtraExp;
    public float CD_reduction;
    public float casting_reduction;

    //Stats
    public string[] DetailedStats;


    #region Stats 
    [Header("STATS")]
    public int[] PlayerCustomStats_lvl;
    public int[] PlayerCustomStats_reb;
    public int[] free_PlayerCustomStats = new int[2]; //[0]lvl - [1] reb

    public float Damage_str = 0;
    public float Damage_int = 0;





    #endregion

    #endregion
    #region Debug
    void OnDrawGizmosSelected()
    {
        var center = this.transform.position;
        Color tmp = Color.yellow;
        tmp.a = tmp.a / 3f;
        Gizmos.color = tmp;
        Gizmos.DrawSphere(center, SkillRange);

    }
    #endregion


    private void Awake()//No puede haber nada que use PlayerGeneral.ObjectHelper aqui
    {
        PlayerDeath = GetComponent<PlayerDeath>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerInventory = GetComponent<PlayerInventory>();
        PlayerConditions = GetComponent<PlayerConditions>();
        PlayerSkills = GetComponent<PlayerSkills>();
        PlayerAnimatorC = GetComponent<PlayerAnimatorC>();
        PlayerTargetController = GetComponent<PlayerTargetController>();
    }
    public override void OnStartClient()
    {
        //The values of SyncVars on object are guaranteed
        if (PlayerSelectedClass == "Warrior")
        {
            var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
            main.startSize = 1.9f;
            PlayerSelectedClass_now = PlayerClass.Warrior;
        }
        else if (PlayerSelectedClass == "Archer")
        {
            var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
            main.startSize = 3.8f;
            PlayerSelectedClass_now = PlayerClass.Hunter;
        }
        else if (PlayerSelectedClass == "Mage")
        {
            var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
            main.startSize = 5.6f;
            PlayerSelectedClass_now = PlayerClass.Wizard;

        }
        else if (PlayerSelectedClass == "Support")
        {
            var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
            main.startSize = 1.9f;
            PlayerSelectedClass_now = PlayerClass.Paladin;
        }

        PlayerAnimatorC.load_skins_now();

    }
    public override void OnStartLocalPlayer()
    {
        DetailedStats = new string[60];
    }
    private void Start()
    {
        updateMiniHpBar();
    }


    #region Hooks
    void UpdateLevel(int currentLVL)
    {
        PlayerLevel = currentLVL;
        RefreshUIBars();
    }
    void UpdateCurrentHP(float hp)
    {
        CurrentHP = hp;
        if (CurrentHP <= 0)
        {
            PlayerDeath.triggerPlayerDeath();
            PlayerSkills.stop_casting();
        }
        else
        {
            if (PlayerDeath.isPlayerDead)
            {
                PlayerDeath.triggerRevival();
            }
        }
        updateMiniHpBar();
        //update UI bars
        RefreshUIBars();
    }
    void UpdateCurrentMaxHP(float mhp)
    {
        MaxHealth = mhp;
        updateMiniHpBar();
        RefreshUIBars();
    }
    void UpdateClass(string newclass)
    {
        //.LogError(PlayerSelectedClass + " PlayerSelectedClass" + newclass + " newclass");
        PlayerSelectedClass = newclass;

        if (PlayerSelectedClass == "Warrior")
        {
            PlayerSelectedClass_now = PlayerClass.Warrior;
        }
        else if (PlayerSelectedClass == "Archer")
        {
            PlayerSelectedClass_now = PlayerClass.Hunter;
        }
        else if (PlayerSelectedClass == "Mage")
        {
            PlayerSelectedClass_now = PlayerClass.Wizard;
        }
        else if (PlayerSelectedClass == "Support")
        {
            PlayerSelectedClass_now = PlayerClass.Paladin;
        }

        if (isLocalPlayer)
        {
            var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
            main.startSize = SkillRange * 2;
        }

        PlayerAnimatorC.load_skins_now();
    }
    void UpdateCurrentMP(float mp)
    {
        CurrentMP = mp;
        RefreshUIBars();
    }
    void UpdateCurrentMaxMP(float mmp)
    {
        MaxMana = mmp;
        RefreshUIBars();
    }
    #endregion


    #region Networking Server
    [Command]
    public void CmdPlayerCurrentSelectedClass(string PlayerSelectedClassCmd) { }
    [Command]
    public void CmdChangeClassWithGems(string PlayerSelectedClassCmd) { }
    [Command]
    public void CmdRebirthNow(int reward) { }
    [Command]
    public void CmdSaveCustomStats(int[] add_level_stats, int[] add_rebirth_stats) { }
    [Command]
    public void CmdPONG(int ping){}
    #endregion

    #region Networking Client      
    [TargetRpc]
    void TargetConfirmClassChange(NetworkConnection target, string PlayerSelectedClassCmd)
    {
        PlayerSelectedClass = PlayerSelectedClassCmd;
        PlayerGeneral.ObjectHelper.close_class_panel();
    }
    [TargetRpc]
    void TargetReceiveExpData(NetworkConnection target, float currentExpServer, float thisLevelExp, float Exp100, float nextLevelExp)
    {
        CurrentEXP = currentExpServer;
        this.thisLevelExp = thisLevelExp;
        this.Exp100 = Exp100;
        this.nextLevelExp = nextLevelExp;

        if (CurrentEXP == 1000)
        {
            //tutorial
            PlayerGeneral.ObjectHelper.tuto_account_setup_0.SetActive(true);
            in_tutorial = true;
#if UNITY_ANDROID
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialBegin);
#endif
        }
        if (CurrentEXP == 1001 && in_tutorial)
        {
            in_tutorial = false;
#if UNITY_ANDROID
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialComplete);
#endif
        }
        RefreshUIBars();

    }
    [TargetRpc]
    void TargetReceiveCustomStats(NetworkConnection target, int[] lvl_stats, int lvl_free, int[] reb_stats, int reb_free)
    {
        //todo: wtf is this doing here? lol get this out of here asap
        GameObject.Find("DeathRespawnButton").GetComponent<Button>().interactable = true;

        PlayerCustomStats_lvl = lvl_stats;
        PlayerCustomStats_reb = reb_stats;
        free_PlayerCustomStats = new int[2];
        free_PlayerCustomStats[0] = lvl_free;
        free_PlayerCustomStats[1] = reb_free;
        PlayerGeneral.ObjectHelper.UIshowStatsDetails.UpdateStats();
        //todo-after-ui: show new stats on UI
    }
    [TargetRpc]
    public void TargetTotalRebirths(NetworkConnection target, int totalRebirths)
    {
        Total_rebirths = totalRebirths;
    }
    [TargetRpc]
    void TargetUpdateSpecialMobsKilled(NetworkConnection target, int mobs)
    {
        special_mobs_killed = mobs;
    }
    [TargetRpc]
    void TargetUpdateStats_base(NetworkConnection target, float[] detailedStats)
    {
        //this is because im not sure how many times this is called on server
        // //Debug.LogWarning("ProcessStats() on server");


        //base stats        

        DetailedStats[0] = detailedStats[0].ToString();
        DetailedStats[1] = detailedStats[1].ToString();
        DetailedStats[2] = detailedStats[2].ToString();
        DetailedStats[3] = detailedStats[3].ToString();
        DetailedStats[4] = detailedStats[4].ToString();
        DetailedStats[5] = detailedStats[5].ToString();
        DetailedStats[6] = detailedStats[6].ToString();
        DetailedStats[7] = detailedStats[7].ToString();
        DetailedStats[8] = detailedStats[8].ToString();

        DetailedStats[9] = detailedStats[9].ToString() + "%";
        DetailedStats[10] = detailedStats[10].ToString() + "s";
        DetailedStats[11] = detailedStats[11].ToString() + "%";
        DetailedStats[12] = detailedStats[12].ToString() + "s";
        DetailedStats[13] = detailedStats[13].ToString();
        DetailedStats[14] = detailedStats[14].ToString() + "%";
        DetailedStats[15] = detailedStats[15].ToString() + "m";
        DetailedStats[16] = (detailedStats[16]).ToString();
        DetailedStats[17] = (detailedStats[17]).ToString();
        DetailedStats[18] = (detailedStats[18] * 100f).ToString() + "%";
        DetailedStats[19] = detailedStats[19].ToString() + "m";
        if (detailedStats[20] >= 50f)
        {
            DetailedStats[20] = string.Format("<color=red>{0}% Cap.</color>", detailedStats[20]);
        }
        else
        {
            DetailedStats[20] = detailedStats[20].ToString() + "%";
        }

        DetailedStats[21] = detailedStats[21].ToString();
        DetailedStats[22] = detailedStats[22].ToString() + "%";
        DetailedStats[23] = detailedStats[23].ToString();
        DetailedStats[24] = detailedStats[24].ToString();

        DetailedStats[25] = "+" + detailedStats[25].ToString();
        DetailedStats[26] = "+" + detailedStats[26].ToString();
        DetailedStats[27] = "+" + detailedStats[27].ToString();
        DetailedStats[28] = "+" + detailedStats[28].ToString();
        DetailedStats[29] = "+" + detailedStats[29].ToString();
        DetailedStats[30] = "+" + detailedStats[30].ToString();
        DetailedStats[31] = "+" + detailedStats[31].ToString();
        DetailedStats[32] = "+" + detailedStats[32].ToString();
        DetailedStats[33] = "+" + detailedStats[33].ToString();
        DetailedStats[34] = "+" + detailedStats[34].ToString();
        DetailedStats[35] = detailedStats[35].ToString() + "%";
        DetailedStats[36] = detailedStats[36].ToString() + "%";
        DetailedStats[37] = detailedStats[37].ToString() + "%";
        DetailedStats[38] = detailedStats[38].ToString() + "%";
        DetailedStats[39] = detailedStats[39].ToString() + "%";
        DetailedStats[40] = detailedStats[40].ToString() + "%";
        DetailedStats[41] = detailedStats[41].ToString() + "%";
        DetailedStats[42] = detailedStats[42].ToString() + "%";
        DetailedStats[43] = detailedStats[43].ToString() + "%";
        DetailedStats[44] = detailedStats[44].ToString() + "%";

        DetailedStats[45] = detailedStats[45].ToString() + "%";
        DetailedStats[46] = detailedStats[46].ToString() + "%";
        DetailedStats[47] = detailedStats[47].ToString() + "%";
        DetailedStats[48] = detailedStats[48].ToString() + "%";
        DetailedStats[49] = detailedStats[49].ToString() + "%";
        DetailedStats[50] = detailedStats[50].ToString() + "%";
        DetailedStats[51] = detailedStats[51].ToString() + "%";

        DetailedStats[52] = detailedStats[52].ToString() + "%";
        DetailedStats[53] = detailedStats[53].ToString() + "%";
        DetailedStats[54] = detailedStats[54].ToString() + "%";
        DetailedStats[55] = detailedStats[55].ToString() + "%";

        DetailedStats[56] = detailedStats[56].ToString() + "%";
        DetailedStats[57] = detailedStats[57].ToString() + "%";
        DetailedStats[58] = detailedStats[58].ToString() + "%";
        if (detailedStats[59] >= 30f)
        {
            DetailedStats[59] = string.Format("<color=red>{0}% Cap.</color>", detailedStats[59]);
        }
        else
        {
            DetailedStats[59] = detailedStats[59].ToString() + "%";
        }



        Damage_str = detailedStats[13];
        Damage_int = detailedStats[17];

        PlayerDropChance = detailedStats[56];
        ExtraGoldDrop = detailedStats[57];
        ExtraExp = detailedStats[58];
        CD_reduction = detailedStats[59];
        casting_reduction = detailedStats[20];

        AutoAttackSpeed = detailedStats[16];
        AutoAttackRange = detailedStats[19];

        SkillRange = detailedStats[15];
        var main = PlayerTargetController.attackArea.GetComponent<ParticleSystem>().main;
        main.startSize = SkillRange * 2;

        PlayerGeneral.ObjectHelper.UIshowStatsDetails.UpdateStats();
    }
    [TargetRpc]
    void TargetUpdateStats_base_account(NetworkConnection target, string[] detailedStats)
    {

        PlayerGeneral.ObjectHelper.UIshowStatsDetails.UdateAccountStatistics(detailedStats);
    }
    [TargetRpc]
    public void TargetSendTraning_Exp_Data(NetworkConnection target, int[] stat_training_this_level_exp, float[] stat_effects, int[] stat_training_exp_100, int[] stat_training_levels)
    {
        PlayerGeneral.ObjectHelper.UIshowStatsDetails.Update_traning_Exp_Data(stat_training_this_level_exp, stat_effects, stat_training_exp_100, stat_training_levels);
    }
    [TargetRpc]
    public void TargetEquippedEnchants(NetworkConnection target, byte[] dictionary)
    {
        System.IO.MemoryStream StreamItem;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter;
        RearmEquippedEnchants(dictionary, out StreamItem, out binFormatter);
        enchants_grouped = binFormatter.Deserialize(StreamItem) as Dictionary<int, int>;
    }
    [TargetRpc]
    public void TargetPING(NetworkConnection target, int pong) {

        CmdPONG(pong);
    }
    #endregion

    #region Networking RPC
    [ClientRpc]
    void RpcLvlUp(GameObject player)
    {
        Animator animator = player.transform.GetChild(3).GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Effects/PixelEffects/Animation/Teleport") as RuntimeAnimatorController;
        animator.SetTrigger("Trigger");

#if UNITY_ANDROID

        if (isLocalPlayer)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
         Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
         new Firebase.Analytics.Parameter[] {
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, Total_rebirths),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterLevel, PlayerLevel),
         }
       );
        }

#endif
    }
    #endregion

    #region Utilidades
    public int getLevelfromExp(float current)
    {
        return (int)Mathf.Floor(Mathf.Pow((current / baseXP), 1f / (exponent + (Total_rebirths * 0.03f))));
    }
    private void RefreshUIBars()
    {
        //only update UI bars if its localplayer
        if (isLocalPlayer)
        {
            PlayerGeneral.ObjectHelper.UIbars.UpdateUIBars();
        }

    }
    public void rebirth_levelcheck()
    {
        if (PlayerLevel >= 150)
        {
            if (PlayerInventory.Gold >= 250000)
            {
                confirm_rebirth();
            }
            else
            {
                PlayerGeneral.ObjectHelper.NotificationHandler.UpdateWarnings("You need 250,000 Gold for a full rebirth");
            }
        }
        else
        {
            PlayerGeneral.ObjectHelper.NotificationHandler.UpdateWarnings("You need to be level 150 first...");
        }

    }
    public void confirm_rebirth()
    {
        PlayerGeneral.ObjectHelper.rebirth_confirmedBtn.onClick.RemoveAllListeners();
        PlayerGeneral.ObjectHelper.rebirth_confirmedBtn.onClick.AddListener(() => { rebirth_confirmed(); });
        PlayerGeneral.ObjectHelper.RebirthSection.SetActive(true);
    }
    public void rebirth_confirmed()
    {
        CmdRebirthNow(0);
        PlayerGeneral.ObjectHelper.Account_panel.SetActive(false);
        PlayerGeneral.ObjectHelper.RebirthSection.SetActive(false);
    }
    private void updateMiniHpBar()
    {
        playerHealthBar.fillAmount = CurrentHP / MaxHealth;
        Color healthBarInitialColor = Color.green;
        if (playerHealthBar.fillAmount >= 0.71)
        {
            playerHealthBar.color = healthBarInitialColor;
        }
        else if (playerHealthBar.fillAmount <= 0.7 && playerHealthBar.fillAmount >= 0.3)
        {
            playerHealthBar.color = Color.yellow;
        }
        else if (playerHealthBar.fillAmount <= 0.29)
        {
            playerHealthBar.color = Color.red;
        }
    }
    #endregion

    #region equipped enchants
    private static void RearmEquippedEnchants(byte[] dictionary, out System.IO.MemoryStream mStream, out System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter)
    {
        mStream = new System.IO.MemoryStream();
        binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        // Where 'objectBytes' is your byte array.
        mStream.Write(dictionary, 0, dictionary.Length);
        mStream.Position = 0;
    }
    #endregion

}
