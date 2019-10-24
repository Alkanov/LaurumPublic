using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the <see cref="craftingRecipesDatabase" />
/// </summary>
public class craftingRecipesDatabase : MonoBehaviour
{
    /// <summary>
    /// Defines the craftingRecipes_list
    /// </summary>
    public List<craftingRecipe> craftingRecipes_list = new List<craftingRecipe>();

    /// <summary>
    /// The Awake
    /// </summary>
    private void Awake()
    {

        //**************EXAMPLES -1*********************
        //example 1 - recipe that requires materials and outputs 1 material "1 Bat Blood"
        craftingRecipes_list.Add(new craftingRecipe(
            9,//recipe ID - very important to not have duplicates
            new Dictionary<material.material_translation, int>() {//list of materials with amount required
                { material.material_translation.aut_leaf, 34 },
                { material.material_translation.bat_blood, 34 },
                 { material.material_translation.cha_bone, 30 },
                  { material.material_translation.demo_coin, 25 },
                   { material.material_translation.ectoplasm, 20 },
                    { material.material_translation.fire_orb, 15 },
                     { material.material_translation.grn_mush, 10 },
                      { material.material_translation.hell_core, 5 },
                       { material.material_translation.i_bracelet, 2 },
                        { material.material_translation.leaf, 1 }
            },
            200000,//gold required
            0,//playerlevel required
            craftingType.profession.none,//not in use
            0,//not in use
            material.material_translation.bat_blood,//material result
            -1//category number used in client UI
            ));


        //example 2 - recipe that requires materials and outputs 1 item
        craftingRecipes_list.Add(new craftingRecipe(9,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.aut_leaf, 34 },
                { material.material_translation.bat_blood, 34 },
                 { material.material_translation.cha_bone, 30 },
                  { material.material_translation.demo_coin, 25 },
                   { material.material_translation.ectoplasm, 20 },
                    { material.material_translation.fire_orb, 15 },
                     { material.material_translation.grn_mush, 10 },
                      { material.material_translation.hell_core, 5 },
                       { material.material_translation.i_bracelet, 2 },
                        { material.material_translation.leaf, 1 }
            },
            200000,//gold required
            0,//playerlevel required
            craftingType.profession.none,//not in use
            0,//not in use
            29041,//item ID in this case "Large Experience Potion"
            -1//category number used in client UI
            ));

        //**************CATEGORY 0 - SCROLLS*********************
        //**************CATEGORY 1 - POTIONS*********************
        //**************CATEGORY 2 - JEWELS*********************
        //1 Failsafe tickets for 1 perfect jewel A
        craftingRecipes_list.Add(new craftingRecipe(1,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 1 }},
            0, 0, craftingType.profession.jewelry, 0,

            3312,//this is the ItemID = result of the crafting
            2
            ));
        //2 silver tickets for 1 Failsafe jewel A
        craftingRecipes_list.Add(new craftingRecipe(5,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 2 }},
            0, 0, craftingType.profession.jewelry, 0,

            3308,//this is the ItemID = result of the crafting
            2
            ));

        //2 Failsafe tickets for 1 perfect jewel B
        craftingRecipes_list.Add(new craftingRecipe(2,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 2 }},
            0, 0, craftingType.profession.jewelry, 0,

            3313,//this is the ItemID = result of the crafting
            2
            ));
        //4 silver tickets for 1 Failsafe jewel B
        craftingRecipes_list.Add(new craftingRecipe(6,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 4 }},
            0, 0, craftingType.profession.jewelry, 0,

            3309,//this is the ItemID = result of the crafting
            2
            ));
        //3 Failsafe tickets for 1 perfect jewel C
        craftingRecipes_list.Add(new craftingRecipe(3,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 3 }},
            0, 0, craftingType.profession.jewelry, 0,

            3314,//this is the ItemID = result of the crafting
            2
            ));
        //6 silver tickets for 1 Failsafe jewel C
        craftingRecipes_list.Add(new craftingRecipe(7,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 6 }},
            0, 0, craftingType.profession.jewelry, 0,

            3310,//this is the ItemID = result of the crafting
            2
            ));
        //4 Failsafe tickets for 1 perfect jewel D
        craftingRecipes_list.Add(new craftingRecipe(4,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 4 }},
            0, 0, craftingType.profession.jewelry, 0,

            3315,//this is the ItemID = result of the crafting
            2
            )); 
        //10 silver tickets for 1 Failsafe jewel D
        craftingRecipes_list.Add(new craftingRecipe(8,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 10 }},
            0, 0, craftingType.profession.jewelry, 0,

            3311,//this is the ItemID = result of the crafting
            2
            ));

        //**************CATEGORY 3 - MISC*********************
        //+100k gold ingot
        craftingRecipes_list.Add(new craftingRecipe(12,
            100000, //gold required
            0, craftingType.profession.jewelry, 0,
            29060,//this is the ItemID = result of the crafting
            3
            ));

        //+500k gold ingot
        craftingRecipes_list.Add(new craftingRecipe(13,
            500000, //gold required
            0, craftingType.profession.jewelry, 0,
            29061,//this is the ItemID = result of the crafting
            3
            ));
        //+1M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(14,
            1000000, //gold required
            0, craftingType.profession.jewelry, 0,
            29062,//this is the ItemID = result of the crafting
            3
            ));
        //+10M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(15,
            10000000, //gold required
            0, craftingType.profession.jewelry, 0,
            29063,//this is the ItemID = result of the crafting
            3
            ));
        //+50M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(16,
            50000000, //gold required
            0, craftingType.profession.jewelry, 0,
            29064,//this is the ItemID = result of the crafting
            3
            ));

    }

    /// <summary>
    /// The FetchCraftingRecipe_by_ID
    /// </summary>
    /// <param name="recipe_id">The recipe_id<see cref="int"/></param>
    /// <returns>The <see cref="craftingRecipe"/></returns>
    public craftingRecipe FetchCraftingRecipe_by_ID(int recipe_id)
    {
        for (int i = 0; i < craftingRecipes_list.Count; i++)
        {
            if (craftingRecipes_list[i].ID == recipe_id)
            {
                return craftingRecipes_list[i];
            }
        }
        return null;
    }
}
