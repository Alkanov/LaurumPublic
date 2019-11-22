
[System.Serializable]
public class Item
{

    public enum UseAs
    {
        RightHand,
        LeftHand,
        Helmet,
        Belt,
        Chest,
        Gloves,
        Pants,
        Boots,
        Neck,
        Ring,
        HPPotion,
        MPPotion,
        Currency,
        Consumable,
        SkinBody,
        SkinWeapon,
        Summon,
        Teleport,
        ExpFarmStone,
        UpgradeStone,
        Pet,
        Egg,
        SkillMain,
        SkillSecondary,
        Misc,
        RewardChest
    }
    public enum Restrictions
    {
        tradeable,
        NOT_tradeable
    }


    public int ItemID;
    public UseAs useAs = UseAs.Currency;
    public Restrictions restrictions = Restrictions.NOT_tradeable;
    public int[] misc_data;//Use for anything, HP for potions, chest rewards.. etc
    public int ItemLevel = 1;

    public PlayerStats.PlayerClass[] requiredClass = new PlayerStats.PlayerClass[1];
    public float[] ItemStats = new float[9];
    public float UseCoolDown = 0;
    public int ConsumibleCharges = 0;//how many charges this consumable has

    public Item()
    {
        ItemID = -1;
    }
    /// <summary>
    /// Used only for currencies so far.
    /// </summary>
    public Item(int itemID, UseAs useAs)
    {
        ItemID = itemID;
        this.useAs = useAs;
    }
    /// <summary>
    /// Used for EQUIPABLE items that have stats like armor, weapons, etc.. not for skins
    /// </summary>
    public Item(int itemID, UseAs useAs, PlayerStats.PlayerClass[] requiredClass, float[] itemStats, int itemLevel, Restrictions restrictions)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.requiredClass = requiredClass;
        //Decimals round
        for (int i = 0; i < itemStats.Length; i++)
        {
            float currentValue = itemStats[i];
            float roundedValue = (float)Math.Round((Decimal)currentValue, 2, MidpointRounding.AwayFromZero);
            itemStats[i] = currentValue;
        }
        ItemStats = itemStats;
        ItemLevel = itemLevel;
        this.restrictions = restrictions;
    }

    /// <summary>
    /// Used for TELEPORTS
    /// </summary>
    public Item(int itemID, UseAs useAs, PlayerStats.PlayerClass[] requiredClass, int itemLevel, Restrictions restrictions, int consumibleCharges)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.requiredClass = requiredClass;
        ItemLevel = itemLevel;
        this.restrictions = restrictions;
        ConsumibleCharges = consumibleCharges;
    }

    /// <summary>
    /// Used for EQUIPABLE+CONSUMABLE items like potions that player can use
    /// </summary>
    public Item(int itemID, UseAs useAs, int itemLevel, Restrictions restrictions, float useCoolDown, int consumibleCharges, int useMultiplier)
    {
        ItemID = itemID;
        this.useAs = useAs;
        ItemLevel = itemLevel;
        this.restrictions = restrictions;
        UseCoolDown = useCoolDown;
        ConsumibleCharges = consumibleCharges;
        misc_data = new int[1] { useMultiplier };
    }

    /// <summary>
    /// Used for CONSUMABLES that burn after InventoryItem durability reaches 0, like exp potions premium or not. Data is inside misc_data[0]
    /// </summary>
    public Item(int itemID, UseAs useAs, Restrictions restrictions, int useMultiplier, int ConsumibleCharges, int ItemLevel)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        misc_data = new int[1] { useMultiplier };
        this.ConsumibleCharges = ConsumibleCharges;
    }

    /// <summary>
    /// Used for SKINS+EQUIPABLE premium or not
    /// </summary>
    public Item(int itemID, UseAs useAs, Restrictions restrictions, PlayerStats.PlayerClass[] requiredClass)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        this.requiredClass = requiredClass;
    }

    /// <summary>
    /// Used for STAT TRAINING 
    /// </summary>
    public Item(int itemID, UseAs useAs, int itemLevel, Restrictions restrictions)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        ItemLevel = itemLevel;
    }
    /// <summary>
    /// Used for UPGRADE STONES
    /// </summary>
    public Item(int itemID, UseAs useAs, Restrictions restrictions)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
    }
    /// <summary>
    /// Used for PET/Eggs
    /// </summary>
    public Item(int itemID, Restrictions restrictions, UseAs useAs, int[] misc_data)
    {
        /*
        misc_data[0]
        0 - nothing
        1 - bank
        2 - pot refiller
        3 - store seller
        4 - remote ds
        5 - remote broker
        6 - teleport hub
        7 - remote quest
        8 - auto loot 
         */
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        this.misc_data = misc_data;
    }
    /// <summary>
    /// Used for Skills misc_data[rebirth_required,skill_ID,skill_ID...]
    /// </summary>
    public Item(int itemID, UseAs useAs, Restrictions restrictions, PlayerStats.PlayerClass[] requiredClass, int itemLevel)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        this.requiredClass = requiredClass;
        ItemLevel = itemLevel;
    }

    /// <summary>
    /// Used for Chest Rewards
    /// </summary>
    public Item(int itemID, UseAs useAs, Restrictions restrictions, int[] misc_data, int itemLevel)
    {
        ItemID = itemID;
        this.useAs = useAs;
        this.restrictions = restrictions;
        this.misc_data = misc_data;
        ItemLevel = itemLevel;
    }
}