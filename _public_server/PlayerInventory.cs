using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityWebRequest = UnityEngine.Networking.UnityWebRequest;

public class PlayerInventory : NetworkBehaviour
{
    #region Player
    PlayerStatistics PlayerStatistics;
    PlayerAccountInfo PlayerAccountInfo;
    PlayerStats PlayerStats;
    PlayerAnimatorC PlayerAnimatorC;
    PlayerGeneral PlayerGeneral;
    PlayerSkills PlayerSkills;
    [HideInInspector]
    public Inventory Inventory;
    PlayerPVPDamage PlayerPVPDamage;
    PlayerConditions PlayerConditions;
    PlayerGuild PlayerGuild;
    PlayerDeath PlayerDeath;
    #endregion

    #region Inventory Data
    [HideInInspector]
    public int invSize;
    [HideInInspector]
    public int bankSize;
    [HideInInspector]
    public int Gold;
    [SerializeField]
    GameObject droppedItemPrefab;
    bool ServerConsumibleOneCD;
    bool ServerConsumibleTwoCD;
    #region Trading

    bool trade_locked;

    bool remote_trade_locked;
    string TradingWith;
    int currentGoldOffered;
    bool tradingCD;
    #endregion
    int gold_to_save_buffer;
    bool save_gold_allowed;
    #endregion
    #region Unity events
    private void Awake()
    {
        Inventory = GetComponent<Inventory>();
        PlayerStatistics = GetComponent<PlayerStatistics>();
        PlayerAccountInfo = GetComponent<PlayerAccountInfo>();
        PlayerStats = GetComponent<PlayerStats>();
        PlayerConditions = GetComponent<PlayerConditions>();
        PlayerAnimatorC = GetComponent<PlayerAnimatorC>();
        PlayerGeneral = GetComponent<PlayerGeneral>();
        PlayerPVPDamage = GetComponent<PlayerPVPDamage>();
        PlayerGuild = GetComponent<PlayerGuild>();
        PlayerSkills = GetComponent<PlayerSkills>();
        PlayerDeath = GetComponent<PlayerDeath>();
        StartCoroutine(save_gold_cd());
    }
    public void OnDestroy()
    {
        if (PlayerGeneral.playerLoaded)
        {
            var TradingWith_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(TradingWith);
            PlayerGeneral.ServerNetworkManager.cancelTrade(TradingWith_GO);
            //save whatever gold is in the buffer when player disconnects
            save_gold_to_db(0, string.Empty, true);
        }
    }
    #endregion
    #region test
    /* private void Update()
     {
         if (Input.GetKeyDown("space"))
         {
             material_add_amount_and_save(material.material_translation.ironIngot, 3, "space");
         }
         if (Input.GetKeyDown("return"))
         {
             material_add_amount_and_save(material.material_translation.ironIngot, -10, "return");
         }
     }*/
    #endregion

    #region Init   
    public IEnumerator getPlayerItems_and_materials(bool loadingProcess)
    {

        var urlServer = PlayerGeneral.ServerDBHandler.getPlayerInventory(PlayerAccountInfo.PlayerAccount);
        UnityWebRequest uwr = UnityWebRequest.Get(urlServer);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(getPlayerItems_and_materials(loadingProcess));
        }
        else
        {
            //now load player materials
            var urlServer_mats = PlayerGeneral.ServerDBHandler.getPlayerMaterials(PlayerAccountInfo.PlayerAccount);
            var uwr_mats = UnityWebRequest.Get(urlServer_mats);
            yield return uwr_mats.SendWebRequest();

            if (uwr_mats.isNetworkError || uwr_mats.isHttpError)
            {
                yield return new WaitForSeconds(5f);
                StartCoroutine(getPlayerItems_and_materials(loadingProcess));
            }
            else
            {
                //parse materials
                string[] materials_raw = uwr_mats.downloadHandler.text.Split(new string[] { "," }, System.StringSplitOptions.None);

                for (int i = 0; i < materials_raw.Length; i++)
                {
                    Inventory.MaterialsList.Add(new material((material.material_translation)i, int.Parse(materials_raw[i])));
                }
                //update on client
                MemoryStream mStream = SerializeItem(Inventory.MaterialsList);
                //lo envio
                TargetGetMaterials(connectionToClient, mStream.ToArray());
                //continue with items and loading process
                Load_and_Send_InventoryItemsFromDB(uwr.downloadHandler.text, loadingProcess);
            }
        }

    }
    private void Load_and_Send_InventoryItemsFromDB(string www, bool loadingProcess)
    {
        Inventory.InventoryList = new List<InventoryItem>();
        Inventory.EquippedList = new List<InventoryItem>();
        Inventory.TradingList = new List<InventoryItem>();
        Inventory.BankList = new List<InventoryItem>();

        string[] inventoryRaw = www.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var itemDataRaw in inventoryRaw)
        {
            string[] itemData = itemDataRaw.Split(new string[] { "+" }, System.StringSplitOptions.RemoveEmptyEntries);
            int itemID = int.Parse(itemData[0]);
            var _item = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(itemID);
            int itemUID = int.Parse(itemData[1]);
            int itemDurability = int.Parse(itemData[2]);
            List<int> itemMods = new List<int>();
            int itemUpgrades = int.Parse(itemData[5]);
            int enchantment = int.Parse(itemData[6]);
            int corruption = 0;
            int elemental = 0;
            string meta_data = string.Empty;
            if (itemData[4] != "0")
            {
                if (_item.useAs == Item.UseAs.Summon)
                {
                    meta_data = itemData[4];
                    //pet data
                    //pet_data[] indexes----[-0][---1---][-----2----][---3--][----4-----][----5----][--------6--------][---7---][---8--][--9--]
                    //mods comes like this: [P:][breeder][given_name][gender][metabolism][obedience][skill-skill-skill][ability][effect][color]
                    //example: [P:][Alkanovoo][MrPeto][0][100][100][10-12-13][34][50][128]
                    // string[] pet_data = itemData[4].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    //convert [--------6--------] to int[]
                    //int[] pet_skills = Array.ConvertAll(pet_data[6].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
                    //populate pet list   

                }
                else
                {
                    //normal item data
                    string[] modsData = itemData[4].Split(new string[] { "-" }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < modsData.Length; i++)
                    {
                        itemMods.Add(int.Parse(modsData[i]));
                    }
                }

            }
            if (itemData[3] == "inv")
            {
                AddItemToInv(itemID, itemUID, itemMods, itemUpgrades, itemDurability, enchantment, corruption, elemental, meta_data);
            }
            else if (itemData[3] == "equ")
            {
                AddItemToEquipped(itemID, itemUID, itemMods, itemUpgrades, itemDurability, enchantment, corruption, elemental, meta_data);
            }
            else if (itemData[3] == "ban")
            {
                AddItemToBank(itemID, itemUID, itemMods, itemUpgrades, itemDurability, enchantment, corruption, elemental, meta_data);
            }
        }
        apply_equipped_stats();
        SendWholeEquipmentToClient();
        SendWholeInventoryToClient();
        if (loadingProcess)
        {
            PlayerGeneral.E_loadStatistics();
        }

    }
    #endregion

    #region Contar STATS equipados

    public void apply_equipped_stats()
    {
        //Inicializamos todo a 0
        ZeroStats();
        //Requerido para devolver los skins a -1 cuando no se encuentra ningun skin en la lista de equipados
        bool has_armorSkin = false;
        bool has_weaponSkin = false;
        //Por cada item equipado
        foreach (var equippedItem in Inventory.EquippedList)
        {
            //Buscamos el item en la base de datos
            Item ItemData = PlayerGeneral.ItemDatabase.FetchItemByID(equippedItem.itemID);
            if (ItemData.useAs != Item.UseAs.Summon)
            {
                //MODS
                for (int i = 0; i < equippedItem.itemMods.Count; i++)
                {

                    apply_mod_translations(equippedItem.itemMods[i], ItemData.ItemLevel, false, false);

                }

                //STATS
                for (int i = 0; i < ItemData.ItemStats.Length; i++)
                {
                    //Item upgrade operation
                    if (ItemData.ItemStats[i] > 0)//if this stat is above 0 - we check this because each item has an float[8] of stats and we dont want to upgrade stats that are 0
                    {
                        var final_stat = ItemData.ItemStats[i] + (float)Math.Round((Decimal)(ItemData.ItemStats[i] * PlayerGeneral.x_ObjectHelper.item_upgrade_stat_increase(equippedItem.itemUpgrade) / 100f), 2, MidpointRounding.AwayFromZero);
                        if (ItemData.useAs == Item.UseAs.LeftHand && PlayerStats.passive_off_hand_mastery > 0)
                        {
                            final_stat *= (1f + (PlayerStats.passive_off_hand_mastery / 100f));
                        }
                        PlayerStats.PlayerEquipStats[i] += final_stat;
                    }

                }

                //Enchants
                PlayerStats.enchants_equipped.Add(equippedItem.enchants);

                //SKIN +STAT TRAINING
                switch (ItemData.useAs)
                {
                    case Item.UseAs.SkinBody:
                        PlayerAnimatorC.PlayerArmorSkin = ItemData.ItemID;
                        has_armorSkin = true;
                        break;
                    case Item.UseAs.SkinWeapon:
                        PlayerAnimatorC.PlayerWeaponSkin = ItemData.ItemID;
                        has_weaponSkin = true;
                        break;
                    case Item.UseAs.ExpFarmStone:
                        Inventory.equipped_farm_stones.Add(new ItemFullData(ItemData, equippedItem));
                        break;
                    case Item.UseAs.SkillMain:
                        PlayerSkills.equipped_skills[equippedItem.itemDurability] = PlayerGeneral.x_ObjectHelper.skillDB.FetchSkillByID(ItemData.ItemID);
                        break;
                    case Item.UseAs.SkillSecondary:
                        PlayerSkills.equipped_skills[equippedItem.itemDurability] = PlayerGeneral.x_ObjectHelper.skillDB.FetchSkillByID(ItemData.ItemID);
                        break;
                    default:
                        break;
                }

            }
        }

        //send equipped skills to client
        PlayerSkills.sendEquippedSkillstoClient();
        //Actualizamos todos los stats
        PlayerStats.ProcessStats();
        //apply enchants
        PlayerStats.applyEnchants();
        //Si no tiene skin, se devuelve a -1
        if (!has_armorSkin)
        {
            PlayerAnimatorC.PlayerArmorSkin = -1;
        }
        if (!has_weaponSkin)
        {
            PlayerAnimatorC.PlayerWeaponSkin = -1;
        }
    }
    private void ZeroStats()
    {
        PlayerStats.modSTR = 0;//l
        PlayerStats.modDEX = 0;//l
        PlayerStats.modINT = 0;//l
        PlayerStats.modWIS = 0;//l
        PlayerStats.modSTA = 0;//l
        PlayerStats.modDEF = 0;//l
        PlayerStats.modMDEF = 0;//l
        //Raros
        PlayerStats.modMaxHP = 0;//l
        PlayerStats.modMaxMP = 0;//l
        PlayerStats.modDodge = 0;//l
        PlayerStats.modReflectSTR = 0;//l
        PlayerStats.modAttkSPD = 0;//l
        PlayerStats.modHPRegen = 0;//l
        PlayerStats.modHPonKill = 0;//l
        PlayerStats.modMPonKill = 0;//l
        PlayerStats.modCastingSpeed = 0;//no usada
        PlayerStats.modCritChance = 0;//l
        PlayerStats.modCritDmg = 0;//l
        PlayerStats.modSPD = 0;//l
        PlayerStats.modDropRate = 0;//l
        PlayerStats.modGoldDrop = 0;//l
        PlayerStats.modExpRate = 0;//l
        PlayerStats.modCDReduction = 0;//l

        PlayerStats.PlayerEquipStats = new float[9];

        PlayerStats.enchants_equipped = new List<int>();
        Inventory.equipped_farm_stones = new List<ItemFullData>();

        PlayerSkills.equipped_skills.Clear();
        skill null_skill = null;
        PlayerSkills.equipped_skills.Add(null_skill);
        PlayerSkills.equipped_skills.Add(null_skill);
        PlayerSkills.equipped_skills.Add(null_skill);
        PlayerSkills.equipped_skills.Add(null_skill);
        PlayerSkills.equipped_skills.Add(null_skill);
        PlayerSkills.equipped_skills.Add(null_skill);
    }
    public void apply_mod_translations(int mod, int item_lvl, bool negative, bool double_it)
    {
        //because this is not in use
        negative = false;
        double_it = false;
        //----------

        int mod_result = 1;
        //Normales
        //0 nada
        //1 +mod_result STR    = modSTR
        //2 +mod_result DEX    = modDEX
        //3 +mod_result INT    = modINT
        //4 +mod_result WIS    = modWIS
        //5 +mod_result STA    = modSTA
        //6 +mod_result DEF    = modDEF
        //7 +mod_result MDEF   = modMDEF

        //Raros
        //8     +item_lvl+20 MaxHP              = modMaxHP
        //9     +item_lvl+20 MaxMP              = modMaxMP

        //10    +2% of Dodge           = modDodge
        //11    +2% Reflect            = modReflectSTR
        //12    +3% Attk SPD           = modAttkSPD
        //13    +1% HP Regen           = modHPRegen
        //14    +3% maxHP on kill      = modHPonKill
        //15    +3% maxMP on kill      = modMPonKill
        //16    +1% Crit chance        = modCritChance
        //17    +2% Crit DMG           = modCritDmg
        //18    +2% SPD                = modSPD
        //19    +1% Reflect as INT     = modCastingSpeed

        if (item_lvl < 10)
        {
            mod_result = 1;
        }
        else
        {
            mod_result = Mathf.RoundToInt((item_lvl / 10f) + 1f);
        }

        switch (mod)
        {
            case 0:
                break;
            case 1:
                PlayerStats.modSTR += (mod_result + 2);
                break;
            case 2:
                PlayerStats.modDEX += (mod_result + 3);
                break;
            case 3:
                PlayerStats.modINT += (mod_result + 2);
                break;
            case 4:
                PlayerStats.modWIS += mod_result;
                break;
            case 5:
                PlayerStats.modSTA += mod_result;
                break;
            case 6:
                PlayerStats.modDEF += (mod_result + 4);
                break;
            case 7:
                PlayerStats.modMDEF += (mod_result + 4);
                break;
            case 8:
                PlayerStats.modMaxHP += (item_lvl + 10);
                break;
            case 9:
                PlayerStats.modMaxMP += ((item_lvl / 2) + 5);
                break;
            case 10:
                PlayerStats.modDodge += 2;
                break;
            case 11:
                PlayerStats.modReflectSTR += 2;
                break;
            case 12:
                PlayerStats.modAttkSPD += 3f;
                break;
            case 13:
                PlayerStats.modHPRegen += 1;
                break;
            case 14:
                PlayerStats.modHPonKill += 3;
                break;
            case 15:
                PlayerStats.modMPonKill += 3;
                break;
            case 16:
                PlayerStats.modCritChance += 1;
                break;
            case 17:
                PlayerStats.modCritDmg += 2;
                break;
            case 18:
                PlayerStats.modSPD += 2;
                break;
            case 19:
                PlayerStats.modCastingSpeed += 3;
                break;
            case 20:
                PlayerStats.modDropRate += 4;
                break;
            case 21:
                PlayerStats.modGoldDrop += 4;
                break;
            case 22:
                PlayerStats.modExpRate += 3;
                break;
            case 23:
                PlayerStats.modCDReduction += 3;
                break;
        }
    }
    #endregion

    #region WIP     
    public void delete_from_inv_db_and_log_it(string log, InventoryItem ItemReference)
    {
        //Se actualiza el flag de este item en la BD
        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.v2_inv_actions_delete_item("delete", PlayerAccountInfo.PlayerAccount, ItemReference.itemUniqueID, log)));
    }
    public void Change_durabil(int new_durabil, InventoryItem ItemReference)
    {
        //Se actualiza el flag de este item en la BD
        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.v2_inv_actions_durability("durability", PlayerAccountInfo.PlayerAccount, ItemReference.itemUniqueID, new_durabil)));
    }
    public void Change_durabil(int new_durabil, int ItemUID, List<InventoryItem> FromthisList)
    {
        InventoryItem ItemReference = FetchItemInListBy_UID(ItemUID, FromthisList);
        //Se actualiza el flag de este item en la BD
        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.v2_inv_actions_durability("durability", PlayerAccountInfo.PlayerAccount, ItemReference.itemUniqueID, new_durabil)));
    }
    public void Change_invSize(int new_size)
    {
        //Se actualiza en la BD
        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.IAP_ModifyInvSize(PlayerAccountInfo.PlayerAccount, new_size)));

    }
    public void Change_bankSize(int new_size)
    {
        //Se actualiza en la BD
        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.IAP_ModifyBankSize(PlayerAccountInfo.PlayerAccount, new_size)));

    }
    #endregion

    #region Networking Server   
    [Command]
    public void CmdEquipmentActions(int itemUID, string cmd)
    {
        //search Item by verifying itemUID
        InventoryItem ItemReference = Inventory.EquippedList[getItemIndex_inList(itemUID, Inventory.EquippedList)];
        Item Item_ = PlayerGeneral.ItemDatabase.FetchItemByID(ItemReference.itemID);
        if (Item_ != null)
        {
            if (cmd == "unequip")
            {
                if (inventoryFreeSpaces() > 0)
                {

                    //quita el item de la lista y envia la orden al cliente
                    for (int i = 0; i < Inventory.EquippedList.Count; i++)
                    {
                        if (Inventory.EquippedList[i].itemUniqueID == ItemReference.itemUniqueID)
                        {
                            //Se actualiza el flag de este item en la BD
                            PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB("inv", ItemReference, PlayerAccountInfo.PlayerAccount);
                            //if item is a skill needs to be unequipped
                            if (Item_.useAs == Item.UseAs.SkillMain)
                            {
                                Inventory.EquippedList[i].itemDurability = 0;
                                Change_durabil(0, Inventory.EquippedList[i]);
                                /*PlayerSkills.equipped_skills[0] = null;
                                PlayerSkills.equipped_skills[1] = null;
                                PlayerSkills.equipped_skills[2] = null;*/
                            }
                            if (Item_.useAs == Item.UseAs.SkillSecondary)
                            {
                                Inventory.EquippedList[i].itemDurability = 0;
                                Change_durabil(0, Inventory.EquippedList[i]);
                                /* PlayerSkills.equipped_skills[3] = null;
                                 PlayerSkills.equipped_skills[4] = null;
                                 PlayerSkills.equipped_skills[5] = null;*/
                            }
                            //se remueve de la lista
                            Inventory.EquippedList.Remove(Inventory.EquippedList[i]);
                            //se envia completo
                            SendWholeEquipmentToClient();
                            break;
                        }
                    }

                    //Se agrega el item al inventario en server y cliente
                    AddItemToInv_and_sendToClient(ItemReference);
                    //Se actualizan los STATS
                    apply_equipped_stats();
                }
                else
                {
                    PlayerGeneral.TargetSendToChat(connectionToClient, "Inventory full");
                }

            }
        }
        apply_equipped_stats();
    }
    [Command]
    public void CmdInvActions(int itemUID, string cmd, int meta)
    {
        //search Item by verifying itemUID
        InventoryItem ItemReference = Inventory.InventoryList[getItemIndex_inList(itemUID, Inventory.InventoryList)];
        Item Item_ = PlayerGeneral.ItemDatabase.FetchItemByID(ItemReference.itemID);

        if (Item_ != null)
        {
            //V2
            if (cmd == "equip")
            {
                if (Item_.ItemLevel <= PlayerStats.PlayerLevel)
                {
                    bool valid = true;

                    if (valid)
                    {
                        PlayerStatistics.equip_session++;
                        //check user class and item class
                        if (Array.IndexOf(Item_.requiredClass, PlayerStats.PlayerClass_now) >= 0 || Array.IndexOf(Item_.requiredClass, PlayerStats.PlayerClass.Any) >= 0)
                        {
                            //deal with Equipped and Inventory lists                     
                            bool is_Swap = false;
                            int d = 0;
                            for (int i = 0; i < Inventory.EquippedList.Count; i++)
                            {
                                d = i;
                                var equipped_item = PlayerGeneral.ItemDatabase.FetchItemByID(Inventory.EquippedList[i].itemID);

                                if ((Item_.useAs == Item.UseAs.SkillMain || Item_.useAs == Item.UseAs.SkillSecondary) && (equipped_item.useAs == Item.UseAs.SkillMain || equipped_item.useAs == Item.UseAs.SkillSecondary))
                                {
                                    //we store skill position in the durability field
                                    if (meta >= 0 && meta <= 5)
                                    {
                                        if (Inventory.EquippedList[i].itemDurability == meta)
                                        {
                                            //this is a swap because player already has one item equipped on this position
                                            is_Swap = true;
                                            break;
                                        }
                                    }

                                }
                                else if (equipped_item.useAs == Item_.useAs)
                                {
                                    //this is a swap because player already has one item like this (itemPart) equipped
                                    is_Swap = true;
                                    break;
                                }
                            }
                            if (is_Swap)
                            {
                                var temp_oldItemEquipped = Inventory.EquippedList[d];
                                if (Item_.useAs == Item.UseAs.SkillMain || Item_.useAs == Item.UseAs.SkillSecondary)
                                {
                                    //make sure pos is between 0 and 5
                                    ItemReference.itemDurability = meta;
                                    Change_durabil(meta, ItemReference);
                                    temp_oldItemEquipped.itemDurability = 0;
                                    Change_durabil(0, temp_oldItemEquipped);
                                }
                                //Se agrega el item que estaba equipado al inventario y se envia
                                AddItemToInv_and_sendToClient(temp_oldItemEquipped);
                                //quita el item de la lista y envia la orden al cliente
                                Inventory.InventoryList.Remove(ItemReference);
                                TargetOperateInventory(connectionToClient, itemUID, 1, null);
                                //Se actualiza el flag de este item en la BD                            
                                PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB("inv", temp_oldItemEquipped, PlayerAccountInfo.PlayerAccount);
                                //se remueve el item equipado de la lista
                                Inventory.EquippedList.Remove(Inventory.EquippedList[d]);
                                //agrega item a la lista de equipados                                    
                                AddItemToEquipped(ItemReference);
                                //se envia equipaje completo
                                SendWholeEquipmentToClient();
                                //Se actualiza el flag de este item en la BD
                                PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB("equ", ItemReference, PlayerAccountInfo.PlayerAccount);

                            }
                            //Si no se tiene ningun item equipado
                            if (!is_Swap)
                            {
                                //quita el item de la lista y envia la orden al cliente
                                Inventory.InventoryList.Remove(ItemReference);
                                TargetOperateInventory(connectionToClient, itemUID, 1, null);
                                //agrega item a la lista de equipados
                                if (Item_.useAs == Item.UseAs.SkillMain || Item_.useAs == Item.UseAs.SkillSecondary)
                                {
                                    //we store skill position in the durability field
                                    if (meta >= 0 && meta <= 5)
                                    {
                                        //make sure pos is between 0 and 5
                                        ItemReference.itemDurability = meta;
                                        Change_durabil(meta, ItemReference);
                                    }
                                }
                                //1.agrega item a la lista de equipados
                                AddItemToEquipped(ItemReference);
                                //2.se envia equipaje completo
                                SendWholeEquipmentToClient();
                                PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB("equ", ItemReference, PlayerAccountInfo.PlayerAccount);
                            }

                            //Se actualizan los STATS
                            apply_equipped_stats();
                        }
                    }
                }
            }
            else if (cmd == "drop")
            {
                dropItem(1, ItemReference, "manual_drop");
            }
            else if (cmd == "sell")
            {
                var sell_price = PlayerGeneral.x_ObjectHelper.getItemSellPrice(Item_);
                //borrar en db
                delete_from_inv_db_and_log_it("sold_to_npc", ItemReference);
                //add to buyback
                PlayerGeneral.x_ObjectHelper.buyback_list.Add(new buyBack(PlayerAccountInfo.PlayerAccount, ItemReference, sell_price));

                //quita el item de la lista y envia la orden al cliente
                Inventory.InventoryList.RemoveAll(item => item.itemUniqueID == ItemReference.itemUniqueID);
                TargetOperateInventory(connectionToClient, itemUID, 1, null);
                //LOGS
                StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, "SELL - soldUID: " + itemUID + " had:" + Gold, this.GetComponent<PlayerAccountInfo>().PlayerIP)));
                //track
                PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.items_sold_session, 1);
                //give and save gold
                ChangeGold_NEGATIVE_or_POSITIVE_gold(sell_price, string.Empty,false);               
            }
            //___V2
            else if (cmd == "use")
            {
                if (Item_.ItemLevel <= PlayerStats.PlayerLevel)
                {
                    if (Item_.useAs == Item.UseAs.Teleport)
                    {
                        if (!PlayerGeneral.in_devilSquare)
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                var teleport_allowed = PlayerGeneral.x_ObjectHelper.PlayerTeleport.is_tp_ready(PlayerAccountInfo.PlayerAccount);//--->teleport cool down is boring
                                if (teleport_allowed == 0)
                                {
                                    bool used = false;
                                    if (Item_.ItemID == 29010)//laurumharbor
                                    {
                                        PlayerGeneral.use_teleport_stone(PlayerGeneral.x_ObjectHelper.Teleport_stones[0].transform.position);
                                        used = true;
                                    }
                                    else if (Item_.ItemID == 29011)//libra
                                    {
                                        PlayerGeneral.use_teleport_stone(PlayerGeneral.x_ObjectHelper.Teleport_stones[1].transform.position);
                                        used = true;
                                    }
                                    else if (Item_.ItemID == 29014)//Ghost
                                    {
                                        PlayerGeneral.use_teleport_stone(PlayerGeneral.x_ObjectHelper.Teleport_stones[2].transform.position);
                                        used = true;
                                    }
                                    else if (Item_.ItemID == 29015)//Fire Cave
                                    {
                                        PlayerGeneral.use_teleport_stone(PlayerGeneral.x_ObjectHelper.Teleport_stones[5].transform.position);
                                        used = true;
                                    }
                                    else if (Item_.ItemID == 29016)//Rynthia
                                    {
                                        PlayerGeneral.use_teleport_stone(PlayerGeneral.x_ObjectHelper.Teleport_stones[4].transform.position);
                                        used = true;
                                    }
                                    else if (Item_.ItemID == 29012)//back to death pos teleport
                                    {
                                        if (PlayerDeath.respawn_position_before_death_world != Vector3.zero)
                                        {
                                            PlayerGeneral.use_teleport_stone(PlayerDeath.respawn_position_before_death_world);
                                            PlayerDeath.respawn_position_before_death_world = Vector3.zero;
                                            used = true;
                                        }
                                        else
                                        {
                                            used = false;
                                            PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Failed: You haven't died yet!");
                                        }
                                    }
                                    else if (Item_.ItemID == 29013)// to party leader
                                    {
                                        if (PlayerGeneral.PartyID != string.Empty)//if we are in party
                                        {
                                            var leader = PlayerGeneral.x_ObjectHelper.PartyController.get_party_leader(PlayerGeneral.PartyID);//get leader
                                            if (leader != null && leader.Player != null)//if leader found
                                            {
                                                if (leader.Player != gameObject)//if we are not the leader
                                                {
                                                    if (!leader.Player.GetComponent<PlayerGeneral>().in_devilSquare)
                                                    {
                                                        PlayerGeneral.use_teleport_stone(leader.Player.transform.position);
                                                        used = true;
                                                    }
                                                    else
                                                    {
                                                        used = false;
                                                        PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Failed: Party leader inside Devil Square, try again later");
                                                    }

                                                }
                                                else
                                                {
                                                    used = false;
                                                    PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Failed: You are the party leader.");
                                                }
                                            }
                                            else
                                            {
                                                used = false;
                                                PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Failed: Party Leader not found, try again later.");
                                            }

                                        }
                                        else
                                        {
                                            used = false;
                                            PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Failed: You need to be in a party to use this scroll");
                                        }
                                    }

                                    if (used)
                                    {
                                        //take one durability now
                                        ItemReference.itemDurability--;
                                        //change and save durability of item
                                        Change_durabil(ItemReference.itemDurability, ItemReference);
                                        //if this item still has durability above 0 then keep it, if not destroy it
                                        if (ItemReference.itemDurability <= 0)
                                        {
                                            //delete from db and memmory and client 
                                            remove_item_from_DB_and_player_INVENTORY(ItemReference, "tp_scroll_fully_used");
                                            PlayerGeneral.TargetSendToChat(connectionToClient, "Teleport Scroll fully used");
                                        }
                                        else
                                        {
                                            //change inv on mem and send update client
                                            Mody_durability_on_inventory_and_send(ItemReference);
                                            //notify client
                                            PlayerGeneral.TargetSendToChat(connectionToClient, "Scroll Used");
                                        }                                       
                                        //add to cd
                                        PlayerGeneral.x_ObjectHelper.PlayerTeleport.teleport_cd.Add(new PlayerTeleport.TeleportCD(PlayerAccountInfo.PlayerAccount, Time.time + 120));
                                    }

                                }
                                else
                                {
                                    var msg = "Teleport in cooldown for: " + Mathf.RoundToInt(teleport_allowed) + " seconds";
                                    if (teleport_allowed > 60f)
                                    {
                                        teleport_allowed = Mathf.RoundToInt(teleport_allowed / 60);
                                        msg = "Teleport in cooldown for: " + teleport_allowed + " minutes";
                                    }
                                    PlayerGeneral.TargetSendToChat(connectionToClient, msg);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "tp_scroll_fully_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Teleport Scroll fully used");
                            }
                        }
                        else
                        {
                            PlayerGeneral.TargetSendToChat(connectionToClient, "You can't use teleport scrolls inside the Devil Square");
                        }
                    }
                    else if (Item_.useAs == Item.UseAs.Consumable)
                    {
                        if (Item_.ItemID >= 29040 && Item_.ItemID <= 29044)//exp potions
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerStats.player_exp_change(Item_.misc_data[0], PlayerStats.exp_source.ignore);
                                Change_durabil(ItemReference.itemDurability, ItemReference);

                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_exp_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, "Experience potion fully used");
                                }
                                else
                                {
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_exp_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }
                        }
                        else if (Item_.ItemID == 29050)//LP reset potion
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerStats.resetCustomStats_lvl();
                                //refresh player stats
                                PlayerStats.ProcessStats();
                                Change_durabil(ItemReference.itemDurability, ItemReference);

                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_LP_reset_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, "LP points reset");
                                }
                                else
                                {
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_LP_reset_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }

                        }
                        else if (Item_.ItemID == 29051)//RP reset potion
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerStats.resetCustomStats_reb();
                                //refresh player stats
                                PlayerStats.ProcessStats();
                                Change_durabil(ItemReference.itemDurability, ItemReference);

                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_RP_reset_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, "RP points reset");
                                }
                                else
                                {
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_RP_reset_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }
                        }
                        else if (Item_.ItemID == 29052 || Item_.ItemID == 29055 || Item_.ItemID == 29056)
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                //take one durability now
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerAccountInfo.ModifyKarma(Item_.misc_data[0], false);
                                //change and save durability of potion
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                //if this item still has durability above 0 then keep it, if not destroy it
                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_plus_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("{0} Karma added", Item_.misc_data[0]));
                                }
                                else
                                {
                                    //change inv on mem and send update client
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_plus_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }
                        }
                        else if (Item_.ItemID == 29053)
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                //take one durability now
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerAccountInfo.ModifyKarma(0, true);
                                //change and save durability of potion
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                //if this item still has durability above 0 then keep it, if not destroy it
                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_reset_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("Karma Cleared", Item_.misc_data[0]));
                                }
                                else
                                {
                                    //change inv on mem and send update client
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_reset_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }
                        }
                        else if (Item_.ItemID == 29054)
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                //take one durability now
                                ItemReference.itemDurability--;
                                //add potion benefit
                                PlayerAccountInfo.ModifyKarma(Item_.misc_data[0], false);
                                //change and save durability of potion
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                //if this item still has durability above 0 then keep it, if not destroy it
                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_neg_potion_used");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("{0} Karma added", Item_.misc_data[0]));
                                }
                                else
                                {
                                    //change inv on mem and send update client
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "charges_karma_neg_potion_used");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Potion fully used");
                            }
                        }
                        else if (Item_.ItemID >= 29060 && Item_.ItemID <= 29069)//gold ingots
                        {
                            if (ItemReference.itemDurability > 0)
                            {
                                //take one durability now
                                ItemReference.itemDurability--;
                                //add gold
                                ChangeGold_NEGATIVE_or_POSITIVE_gold(Item_.misc_data[0], string.Format("{0}_gold_ingot_open", Item_.ItemID),false);
                                //change and save durability of potion
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                //if this item still has durability above 0 then keep it, if not destroy it
                                if (ItemReference.itemDurability <= 0)
                                {
                                    //delete from db and memmory and client 
                                    remove_item_from_DB_and_player_INVENTORY(ItemReference, "gold_ingot_open");
                                    PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("+{0} Gold Added", Item_.misc_data[0]));
                                }
                                else
                                {
                                    //change inv on mem and send update client
                                    Mody_durability_on_inventory_and_send(ItemReference);
                                }
                            }
                            else
                            {
                                //delete from db and memmory and client 
                                remove_item_from_DB_and_player_INVENTORY(ItemReference, "gold_ingot_open");
                                PlayerGeneral.TargetSendToChat(connectionToClient, "Ingot fully used");
                            }
                        }
                    }
                    else if (Item_.useAs == Item.UseAs.Summon)
                    {
                        PlayerGeneral.spawnPet(Item_, ItemReference);
                    }
                    else if (Item_.useAs == Item.UseAs.RewardChest)
                    {

                        //search item or material
                        var reward = PlayerGeneral.x_ObjectHelper.rewardDatabase.FetchRewards_by_chestID(Item_.misc_data[0]);
                        List<int> mats_IDs = new List<int>();
                        List<int> mats_amount = new List<int>();
                        List<int> items_IDs = new List<int>();
                        List<int> items_amount = new List<int>();
                        int IAP_amount = 0;
                        //give reward
                        switch (reward.rewards_type)
                        {
                            case rewards.reward_type.mats_only:
                                for (int i = 0; i < reward.mat_Amounts.Count; i++)
                                {
                                    var amount = UnityEngine.Random.Range(reward.mat_Amounts[i].min, reward.mat_Amounts[i].max + 1);
                                    material_add_amount_and_save(reward.mat_Amounts[i].materials_inside, amount, string.Format("UID:{0} opened", ItemReference.itemUniqueID));
                                    mats_IDs.Add((int)reward.mat_Amounts[i].materials_inside);
                                    mats_amount.Add(amount);
                                }
                                break;
                            case rewards.reward_type.items_only:
                                break;
                            case rewards.reward_type.mixed:
                                break;
                            case rewards.reward_type.iap_only:
                                IAP_amount = UnityEngine.Random.Range(reward.IAP_gems_amounts.min, reward.IAP_gems_amounts.max + 1);
                                var had_gems = PlayerAccountInfo.PlayerIAPcurrency;
                                //log the action
                                var log = string.Format("UID:{0} opened - inside:{1} gems", ItemReference.itemUniqueID, IAP_amount);
                                StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, PlayerAccountInfo.PlayerIP));
                                //change variable on memmory
                                PlayerAccountInfo.PlayerIAPcurrency += IAP_amount;
                                //change on database and log it                
                                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.IAPmanager.changeIAPcurrency(had_gems, PlayerAccountInfo.PlayerIAPcurrency, IAP_amount, gameObject));
                                break;
                            default:
                                break;
                        }
                        //delete from db and memmory and client 
                        remove_item_from_DB_and_player_INVENTORY(ItemReference, "chest_opened");
                        //to chat
                        PlayerGeneral.TargetSendToChat(connectionToClient, "Crate Opened!");
                        //send results
                        //lo envio
                        TargetInsideCrate(connectionToClient, mats_IDs.ToArray(), mats_amount.ToArray(), items_IDs.ToArray(), items_amount.ToArray(), IAP_amount);

                    }
                }
                else
                {
                    PlayerGeneral.TargetSendToChat(connectionToClient, "Usage requires Level:" + Item_.ItemLevel);
                }

            }
            else if (cmd == "exch")//skin and pet (premium) exchange
            {
                var had_gems = PlayerAccountInfo.PlayerIAPcurrency;
                int gems_to_pay = 0;
                //incrase gems
                switch (Item_.useAs)
                {
                    case Item.UseAs.SkinBody:
                    case Item.UseAs.SkinWeapon:
                        gems_to_pay = Mathf.RoundToInt(PlayerGeneral.x_ObjectHelper.IAPmanager.skin_price / 2f);
                        break;
                    case Item.UseAs.Summon:
                        gems_to_pay = Mathf.RoundToInt(PlayerGeneral.x_ObjectHelper.IAPmanager.pet_price / 2f);
                        break;
                    default:
                        break;
                }
                //log the action
                var log = string.Format("{0} exchange UID:{1} paid:{2}", Item_.useAs.ToString(), ItemReference.itemUniqueID, gems_to_pay);
                StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, PlayerAccountInfo.PlayerIP));
                //change variable on memmory
                PlayerAccountInfo.PlayerIAPcurrency += gems_to_pay;
                //change on database and log it                
                StartCoroutine(PlayerGeneral.x_ObjectHelper.IAPmanager.changeIAPcurrency(had_gems, PlayerAccountInfo.PlayerIAPcurrency, gems_to_pay, gameObject));
                //delete from db and memmory and client 
                remove_item_from_DB_and_player_INVENTORY(ItemReference, "gem_exchange");
                PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("Exchange completed and {0} gems added to your account", gems_to_pay));
            }
        }

    }
    [Command]
    public void CmdInvMassSell(int[] itemUID)
    {
        if (itemUID.Length > 0)
        {
            int gold_pool = 0;
            List<int> item_pool = new List<int>();
            for (int i = 0; i < itemUID.Length; i++)
            {
                InventoryItem ItemReference = Inventory.InventoryList[getItemIndex_inList(itemUID[i], Inventory.InventoryList)];
                Item Item_ = PlayerGeneral.ItemDatabase.FetchItemByID(ItemReference.itemID);
                if (ItemReference != null && Item_ != null)
                {
                    var sell_price = PlayerGeneral.x_ObjectHelper.getItemSellPrice(Item_);
                    //-add to buyback
                    PlayerGeneral.x_ObjectHelper.buyback_list.Add(new buyBack(PlayerAccountInfo.PlayerAccount, ItemReference, sell_price));
                    //-quita el item de la lista
                    Inventory.InventoryList.RemoveAll(item => item.itemUniqueID == ItemReference.itemUniqueID);
                    //add gold
                    gold_pool += sell_price;
                    //track
                    PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.items_sold_session, 1);
                    //add to list
                    item_pool.Add(ItemReference.itemUniqueID);
                }
            }
            if (item_pool.Count > 0)
            {
                //-borrar en db
                PlayerGeneral.x_ObjectHelper.StartCoroutine(
                    PlayerGeneral.x_ObjectHelper.safeWWWrequest(
                        PlayerGeneral.x_ObjectHelper.ServerDBHandler.v2_inv_mass_delete(PlayerAccountInfo.PlayerAccount, string.Join(",", item_pool.ToArray()))
                        ));
                //-save logs
                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(
                    PlayerGeneral.ServerDBHandler.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("mass_locked_sell:{0}", string.Join(",", item_pool.ToArray())), PlayerAccountInfo.PlayerIP)));               
                //save and update gold on client
                ChangeGold_NEGATIVE_or_POSITIVE_gold(gold_pool, string.Empty,false);
                //notify
                PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("{0} items sold for {1} gold", item_pool.Count, gold_pool));
            }
        }
        else
        {
            //notify
            PlayerGeneral.TargetSendToChat(connectionToClient, string.Format("No items to sell"));
        }
    }


    [Command]
    void CmdPickUpItem(GameObject pickingUpThisItem)
    {
        PlayerStatistics.pickup_requests_session++;

        if (pickingUpThisItem != null)
        {
            if (pickingUpThisItem.activeInHierarchy && Vector2.Distance(pickingUpThisItem.transform.position, transform.position) <= 2)
            {
                //Si es oro
                if (pickingUpThisItem.GetComponent<droppedItemInfo>().itemID == 7700)
                {
                    if (!pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup)
                    {
                        pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup = true;


                        AddGoldAndDestroyObject(pickingUpThisItem, pickingUpThisItem.GetComponent<droppedItemInfo>().gold);

                    }
                }
                else
                {
                    //is material
                    if (pickingUpThisItem.GetComponent<droppedItemInfo>().materialID != -1)
                    {
                        pickUp_material_and_destroy_item(pickingUpThisItem);
                    }
                    else
                    {
                        //suficiente espacio?
                        if (inventoryFreeSpaces() > 0)
                        {
                            //Si no es oro
                            AddCollectedItemToInv(pickingUpThisItem);
                            PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.items_collected, 1);
                        }
                    }


                }

            }

        }

    }
    [Command]
    void CmdDeleteInventoryItem(int itemUID)
    {
        for (int i = 0; i < Inventory.InventoryList.Count; i++)
        {
            if (Inventory.InventoryList[i].itemUniqueID == itemUID)
            {
                Inventory.InventoryList.Remove(Inventory.InventoryList[i]);
                TargetOperateInventory(connectionToClient, itemUID, 1, null);
                break;
            }
        }
    }
    [Command]
    void CmdRequestWholeInventory()
    {
        ////////.Log("Whole inventory requested");
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Inventory.InventoryList);
        //This gives you the byte array.
        TargetSendWholeInventory(connectionToClient, mStream.ToArray());
        //
    }
    public MemoryStream SerializeItem(InventoryItem InventoryItem)
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, InventoryItem);
        return mStream;
    }
    public void SendWholeEquipmentToClient()
    {
        //////////.Log("Sending whole equipment...");
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Inventory.EquippedList);
        TargetSendWholeEquipment(connectionToClient, mStream.ToArray());
    }
    public void SendWholeInventoryToClient()
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Inventory.InventoryList);
        TargetSendWholeInventory(connectionToClient, mStream.ToArray());
    }
    public void AddItemToInv_and_sendToClient(InventoryItem item_to_send)
    {
        AddItemToInv(item_to_send);
        var lastIndex = Inventory.InventoryList.Count - 1;
        //serializo el ultimo item agregado
        MemoryStream mStream = SerializeItem(Inventory.InventoryList[lastIndex]);
        //lo envio
        TargetAddItemToInventory(connectionToClient, mStream.ToArray(), lastIndex);
    }
    [Command]
    public void CmdUseConsumible(int ItemID)
    {
        if (!PlayerConditions.potion_blocked)
        {

            for (int i = 0; i < Inventory.EquippedList.Count; i++)
            {
                if (Inventory.EquippedList[i].itemID == ItemID)
                {
                    var itemData = PlayerGeneral.ItemDatabase.FetchItemByID(ItemID);
                    InventoryItem ItemReference = Inventory.EquippedList[i];
                    switch (itemData.useAs)
                    {
                        case Item.UseAs.HPPotion:
                            PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.hpPotions_used_session, 1);
                            //Verificar nosotros la durabilidar en vez de confiar en el usuario
                            if (Inventory.EquippedList[i].itemDurability > 0 && !ServerConsumibleOneCD && PlayerStats.CurrentHP > 0)
                            {
                                if (!PlayerStats.ench_potion_block)
                                {
                                    //Guardamos la neuva vida en Memoria
                                    var to_gain = Mathf.RoundToInt((itemData.misc_data[0] + (itemData.misc_data[0] * PlayerGeneral.x_ObjectHelper.potions_upgrades_stat_increase(Inventory.EquippedList[i].itemUpgrade) / 100f)) * (1f + (PlayerStats.ench_extra_hp_from_pots / 100f)));
                                    var futureHP = PlayerStats.CurrentHP + to_gain;
                                    if (futureHP >= PlayerStats.MaxHealth)
                                    {
                                        PlayerStats.CurrentHP = PlayerStats.MaxHealth;
                                    }
                                    else
                                    {
                                        PlayerStats.CurrentHP += to_gain;
                                    }
                                    PlayerGeneral.showCBT(gameObject, false, false, to_gain, "heal");
                                }
                                else
                                {
                                    PlayerGeneral.showCBT(gameObject, false, false, 0, "heal");
                                }

                                Inventory.EquippedList[i].itemDurability -= 1;
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                StartCoroutine(CDConsumibleOneServer(itemData));
                                SendWholeEquipmentToClient();
                            }
                            break;

                        case Item.UseAs.MPPotion:
                            //Verificar nosotros la durabilidar en vez de confiar en el usuario
                            PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.mpPotions_used_session, 1);
                            if (Inventory.EquippedList[i].itemDurability > 0 && !ServerConsumibleTwoCD && PlayerStats.CurrentHP > 0)
                            {
                                if (!PlayerStats.ench_potion_block)
                                {
                                    //Guardamos la neuva mana en Memoria
                                    var to_gain = Mathf.RoundToInt((itemData.misc_data[0] + (itemData.misc_data[0] * PlayerGeneral.x_ObjectHelper.potions_upgrades_stat_increase(Inventory.EquippedList[i].itemUpgrade) / 100f)) * (1f + (PlayerStats.ench_extra_mp_from_pots / 100f)));
                                    var futureMP = PlayerStats.CurrentMP + to_gain;
                                    if (futureMP >= PlayerStats.MaxMana)
                                    {
                                        PlayerStats.CurrentMP = PlayerStats.MaxMana;
                                    }
                                    else
                                    {
                                        PlayerStats.CurrentMP += to_gain;
                                    }
                                    PlayerGeneral.showCBT(gameObject, false, false, to_gain, "heal_mana");
                                }
                                else
                                {
                                    PlayerGeneral.showCBT(gameObject, false, false, 0, "heal_mana");
                                }

                                Inventory.EquippedList[i].itemDurability -= 1;
                                Change_durabil(ItemReference.itemDurability, ItemReference);
                                StartCoroutine(CDConsumibleTwoServer(itemData));
                                SendWholeEquipmentToClient();


                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (PlayerGeneral.PartyID != string.Empty)
            {
                PlayerGeneral.x_ObjectHelper.PartyController.refreshMemberStats_UI_client(PlayerGeneral.PartyID, gameObject);
            }
        }
    }
    public void Mody_durability_on_inventory_and_send(InventoryItem ItemModified)
    {
        MemoryStream mStream = SerializeItem(ItemModified);
        TargetOperateInventory(connectionToClient, ItemModified.itemUniqueID, 2, mStream.ToArray());
    }
    public void Mody_item_on_inventory_and_send(InventoryItem ItemModified)
    {
        MemoryStream mStream = SerializeItem(ItemModified);
        TargetOperateInventory(connectionToClient, ItemModified.itemUniqueID, 3, mStream.ToArray());
    }
    public void Remove_item_on_client_inventory(InventoryItem ItemToRemove)
    {
        MemoryStream mStream = SerializeItem(ItemToRemove);
        TargetOperateInventory(connectionToClient, ItemToRemove.itemUniqueID, 1, mStream.ToArray());
    }
    #region Trading
    [Command]
    public void CmdSendTradeRequest_name(string PlayerB)
    {
        var player = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(PlayerB);
        if (player != null)
        {
            SendTradeRequest(player);
        }
        else
        {
            TargetError(connectionToClient, "Player not online");
        }
    }
    private void SendTradeRequest(GameObject PlayerB)
    {
        if (TradingWith == null)
        {
            if (!tradingCD)
            {
                if (PlayerB.GetComponent<PlayerInventory>().TradingWith == null)
                {
                    tradingCD = true;
                    StartCoroutine(tradingonCD());
                    PlayerB.GetComponent<PlayerInventory>().TargetOpenTradeRequest(PlayerB.GetComponent<NetworkIdentity>().connectionToClient, PlayerAccountInfo.PlayerNickname, PlayerGuild.PlayerGuildID);
                }
                else
                {
                    TargetError(connectionToClient, "Remote player is already trading with someone else");
                }
            }
            else
            {
                TargetError(connectionToClient, "Trading in cooldown (30 seconds)");
            }
        }
        else
        {
            TargetError(connectionToClient, "You are already trading");
        }
    }

    IEnumerator tradingonCD()
    {
        yield return new WaitForSeconds(30f);
        tradingCD = false;
    }
    [Command]
    public void CmdTradeAccepted(string PlayerA)
    {
        var PlayerA_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(PlayerA);
        if (PlayerA_GO != null)
        {
            PlayerA_GO.GetComponent<PlayerInventory>().TargetTradeRequest_Accepted(PlayerA_GO.GetComponent<NetworkIdentity>().connectionToClient, PlayerAccountInfo.PlayerNickname);
            PlayerA_GO.GetComponent<PlayerInventory>().TradingWith = PlayerAccountInfo.PlayerNickname;
            TradingWith = PlayerA;
        }
    }
    [Command]
    public void CmdLockTrade(string RemotePlayer)
    {
        var RemotePlayer_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(RemotePlayer);
        var TradingWith_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(TradingWith);
        if (RemotePlayer_GO != null && !trade_locked)
        {
            trade_locked = true;
            //para avisarte si el player contrario lockea
            RemotePlayer_GO.GetComponent<PlayerInventory>().remote_trade_locked = true;
            RemotePlayer_GO.GetComponent<PlayerInventory>().TargetTradeRemotePlayerLocked(RemotePlayer_GO.GetComponent<NetworkIdentity>().connectionToClient, PlayerAccountInfo.PlayerNickname);

            if (isTradeLocked_onBothSides(RemotePlayer_GO))
            {
                if (TradingWith_GO.GetComponent<PlayerInventory>().Gold >= TradingWith_GO.GetComponent<PlayerInventory>().currentGoldOffered && Gold >= currentGoldOffered)
                {
                    if (PlayerGeneral.TradeCenter.completetransaction(gameObject, TradingWith_GO, PlayerAccountInfo.PlayerNickname, PlayerAccountInfo.PlayerAccount, Inventory.TradingList, currentGoldOffered, TradingWith_GO.GetComponent<PlayerAccountInfo>().PlayerNickname, TradingWith_GO.GetComponent<PlayerAccountInfo>().PlayerAccount, TradingWith_GO.GetComponent<Inventory>().TradingList, TradingWith_GO.GetComponent<PlayerInventory>().currentGoldOffered))
                    {

                        TargetTradeSuccessful(connectionToClient);
                        RemotePlayer_GO.GetComponent<PlayerInventory>().TargetTradeSuccessful(RemotePlayer_GO.GetComponent<NetworkIdentity>().connectionToClient);
                        PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.trades_successful, 1);
                        RemotePlayer_GO.GetComponent<PlayerStatistics>().track_statistics(PlayerStatistics.tracked_statistics.trades_successful, 1);
                        /*StartCoroutine(InventorySend_delayed());
                        StartCoroutine(TradingWith_GO.GetComponent<PlayerInventory>().InventorySend_delayed());*/


                        //gold
                        if (TradingWith_GO.GetComponent<PlayerInventory>().currentGoldOffered > 0)
                        {
                            TradingWith_GO.GetComponent<PlayerInventory>().Gold -= TradingWith_GO.GetComponent<PlayerInventory>().currentGoldOffered;
                            Gold += TradingWith_GO.GetComponent<PlayerInventory>().currentGoldOffered;
                        }
                        if (currentGoldOffered > 0)
                        {
                            Gold -= currentGoldOffered;
                            TradingWith_GO.GetComponent<PlayerInventory>().Gold += currentGoldOffered;
                        }


                        TradingWith_GO.GetComponent<PlayerInventory>().TargetSetGoldInPlayer(TradingWith_GO.GetComponent<NetworkIdentity>().connectionToClient, TradingWith_GO.GetComponent<PlayerInventory>().Gold);
                        TargetSetGoldInPlayer(connectionToClient, Gold);
                        //___gold

                        trade_defaults();
                        RemotePlayer_GO.GetComponent<PlayerInventory>().trade_defaults();
                    }

                }

            }
        }
        else
        {
            TargetCancelTrade(connectionToClient, null, "Trade Cancelled: Remote player no longer online");
        }

    }
    [Command]
    public void CmdCancelTrade(string RemotePlayer)
    {
        var RemotePlayer_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(RemotePlayer);

        if (RemotePlayer_GO != null)
        {
            RemotePlayer_GO.GetComponent<PlayerInventory>().TargetCancelTrade(RemotePlayer_GO.GetComponent<NetworkIdentity>().connectionToClient, this.gameObject, "Trade cancelled by remote player");
            RemotePlayer_GO.GetComponent<PlayerInventory>().trade_defaults();
            RemotePlayer_GO.GetComponent<PlayerInventory>().SendWholeInventoryToClient();
        }
        ////////.Log("Trade Cancelled");
        trade_defaults();
        SendWholeInventoryToClient();
        PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.trades_cancelled, 1);
    }
    [Command]
    public void CmdOfferItem(string RemotePlayer, int ItemUID)
    {
        var RemotePlayer_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(RemotePlayer);


        if (RemotePlayer == TradingWith)
        {
            if (!isTradeLocked_onEitherside(RemotePlayer_GO))
            {

                for (int i = 0; i < Inventory.InventoryList.Count; i++)
                {
                    if (Inventory.InventoryList[i].itemUniqueID == ItemUID && Inventory.TradingList.Count < 6 && !Inventory.TradingList.Contains(Inventory.InventoryList[i]))
                    {
                        Inventory.TradingList.Add(Inventory.InventoryList[i]);
                        //serializo el ultimo item agregado
                        MemoryStream mStream = SerializeItem(Inventory.InventoryList[i]);
                        //lo envio
                        RemotePlayer_GO.GetComponent<PlayerInventory>().TargetItemsOfferedByRemotePlayer(RemotePlayer_GO.GetComponent<NetworkIdentity>().connectionToClient, mStream.ToArray());
                        //if item is pet, send pet data via a different Target and store it on client as a temp data for inspection (to show pet_details on the receiving end)
                        TargetOperateInventory(connectionToClient, ItemUID, 1, null);
                    }
                }
            }
        }

    }
    [Command]
    public void CmdOfferGold(string RemotePlayer, int amount)
    {
        var RemotePlayer_GO = PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(RemotePlayer);
        if (RemotePlayer == TradingWith)
        {
            if (!isTradeLocked_onEitherside(RemotePlayer_GO))
            {
                if (amount > 0 && amount <= Gold)
                {
                    currentGoldOffered = amount;
                    RemotePlayer_GO.GetComponent<PlayerInventory>().TargetGoldOfferedByRemotePlayer(RemotePlayer_GO.GetComponent<NetworkIdentity>().connectionToClient, amount);
                }
            }

        }
    }
    #endregion       
    [Command]
    public void CmdOpenBank()
    {
        //serializo el Bank
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Inventory.BankList);
        if (Inventory.BankList.Count > bankSize)
        {
            TargetSendBankSize(connectionToClient, Inventory.BankList.Count);
        }
        else
        {
            TargetSendBankSize(connectionToClient, bankSize);
        }
        //lo envio
        TargetBankList(connectionToClient, mStream.ToArray(), true);
    }
    [Command]
    public void CmdGetBankList()
    {
        //serializo el Bank
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Inventory.BankList);
        //lo envio
        TargetBankList(connectionToClient, mStream.ToArray(), false);
    }
    [Command]
    public void CmdOperBank(int oper, int itemUID)
    {
        if (Inventory.BankList.Count > bankSize)
        {
            TargetSendBankSize(connectionToClient, Inventory.BankList.Count);
        }
        else
        {
            TargetSendBankSize(connectionToClient, bankSize);
        }
        switch (oper)
        {
            case 1://deposit
                //si existe
                if (Inventory.BankList.Count < bankSize)
                {
                    for (int i = 0; i < Inventory.InventoryList.Count; i++)
                    {
                        if (Inventory.InventoryList[i].itemUniqueID == itemUID)
                        {
                            if (!PlayerGeneral.pets_alive_UID.Contains(itemUID))
                            {           //if item being deposited is not a summoned pet                 

                                if (!isItemDuplicatedHere(Inventory.BankList, itemUID) && !isItemDuplicatedHere(Inventory.EquippedList, itemUID))
                                {
                                    var item = Inventory.InventoryList[i];
                                    //quitar item de la lista del inventario
                                    Inventory.InventoryList.Remove(item);
                                    //quitarlo del cliente
                                    TargetOperateInventory(connectionToClient, itemUID, 1, null);
                                    //agregarlo a la lista del banco
                                    Inventory.BankList.Add(item);
                                    //salvarlo en la db
                                    var urlServer = PlayerGeneral.ServerDBHandler.bankOperation(PlayerAccountInfo.PlayerAccount, "deposit", item.itemUniqueID);
                                    StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
                                    break;
                                }
                                else
                                {
                                    var item = Inventory.InventoryList[i];
                                    //quitar item de la lista del inventario
                                    Inventory.InventoryList.Remove(item);
                                    //quitarlo del cliente
                                    TargetOperateInventory(connectionToClient, itemUID, 1, null);
                                }

                            }
                        }
                    }
                }

                break;
            case 2://withdraw
                //si existe en la lista del banco              
                for (int i = 0; i < Inventory.BankList.Count; i++)
                {
                    if (Inventory.BankList[i].itemUniqueID == itemUID)
                    {
                        if (!isItemDuplicatedHere(Inventory.InventoryList, itemUID) && !isItemDuplicatedHere(Inventory.EquippedList, itemUID))
                        {
                            var item = Inventory.BankList[i];
                            //quitar item de la lista del inventario
                            Inventory.BankList.Remove(item);
                            //agregarlo al inventario en el cliente y enviarlo
                            AddItemToInv_and_sendToClient(item);
                            //salvarlo en la db
                            var urlServer = PlayerGeneral.ServerDBHandler.bankOperation(PlayerAccountInfo.PlayerAccount, "withdraw", item.itemUniqueID);
                            StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
                            break;
                        }
                        else
                        {
                            var item = Inventory.BankList[i];
                            //quitar item de la lista del inventario
                            Inventory.BankList.Remove(item);
                        }
                    }
                }
                //quitar item de la lista del banco
                //agregarlo a la lista del inventario
                //enviar item al inventario del cliente
                break;
            default:
                break;
        }
    }
    //Mod Re roll
    [Command]
    public void CmdModReRoll(int itemUID, int itemID)
    {
        rollrequest(itemUID, itemID);
    }
    //board system
    [Command]
    public void CmdRequestItemBoards(int minLvl, int maxLvl, int Class, int Part, int augNumber, int augID, int upgrade, int price, int vendor, int vendor_online, int enchant)
    {
        RequestItemBoards(minLvl, maxLvl, Class, Part, augNumber, augID, upgrade, price, vendor, vendor_online, enchant);
    }
    [Command]
    public void CmdAddItemToBoards(int itemUID, int price, string message, bool premium)
    {
        message = removeTags(message);
        if (itemUID > 0 && price > 0)
        {
            //verificar que el precio,item y mensage es normal, sanitizar message, Resetear despues de agregar item, terminar cliente...


            var expiresOn = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
            if (premium)
            {
                expiresOn += (60 * 60 * 24);
            }
            else
            {
                expiresOn += (60 * 60 * 12);
            }

            addItemToBoards(itemUID, price, message, premium, expiresOn);

        }


    }
    [Command]
    public void CmdRemoveMyItem(int itemUID)
    {
        removeItemListing(itemUID);
    }
    [Command]
    public void CmdRequestTotalListingNum()
    {
        TargetTotalListings(connectionToClient, PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList.Count);
    }
    [Command]
    public void CmdBuyBoardItem(int itemUID)
    {
        var boardItem = PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList.SingleOrDefault(x => x.InventoryItem.itemUniqueID == itemUID);
        if (boardItem != null)
        {
            if (Gold >= boardItem.price)
            {
                if (inventoryFreeSpaces() > 0)
                {
                    //remove it from broker
                    PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList.Remove(boardItem);
                    //save
                    PlayerGeneral.x_ObjectHelper.ServerItemBoards.SaveManager.SaveBoardsToXML("boards.xml", PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList);
                    //take gold
                    ChangeGold_NEGATIVE_or_POSITIVE_gold(-boardItem.price, string.Format("had:{0} now:{1} gold buying from broker", Gold, Gold - boardItem.price),false);
                    //change item owner to the buyer
                    PlayerGeneral.x_ObjectHelper.TradeCenter.change_item_owner(boardItem.vendor_account_ID, boardItem.InventoryItem.itemUniqueID, PlayerAccountInfo.PlayerAccount);
                    //change item flag from broker to inventory              
                    PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB("inv", boardItem.InventoryItem);
                    //add and send item to buyer
                    AddItemToInv_and_sendToClient(boardItem.InventoryItem);
                    //send gold to original owner
                    PlayerGeneral.x_ObjectHelper.change_user_gold(boardItem.price, boardItem.vendor_account_ID, string.Format("sold_for:{0} itemUID:{1}", boardItem.price, boardItem.InventoryItem.itemUniqueID));
                    //notify
                    PlayerGeneral.TargetSendToChat(connectionToClient, "Item bought!");
                    //confirm client so he can delete the game object
                    TargetItemBoughtConfirmation(connectionToClient, boardItem.InventoryItem.itemUniqueID);
                    //update listing amount
                    TargetTotalListings(connectionToClient, PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList.Count);
                }
                else
                {
                    PlayerGeneral.TargetSendToChat(connectionToClient, "Inventory full");
                }
            }
            else
            {
                PlayerGeneral.TargetSendToChat(connectionToClient, "Not enough gold");
            }
        }
        else
        {
            PlayerGeneral.TargetSendToChat(connectionToClient, "Error: Item no longer in broker");
        }
    }
    //expand inv
    [Command]
    public void CmdExpand_gold(bool isInv)
    {
        if (isInv)
        {
            if (invSize < 60)
            {
                int expan_inv_gold = 15500;
                if (Gold >= expan_inv_gold)
                {
                    addInventorySlot();
                    ChangeGold_NEGATIVE_or_POSITIVE_gold(-expan_inv_gold, "Inventory Expanded",false);
                }
            }
            else
            {
                TargetError(connectionToClient, "You can not use gold to expand your inventory above 60 slots");
            }
        }
        else//is bank
        {
            if (bankSize < 80)
            {
                int expan_bank_gold = 7500;
                if (Gold >= expan_bank_gold)
                {
                    addBankSlot();
                    ChangeGold_NEGATIVE_or_POSITIVE_gold(-expan_bank_gold, "Bank Expanded",false);
                }
            }
            else
            {
                TargetError(connectionToClient, "You can not use gold to expand your bank above 80 slots");
            }

        }

    }
    [Command]
    public void CmdEnchant_transfer(int from_uid, int to_uid)
    {
        transfer_enchant(from_uid, to_uid);
    }
    [Command]
    public void CmdItemUpgrade(int mat_used_uid, int to_uid)
    {
        item_upgrade(mat_used_uid, to_uid);
    }
    [Command]
    public void CmdCraftRecipe(int recipeID)
    {
        if (recipeID != 0)
        {
            bool allowed = true;
            var recipe = PlayerGeneral.x_ObjectHelper.craftingRecipesDatabase.FetchCraftingRecipe_by_ID(recipeID);
            List<InventoryItem> Items_to_burn = new List<InventoryItem>();
            //checks
            if (recipe != null)
            {
                //gold
                if (recipe.gold_required > Gold)
                {
                    allowed = false;//not enough gold
                }
                //items required
                foreach (var item_required_ID in recipe.items_required)
                {
                    Items_to_burn = Inventory.InventoryList.FindAll(x => x.itemID == item_required_ID.Key);//search all items required currently in inventory
                    if (Items_to_burn.Count < item_required_ID.Value)//if the amount less than required by recipe
                    {
                        allowed = false;//not enough items required for the recipe
                    }
                }

                //materials required
                foreach (var material_required in recipe.materials_required)
                {
                    if (material_required.Value > 0)
                    {
                        if (Inventory.MaterialsList.Find(x => x.Material_name == material_required.Key).Amount_held < material_required.Value)
                        {
                            allowed = false;//not enough materials
                        }

                    }

                }
                //player level
                if (recipe.player_level_required > PlayerStats.PlayerLevel)
                {
                    allowed = false;//level requirement not met
                }

            }
            else
            {
                allowed = false;//recipe doesnt exist
            }

            //craft the result
            if (allowed)
            {
                var uid = PlayerGeneral.x_ObjectHelper.randomString();
                //take item requirements
                if (recipe.ItemID_crafted_result != -1)//if there is an item as result in this recipe
                {
                    for (int i = 0; i < Items_to_burn.Count; i++)
                    {
                        remove_item_from_DB_and_player_INVENTORY(Items_to_burn[i], "{uid:" + uid + " crafting_recipe_ID->" + recipe.ID + "}");
                    }
                }
                //take material requirements                
                foreach (var material_required in recipe.materials_required)
                {
                    if (material_required.Value > 0)
                    {
                        material_add_amount_and_save(material_required.Key, -material_required.Value, "{uid:" + uid + " crafting_recipe_ID->" + recipe.ID + "}");
                    }
                }
                //take gold requirements
                ChangeGold_NEGATIVE_or_POSITIVE_gold(-recipe.gold_required, "{uid:" + uid + " crafting_recipe_ID->" + recipe.ID + "}",false);
                //---award results
                //items
                if (recipe.ItemID_crafted_result != -1)
                {
                    var item_to_reward = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(recipe.ItemID_crafted_result);
                    if (item_to_reward != null)
                    {
                        var InventoryItem_to_drop = PlayerGeneral.x_ObjectHelper.createNewItem(false, 0f, false, 0f, false, 0f, false, 0f, item_to_reward);

                        if (InventoryItem_to_drop != null)
                        {
                            string mods = string.Join("-", InventoryItem_to_drop.itemMods.Select(x => x.ToString()).ToArray());
                            if (mods == string.Empty)
                            {
                                mods = "0";
                            }
                            string newitemurlServer = PlayerGeneral.x_ObjectHelper.ServerDBHandler.CreateNewItemInDB(InventoryItem_to_drop.itemID, mods, InventoryItem_to_drop.itemUpgrade, InventoryItem_to_drop.enchants, InventoryItem_to_drop.not_in_use_one, InventoryItem_to_drop.not_in_use_two);
                            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.create_item_in_DB_and_add_to_player(item_to_reward, InventoryItem_to_drop, gameObject, "Result:" + item_to_reward.ItemID + "{uid:" + uid + " crafting_recipe_ID->" + recipe.ID + "}", 0));
                            //track quest crafted item
                            gameObject.GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.crafted_item, InventoryItem_to_drop.itemID, gameObject, 1);
                        }
                    }
                }
                else//materials  -- check if there is any material to be crafted first
                {
                    material_add_amount_and_save(recipe.material_crafted_result, 1, "Result:{uid:" + uid + " crafting_recipe_ID->" + recipe.ID + "}");
                    gameObject.GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.crafted_item, (int)recipe.material_crafted_result, gameObject, 1);
                }

                //increase player crafting exp and others here
                //to chat
                PlayerGeneral.TargetSendToChat(connectionToClient, "Item crafted!");
                //let client know it has to reload its materials
                PlayerGeneral.TargetCraftingComplete(connectionToClient, true, recipe.ID);
            }
            else
            {
                PlayerGeneral.TargetSendToChat(connectionToClient, "Not enough materials");
                PlayerGeneral.TargetCraftingComplete(connectionToClient, false, recipe.ID);
            }
        }
    }

    #endregion

    #region Networking Client
    [TargetRpc]
    void TargetSendBankSize(NetworkConnection target, int s_bankSize) { }
    [TargetRpc]
    public void TargetSendInvSize(NetworkConnection target, int invSize, int s_bankSize) { }
    [TargetRpc]
    public void TargetOperateInventory(NetworkConnection target, int itemUID, int operation, byte[] itemToModify) { }
    [TargetRpc]
    void TargetAddItemToInventory(NetworkConnection target, byte[] invReceived, int lastIndex) { }
    [TargetRpc]
    void TargetSendWholeInventory(NetworkConnection target, byte[] invReceived) { }
    [TargetRpc]
    void TargetSendWholeEquipment(NetworkConnection target, byte[] equipReceived) { }
    [TargetRpc]
    public void TargetSetGoldInPlayer(NetworkConnection target, int gold) { }
    [TargetRpc]
    public void TargetOpenTradeRequest(NetworkConnection target, string PlayerA, int PlayerA_guild) { }
    [TargetRpc]
    public void TargetTradeRequest_Accepted(NetworkConnection target, string PlayerB) { }
    [TargetRpc]
    public void TargetTradeRemotePlayerLocked(NetworkConnection target, string Player) { }
    [TargetRpc]
    public void TargetCancelTrade(NetworkConnection target, GameObject Player, string reason) { }
    [TargetRpc]
    void TargetItemsOfferedByRemotePlayer(NetworkConnection target, byte[] invReceived) { }
    [TargetRpc]
    public void TargetGoldOfferedByRemotePlayer(NetworkConnection target, int amount) { }
    [TargetRpc]
    public void TargetTradeSuccessful(NetworkConnection target) { }
    [TargetRpc]
    public void TargetError(NetworkConnection target, string error) { }
    [TargetRpc]
    void TargetBankList(NetworkConnection target, byte[] bankList, bool openUI) { }
    //mod reroll
    [TargetRpc]
    public void TargetModRollAnswer(NetworkConnection target, string mods_mash, int item_lvl, int corruption) { }
    //item boards
    [TargetRpc]
    public void TargetLatestBoards(NetworkConnection target, byte[] latest)
    {

    }
    [TargetRpc]
    public void TargetListingReply(NetworkConnection target, bool isOk)
    {

    }
    [TargetRpc]
    public void TargetItemBoughtConfirmation(NetworkConnection target, int itemUID)
    {

    }
    [TargetRpc]
    public void TargetTotalListings(NetworkConnection target, int totalListings)
    {

    }
    [TargetRpc]
    public void TargetEnchantTransferResult(NetworkConnection target, bool success, int item_uid)
    {

    }
    [TargetRpc]
    public void TargetItemUpgradeResult(NetworkConnection target, bool success, int item_uid)
    {

    }
    //materials
    [TargetRpc]
    public void TargetGetMaterials(NetworkConnection target, byte[] full_list)
    {

    }
    [TargetRpc]
    public void TargetOperateMaterial(NetworkConnection target, int materialID, int amount)
    {

    }
    //opening boxes
    [TargetRpc]
    public void TargetInsideCrate(NetworkConnection target, int[] mat_ID, int[] mat_amounts, int[] items_id, int[] items_amount, int IAP_amount)
    {

    }

    #endregion

    #region DB y Acciones      
    public void remote_pickup(GameObject pickingUpThisItem)
    {
        if (pickingUpThisItem != null)
        {
            if (pickingUpThisItem.activeInHierarchy)
            {
                if (pickingUpThisItem.GetComponent<droppedItemInfo>())
                {

                    //Si es oro
                    if (pickingUpThisItem.GetComponent<droppedItemInfo>().itemID == 7700)
                    {
                        if (!pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup)
                        {
                            pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup = true;


                            AddGoldAndDestroyObject(pickingUpThisItem, pickingUpThisItem.GetComponent<droppedItemInfo>().gold);

                        }
                    }
                    else
                    {
                        //is material
                        if (pickingUpThisItem.GetComponent<droppedItemInfo>().materialID != -1)
                        {
                            pickUp_material_and_destroy_item(pickingUpThisItem);
                        }
                        else
                        {
                            //suficiente espacio?
                            if (inventoryFreeSpaces() > 0)
                            {
                                //Si no es oro
                                AddCollectedItemToInv(pickingUpThisItem);
                                PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.items_collected, 1);
                            }
                        }

                    }
                }

            }

        }
    }

    private void pickUp_material_and_destroy_item(GameObject pickingUpThisItem)
    {
        //the item we gonna pickup
        var thisItem = pickingUpThisItem.GetComponent<droppedItemInfo>();
        bool was_pickedup = false;
        try
        {
            if (thisItem.situation == "dropped")
            {
                //in case the item is no longer loot protected
                thisItem.was_pickedup = true;
                was_pickedup = true;
                material_add_amount_and_save((material.material_translation)pickingUpThisItem.GetComponent<droppedItemInfo>().materialID, pickingUpThisItem.GetComponent<droppedItemInfo>().matAmount, "pick_up");
            }
            else if (thisItem.situation == "loot")
            {
                if (thisItem.owner == PlayerAccountInfo.PlayerNickname)
                {
                    thisItem.was_pickedup = true;
                    was_pickedup = true;
                    material_add_amount_and_save((material.material_translation)pickingUpThisItem.GetComponent<droppedItemInfo>().materialID, pickingUpThisItem.GetComponent<droppedItemInfo>().matAmount, "pick_up");
                }
                else if (thisItem.owner == PlayerGeneral.PartyID && PlayerGeneral.PartyID != string.Empty)
                {
                    thisItem.was_pickedup = true;
                    was_pickedup = true;
                    material_add_amount_and_save((material.material_translation)pickingUpThisItem.GetComponent<droppedItemInfo>().materialID, pickingUpThisItem.GetComponent<droppedItemInfo>().matAmount, "pick_up");
                }
            }

            //party quest sharing
            if (was_pickedup)
            {
                //increment counters on close party members
                if (PlayerGeneral.PartyID != string.Empty)
                {
                    //pull list of all party members
                    var partygroup = PlayerGeneral.x_ObjectHelper.PartyController.getPartyMembers_go(PlayerGeneral.PartyID);
                    for (int i = 0; i < partygroup.Count; i++)
                    {
                        //if its not me and he is close to me
                        if (partygroup[i] != gameObject && Vector2.Distance(partygroup[i].transform.position, gameObject.transform.position) <= 6f)
                        {
                            //grant QUEST mat ONLY to avoid material duplication                            
                            partygroup[i].GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.material_collection, pickingUpThisItem.GetComponent<droppedItemInfo>().materialID, gameObject, pickingUpThisItem.GetComponent<droppedItemInfo>().matAmount);
                        }
                    }
                }

                //finally destroy the item
                Destroy(pickingUpThisItem);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return;
        }

    }

    void AddGoldAndDestroyObject(GameObject pickingUpThisItem, int goldToAdd)
    {

        if (PlayerGeneral.PartyID != string.Empty)
        {
            var partygroup = PlayerGeneral.x_ObjectHelper.PartyController.getPartyMembers_go(PlayerGeneral.PartyID);
            //get players close

            var members_around = new List<GameObject>();
            members_around.Add(gameObject);//add myself to list
            for (int i = 0; i < partygroup.Count; i++)
            {
                //if this this player is not myself and is close to me consider him as member_around
                if (partygroup[i] != gameObject && Vector2.Distance(partygroup[i].transform.position, gameObject.transform.position) <= 5f)
                {
                    members_around.Add(partygroup[i]);
                }
            }
            int distributedGold = goldToAdd / members_around.Count;
            for (int i = 0; i < members_around.Count; i++)
            {
                members_around[i].GetComponent<PlayerInventory>().ChangeGold_NEGATIVE_or_POSITIVE_gold(distributedGold, string.Empty,false);
            }

        }
        else
        {
            ChangeGold_NEGATIVE_or_POSITIVE_gold(goldToAdd, string.Empty,false);
        }
        Destroy(pickingUpThisItem);
    }
    /*public void AddGoldAndSaveToDB(int goldToAdd)
    {
        Gold += goldToAdd;
        if (Gold > 15000000)
        {
            PlayerGeneral.rewadTitleAndUpdateClient(30);//Goldilocks
        }
        PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.gold_session, goldToAdd);
        var urlServer = PlayerGeneral.ServerDBHandler.addGold(PlayerAccountInfo.PlayerAccount, goldToAdd);
        StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
        TargetSetGoldInPlayer(connectionToClient, Gold);
    }*/

    
    public void ChangeGold_NEGATIVE_or_POSITIVE_gold(int goldToModify, string savelog,bool forced)
    {
        //always increase buffer
        gold_to_save_buffer += goldToModify;
        //see if we can save gold now
        save_gold_to_db(goldToModify, savelog, forced);
        //always update on memory even if we dont save it to DB
        Gold += goldToModify;
        if (Gold > 15000000)
        {
            PlayerGeneral.rewadTitleAndUpdateClient(30);//Goldilocks
        }
        if (goldToModify > 0)
        {
            PlayerStatistics.track_statistics(PlayerStatistics.tracked_statistics.gold_session, goldToModify);
        }
        TargetSetGoldInPlayer(connectionToClient, Gold);
    }

    private void save_gold_to_db(int goldToModify, string savelog, bool forced)
    {
        int goldToSaveInDb = gold_to_save_buffer;
        //save if allowed,forced or if there is a log which usually happens when the event is important
        if (save_gold_allowed || savelog != string.Empty || forced)
        {
            save_gold_allowed = false;
            if (savelog != string.Empty && savelog != null)
            {
                string full_log = string.Format("{0} taken:{1} had:{2} buffer:{3}", savelog, goldToModify, Gold, goldToSaveInDb);
                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.ServerDBHandler.saveLog("logsgame", this.GetComponent<PlayerAccountInfo>().PlayerAccount, full_log, this.GetComponent<PlayerAccountInfo>().PlayerIP)));
            }
            var urlServer = PlayerGeneral.ServerDBHandler.addGold(PlayerAccountInfo.PlayerAccount, goldToSaveInDb);
            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
            //all saved (hopefully) so clear the buffer
            gold_to_save_buffer -= goldToSaveInDb;
            
        }
    }

    IEnumerator save_gold_cd()
    {
        yield return new WaitForSeconds(30f);
        save_gold_allowed = true;
        StartCoroutine(save_gold_cd());
    }
    void AddCollectedItemToInv(GameObject pickingUpThisItem)
    {
        var thisItem = pickingUpThisItem.GetComponent<droppedItemInfo>();
        var itemID = thisItem.itemID;
        var uniqueItemID = thisItem.uniqueItemID;
        var itemDurability = thisItem.itemDurability;
        var itemMods = thisItem.mods;
        var itemModsPlus = thisItem.modsplus;
        var quality = thisItem.enchant;
        var corruption = thisItem.corruption;
        var elemental = thisItem.elemental;
        var meta_Data = thisItem.meta_data;


        if (thisItem.situation == "dropped")
        {
            AddItemAndDestroyObject(pickingUpThisItem, itemID, uniqueItemID, itemDurability, itemMods, itemModsPlus, quality, corruption, elemental, meta_Data);
        }
        else if (thisItem.situation == "loot")
        {
            if (thisItem.owner == PlayerAccountInfo.PlayerNickname)
            {
                AddItemAndDestroyObject(pickingUpThisItem, itemID, uniqueItemID, itemDurability, itemMods, itemModsPlus, quality, corruption, elemental, meta_Data);
            }
            else if (thisItem.owner == PlayerGeneral.PartyID && PlayerGeneral.PartyID != string.Empty)
            {
                AddItemAndDestroyObject(pickingUpThisItem, itemID, uniqueItemID, itemDurability, itemMods, itemModsPlus, quality, corruption, elemental, meta_Data);
            }

        }


    }
    void AddItemAndDestroyObject(GameObject pickingUpThisItem, int itemID, int uniqueItemID, int itemDurability, List<int> itemMods, int itemUpgrade, int quality, int corruption, int elemental, string meta_data)
    {
        if (Inventory.InventoryList.Count < invSize)
        {
            if (!pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup)
            {
                pickingUpThisItem.GetComponent<droppedItemInfo>().was_pickedup = true;

                AddItemToInv(itemID, uniqueItemID, itemMods, itemUpgrade, itemDurability, quality, corruption, elemental, meta_data);
                var lastIndex = Inventory.InventoryList.Count - 1;
                //serializo el ultimo item agregado
                MemoryStream mStream = SerializeItem(Inventory.InventoryList[lastIndex]);
                //lo envio
                TargetAddItemToInventory(connectionToClient, mStream.ToArray(), lastIndex);

                var urlServer = PlayerGeneral.ServerDBHandler.addItemToInventory(PlayerAccountInfo.PlayerAccount, itemID, uniqueItemID, transform.position.ToString(), itemDurability.ToString());
                StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(urlServer));
                Destroy(pickingUpThisItem);
            }
        }
        else
        {
            //////////.LogError("Not enough slots");
        }

    }   
    #endregion

    #region Utilidades
    bool isItemDuplicatedHere(List<InventoryItem> thisList, int thisItemUID)
    {
        for (int d = 0; d < thisList.Count; d++)
        {
            if (thisList[d].itemUniqueID == thisItemUID)
            {
                return true;
            }
        }
        return false;
    }
    int getItemIndex_inList(int ItemUID, List<InventoryItem> thisList)
    {
        for (int i = 0; i < thisList.Count; i++)
        {
            if (thisList[i].itemUniqueID == ItemUID)
            {
                return i;
            }
        }
        return -1;
    }
    public int getItemIndex_inList_byID(int ItemID, List<InventoryItem> thisList)
    {
        for (int i = 0; i < thisList.Count; i++)
        {
            if (thisList[i].itemID == ItemID)
            {
                return i;
            }
        }
        return -1;
    }
    private void AddItemToInv(int itemID, int itemUID, List<int> itemMods, int itemUpgrade, int durability, int quality, int corruption, int elemental, string meta_data)
    {
        Inventory.InventoryList.Add(new InventoryItem(itemID, itemUID, itemMods, itemUpgrade, durability, quality, corruption, elemental, meta_data));
    }
    private void AddItemToInv(InventoryItem InventoryItem)
    {
        Inventory.InventoryList.Add(InventoryItem);
    }
    private void AddItemToEquipped(int itemID, int itemUID, List<int> itemMods, int itemUpgrade, int durability, int quality, int corruption, int elemental, string meta_data)
    {
        Inventory.EquippedList.Add(new InventoryItem(itemID, itemUID, itemMods, itemUpgrade, durability, quality, corruption, elemental, meta_data));
    }
    private void AddItemToEquipped(InventoryItem InventoryItem)
    {
        Inventory.EquippedList.Add(InventoryItem);
    }
    private void AddItemToBank(int itemID, int itemUID, List<int> itemMods, int itemUpgrade, int durability, int quality, int corruption, int elemental, string meta_data)
    {
        if (!isItemDuplicatedHere(Inventory.BankList, itemUID))
        {
            Inventory.BankList.Add(new InventoryItem(itemID, itemUID, itemMods, itemUpgrade, durability, quality, corruption, elemental, meta_data));
        }

    }
    public int inventoryFreeSpaces()
    {
        Inventory.InventoryList.RemoveAll(item => item == null);
        int freeSpaces = invSize - Inventory.InventoryList.Count;
        return freeSpaces;
    }
    public InventoryItem parseItem(string mash)
    {
        string[] itemData = mash.Split(new string[] { "+" }, System.StringSplitOptions.RemoveEmptyEntries);
        int itemID = int.Parse(itemData[0]);
        int itemUID = int.Parse(itemData[1]);
        int itemDurability = int.Parse(itemData[2]);
        List<int> itemMods = new List<int>();
        if (itemData[3] != "0")
        {
            string[] modsData = itemData[3].Split(new string[] { "-" }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < modsData.Length; i++)
            {
                itemMods.Add(int.Parse(modsData[i]));
            }

        }
        int itemUpgrades = int.Parse(itemData[4]);
        var quality = int.Parse(itemData[5]);
        var corruption = int.Parse(itemData[6]);
        var elemental = int.Parse(itemData[7]);
        string meta_data = itemData[8];

        return new InventoryItem(itemID, itemUID, itemMods, itemUpgrades, itemDurability, quality, corruption, elemental, meta_data);
    }
    public void UnequipEverything()
    {
        for (int i = 0; i < Inventory.EquippedList.Count; i++)
        {
            AddItemToInv_and_sendToClient(Inventory.EquippedList[i]);
        }
        Inventory.EquippedList = new List<InventoryItem>();
        SendWholeEquipmentToClient();
        apply_equipped_stats();
    }
    //CD del boton de consumible uno
    IEnumerator CDConsumibleOneServer(Item item)
    {
        ServerConsumibleOneCD = true;
        yield return new WaitForSeconds(item.UseCoolDown - 1f);
        ServerConsumibleOneCD = false;
    }
    //CD del boton de consumible uno
    IEnumerator CDConsumibleTwoServer(Item item)
    {
        ServerConsumibleTwoCD = true;
        yield return new WaitForSeconds(item.UseCoolDown - 1f);
        ServerConsumibleTwoCD = false;
    }
    //Reiniciar todo despues de un trade
    public void trade_defaults()
    {
        trade_locked = false;
        remote_trade_locked = false;
        TradingWith = null;
        Inventory.TradingList = new List<InventoryItem>();
        currentGoldOffered = 0;
    }
    bool isTradeLocked_onEitherside(GameObject Player)
    {
        if (Player.GetComponent<PlayerInventory>().remote_trade_locked || Player.GetComponent<PlayerInventory>().trade_locked || remote_trade_locked || trade_locked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool isTradeLocked_onBothSides(GameObject Player)
    {
        if (Player.GetComponent<PlayerInventory>().remote_trade_locked && Player.GetComponent<PlayerInventory>().trade_locked && remote_trade_locked && trade_locked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /* public IEnumerator InventorySend_delayed()
     {
         yield return new WaitForSeconds(0.5f);
         StartCoroutine(getPlayerItems(false));
     }*/
    public void cancelProcess()
    {
        TargetCancelTrade(connectionToClient, null, "Trade cancelled by remote player");
        //StartCoroutine(getPlayerItems(false));
        SendWholeInventoryToClient();
        trade_defaults();
    }

    public InventoryItem FetchItemInListBy_UID(int itemUID, List<InventoryItem> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].itemUniqueID == itemUID)
            {
                return List[i];
            }
        }
        return null;
    }
    public InventoryItem FetchEquippedItemByType(Item.UseAs useAs)
    {
        for (int i = 0; i < Inventory.EquippedList.Count; i++)
            if (PlayerGeneral.ItemDatabase.FetchItemByID(Inventory.EquippedList[i].itemID).useAs == useAs)
                return Inventory.EquippedList[i];
        return null;
    }
    void remove_item_from_DB_and_player_EQUIPMENT(InventoryItem ItemReference, string for_the_logs)
    {
        //borrar en db
        delete_from_inv_db_and_log_it(for_the_logs, ItemReference);
        //quita el item de la lista y envia la orden al cliente
        //Inventory.EquippedList.Remove(ItemReference);--->doesnt work against duped items
        delete_item_from_everywhere(ItemReference);
        SendWholeEquipmentToClient();
    }
    void remove_item_from_DB_and_player_INVENTORY(InventoryItem ItemReference, string for_the_logs)//inventory list default
    {
        //borrar en db
        delete_from_inv_db_and_log_it(for_the_logs, ItemReference);
        //quita el item de la lista y envia la orden al cliente
        //Inventory.InventoryList.Remove(ItemReference);--->doesnt work against duped items
        delete_item_from_everywhere(ItemReference);
        //remove item from client
        TargetOperateInventory(connectionToClient, ItemReference.itemUniqueID, 1, null);
    }
    void delete_item_from_everywhere(InventoryItem ItemReference)
    {
        bool inv_dupe = false;
        bool bank_dupe = false;
        bool equ_dupe = false;
        if (Inventory.InventoryList.RemoveAll(x => x.itemUniqueID == ItemReference.itemUniqueID) > 1)
        {
            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("inv_duped: {0}", ItemReference.itemUniqueID), PlayerAccountInfo.PlayerIP));
            inv_dupe = true;
        }
        if (Inventory.BankList.RemoveAll(x => x.itemUniqueID == ItemReference.itemUniqueID) > 1)
        {
            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("bank_duped: {0}", ItemReference.itemUniqueID), PlayerAccountInfo.PlayerIP));
            bank_dupe = true;
        }
        if (Inventory.EquippedList.RemoveAll(x => x.itemUniqueID == ItemReference.itemUniqueID) > 1)
        {
            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("equ_duped: {0}", ItemReference.itemUniqueID), PlayerAccountInfo.PlayerIP));
            equ_dupe = true;
        }
        //wtf is this? why is it even here? who cares if the item was "dual" dupped or not??
        if (bank_dupe && inv_dupe)
        {
            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.ServerNetworkManager.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("dual_duped: {0}", ItemReference.itemUniqueID), PlayerAccountInfo.PlayerIP));
        }
        if (inv_dupe || bank_dupe || equ_dupe)
        {
            PlayerGeneral.PlayerMPSync.TargetLogStatus(connectionToClient, "ERROR#0W033", 0.5f);
            GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        }
    }
    public void dropItem(int list_selector, InventoryItem item_to_drop, string log)// for items only
    {
        //list_selector = 1 = inventory
        //list_selector = 2 = equipped
        //object
        GameObject droppedItemInstance = Instantiate(droppedItemPrefab, transform.position, new Quaternion(0, 0, 0, 0));
        droppedItemInfo droppedItemInfo = droppedItemInstance.GetComponent<droppedItemInfo>();
        //basic info       
        droppedItemInfo.situation = "dropped";
        droppedItemInfo.owner = PlayerAccountInfo.PlayerNickname;
        droppedItemInfo.itemDurability = item_to_drop.itemDurability;
        droppedItemInfo.destroy_in = 300f;
        //item info
        droppedItemInfo.itemID = item_to_drop.itemID;
        droppedItemInfo.uniqueItemID = item_to_drop.itemUniqueID;
        droppedItemInfo.enchant = item_to_drop.enchants;
        droppedItemInfo.mods = item_to_drop.itemMods;
        droppedItemInfo.rarity = PlayerGeneral.x_ObjectHelper.getItemRarityAndColorTitle(item_to_drop.itemUpgrade, item_to_drop.itemMods.Count, item_to_drop.itemID);
        droppedItemInfo.modsplus = item_to_drop.itemUpgrade;
        droppedItemInfo.meta_data = item_to_drop.meta_data;
        //take it from player
        if (list_selector == 1)
        {
            remove_item_from_DB_and_player_INVENTORY(item_to_drop, log);
        }
        else if (list_selector == 2)
        {
            remove_item_from_DB_and_player_EQUIPMENT(item_to_drop, log);
            //update stats
            PlayerStats.RefreshStats();
        }

        //net spawn
        droppedItemInstance.transform.position = this.transform.position;
        NetworkServer.Spawn(droppedItemInstance);
    }
    public void dropGold(int amount, string log, string situation)//Gold
    {
        if (amount > 0)
        {
            //instantiate object
            GameObject droppedItemInstance = Instantiate(droppedItemPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            droppedItemInfo droppedItemInfo = droppedItemInstance.GetComponent<droppedItemInfo>();
            //basic info        
            droppedItemInfo.situation = situation;
            droppedItemInfo.destroy_in = 300f;
            //gold info
            droppedItemInfo.itemID = 7700;
            droppedItemInfo.gold = amount;
            //take it from player
            ChangeGold_NEGATIVE_or_POSITIVE_gold(-amount, log,false);
            //spawn
            droppedItemInstance.transform.position = this.transform.position;
            NetworkServer.Spawn(droppedItemInstance);
        }
    }
    #endregion

    #region ModReroll
    public void rollrequest(int itemUID, int itemID)
    {
        //string reason = "";
        var itemFound = FetchItemInListBy_UID(itemUID, Inventory.InventoryList);
        if (itemFound != null)//UID confirmado
        {
            if (itemFound.itemID == itemID)//item ID confirmado
            {
                if (itemFound.itemMods.Count > 0)//si tiene mods
                {
                    var itemData = PlayerGeneral.ItemDatabase.FetchItemByID(itemID);
                    int buy_price = PlayerGeneral.x_ObjectHelper.getItemBuyPrice(itemData);
                    if (Gold >= buy_price)//oro confirmado
                    {
                        List<int> new_mods = new List<int>();//new mods
                        for (int i = 0; i < itemFound.itemMods.Count; i++)
                        {
                            new_mods.Add(UnityEngine.Random.Range(1, 19 + 1));
                        }

                        for (int i = 0; i < Inventory.InventoryList.Count; i++)//change item mods
                        {
                            if (Inventory.InventoryList[i].itemUniqueID == itemUID)
                            {
                                //quito el oro y guardo log en db
                                var log = "item:" + Inventory.InventoryList[i].itemUniqueID + " old_mods:" + string.Join("-", new List<int>(Inventory.InventoryList[i].itemMods).ConvertAll(r => r.ToString()).ToArray()) + " new_mods:" + string.Join("-", new List<int>(new_mods).ConvertAll(r => r.ToString()).ToArray()) + " plus:" + Inventory.InventoryList[i].itemUpgrade;
                                ChangeGold_NEGATIVE_or_POSITIVE_gold(-buy_price, log,false);
                                for (int d = 0; d < new_mods.Count; d++)
                                {
                                    Inventory.InventoryList[i].itemMods[d] = new_mods[d];
                                }
                                //lo envio
                                Mody_item_on_inventory_and_send(Inventory.InventoryList[i]);

                                //guardar modificaciones del item in DB
                                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.modifyItem(Inventory.InventoryList[i].itemUniqueID, string.Join("-", new List<int>(new_mods).ConvertAll(r => r.ToString()).ToArray()), Inventory.InventoryList[i].itemUpgrade, Inventory.InventoryList[i].enchants)));
                                //envio resultados para el UI
                                TargetModRollAnswer(connectionToClient, string.Join(",", new List<int>(new_mods).ConvertAll(r => r.ToString()).ToArray()), itemData.ItemLevel, Inventory.InventoryList[i].not_in_use_one);
                                break;
                            }
                            if (i == (Inventory.InventoryList.Count - 1))
                            {
                                //reason = "itemUID not found";
                            }
                        }

                    }
                    else
                    {
                        //reason = "Gold not found";
                    }
                }
                else
                {
                    //reason = "ID not found";
                }
            }
            else
            {
                // reason = "ID not found";
            }
        }
        else
        {
            // reason = "UI not found";
        }

    }
    #endregion

    #region Board System
    void RequestItemBoards(int minLvl, int maxLvl, int Class, int Part, int augNumber, int augID, int rarity, int price, int vendor, int vendor_online, int enchant)
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        var boardlist = PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList;
        List<BoardItem> sendList = new List<BoardItem>();
        List<BoardItem> expiredList = new List<BoardItem>();
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;

        for (int i = 0; i < boardlist.Count; i++)
        {
            if (sendList.Count <= 20)
            {
                if (boardlist[i].expiresOn > unixTimestamp)
                {
                    if (MatchesVendorRequested(vendor, boardlist, i))
                    {
                        var itemdata = PlayerGeneral.ItemDatabase.FetchItemByID(boardlist[i].InventoryItem.itemID);
                        if (itemdata.ItemLevel >= minLvl && (itemdata.ItemLevel <= maxLvl || maxLvl == 9999))//min max level
                        {
                            if (MatchesClassSelected(Class, itemdata))//class
                            {
                                if (MatchesPartSelected(Part, itemdata))//part
                                {
                                    if (boardlist[i].InventoryItem.itemMods.Count >= augNumber)//min mods number
                                    {
                                        if (MatchesAugSelected(augNumber, augID, boardlist, i))//min specific mod
                                        {
                                            if (MatchesRarityRequested(rarity, boardlist, i))//exact upgrade
                                            {
                                                if (MatchesEnchant(enchant, boardlist, i))//enchant
                                                {

                                                    if (MatchesMaxPrice(price, boardlist, i))//max price
                                                    {
                                                        //is vendor online
                                                        if (PlayerGeneral.x_ObjectHelper.PlayersConnected.getPlayerObject(boardlist[i].vendor) == null)
                                                        {
                                                            boardlist[i].vendor_online = false;
                                                        }
                                                        else
                                                        {
                                                            boardlist[i].vendor_online = true;
                                                        }

                                                        if (MatchesOnlineSelected(vendor_online, boardlist, i))
                                                        {
                                                            sendList.Add(boardlist[i]);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //Debug.LogError("Not Match");
                                                }

                                            }
                                            else
                                            {
                                                ////.LogError("Not Match");
                                            }
                                        }
                                        else
                                        {
                                            ////.LogError("Not Match");
                                        }
                                    }
                                    else
                                    {
                                        ////.LogError("Not Match");
                                    }
                                }
                                else
                                {
                                    ////.LogError("Not Match");
                                }
                            }
                            else
                            {
                                ////.LogError("Not Match");
                            }
                        }
                        else
                        {
                            ////.LogError("Not Match " + maxLvl + " " + minLvl + " " + itemdata.ItemLevel);
                        }
                    }
                    else
                    {
                        ////.LogError("Not Match " + boardlist[i].vendor + " " + vendor);
                    }
                }
                else
                {
                    ////.LogError("Board item expired");
                    expiredList.Add(boardlist[i]);

                }
            }

        }
        if (expiredList.Count > 0)
        {
            //remove it from list
            PlayerGeneral.x_ObjectHelper.ServerItemBoards.RemoveExpired(expiredList);

        }
        //order list by gold
        sendList = sendList.OrderBy(x => x.price).ToList();
        binFormatter.Serialize(mStream, sendList);
        TargetLatestBoards(connectionToClient, mStream.ToArray());
    }

    private bool MatchesEnchant(int enchant, List<BoardItem> boardlist, int i)
    {
        bool valid = false;
        if (enchant != -2)
        {

            if (enchant == -1)//any corrupted
            {
                if (boardlist[i].InventoryItem.enchants < 0)
                {
                    valid = true;
                }

            }
            else
            {
                var selected_enchant = (enchant.enchant_base)enchant;
                //Debug.LogError(boardlist[i].InventoryItem.enchants);
                var this_enchant = PlayerGeneral.x_ObjectHelper.enchantsDB.FetchEnchantBase(boardlist[i].InventoryItem.enchants);
                if (this_enchant != null)
                {
                    if (selected_enchant == this_enchant.ench_base)//specific enchant
                    {
                        valid = true;
                    }
                }

            }
        }
        else//any enchant
        {
            valid = true;
        }
        return valid;
    }

    private bool MatchesOnlineSelected(int vendor_online, List<BoardItem> boardlist, int i)
    {
        bool valid = false;
        if (vendor_online == 0)//any
        {
            valid = true;
        }
        else if (vendor_online == 1 && !boardlist[i].vendor_online)//ofline only
        {
            valid = true;
        }
        else if (vendor_online == 2 && boardlist[i].vendor_online)//online
        {
            valid = true;
        }
        return valid;
    }
    private bool MatchesVendorRequested(int vendor, List<BoardItem> boardlist, int i)
    {
        bool valid = false;
        if (vendor == 1)
        {
            valid = true;
        }
        else if (vendor == 0)
        {
            if (boardlist[i].vendor != PlayerAccountInfo.PlayerNickname)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
        }
        else if (vendor == 2)
        {
            if (boardlist[i].vendor == PlayerAccountInfo.PlayerNickname)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
        }
        return valid;
    }
    private bool MatchesMaxPrice(int price, List<BoardItem> boardlist, int i)
    {
        bool valid = false;
        if (price > 0)
        {
            if (boardlist[i].price <= price)//max price
            {
                valid = true;
            }
        }
        else
        {
            valid = true;
        }
        return valid;
    }
    private bool MatchesRarityRequested(int rarity_requested, List<BoardItem> boardlist, int i)
    {
        bool valid = false;

        if (rarity_requested > -1)
        {
            if (PlayerGeneral.x_ObjectHelper.getItemRarity(boardlist[i].InventoryItem.itemUpgrade, boardlist[i].InventoryItem.itemMods.Count, boardlist[i].InventoryItem.itemID) == rarity_requested)
            {
                valid = true;
            }
        }
        else
        {
            valid = true;
        }
        return valid;
    }
    private bool MatchesAugSelected(int augNumber, int augID, List<BoardItem> boardlist, int i)
    {
        if (augID != 0)
        {
            int matches = 0;
            bool valid = false;
            for (int r = 0; r < boardlist[i].InventoryItem.itemMods.Count; r++)
            {
                if (boardlist[i].InventoryItem.itemMods[r] == augID)
                {
                    matches++;
                }
            }
            if (matches >= augNumber)
            {
                valid = true;
            }
            return valid;
        }
        else
        {
            return true;
        }
    }
    private bool MatchesPartSelected(int Part, Item itemdata)
    {
        bool valid = false;
        switch (itemdata.useAs)
        {
            case Item.UseAs.RightHand:
                if (Part == 1 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.LeftHand:
                if (Part == 2 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Helmet:
                if (Part == 3 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Belt:
                if (Part == 4 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Chest:
                if (Part == 5 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Gloves:
                if (Part == 6 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Pants:
                if (Part == 7 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Boots:
                if (Part == 8 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Neck:
                if (Part == 9 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Ring:
                if (Part == 10 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.HPPotion:
                if (Part == 11 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.MPPotion:
                if (Part == 12 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.Consumable:
                if (Part == 13 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.ExpFarmStone:
                if (Part == 14 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.UpgradeStone:
                if (Part == 15 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.SkillMain:
                if (Part == 16 || Part == 0)
                    valid = true;
                break;
            case Item.UseAs.SkillSecondary:
                if (Part == 17 || Part == 0)
                    valid = true;
                break;
            default:
                if (Part == 0 || Part == 18)
                    valid = true;
                break;
        }
        return valid;
    }
    private bool MatchesClassSelected(int Class, Item itemdata)
    {
        bool valid = false;
        if (Class == 0)
        {
            valid = true;
        }
        else if (Array.IndexOf(itemdata.requiredClass, PlayerStats.PlayerClass.Warrior) >= 0 && Class == 1)
        {
            valid = true;
        }
        else if (Array.IndexOf(itemdata.requiredClass, PlayerStats.PlayerClass.Paladin) >= 0 && Class == 2)
        {
            valid = true;
        }
        else if (Array.IndexOf(itemdata.requiredClass, PlayerStats.PlayerClass.Hunter) >= 0 && Class == 3)
        {
            valid = true;
        }
        else if (Array.IndexOf(itemdata.requiredClass, PlayerStats.PlayerClass.Wizard) >= 0 && Class == 4)
        {
            valid = true;
        }
        return valid;
    }
    void addItemToBoards(int itemUID, int price, string message, bool premium, int expiresOn)
    {
        var item_to_add = FetchItemInListBy_UID(itemUID, Inventory.InventoryList);
        var item = PlayerGeneral.ItemDatabase.FetchItemByID(item_to_add.itemID);
        int fee = calculateBoardListingFee(premium, price);
        if (Gold >= fee && fee > 0)
        {
            if (item_to_add != null)
            {
                bool capped = false;
                //hold > 300 for unlimited listings or limit to 5 by hwid
                if (PlayerAccountInfo.PlayerIAPcurrency < 300)
                {
                    if (PlayerGeneral.x_ObjectHelper.ServerItemBoards.ItemBoardsList.FindAll(x => x.vendor_hwid == PlayerAccountInfo.PlayerHWID).Count >= 5)
                    {
                        capped = true;
                    }
                }
                if (!capped)
                {
                    var reply = PlayerGeneral.x_ObjectHelper.ServerItemBoards.boards_precheck(item_to_add, PlayerAccountInfo.PlayerNickname, price, message, premium, expiresOn, PlayerAccountInfo.PlayerAccount, PlayerAccountInfo.PlayerHWID);
                    if (reply == "ok")
                    {
                        TargetListingReply(connectionToClient, true);
                        PlayerGeneral.TargetSendToChat(connectionToClient, "Item listed");
                        ChangeGold_NEGATIVE_or_POSITIVE_gold(-fee, "item_listed:" + itemUID + " for:" + price + " had:" + Gold + " premium:" + premium.ToString(),false);
                        //---------offline stuff
                        //change flag
                        PlayerGeneral.x_ObjectHelper.ChangeItemFlagOn_DB(string.Format("bro-{0}", PlayerGeneral.x_ObjectHelper.IRCchat.nickName), item_to_add, PlayerAccountInfo.PlayerAccount);
                        //quita el item de la lista y envia la orden al cliente
                        Inventory.InventoryList.RemoveAll(x => x.itemUniqueID == item_to_add.itemUniqueID);
                        TargetOperateInventory(connectionToClient, itemUID, 1, null);
                    }
                    else
                    {
                        TargetListingReply(connectionToClient, false);
                        PlayerGeneral.TargetSendToChat(connectionToClient, reply);
                    }
                }
                else
                {
                    PlayerGeneral.TargetSendToChat(connectionToClient, "You already have 5 or more items listed. To list more items you must hold 300 or more gems");
                }
            }
        }
    }
    string removeTags(string sendString)
    {
        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]");
        sendString = rgx.Replace(sendString, "");
        return sendString;
    }
    void removeItemListing(int ItemUID)
    {
        if (PlayerGeneral.x_ObjectHelper.ServerItemBoards.RemoveListing(ItemUID, PlayerAccountInfo.PlayerNickname))
        {
            PlayerGeneral.TargetSendToChat(connectionToClient, "Item Removed");
        }
        else
        {
            PlayerGeneral.TargetSendToChat(connectionToClient, "Error removing item: Not found");
        }
    }
    int calculateBoardListingFee(bool premiumListing, int sell_price)
    {

        if (premiumListing)
        {
            return Mathf.RoundToInt(sell_price * 0.1f);
        }
        else
        {
            return Mathf.RoundToInt(sell_price * 0.03f);
        }
    }
    #endregion

    #region Inventory Expand
    public void addInventorySlot()
    {
        invSize++;
        TargetSendInvSize(connectionToClient, invSize, bankSize);
        Change_invSize(invSize);
    }
    public void addBankSlot()
    {
        bankSize++;
        TargetSendInvSize(connectionToClient, invSize, bankSize);
        Change_bankSize(bankSize);
    }
    #endregion

    #region Enchantment Transfer
    public void transfer_enchant(int from_uid, int to_uid)
    {
        InventoryItem from_item = FetchItemInListBy_UID(from_uid, Inventory.InventoryList);
        if (from_item != null && from_item.enchants != 0)
        {
            Item from_item_base = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(from_item.itemID);
            InventoryItem to_item = FetchItemInListBy_UID(to_uid, Inventory.InventoryList);
            if (to_item != null)
            {
                Item to_item_base = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(to_item.itemID);
                //rules
                if (from_item_base.ItemLevel >= to_item_base.ItemLevel)
                {
                    //chances
                    var chance_succeed = 90f;
                    if (from_item_base.useAs != to_item_base.useAs)
                    {
                        chance_succeed = 36f;
                    }

                    if (UnityEngine.Random.Range(0f, 100f) <= chance_succeed)
                    {
                        //change it      
                        to_item.enchants = from_item.enchants;
                        //send it to client               
                        Mody_item_on_inventory_and_send(to_item);
                        //remove item
                        Remove_item_on_client_inventory(from_item);
                        //save the new item changes
                        PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.modifyItem(to_item.itemUniqueID, string.Join("-", new List<int>(to_item.itemMods).ConvertAll(r => r.ToString()).ToArray()), to_item.itemUpgrade, to_item.enchants)));
                        //
                        TargetEnchantTransferResult(connectionToClient, true, to_item.itemUniqueID);
                    }
                    else
                    {
                        TargetEnchantTransferResult(connectionToClient, false, 0);
                    }
                    //remove from_item from inventory
                    remove_item_from_DB_and_player_INVENTORY(from_item, "Enchant_transfer");
                }


            }
        }
    }
    #endregion

    #region Item upgrade
    void item_upgrade(int mat_used_uid, int to_uid)
    {
        InventoryItem mat_used_item = FetchItemInListBy_UID(mat_used_uid, Inventory.InventoryList);
        if (mat_used_item != null)
        {
            Item item_material = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(mat_used_item.itemID);
            if (item_material.useAs == Item.UseAs.UpgradeStone)
            {
                InventoryItem InventoryItem_to_change = FetchItemInListBy_UID(to_uid, Inventory.InventoryList);
                if (InventoryItem_to_change != null)
                {
                    Item to_item_base = PlayerGeneral.x_ObjectHelper.ItemDatabase.FetchItemByID(InventoryItem_to_change.itemID);
                    //rules

                    //chances
                    var chance_succeed = 0f;
                    var upgrade_amount = 1;
                    //penalty
                    var downgrade_penalty = 0;

                    /*
                     normal stones 3300-3303
                     Failsafe jewels 3308-3011
                     corrupted stones 3304 - 3307
                     perfect stones 3312-3315
                    */

                    //----------------
                    // AAAAAAAAAAAAAAA [+1 to +14]
                    //----------------
                    if (InventoryItem_to_change.itemUpgrade < 15)
                    {
                        switch (item_material.ItemID)
                        {
                            case 3300://Normal Jewel A 
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 1;
                                break;
                            case 3304: //Corrupted Jewel A
                                chance_succeed = 50f;
                                downgrade_penalty = -3;
                                upgrade_amount = 3;
                                break;
                            case 3308: //Failsafe Jewel A                                
                                chance_succeed = 50f;
                                downgrade_penalty = 0;
                                upgrade_amount = 3;
                                break;
                            case 3312: //Perfect Jewel A
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 6;
                                break;
                        }
                    }
                    //----------------
                    // BBBBBBBBBBBBBBB [+15 to +49]
                    //----------------
                    else if (InventoryItem_to_change.itemUpgrade >= 15 && InventoryItem_to_change.itemUpgrade < 50)
                    {
                        switch (item_material.ItemID)
                        {
                            case 3301://Normal Jewel B
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 1;
                                break;
                            case 3305: //Corrupted Jewel B
                                chance_succeed = 50f;
                                downgrade_penalty = -5;
                                upgrade_amount = 5;
                                break;
                            case 3309: //Failsafe Jewel B                        
                                chance_succeed = 50f;
                                downgrade_penalty = 0;
                                upgrade_amount = 5;
                                break;
                            case 3313: //Perfect Jewel B
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 10;
                                break;
                        }
                    }
                    //----------------
                    // CCCCCCCCCCCCCCC [+50 to +199]
                    //---------------- 
                    else if (InventoryItem_to_change.itemUpgrade >= 50 && InventoryItem_to_change.itemUpgrade < 200)
                    {
                        switch (item_material.ItemID)
                        {
                            case 3302://Normal Jewel C
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 1;
                                break;
                            case 3306: //Corrupted Jewel C
                                chance_succeed = 50f;
                                downgrade_penalty = -10;
                                upgrade_amount = 10;
                                break;
                            case 3310: //Failsafe Jewel C
                                chance_succeed = 50f;
                                downgrade_penalty = 0;
                                upgrade_amount = 10;
                                break;
                            case 3314: //Perfect Jewel C
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 20;
                                break;
                        }
                    }
                    //----------------
                    // DDDDDDDDDDDDDDD [+13 to +16]
                    //---------------- 
                    else if (InventoryItem_to_change.itemUpgrade >= 200)
                    {
                        switch (item_material.ItemID)
                        {
                            case 3303://Normal Jewel D
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 1;
                                break;
                            case 3307: //Corrupted Jewel D
                                chance_succeed = 50f;
                                downgrade_penalty = -15;
                                upgrade_amount = 15;
                                break;
                            case 3311: //Failsafe Jewel D
                                chance_succeed = 50f;
                                downgrade_penalty = 0;
                                upgrade_amount = 15;
                                break;
                            case 3315: //Perfect Jewel D
                                chance_succeed = 100f;
                                downgrade_penalty = 0;
                                upgrade_amount = 30;
                                break;
                        }
                    }

                    //operation ID
                    string log_uid = PlayerGeneral.x_ObjectHelper.randomString();

                    if (chance_succeed > 0)
                    {
                        if (UnityEngine.Random.Range(0f, 100f) <= chance_succeed)
                        {
                            //change it      
                            InventoryItem_to_change.itemUpgrade += upgrade_amount;
                            //limit
                            if (InventoryItem_to_change.itemUpgrade > 500)
                            {
                                InventoryItem_to_change.itemUpgrade = 500;
                            }
                            //log it                           
                            PlayerGeneral.x_ObjectHelper.save_game_log("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("item_upgrade_success->UID:{0} now:{1} up_by:{2}", InventoryItem_to_change.itemUniqueID, InventoryItem_to_change.itemUpgrade, upgrade_amount), log_uid, PlayerAccountInfo.PlayerIP);

                            //save the new item changes
                            PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.modifyItem(InventoryItem_to_change.itemUniqueID, string.Join("-", new List<int>(InventoryItem_to_change.itemMods).ConvertAll(r => r.ToString()).ToArray()), InventoryItem_to_change.itemUpgrade, InventoryItem_to_change.enchants)));
                            //send result
                            TargetItemUpgradeResult(connectionToClient, true, InventoryItem_to_change.itemUniqueID);
                        }
                        else
                        {
                            if (downgrade_penalty < 0)
                            {
                                //change it      
                                InventoryItem_to_change.itemUpgrade += downgrade_penalty;
                                //limit
                                if (InventoryItem_to_change.itemUpgrade < 0)
                                {
                                    InventoryItem_to_change.itemUpgrade = 0;
                                }
                                //save the new item changes
                                PlayerGeneral.x_ObjectHelper.StartCoroutine(PlayerGeneral.x_ObjectHelper.safeWWWrequest(PlayerGeneral.x_ObjectHelper.ServerDBHandler.modifyItem(InventoryItem_to_change.itemUniqueID, string.Join("-", new List<int>(InventoryItem_to_change.itemMods).ConvertAll(r => r.ToString()).ToArray()), InventoryItem_to_change.itemUpgrade, InventoryItem_to_change.enchants)));
                            }
                            //log it                           
                            PlayerGeneral.x_ObjectHelper.save_game_log("logsgame", PlayerAccountInfo.PlayerAccount, string.Format("item_upgrade_fail->UID:{0} now:{1} down_by:{2}", InventoryItem_to_change.itemUniqueID, InventoryItem_to_change.itemUpgrade, downgrade_penalty), log_uid, PlayerAccountInfo.PlayerIP);
                            //send result
                            TargetItemUpgradeResult(connectionToClient, false, 0);
                        }

                        //send changed item to client               
                        Mody_item_on_inventory_and_send(InventoryItem_to_change);
                        //remove item material on client
                        Remove_item_on_client_inventory(mat_used_item);
                        //remove from_item from inventory on DB
                        remove_item_from_DB_and_player_INVENTORY(mat_used_item, "item_upgrade uid:" + log_uid);
                    }

                }
            }

        }
    }
    #endregion

    #region Material handling
    public void material_add_amount_and_save(material.material_translation material_Translation, int amount, string logs_obtained_from)
    {
        var mat = fetch_materials(material_Translation);
        string log = string.Format("Mat:{0} Change:{1} From:{2} Had:{3}", mat.Material_name.ToString(), amount, logs_obtained_from, mat.Amount_held);
        if (mat != null)
        {
            mat.Amount_held += amount;
            if (mat.Amount_held < 0)
            {
                mat.Amount_held = 0;
            }
        }
        //save to DB
        PlayerGeneral.x_ObjectHelper.StartCoroutine(
            PlayerGeneral.x_ObjectHelper.safeWWWrequest(
                PlayerGeneral.x_ObjectHelper.ServerDBHandler.setPlayerMaterials(PlayerAccountInfo.PlayerAccount, material_Translation, mat.Amount_held)));
        //update on client
        TargetOperateMaterial(connectionToClient, (int)mat.Material_name, mat.Amount_held);
        //save log

        if (logs_obtained_from != string.Empty)
        {
            PlayerGeneral.x_ObjectHelper.StartCoroutine(
                PlayerGeneral.x_ObjectHelper.safeWWWrequest(
                    PlayerGeneral.ServerDBHandler.saveLog("logsgame", PlayerAccountInfo.PlayerAccount, log, this.GetComponent<PlayerAccountInfo>().PlayerIP)));
        }
        //quest
        if (amount > 0)//dont wanna track when we take materials out
        {
            gameObject.GetComponent<PlayerQuestInfo>().change_quest_progress(Task.task_types.material_collection, (int)material_Translation, gameObject, amount);
        }

    }
    public material fetch_materials(material.material_translation material_Translation)
    {
        for (int i = 0; i < Inventory.MaterialsList.Count; i++)
        {
            if (Inventory.MaterialsList[i].Material_name == material_Translation)
            {
                return Inventory.MaterialsList[i];
            }
        }
        return null;
    }
    private MemoryStream SerializeItem(List<material> MaterialsList)
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, MaterialsList);
        return mStream;
    }
    #endregion
}
