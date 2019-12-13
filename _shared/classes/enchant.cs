using System.Collections.Generic;

[System.Serializable]
public class enchant
{
    public enum enchant_base
    {
        lazuli,
        corrupted_lazuli,
        jade,
        corrupted_jade,
        saphire,
        corrupted_saphire,
        siderite,
        corrupted_siderite,
        gypsum,
        corrupted_gypsum
    }

    public enchant_base ench_base;//to know which bonus to touch otherwise we would have to switch each ID
    public List<int> IDs = new List<int>(); //ids of the different levels of enchants Lesser ,Greater ,Mighty ,Battle ,Perfect 
    public List<float[]> set_bonuses = new List<float[]>();//bonuses per sets per levels set_bonuses[0]=float[0]{2 items, 3 items, 4 items}<--this is Lesser because index of set_bonuses is 0
    public float chance_to_drop;//general chance to drop this enchant


    public enchant()
    {

    }

    public enchant(enchant_base ench_base, List<int> ds, List<float[]> set_bonuses, float chance_to_drop)
    {
        this.ench_base = ench_base;
        IDs = ds;
        this.set_bonuses = set_bonuses;
        this.chance_to_drop = chance_to_drop;
    }
}
