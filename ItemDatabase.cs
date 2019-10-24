using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    public List<Item> items = new List<Item>();

    void Start()
    {


        //Gold
        items.Add(new Item(7700, Item.UseAs.Currency));
        //IAP gems
        items.Add(new Item(7701, Item.UseAs.Currency));

        #region Equipable items
        /*
  CLASS: WARRIOR
  LEVEL: 1
  */
        items.Add(new Item(1101, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 22f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1201, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 6f, 4f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1601, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 32f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1701, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 24f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2001, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0.12f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2101, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 12, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1801, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 18f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1901, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.11f, 0f }, 1, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 10
        */
        items.Add(new Item(1102, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 54f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1202, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 14f, 10f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1604, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 80f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1702, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 62f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2002, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0.30f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2102, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 28, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1802, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 46f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1902, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.27f, 0f }, 10, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 20
        */
        items.Add(new Item(1103, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 108f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1203, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 26f, 22f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1611, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 160f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1715, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 122f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2009, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 1.60f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2108, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 56, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1808, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 94f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1911, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.54f, 0f }, 20, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 30
        */
        items.Add(new Item(1104, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 162f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1204, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 40f, 32f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1612, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 240f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1716, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 184f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2010, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 1.90f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2109, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 84, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1809, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 140f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1912, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.81f, 0f }, 30, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 40
        */
        items.Add(new Item(1105, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 216f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1205, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 54f, 42f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1613, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 320f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1717, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 246f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2011, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 2.20f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2110, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 112, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1810, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 186f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1913, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.08f, 0f }, 40, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 50
        */
        items.Add(new Item(1106, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 270f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1206, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 68f, 52f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1614, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 400f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1718, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 308f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2012, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 3.50f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2111, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 140, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1811, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 232f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1914, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.35f, 0f }, 50, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 60
        */
        items.Add(new Item(1107, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 324f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1207, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 80f, 64f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1615, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 480f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1719, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 368f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2013, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 3.80f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2112, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 168, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1812, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 280f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1915, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.62f, 0f }, 60, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 70
        */
        items.Add(new Item(1108, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 378f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1208, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 94f, 74f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1616, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 560f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1720, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 430f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2014, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 4.10f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2113, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 196, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1813, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 326f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1916, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.89f, 0f }, 70, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 80
        */
        items.Add(new Item(1109, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 432f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1209, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 108f, 84f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1617, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 640f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1721, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 492f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2015, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 4.40f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2114, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 224, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1814, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 372f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1917, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.16f, 0f }, 80, Item.Restrictions.tradeable));
        /*
        CLASS: WARRIOR
        LEVEL: 90
        */
        items.Add(new Item(1110, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 486f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1210, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 120f, 96f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1643, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 720f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1748, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 552f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2042, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 5.70f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2140, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 252, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1840, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 420f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1943, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.43f, 0f }, 90, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 1
        */
        items.Add(new Item(1501, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 20f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1633, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 30f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1737, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 22f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2032, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0.10f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2130, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 16, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1830, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 22f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1933, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.12f, 0f }, 1, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 10
        */
        items.Add(new Item(1502, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 48f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1634, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 76f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1738, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 54f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2033, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0.25f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2131, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 42, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1831, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 54f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1934, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.31f, 0f }, 10, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 20
        */
        items.Add(new Item(1503, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 96f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1635, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 152f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1739, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 106f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2034, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0.50f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2132, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 84, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1832, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 110f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1935, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.62f, 0f }, 20, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 30
        */
        items.Add(new Item(1504, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 144f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1636, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 228f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1740, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 160f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2035, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 1.75f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2133, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 126, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1833, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 164f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1936, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.93f, 0f }, 30, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 40
        */
        items.Add(new Item(1505, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 192f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1637, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 304f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1741, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 214f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2036, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 2.00f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2134, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 168, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1834, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 218f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1937, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.24f, 0f }, 40, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 50
        */
        items.Add(new Item(1506, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 240f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1638, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 380f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1742, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 268f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2037, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 2.25f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2135, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 210, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1835, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 272f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1938, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.55f, 0f }, 50, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 60
        */
        items.Add(new Item(1507, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 288f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1639, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 456f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1743, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 320f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2038, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 3.50f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2136, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 252, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1836, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 328f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1939, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.86f, 0f }, 60, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 70
        */
        items.Add(new Item(1508, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 336f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1640, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 532f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1744, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 374f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2039, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 3.75f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2137, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 294, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1837, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 382f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1940, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.17f, 0f }, 70, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 80
        */
        items.Add(new Item(1509, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 384f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1641, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 608f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1745, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 428f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2040, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 4.00f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2138, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 336, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1838, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 436f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1941, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.48f, 0f }, 80, Item.Restrictions.tradeable));
        /*
        CLASS: PALADIN
        LEVEL: 90
        */
        items.Add(new Item(1510, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 432f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1642, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 684f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1746, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 480f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2041, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 4.25f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2139, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 378, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1839, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 492f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1942, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 5.79f, 0f }, 90, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 1
        */
        items.Add(new Item(1301, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 20f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(12011, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 4f, 0.06f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1602, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 26f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1705, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 18f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2003, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0.17f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2103, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 14, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1803, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 16f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1903, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.15f, 0f }, 1, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 10
        */
        items.Add(new Item(1302, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 52f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(12012, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 8f, 0.14f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1608, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 64f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1706, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 44f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2004, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0.42f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2104, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 36, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1804, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 38f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1904, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.38f, 0f }, 10, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 20
        */
        items.Add(new Item(1303, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 102f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(12013, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 16f, 0.28f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1625, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 128f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1729, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 86f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2024, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 1.84f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2122, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 72, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1822, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 76f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1925, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.76f, 0f }, 20, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 30
        */
        items.Add(new Item(1304, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 152f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(12014, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 24f, 0.42f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1626, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 192f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1730, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 128f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2025, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 2.26f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2123, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 108, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1823, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 114f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1926, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.14f, 0f }, 30, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 40
        */
        items.Add(new Item(1305, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 204f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(12015, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 32f, 1.56f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1627, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 256f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1731, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 172f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2026, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 3.68f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2124, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 144, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1824, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 152f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1927, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.52f, 0f }, 40, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 50
        */
        items.Add(new Item(1306, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 256f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(12016, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 40f, 1.70f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1628, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 320f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1732, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 216f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2027, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 4.10f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2125, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 180, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1825, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 190f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1928, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.90f, 0f }, 50, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 60
        */
        items.Add(new Item(1307, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 306f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(12017, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 50f, 1.84f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1629, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 384f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1733, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 258f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2028, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 5.52f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2126, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 216, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1826, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 228f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1929, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.28f, 0f }, 60, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 70
        */
        items.Add(new Item(1308, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 356f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(12018, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 58f, 1.98f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1630, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 448f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1734, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 300f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2029, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 5.94f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2127, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 252, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1827, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 266f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1930, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 5.66f, 0f }, 70, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 80
        */
        items.Add(new Item(1309, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 408f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(12019, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 66f, 2.12f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1631, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 512f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1735, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 344f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2030, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 6.36f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2128, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 288, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1828, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 304f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1931, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 6.04f, 0f }, 80, Item.Restrictions.tradeable));
        /*
        CLASS: HUNTER
        LEVEL: 90
        */
        items.Add(new Item(1310, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 460f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(12020, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 74f, 2.26f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1632, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 576f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1736, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 388f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2031, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 7.78f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2129, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 324, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1829, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 342f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1932, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 6.42f, 0f }, 90, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 1
        */
        items.Add(new Item(1401, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 24f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(12021, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 8f, 0f, 2f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1603, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 22f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1709, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 14f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2006, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0.11f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(2105, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 20, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1806, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 20f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(1908, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.12f, 0f }, 1, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 10
        */
        items.Add(new Item(1402, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 60f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(12022, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 18f, 0f, 8f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1609, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 56f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1714, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 36f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2007, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0.27f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(2106, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 50, 0f, 0f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1807, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 50f, 0f, 0f }, 10, Item.Restrictions.tradeable));
        items.Add(new Item(1909, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.30f, 0f }, 10, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 20
        */
        items.Add(new Item(1403, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 120f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(12023, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 36f, 0f, 14f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1618, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 112f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1722, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 74f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2016, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 1.54f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(2115, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 100, 0f, 0f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1815, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 100f, 0f, 0f }, 20, Item.Restrictions.tradeable));
        items.Add(new Item(1918, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.60f, 0f }, 20, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 30
        */
        items.Add(new Item(1404, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 180f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(12024, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 52f, 0f, 22f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1619, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 168f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1723, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 112f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2017, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 1.81f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(2116, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 150, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1816, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 150f, 0f, 0f }, 30, Item.Restrictions.tradeable));
        items.Add(new Item(1919, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1.90f, 0f }, 30, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 40
        */
        items.Add(new Item(1405, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 240f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(12025, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 70f, 0f, 28f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1620, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 224f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1724, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 148f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2018, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 2.08f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(2117, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 200, 0f, 0f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1817, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 200f, 0f, 0f }, 40, Item.Restrictions.tradeable));
        items.Add(new Item(1920, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.20f, 0f }, 40, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 50
        */
        items.Add(new Item(1408, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 300f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(12026, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 88f, 0f, 36f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1621, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 280f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1725, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 184f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2019, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 2.35f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(2118, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 250, 0f, 0f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1818, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 250f, 0f, 0f }, 50, Item.Restrictions.tradeable));
        items.Add(new Item(1921, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.50f, 0f }, 50, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 60
        */
        items.Add(new Item(1409, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 360f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(12027, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 106f, 0f, 44f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1622, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 336f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1726, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 222f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2021, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 3.62f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(2119, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 300, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1819, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 300f, 0f, 0f }, 60, Item.Restrictions.tradeable));
        items.Add(new Item(1922, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 3.80f, 0f }, 60, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 70
        */
        items.Add(new Item(1410, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 420f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(12028, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 124f, 0f, 50f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1623, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 392f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1727, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 260f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2022, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 3.89f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(2120, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 350, 0f, 0f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1820, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 350f, 0f, 0f }, 70, Item.Restrictions.tradeable));
        items.Add(new Item(1923, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.10f, 0f }, 70, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 80
        */
        items.Add(new Item(1411, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 480f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(12029, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 140f, 0f, 58f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1624, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 448f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1728, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 296f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2023, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 4.16f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(2121, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 400, 0f, 0f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1821, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 400f, 0f, 0f }, 80, Item.Restrictions.tradeable));
        items.Add(new Item(1924, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 4.40f, 0f }, 80, Item.Restrictions.tradeable));
        /*
        CLASS: WIZARD
        LEVEL: 90
        */
        items.Add(new Item(1412, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 540f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(12030, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 158f, 0f, 64f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1645, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 504f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1747, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 332f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2043, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 4.43f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(2141, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 450, 0f, 0f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1841, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 450f, 0f, 0f }, 90, Item.Restrictions.tradeable));
        items.Add(new Item(1944, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 5.70f, 0f }, 90, Item.Restrictions.tradeable));

        //NECKLACE
        items.Add(new Item(2302, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 10f, 0f, 10f, 80f, 60f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+INT+STA+WIS
        items.Add(new Item(2303, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 40f, 0f, 60f, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+STA
        items.Add(new Item(2304, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 40f, 20f, 40f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //INT+STA+WIS
        items.Add(new Item(2305, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 2f, 0f, 0f, 0f, 0f, 40f, 2f, 0f }, 30, Item.Restrictions.tradeable)); //DEX+DEF+MDEF
        items.Add(new Item(2306, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 40f, 20f, 2f, 0f }, 45, Item.Restrictions.tradeable)); //DEF+MDEF+AGI

        /* items.Add(new Item(2307, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 110f, 0f, 0f, 0f, 0f }, 45, Item.Restrictions.tradeable)); //MP
        items.Add(new Item(2308, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 75f, 0f, 0f, 0f, 0f, 25f, 0f }, 60, Item.Restrictions.tradeable)); //INT
        items.Add(new Item(2309, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 75f, 0f, 0f, 0f, 0f, 0f, 25f, 0f, 0f }, 60, Item.Restrictions.tradeable)); //STR
        items.Add(new Item(2310, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 25f, 60f, 0f }, 60, Item.Restrictions.tradeable)); //MDEF
        items.Add(new Item(2311, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 60f, 25f, 0f }, 60, Item.Restrictions.tradeable)); //DEF
        */


        //RING             
        items.Add(new Item(2201, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 5f, 0f, 5f, 90f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+INT+STA
        items.Add(new Item(2202, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 30f, 0f, 0f, 60f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+STA
        items.Add(new Item(2203, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 2f, 0f, 0f, 0f, 20f, 30f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //CRIT+DEF+MDEF
        items.Add(new Item(2204, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 30f, 30f, 30f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //INT+STA+WIS
        items.Add(new Item(2205, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 50f, 50f, 0f, 0f, 2f, 0f }, 30, Item.Restrictions.tradeable)); //STA+WIS+AGI

        /* items.Add(new Item(2206, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 90f, 0f, 0f, 0f, 0f, 0f }, 45, Item.Restrictions.tradeable)); //HP
      items.Add(new Item(2207, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 90f, 0f, 0f, 0f, 0f }, 45, Item.Restrictions.tradeable)); //MP
        items.Add(new Item(2208, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 95f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable)); //INT
        items.Add(new Item(2209, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 95f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 60, Item.Restrictions.tradeable)); //STR
        items.Add(new Item(2210, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 80f, 0f, 0f }, 60, Item.Restrictions.tradeable)); //DEF
        items.Add(new Item(2211, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 80f, 0f }, 60, Item.Restrictions.tradeable)); //MDEF
       */

        /*
        CLASS: ADMIN
        LEVEL: 01
        */
        items.Add(new Item(999901, Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 999999f, 0f, 999999f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999902, Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 999999f, 99999f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999903, Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 999999f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999904, Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 999999f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999905, Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 100f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999906, Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 999999f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999907, Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 999999f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999908, Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 100f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999909, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 9999f, 99999f, 0f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999910, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 9999f, 0f, 0f, 0f, 99999f, 0f, 0f, 0f, 0f }, 1, Item.Restrictions.tradeable));
        items.Add(new Item(999911, Item.UseAs.HPPotion, 1, Item.Restrictions.tradeable, 5f, 9999, 999999));
        items.Add(new Item(999912, Item.UseAs.MPPotion, 1, Item.Restrictions.tradeable, 5f, 9999, 999999));
        #endregion

        #region Usables
        //teleports
        items.Add(new Item(29010, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable));//Teleport Scroll: Laurum Port
        items.Add(new Item(29011, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable));//Teleport Scroll: Fort Libra
        items.Add(new Item(29012, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable));//Teleport Scroll: Last death 
        items.Add(new Item(29013, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable));//Teleport Scroll: Party leader
        items.Add(new Item(29014, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable));//Teleport Scroll: Ghost town
        items.Add(new Item(29015, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable));//Teleport Scroll: Fire cave
        items.Add(new Item(29016, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 100, Item.Restrictions.tradeable));//Teleport Scroll: Rynthia

        //exp potions
        items.Add(new Item(29040, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 20000, 1, 1));//20k event exp potion
        items.Add(new Item(29041, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 15000, 1, 20));//crafted 15000 exp potion
        items.Add(new Item(29042, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 30000, 1, 50));//crafted 30000 exp potion
        items.Add(new Item(29043, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 45000, 1, 80));//crafted 45000 exp potion
        items.Add(new Item(29044, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 60000, 1, 100));//crafted 60000 exp potion        
        //misc
        items.Add(new Item(29050, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 0, 1, 20));//LP reset potion
        items.Add(new Item(29051, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 0, 1, 1));//RP reset potion
        items.Add(new Item(29052, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 500, 1, 1));//positive karma potion
        items.Add(new Item(29053, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 0, 1, 100));//reset karma potion
        items.Add(new Item(29054, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, -5000, 1, 50));//negative karma potion
        items.Add(new Item(29055, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 1000, 1, 50));//positive karma potion
        items.Add(new Item(29056, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 2500, 1, 75));//positive karma potion

        //gold ingots
        items.Add(new Item(29060, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 100000, 1, 1));//+100k gold ingot
        items.Add(new Item(29061, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 500000, 1, 1));//+500k gold ingot
        items.Add(new Item(29062, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 1000000, 1, 1));//+1M gold ingot
        items.Add(new Item(29063, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 10000000, 1, 1));//+10M gold ingot
        items.Add(new Item(29064, Item.UseAs.Consumable, Item.Restrictions.NOT_tradeable, 50000000, 1, 1));//+50M gold ingot
        /*
        Chest Rewards
        */
        items.Add(new Item(4000, Item.UseAs.RewardChest, Item.Restrictions.tradeable, new int[] { 100 }, 1));//Increase +1 player silver ticket amount
        items.Add(new Item(4001, Item.UseAs.RewardChest, Item.Restrictions.tradeable, new int[] { 101 }, 1));//Increase +1 player golden ticket amount
        items.Add(new Item(4002, Item.UseAs.RewardChest, Item.Restrictions.NOT_tradeable, new int[] { 102 }, 1));//IAP rewards
        //items.Add(new Item(4003, Item.UseAs.RewardChest, Item.Restrictions.NOT_tradeable, new int[] { 103,104,105,106 }, 1));//IAP rewards
        #endregion

        #region Pet
        items.Add(new Item(500, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 1 }));//bank
        items.Add(new Item(501, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 2 }));//pot refiller
        items.Add(new Item(502, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 3 }));//store seller
        items.Add(new Item(503, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 4 }));//remote ds
        items.Add(new Item(504, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 5 }));//remote broker
        items.Add(new Item(505, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 6 }));//teleport hub
        items.Add(new Item(506, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 7 }));//remote quest
        items.Add(new Item(507, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 8 }));//auto loot      
        items.Add(new Item(508, Item.Restrictions.tradeable, Item.UseAs.Summon, new int[] { 1 }));//bank ---- original 
        items.Add(new Item(509, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 1 }));
        items.Add(new Item(510, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 2 }));
        items.Add(new Item(511, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 3 }));
        items.Add(new Item(512, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 4 }));
        items.Add(new Item(513, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 5 }));
        items.Add(new Item(514, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 6 }));
        items.Add(new Item(515, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 7 }));
        items.Add(new Item(516, Item.Restrictions.NOT_tradeable, Item.UseAs.Summon, new int[] { 8 }));
        #endregion

        #region HP and MP potions
        //HP
        items.Add(new Item(2701, Item.UseAs.HPPotion, 1, Item.Restrictions.tradeable, 5f, 20, 100));
        items.Add(new Item(2702, Item.UseAs.HPPotion, 10, Item.Restrictions.tradeable, 5f, 40, 120));
        items.Add(new Item(2703, Item.UseAs.HPPotion, 20, Item.Restrictions.tradeable, 6f, 40, 140));
        items.Add(new Item(2704, Item.UseAs.HPPotion, 30, Item.Restrictions.tradeable, 7f, 40, 170));
        items.Add(new Item(2705, Item.UseAs.HPPotion, 40, Item.Restrictions.tradeable, 7f, 40, 200));
        items.Add(new Item(2706, Item.UseAs.HPPotion, 50, Item.Restrictions.tradeable, 7f, 40, 230));
        items.Add(new Item(2707, Item.UseAs.HPPotion, 60, Item.Restrictions.tradeable, 7f, 40, 260));
        items.Add(new Item(2708, Item.UseAs.HPPotion, 70, Item.Restrictions.tradeable, 7f, 40, 290));
        items.Add(new Item(2709, Item.UseAs.HPPotion, 80, Item.Restrictions.tradeable, 7f, 60, 310));
        items.Add(new Item(2710, Item.UseAs.HPPotion, 90, Item.Restrictions.tradeable, 7f, 60, 330));
        items.Add(new Item(2711, Item.UseAs.HPPotion, 95, Item.Restrictions.tradeable, 7f, 60, 350));
        /*items.Add(new Item(2712, Item.UseAs.HPPotion, 100, Item.Restrictions.tradeable, 5f, 1, 50));
        items.Add(new Item(2713, Item.UseAs.HPPotion, 0, Item.Restrictions.tradeable, 5f, 1, 50));
        items.Add(new Item(2714, Item.UseAs.HPPotion, 0, Item.Restrictions.tradeable, 5f, 1, 50));
        items.Add(new Item(2715, Item.UseAs.HPPotion, 0, Item.Restrictions.tradeable, 5f, 1, 50));
        items.Add(new Item(2716, Item.UseAs.HPPotion, 0, Item.Restrictions.tradeable, 5f, 1, 50));*/
        //MP
        items.Add(new Item(2801, Item.UseAs.MPPotion, 1, Item.Restrictions.tradeable, 5f, 24, 40));
        items.Add(new Item(2802, Item.UseAs.MPPotion, 10, Item.Restrictions.tradeable, 6f, 40, 50));
        items.Add(new Item(2803, Item.UseAs.MPPotion, 20, Item.Restrictions.tradeable, 7f, 50, 60));
        items.Add(new Item(2804, Item.UseAs.MPPotion, 30, Item.Restrictions.tradeable, 7f, 60, 70));
        items.Add(new Item(2805, Item.UseAs.MPPotion, 40, Item.Restrictions.tradeable, 7f, 60, 80));
        items.Add(new Item(2806, Item.UseAs.MPPotion, 50, Item.Restrictions.tradeable, 7f, 60, 90));
        items.Add(new Item(2807, Item.UseAs.MPPotion, 60, Item.Restrictions.tradeable, 7f, 60, 100));
        items.Add(new Item(2808, Item.UseAs.MPPotion, 70, Item.Restrictions.tradeable, 7f, 60, 110));
        items.Add(new Item(2809, Item.UseAs.MPPotion, 80, Item.Restrictions.tradeable, 7f, 60, 120));
        items.Add(new Item(2810, Item.UseAs.MPPotion, 90, Item.Restrictions.tradeable, 7f, 60, 130));
        items.Add(new Item(2811, Item.UseAs.MPPotion, 95, Item.Restrictions.tradeable, 7f, 60, 140));
        #endregion

        #region Exp Farm stones
        items.Add(new Item(3100, Item.UseAs.ExpFarmStone, 10, Item.Restrictions.tradeable));
        items.Add(new Item(3101, Item.UseAs.ExpFarmStone, 10, Item.Restrictions.tradeable));
        items.Add(new Item(3102, Item.UseAs.ExpFarmStone, 10, Item.Restrictions.tradeable));
        items.Add(new Item(3103, Item.UseAs.ExpFarmStone, 10, Item.Restrictions.tradeable));
        items.Add(new Item(3104, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3105, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3106, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3107, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3108, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3109, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3110, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3111, Item.UseAs.ExpFarmStone, 20, Item.Restrictions.tradeable));
        items.Add(new Item(3112, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3113, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3114, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3115, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3116, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3117, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3118, Item.UseAs.ExpFarmStone, 40, Item.Restrictions.tradeable));
        items.Add(new Item(3119, Item.UseAs.ExpFarmStone, 80, Item.Restrictions.tradeable));
        items.Add(new Item(3120, Item.UseAs.ExpFarmStone, 80, Item.Restrictions.tradeable));
        items.Add(new Item(3121, Item.UseAs.ExpFarmStone, 80, Item.Restrictions.tradeable));
        items.Add(new Item(3122, Item.UseAs.ExpFarmStone, 90, Item.Restrictions.tradeable));
        items.Add(new Item(3123, Item.UseAs.ExpFarmStone, 90, Item.Restrictions.tradeable));

        #endregion

        #region Upgrade stones
        items.Add(new Item(3300, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3301, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3302, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3303, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        //corrupted stones 3304-3307
        items.Add(new Item(3304, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3305, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3306, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3307, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        //failsafe jewels 3308-3011-->from silver tickets
        items.Add(new Item(3308, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3309, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3310, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3311, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        //perfect stones 3312 - 3315--> from golden tickets
        items.Add(new Item(3312, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3313, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3314, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        items.Add(new Item(3315, Item.UseAs.UpgradeStone, Item.Restrictions.tradeable));
        #endregion

        #region Skill orbs
        //warrior - primary
        items.Add(new Item(61001, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Bleed
        items.Add(new Item(61004, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Execution
        items.Add(new Item(61016, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Armor Crusher
        items.Add(new Item(61024, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Dismember
        items.Add(new Item(61025, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //---------------Utility
        //Charge                                                                                                                                                     
        items.Add(new Item(61006, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Provoke
        items.Add(new Item(61008, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Battle Shout
        items.Add(new Item(61013, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Slow down!
        items.Add(new Item(61026, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //On your knees!
        items.Add(new Item(61027, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //---------------Champion     
        //Shield Stun
        items.Add(new Item(61003, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Soul Cravings
        items.Add(new Item(61010, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Ultimate Defense
        items.Add(new Item(61020, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Arrow Deflect
        items.Add(new Item(61028, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));
        //Shields Up
        items.Add(new Item(61029, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, 1));

        //-----Wizard            
        //---------------Fire
        //Fireball
        items.Add(new Item(62001, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Flame missile
        items.Add(new Item(62002, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Burn
        items.Add(new Item(62003, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Meteor Rain
        items.Add(new Item(62004, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Lava Barrier
        items.Add(new Item(62005, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //---------------Ice
        //Ice Spear
        items.Add(new Item(62006, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Frost Blade
        items.Add(new Item(62007, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Frozen Hands
        items.Add(new Item(62008, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Blizzard
        items.Add(new Item(62009, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Frost Bomb
        items.Add(new Item(62010, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //---------------Util
        //Corpse Life Drain
        items.Add(new Item(62011, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Mana shield
        items.Add(new Item(62012, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Expanded Mana
        items.Add(new Item(62013, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Caster Contract
        items.Add(new Item(62014, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));
        //Concentration
        items.Add(new Item(62015, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, 1));

        //-----Hunter            
        //---------------Marskman
        //Snipe
        items.Add(new Item(63001, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Hawkeye
        items.Add(new Item(63002, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Multishot
        items.Add(new Item(63003, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Steady shot
        items.Add(new Item(63004, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Hamstring shot
        items.Add(new Item(63005, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //---------------Survival
        //Poison Arrow
        items.Add(new Item(63006, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Multi trap
        items.Add(new Item(63007, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Posion Trap
        items.Add(new Item(63008, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Steel trap
        items.Add(new Item(63009, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Booby Trap
        items.Add(new Item(63010, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //---------------Util
        //Hunters Mark
        items.Add(new Item(63011, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Hunters Ritual
        items.Add(new Item(63012, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Camouflage
        items.Add(new Item(63013, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Soul Sacrifice
        items.Add(new Item(63014, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Acrobatics
        items.Add(new Item(63015, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));

        //-----Paladin            
        //---------------Protection
        //Final Protection
        items.Add(new Item(64001, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Magic Protection totem
        items.Add(new Item(64002, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Physical Protection totem
        items.Add(new Item(64003, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Linked Hearts
        items.Add(new Item(64004, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //burn on touch buff
        items.Add(new Item(64005, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Holy Light
        items.Add(new Item(64006, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Self Heal
        items.Add(new Item(64007, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Heal totem
        items.Add(new Item(64008, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Resurrection
        items.Add(new Item(64009, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Cleanse
        items.Add(new Item(64010, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Mana totem
        items.Add(new Item(64011, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Silence
        items.Add(new Item(64012, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Speed totem
        items.Add(new Item(64013, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Buff remover
        items.Add(new Item(64014, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        //Remember me
        items.Add(new Item(64015, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, 1));
        #endregion

        #region Body skins
        //Warrior
        items.Add(new Item(25101, Item.UseAs.SkinBody, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25102, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25103, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25104, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25105, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25106, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25107, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25108, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(25109, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251010, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251011, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251012, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251013, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251014, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251015, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251016, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251017, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251018, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251019, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251020, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251021, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251022, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251023, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251024, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251025, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251026, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251027, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251028, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251029, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251030, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251031, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        //Hunter
        items.Add(new Item(25201, Item.UseAs.SkinBody, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25202, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25203, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25204, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25205, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25206, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25207, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25208, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(25209, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252010, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252011, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252012, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252013, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252014, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252015, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252016, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252017, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252018, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252019, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252020, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252021, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252022, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252023, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252024, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252025, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252026, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252027, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252028, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252029, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252030, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252031, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        //Wizard
        items.Add(new Item(25301, Item.UseAs.SkinBody, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25302, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25303, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25304, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25305, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25306, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25307, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25308, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(25309, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253010, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253011, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253012, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253013, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253014, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253015, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253016, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253017, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253018, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253019, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253020, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253021, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253022, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253023, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253024, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253025, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253026, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253027, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253028, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253029, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253030, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253031, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        //Paladin
        items.Add(new Item(25401, Item.UseAs.SkinBody, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25402, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25403, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25404, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25405, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25406, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25407, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25408, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(25409, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254010, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254011, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254012, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254013, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254014, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254015, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254016, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254017, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254018, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254019, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254020, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254021, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254022, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254023, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254024, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254025, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254026, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254027, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254028, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254029, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254030, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254031, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        #endregion

        #region Weapon skins
        //Warrior
        items.Add(new Item(26101, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26102, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26103, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26104, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26105, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26106, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26107, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26108, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26109, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26110, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26111, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26112, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26113, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26114, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26115, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26116, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(26117, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        //Hunter
        items.Add(new Item(26201, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26202, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26203, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26204, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26205, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26206, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26207, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26208, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26209, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26210, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26211, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26212, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26213, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26214, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26215, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26216, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(26217, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        //Wizard
        items.Add(new Item(26301, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26302, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26303, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26304, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26305, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26306, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26307, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26308, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26309, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26310, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26311, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26312, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26313, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26314, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26315, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26316, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(26317, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        //Paladin
        items.Add(new Item(26401, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26402, Item.UseAs.SkinWeapon, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26403, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26404, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26405, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26406, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26407, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26408, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26409, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26410, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26411, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26412, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26413, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26414, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26415, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26416, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(26417, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        #endregion




    }
    #region Used On both 
    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].ItemID == id)
                return items[i];
        return null;
    }
    #endregion

    #region Used On client 
    public List<Item> FetchItembyItemType(PlayerStats.PlayerClass classToSearch, Item.UseAs equipableType)
    {
        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (System.Array.IndexOf(items[i].requiredClass, classToSearch) >= 0 && items[i].useAs == equipableType)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }
    /// <summary>
    /// Pull item but dont include the ones in the list of ints
    /// </summary>
    public List<Item> FetchItemby_ItemType_and_class_with_exceptions(Item.UseAs equipableType, PlayerStats.PlayerClass classToSearch, List<int> exception_item_ID)
    {
        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (items[i].useAs == equipableType && System.Array.IndexOf(items[i].requiredClass, classToSearch) >= 0 && !exception_item_ID.Contains(items[i].ItemID) && items[i].ItemID < 999900)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }
    #endregion
    #region used On Server
    public List<Item> FetchItembyItemType(Item.UseAs equipableType)
    {
        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (items[i].useAs == equipableType)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }
    public List<Item> FetchItemBy_class_part_level(PlayerStats.PlayerClass classToSearch, int level)
    {
        List<Item> allItems = new List<Item>();
        List<Item.UseAs> set_parts = new List<Item.UseAs>();
        set_parts.Add(Item.UseAs.Helmet);
        set_parts.Add(Item.UseAs.Chest);
        set_parts.Add(Item.UseAs.Gloves);
        set_parts.Add(Item.UseAs.Belt);
        set_parts.Add(Item.UseAs.Pants);
        set_parts.Add(Item.UseAs.Boots);
        set_parts.Add(Item.UseAs.RightHand);
        set_parts.Add(Item.UseAs.LeftHand);
        set_parts.Add(Item.UseAs.Neck);
        set_parts.Add(Item.UseAs.Ring);
        set_parts.Add(Item.UseAs.HPPotion);
        set_parts.Add(Item.UseAs.MPPotion);

        for (int i = 0; i < items.Count; i++)
        {
            if ((System.Array.IndexOf(items[i].requiredClass, classToSearch) >= 0 || System.Array.IndexOf(items[i].requiredClass, PlayerStats.PlayerClass.Any) >= 0) && set_parts.Contains(items[i].useAs) && items[i].ItemLevel == level && items[i].ItemID < 999900)
            {
                allItems.Add(items[i]);
            }
        }


        return allItems;
    }
    public List<Item> FetchItemBy_tutorial()
    {
        List<Item> allItems = new List<Item>();
        //hp
        allItems.Add(FetchItemByID(2701));
        //mp
        allItems.Add(FetchItemByID(2801));
        //PlayerStats.PlayerClass.Hunter:
        allItems.Add(FetchItemByID(63001));
        allItems.Add(FetchItemByID(63009));
        // PlayerStats.PlayerClass.Wizard:
        allItems.Add(FetchItemByID(62001));
        allItems.Add(FetchItemByID(62003));
        // PlayerStats.PlayerClass.Paladin:
        allItems.Add(FetchItemByID(64007));
        allItems.Add(FetchItemByID(64006));
        // PlayerStats.PlayerClass.Warrior:
        allItems.Add(FetchItemByID(61003));
        allItems.Add(FetchItemByID(61024));

        return allItems;
    }
    /// <summary>
    /// Pull item but dont include the ones in the list of ints
    /// </summary>
    public List<Item> FetchItembyItemType_with_exceptions(Item.UseAs equipableType, List<int> exception_item_ID)
    {
        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (items[i].useAs == equipableType && !exception_item_ID.Contains(items[i].ItemID) && items[i].ItemID < 999900)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }
    /// <summary>
    /// Pull specific item list
    /// </summary>
    public List<Item> FetchItem_by_list(List<int> specific_list)
    {
        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (specific_list.Contains(items[i].ItemID) && items[i].ItemID < 999900)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }
    /// <summary>
    /// Pull item by item type and min max level of it
    /// </summary>
    public List<Item> FetchItemby_part_and_minmax_level(List<Item.UseAs> equipableType, int min_level, int max_level)
    {
        if (min_level > 90)
        {
            min_level = 90;
        }
        if (max_level > 90)
        {
            max_level = 95;
        }

        List<Item> allItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
            if (equipableType.Contains(items[i].useAs) && items[i].ItemLevel >= min_level && items[i].ItemLevel <= max_level && items[i].ItemID < 999900)
            {
                allItems.Add(items[i]);
            }

        return allItems;
    }

    #endregion
}
