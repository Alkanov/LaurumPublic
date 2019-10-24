using System.Collections.Generic;

/// <summary>
/// Defines the <see cref="craftingType" />
/// </summary>
[System.Serializable]
public class craftingType
{
    /// <summary>
    /// Defines the profession
    /// </summary>
    public enum profession
    {
        /// <summary>
        /// Defines the none
        /// </summary>
        none,

        /// <summary>
        /// Defines the alchemy
        /// </summary>
        alchemy,

        /// <summary>
        /// Defines the handicrafts
        /// </summary>
        handicrafts,

        /// <summary>
        /// Defines the metalwork
        /// </summary>
        metalwork,

        /// <summary>
        /// Defines the weaponry
        /// </summary>
        weaponry,

        /// <summary>
        /// Defines the cooking
        /// </summary>
        cooking,

        /// <summary>
        /// Defines the tailoring
        /// </summary>
        tailoring,

        /// <summary>
        /// Defines the carpentry
        /// </summary>
        carpentry,

        /// <summary>
        /// Defines the leatherworking
        /// </summary>
        leatherworking,

        /// <summary>
        /// Defines the jewelry
        /// </summary>
        jewelry
    }
}

/// <summary>
/// Defines the <see cref="craftingRecipe" />
/// </summary>
[System.Serializable]
public class craftingRecipe
{
    /// <summary>
    /// Defines the ID
    /// </summary>
    public int ID;

    /// <summary>
    /// Defines the materials_required
    /// </summary>
    public Dictionary<material.material_translation, int> materials_required = new Dictionary<material.material_translation, int>();//material,amount

    /// <summary>
    /// Defines the gold_required
    /// </summary>
    public int gold_required;

    /// <summary>
    /// Defines the player_level_required
    /// </summary>
    public int player_level_required;

    /// <summary>
    /// Defines the profession_required
    /// </summary>
    public craftingType.profession profession_required;

    /// <summary>
    /// Defines the profession_level_required
    /// </summary>
    public int profession_level_required;

    /// <summary>
    /// Defines the ItemID_crafted_result
    /// </summary>
    public int ItemID_crafted_result = -1;

    /// <summary>
    /// Defines the material_crafted_result
    /// </summary>
    public material.material_translation material_crafted_result;

    /// <summary>
    /// Defines the items_required
    /// </summary>
    public Dictionary<int, int> items_required = new Dictionary<int, int>();//ItemID,amount

    /// <summary>
    /// Category on which this recipe is shown in the UI
    /// </summary>
    public int UI_category;

    /// <summary>
    /// Initializes a new instance of the <see cref="craftingRecipe"/> class.
    /// </summary>
    public craftingRecipe()
    {
    }

    /// <summary>
    /// Used on recipes that give ITEMS as end result <see cref="craftingRecipe"/> class.
    /// </summary>
    /// <param name="iD">The iD<see cref="int"/></param>
    /// <param name="materials_required">The materials_required<see cref="Dictionary{material.material_translation, int}"/></param>
    /// <param name="gold_required">The gold_required<see cref="int"/></param>
    /// <param name="player_level_required">The player_level_required<see cref="int"/></param>
    /// <param name="profession_required">The profession_required<see cref="craftingType.profession"/></param>
    /// <param name="profession_level_required">The profession_level_required<see cref="int"/></param>
    /// <param name="itemID_crafted_result">The itemID_crafted_result<see cref="int"/></param>
    public craftingRecipe(int iD, Dictionary<material.material_translation, int> materials_required, int gold_required, int player_level_required, craftingType.profession profession_required, int profession_level_required, int itemID_crafted_result, int UI_category)
    {
        ID = iD;
        this.materials_required = materials_required;
        this.gold_required = gold_required;
        this.player_level_required = player_level_required;
        this.profession_required = profession_required;
        this.profession_level_required = profession_level_required;
        ItemID_crafted_result = itemID_crafted_result;
        this.UI_category = UI_category;
    }

    /// <summary>
    /// Used on recipes that give MATERIALS as end result <see cref="craftingRecipe"/> class.
    /// </summary>
    /// <param name="iD">The iD<see cref="int"/></param>
    /// <param name="materials_required">The materials_required<see cref="Dictionary{material.material_translation, int}"/></param>
    /// <param name="gold_required">The gold_required<see cref="int"/></param>
    /// <param name="player_level_required">The player_level_required<see cref="int"/></param>
    /// <param name="profession_required">The profession_required<see cref="craftingType.profession"/></param>
    /// <param name="profession_level_required">The profession_level_required<see cref="int"/></param>
    /// <param name="itemID_crafted_result">The itemID_crafted_result<see cref="int"/></param>
    public craftingRecipe(int iD, Dictionary<material.material_translation, int> materials_required, int gold_required, int player_level_required, craftingType.profession profession_required, int profession_level_required, material.material_translation material_crafted_result, int UI_category)
    {
        ID = iD;
        this.materials_required = materials_required;
        this.gold_required = gold_required;
        this.player_level_required = player_level_required;
        this.profession_required = profession_required;
        this.profession_level_required = profession_level_required;
        this.material_crafted_result = material_crafted_result;
        this.UI_category = UI_category;
    }

    /// <summary>
    /// Used on recipes that REQUIRE GOLD ONLY and give ITEMS as end result <see cref="craftingRecipe"/> class.
    /// </summary>
    /// <param name="iD">The iD<see cref="int"/></param>
    /// <param name="materials_required">The materials_required<see cref="Dictionary{material.material_translation, int}"/></param>
    /// <param name="gold_required">The gold_required<see cref="int"/></param>
    /// <param name="player_level_required">The player_level_required<see cref="int"/></param>
    /// <param name="profession_required">The profession_required<see cref="craftingType.profession"/></param>
    /// <param name="profession_level_required">The profession_level_required<see cref="int"/></param>
    /// <param name="itemID_crafted_result">The itemID_crafted_result<see cref="int"/></param>
    public craftingRecipe(int iD, int gold_required, int player_level_required, craftingType.profession profession_required, int profession_level_required, int itemID_crafted_result, int UI_category)
    {
        ID = iD;       
        this.gold_required = gold_required;
        this.player_level_required = player_level_required;
        this.profession_required = profession_required;
        this.profession_level_required = profession_level_required;
        ItemID_crafted_result = itemID_crafted_result;
        this.UI_category = UI_category;
    }
}
