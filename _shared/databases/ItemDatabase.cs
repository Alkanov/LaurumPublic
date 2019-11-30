using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    public List<Item> items = new List<Item>();

    //Number of total item tiers
    public const int N_TIERS = 10;

    //Number of total item by tier
    public const int N_ITEMS = 8;

    /*** Level 90 max stats ***/
    public const float MAX_HP = 750f;
    public const float MAX_MP = MAX_HP * 0.6f;
    public const float MAX_Def = 550f;
    public const float MAX_Crit_Dodge = 4.4f;
    public const float MAX_Damage = MAX_Def * 0.8f;

    //Secondary (Quiver, Shield, Book)
    public const float Def_Secondary = MAX_Def * 0.2f;
    public const float Damage_Secondary = MAX_Damage * 0.2f;
    public const float HP_Secondary = MAX_HP * 0.2f;
    public const float MP_Secondary = MAX_MP * 0.2f;
    public const float Crit_Secondary = MAX_Crit_Dodge * 0.6f;

    //Accessories(Necklace, ring)
    public const float Def_Accessory = Def_Secondary * 0.5f;
    public const float Damage_Accessory = Damage_Secondary * 0.5f;
    public const float HP_Accessory = HP_Secondary * 0.5f;
    public const float MP_Accessory = MP_Secondary * 0.5f;
    public const float Crit_Accessory = Crit_Secondary * 0.75f;

    //Declare every item class modifier based on the max item stat and the multiplier associated to that class(same as stat multipliers of each class)
    //We divide that into the max item tiers we have in the game(current 10: 1 to 90)

    //Warrior
    public const float War_Weapon = (MAX_Damage * 1f) / N_TIERS;
    public const float War_Left_Hand = (Def_Secondary) / N_TIERS;
    public const float War_Helm = (MAX_HP * 1f) / N_TIERS;
    public const float War_Chest = (MAX_Def * 1f) / N_TIERS;
    public const float War_Gloves = (MAX_Crit_Dodge * 0.6f) / N_TIERS;
    public const float War_Belt = (MAX_MP * 0.6f) / N_TIERS;
    public const float War_Pants = (MAX_Def * 0.6f) / N_TIERS;
    public const float War_Boots = (MAX_Crit_Dodge * 0.6f) / N_TIERS;

    //Wizard
    public const float Wiz_Weapon = (MAX_Damage * 1f) / N_TIERS;
    public const float Wiz_Left_Hand_1 = (MP_Secondary) / N_TIERS;
    public const float Wiz_Left_Hand_2 = (Def_Secondary) / N_TIERS;
    public const float Wiz_Helm = (MAX_HP * 0.6f) / N_TIERS;
    public const float Wiz_Chest = (MAX_Def * 0.6f) / N_TIERS;
    public const float Wiz_Gloves = (MAX_Crit_Dodge * 0.9f) / N_TIERS;
    public const float Wiz_Belt = (MAX_MP * 1f) / N_TIERS;
    public const float Wiz_Pants = (MAX_Def * 1f) / N_TIERS;
    public const float Wiz_Boots = (MAX_Crit_Dodge * 0.9f) / N_TIERS;

    //Hunter
    public const float Hun_Weapon = (MAX_Damage * 0.9f) / N_TIERS;
    public const float Hun_Left_Hand_1 = (Damage_Secondary) / N_TIERS;
    public const float Hun_Left_Hand_2 = (Crit_Secondary) / N_TIERS;
    public const float Hun_Helm = (MAX_HP * 0.75f) / N_TIERS;
    public const float Hun_Chest = (MAX_Def * 0.75f) / N_TIERS;
    public const float Hun_Gloves = (MAX_Crit_Dodge * 1f) / N_TIERS;
    public const float Hun_Belt = (MAX_MP * 0.75f) / N_TIERS;
    public const float Hun_Pants = (MAX_Def * 0.75f) / N_TIERS;
    public const float Hun_Boots = (MAX_Crit_Dodge * 1f) / N_TIERS;

    //Paladin
    public const float Pal_Weapon = (MAX_Damage * 0.9f) / N_TIERS;
    public const float Pal_Helm = (MAX_HP * 0.9f) / N_TIERS;
    public const float Pal_Chest = (MAX_Def * 0.9f) / N_TIERS;
    public const float Pal_Gloves = (MAX_Crit_Dodge * 0.75f) / N_TIERS;
    public const float Pal_Belt = (MAX_MP * 0.9f) / N_TIERS;
    public const float Pal_Pants = (MAX_Def * 0.9f) / N_TIERS;
    public const float Pal_Boots = (MAX_Crit_Dodge * 0.75f) / N_TIERS;

    /***Items ids by class and tier***/

    //Warrior
    public static int[] War_Lvl_1 = { 1101, 1201, 1601, 1701, 2001, 2101, 1801, 1901 };
    public static int[] War_Lvl_10 = { 1102, 1202, 1604, 1702, 2002, 2102, 1802, 1902 };
    public static int[] War_Lvl_20 = { 1103, 1203, 1611, 1715, 2009, 2108, 1808, 1911 };
    public static int[] War_Lvl_30 = { 1104, 1204, 1612, 1716, 2010, 2109, 1809, 1912 };
    public static int[] War_Lvl_40 = { 1105, 1205, 1613, 1717, 2011, 2110, 1810, 1913 };
    public static int[] War_Lvl_50 = { 1106, 1206, 1614, 1718, 2012, 2111, 1811, 1914 };
    public static int[] War_Lvl_60 = { 1107, 1207, 1615, 1719, 2013, 2112, 1812, 1915 };
    public static int[] War_Lvl_70 = { 1108, 1208, 1616, 1720, 2014, 2113, 1813, 1916 };
    public static int[] War_Lvl_80 = { 1109, 1209, 1617, 1721, 2015, 2114, 1814, 1917 };
    public static int[] War_Lvl_90 = { 1110, 1210, 1643, 1748, 2042, 2140, 1840, 1943 };
    public static int[][] War_Items = {War_Lvl_1, War_Lvl_10, War_Lvl_20, War_Lvl_30, War_Lvl_40,
                                            War_Lvl_50,War_Lvl_60, War_Lvl_70, War_Lvl_80, War_Lvl_90};

    //Wizard
    public static int[] Wiz_Lvl_1 = { 1401, 12021, 1603, 1709, 2006, 2105, 1806, 1908 };
    public static int[] Wiz_Lvl_10 = { 1402, 12022, 1609, 1714, 2007, 2106, 1807, 1909 };
    public static int[] Wiz_Lvl_20 = { 1403, 12023, 1618, 1722, 2016, 2115, 1815, 1918 };
    public static int[] Wiz_Lvl_30 = { 1404, 12024, 1619, 1723, 2017, 2116, 1816, 1919 };
    public static int[] Wiz_Lvl_40 = { 1405, 12025, 1620, 1724, 2018, 2117, 1817, 1920 };
    public static int[] Wiz_Lvl_50 = { 1408, 12026, 1621, 1725, 2019, 2118, 1818, 1921 };
    public static int[] Wiz_Lvl_60 = { 1409, 12027, 1622, 1726, 2021, 2119, 1819, 1922 };
    public static int[] Wiz_Lvl_70 = { 1410, 12028, 1623, 1727, 2022, 2120, 1820, 1923 };
    public static int[] Wiz_Lvl_80 = { 1411, 12029, 1624, 1728, 2023, 2121, 1821, 1924 };
    public static int[] Wiz_Lvl_90 = { 1412, 12030, 1645, 1747, 2043, 2141, 1841, 1944 };
    public static int[][] Wiz_Items = {Wiz_Lvl_1, Wiz_Lvl_10, Wiz_Lvl_20, Wiz_Lvl_30, Wiz_Lvl_40,
                                            Wiz_Lvl_50,Wiz_Lvl_60, Wiz_Lvl_70, Wiz_Lvl_80, Wiz_Lvl_90};

    //Hunter
    public static int[] Hun_Lvl_1 = { 1301, 12011, 1602, 1705, 2003, 2103, 1803, 1903 };
    public static int[] Hun_Lvl_10 = { 1302, 12012, 1608, 1706, 2004, 2104, 1804, 1904 };
    public static int[] Hun_Lvl_20 = { 1303, 12013, 1625, 1729, 2024, 2122, 1822, 1925 };
    public static int[] Hun_Lvl_30 = { 1304, 12014, 1626, 1730, 2025, 2123, 1823, 1926 };
    public static int[] Hun_Lvl_40 = { 1305, 12015, 1627, 1731, 2026, 2124, 1824, 1927 };
    public static int[] Hun_Lvl_50 = { 1306, 12016, 1628, 1732, 2027, 2125, 1825, 1928 };
    public static int[] Hun_Lvl_60 = { 1307, 12017, 1629, 1733, 2028, 2126, 1826, 1929 };
    public static int[] Hun_Lvl_70 = { 1308, 12018, 1630, 1734, 2029, 2127, 1827, 1930 };
    public static int[] Hun_Lvl_80 = { 1309, 12019, 1631, 1735, 2030, 2128, 1828, 1931 };
    public static int[] Hun_Lvl_90 = { 1310, 12020, 1632, 1736, 2031, 2129, 1829, 1932 };
    public static int[][] Hun_Items = {Hun_Lvl_1, Hun_Lvl_10, Hun_Lvl_20, Hun_Lvl_30, Hun_Lvl_40,
                                            Hun_Lvl_50,Hun_Lvl_60, Hun_Lvl_70, Hun_Lvl_80, Hun_Lvl_90};

    //Paladin //We decrease by one because it doesn't declare the shield
    public static int[] Pal_Lvl_1 = { 1501, 1633, 1737, 2032, 2130, 1830, 1933 };
    public static int[] Pal_Lvl_10 = { 1502, 1634, 1738, 2033, 2131, 1831, 1934 };
    public static int[] Pal_Lvl_20 = { 1503, 1635, 1739, 2034, 2132, 1832, 1935 };
    public static int[] Pal_Lvl_30 = { 1504, 1636, 1740, 2035, 2133, 1833, 1936 };
    public static int[] Pal_Lvl_40 = { 1505, 1637, 1741, 2036, 2134, 1834, 1937 };
    public static int[] Pal_Lvl_50 = { 1506, 1638, 1742, 2037, 2135, 1835, 1938 };
    public static int[] Pal_Lvl_60 = { 1507, 1639, 1743, 2038, 2136, 1836, 1939 };
    public static int[] Pal_Lvl_70 = { 1508, 1640, 1744, 2039, 2137, 1837, 1940 };
    public static int[] Pal_Lvl_80 = { 1509, 1641, 1745, 2040, 2138, 1838, 1941 };
    public static int[] Pal_Lvl_90 = { 1510, 1642, 1746, 2041, 2139, 1839, 1942 };
    public static int[][] Pal_Items = {Pal_Lvl_1, Pal_Lvl_10, Pal_Lvl_20, Pal_Lvl_30, Pal_Lvl_40,
                                            Pal_Lvl_50,Pal_Lvl_60, Pal_Lvl_70, Pal_Lvl_80, Pal_Lvl_90};
    void Start()
    {
        //Gold
        items.Add(new Item(7700, Item.UseAs.Currency));
        //IAP gems
        items.Add(new Item(7701, Item.UseAs.Currency));

        #region Equipable items

        //CLASS: WARRIOR
        for (int tier = 0; tier < N_TIERS; tier++)
        {
            int lvl = tier == 0 ? 1 : tier * 10;
            float multiplier = (float)(tier + 1);
            items.Add(new Item(War_Items[tier][0], Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { War_Weapon * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][1], Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior, PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, War_Left_Hand * multiplier, War_Left_Hand * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][2], Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, War_Helm * multiplier, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][3], Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, War_Chest * multiplier, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][4], Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, War_Gloves * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][5], Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, War_Belt * multiplier, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][6], Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, War_Pants * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(War_Items[tier][7], Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, War_Boots * multiplier, 0f }, lvl, Item.Restrictions.tradeable));
        }

        //CLASS: HUNTER
        for (int tier = 0; tier < N_TIERS; tier++)
        {
            int lvl = tier == 0 ? 1 : tier * 10;
            float multiplier = (float)(tier + 1);
            items.Add(new Item(Hun_Items[tier][0], Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { Hun_Weapon * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][1], Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, Hun_Left_Hand_1 * multiplier, Hun_Left_Hand_2 * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][2], Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, Hun_Helm * multiplier, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][3], Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, Hun_Chest * multiplier, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][4], Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, Hun_Gloves * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][5], Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, Hun_Belt * multiplier, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][6], Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, Hun_Pants * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Hun_Items[tier][7], Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, Hun_Boots * multiplier, 0f }, lvl, Item.Restrictions.tradeable));
        }

        //CLASS: WIZARD
        for (int tier = 0; tier < N_TIERS; tier++)
        {
            int lvl = tier == 0 ? 1 : tier * 10;
            float multiplier = (float)(tier + 1);
            items.Add(new Item(Wiz_Items[tier][0], Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, Wiz_Weapon * multiplier, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][1], Item.UseAs.LeftHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, Wiz_Left_Hand_1 * multiplier, 0f, Wiz_Left_Hand_2 * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][2], Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, Wiz_Helm * multiplier, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][3], Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, Wiz_Chest * multiplier, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][4], Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, Wiz_Gloves * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][5], Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, Wiz_Belt * multiplier, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][6], Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, Wiz_Pants * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Wiz_Items[tier][7], Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, Wiz_Boots * multiplier, 0f }, lvl, Item.Restrictions.tradeable));
        }

        //CLASS: PALADIN
        for (int tier = 0; tier < N_TIERS; tier++)
        {
            int lvl = tier == 0 ? 1 : tier * 10;
            float multiplier = (float)(tier + 1);
            items.Add(new Item(Pal_Items[tier][0], Item.UseAs.RightHand, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, Pal_Weapon * multiplier, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][1], Item.UseAs.Helmet, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, Pal_Helm * multiplier, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][2], Item.UseAs.Chest, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, Pal_Chest * multiplier, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][3], Item.UseAs.Gloves, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, Pal_Gloves * multiplier, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][4], Item.UseAs.Belt, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, Pal_Belt * multiplier, 0f, 0f, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][5], Item.UseAs.Pants, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, Pal_Pants * multiplier, 0f, 0f }, lvl, Item.Restrictions.tradeable));
            items.Add(new Item(Pal_Items[tier][6], Item.UseAs.Boots, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, Pal_Boots * multiplier, 0f }, lvl, Item.Restrictions.tradeable));
        }

        //NECKLACE
        items.Add(new Item(2302, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { Damage_Accessory, 0f, Damage_Accessory, HP_Accessory, MP_Accessory, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+INT+STA+WIS
        items.Add(new Item(2303, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { Damage_Accessory, 0f, Damage_Accessory, 0f, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+STA
        items.Add(new Item(2304, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, Damage_Accessory, HP_Accessory, MP_Accessory, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //INT+STA+WIS
        items.Add(new Item(2305, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, Crit_Accessory, 0f, 0f, 0f, 0f, MP_Accessory, Crit_Accessory, 0f }, 30, Item.Restrictions.tradeable)); //DEX+DEF+MDEF
        items.Add(new Item(2306, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, Def_Accessory, Def_Accessory, Crit_Accessory, 0f }, 45, Item.Restrictions.tradeable)); //DEF+MDEF+AGI

        /* items.Add(new Item(2307, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 110f, 0f, 0f, 0f, 0f }, 45, Item.Restrictions.tradeable)); //MP
        items.Add(new Item(2308, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 75f, 0f, 0f, 0f, 0f, 25f, 0f }, 60, Item.Restrictions.tradeable)); //INT
        items.Add(new Item(2309, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 75f, 0f, 0f, 0f, 0f, 0f, 25f, 0f, 0f }, 60, Item.Restrictions.tradeable)); //STR
        items.Add(new Item(2310, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 25f, 60f, 0f }, 60, Item.Restrictions.tradeable)); //MDEF
        items.Add(new Item(2311, Item.UseAs.Neck, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 60f, 25f, 0f }, 60, Item.Restrictions.tradeable)); //DEF
        */


        //RING             
        items.Add(new Item(2201, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { Damage_Accessory, 0f, Damage_Accessory, HP_Accessory, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+INT+STA
        items.Add(new Item(2202, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { Damage_Accessory, 0f, 0f, HP_Accessory, 0f, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //STR+STA
        items.Add(new Item(2203, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, Crit_Accessory, 0f, 0f, 0f, Def_Accessory, Def_Accessory, 0f, 0f }, 30, Item.Restrictions.tradeable)); //CRIT+DEF+MDEF
        items.Add(new Item(2204, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, Damage_Accessory, HP_Accessory, MP_Accessory, 0f, 0f, 0f, 0f }, 30, Item.Restrictions.tradeable)); //INT+STA+WIS
        items.Add(new Item(2205, Item.UseAs.Ring, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, new float[] { 0f, 0f, 0f, HP_Accessory, MP_Accessory, 0f, 0f, Crit_Accessory, 0f }, 30, Item.Restrictions.tradeable)); //STA+WIS+AGI

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
        items.Add(new Item(29010, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable, 15));//Teleport Scroll: Laurum Port
        items.Add(new Item(29011, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable, 15));//Teleport Scroll: Fort Libra
        items.Add(new Item(29012, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable, 15));//Teleport Scroll: Last death 
        items.Add(new Item(29013, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable, 15));//Teleport Scroll: Party leader
        items.Add(new Item(29014, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 1, Item.Restrictions.tradeable, 15));//Teleport Scroll: Ghost town
        items.Add(new Item(29015, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 50, Item.Restrictions.tradeable, 15));//Teleport Scroll: Fire cave
        items.Add(new Item(29016, Item.UseAs.Teleport, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Any }, 100, Item.Restrictions.tradeable, 15));//Teleport Scroll: Rynthia

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
        items.Add(new Item(63002, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Multishot
        items.Add(new Item(63003, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Steady shot
        items.Add(new Item(63004, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Hamstring shot
        items.Add(new Item(63005, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //---------------Survival
        //Poison Arrow
        items.Add(new Item(63006, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Multi trap
        items.Add(new Item(63007, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Posion Trap
        items.Add(new Item(63008, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Steel trap
        items.Add(new Item(63009, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Booby Trap
        items.Add(new Item(63010, Item.UseAs.SkillMain, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //---------------Util
        //Hunter's Mark
        items.Add(new Item(63011, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Hunter's Ritual
        items.Add(new Item(63012, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
        //Camouflage
        items.Add(new Item(63013, Item.UseAs.SkillSecondary, Item.Restrictions.tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }, 1));
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
        //xmas
        items.Add(new Item(251032, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
        items.Add(new Item(251033, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));

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
        //xmas
        items.Add(new Item(252032, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
        items.Add(new Item(252033, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));

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
        //xmas
        items.Add(new Item(253032, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
        items.Add(new Item(253033, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));

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
        //xmas
        items.Add(new Item(254032, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
        items.Add(new Item(254033, Item.UseAs.SkinBody, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
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
        items.Add(new Item(26118, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Warrior }));
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
        items.Add(new Item(26218, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Hunter }));
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
        items.Add(new Item(26318, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Wizard }));
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
        items.Add(new Item(26418, Item.UseAs.SkinWeapon, Item.Restrictions.NOT_tradeable, new PlayerStats.PlayerClass[] { PlayerStats.PlayerClass.Paladin }));
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
