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

        //**************EXAMPLES 0 - SCROLLS*********************
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
        //-------Teleports you to Laurum Harbour
        craftingRecipes_list.Add(new craftingRecipe(1,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.dirt, 5 },
                { material.material_translation.slime_core, 5 },
           },
           1000,//gold required
           1,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29010,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to the Party Leader's Location
        craftingRecipes_list.Add(new craftingRecipe(2,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.rune, 25 },
                { material.material_translation.h_stone, 25 },
           },
           15000,//gold required
           1,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29013,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to your Last Die Location
        craftingRecipes_list.Add(new craftingRecipe(3,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.dus_tome, 25 },
                { material.material_translation.anc_axe, 25 },
           },
           15000,//gold required
           50,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29012,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to Ghost Town
        craftingRecipes_list.Add(new craftingRecipe(4,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.mys_flyer, 10 },
                { material.material_translation.soul, 10 },
           },
           2000,//gold required
           1,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29014,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to Libra
        craftingRecipes_list.Add(new craftingRecipe(5,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.sco_tail, 20 },
                { material.material_translation.e_stone, 20 },
           },
           5000,//gold required
           50,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29011,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to Rynthia
        craftingRecipes_list.Add(new craftingRecipe(6,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.i_bracelet, 25 },
                { material.material_translation.pur_cape, 25 },
           },
           10000,//gold required
           100,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29016,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));

        //-------Teleports you to Fire Cave's Entrance
        craftingRecipes_list.Add(new craftingRecipe(7,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.mah_wood, 15 },
                { material.material_translation.bw_wood, 15 },
                { material.material_translation.f_bracelet, 15 },
                { material.material_translation.f_stone, 15 },
           },
           15000,//gold required
           50,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29015,//item ID in this case "Large Experience Potion"
           0//category number used in client UI
           ));
        //**************CATEGORY 1 - POTIONS*********************
        //-------15k exp potion
        craftingRecipes_list.Add(new craftingRecipe(1000,
           new Dictionary<material.material_translation, int>() {
                { material.material_translation.mliquid, 10 },
                { material.material_translation.worm, 10 },
                { material.material_translation.honey, 10 },
                { material.material_translation.snake_egg, 10 },
                { material.material_translation.spider_venom, 10 },
                { material.material_translation.bat_blood, 10 },
                { material.material_translation.blue_spore, 10 },
                { material.material_translation.red_spore, 10 },
                { material.material_translation.brain, 10 }
           },
           15000,//gold required
           20,//playerlevel required
           craftingType.profession.none,//not in use
           0,//not in use
           29041,//item ID in this case "Large Experience Potion"
           1//category number used in client UI
           ));

        //-------30k exp potion
        craftingRecipes_list.Add(new craftingRecipe(1001,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.ectoplasm, 15 },
                { material.material_translation.dk_ectoplasm, 15 },
                { material.material_translation.tentacles, 15 },
                { material.material_translation.s_water, 15 },
                { material.material_translation.b_staff, 15 },
                { material.material_translation.r_staff, 15 },
                { material.material_translation.shr_skull, 15 },
                { material.material_translation.r_mask, 20 },
                { material.material_translation.grn_mush, 20 },
                { material.material_translation.ring_pro, 20 },

         },
         30000,//gold required
         50,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29042,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------45k exp potion
        craftingRecipes_list.Add(new craftingRecipe(1002,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.leaf, 25 },
                { material.material_translation.aut_leaf, 25 },
                { material.material_translation.scorpion, 30 },
                { material.material_translation.enc_sand, 30 },
                { material.material_translation.g_tooth, 35 },
                { material.material_translation.g_horn, 35 },
                { material.material_translation.fire_orb, 40 },
                { material.material_translation.cha_bone, 40 },
         },
         45000,//gold required
         80,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29043,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------60k exp potion
        craftingRecipes_list.Add(new craftingRecipe(1003,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.dw_leather, 50 },
                { material.material_translation.ob_leather, 50 },
                { material.material_translation.re_crown, 50 },
                { material.material_translation.H_of_ice_skeleton, 50 },
         },
         60000,//gold required
         100,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29044,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------LP reset potion
        craftingRecipes_list.Add(new craftingRecipe(1004,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.mys_flyer, 25 },
                { material.material_translation.soul, 25 }
         },
         10000,//gold required
         20,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29050,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------RP reset potion
        craftingRecipes_list.Add(new craftingRecipe(1005,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.holy_coin, 2 },
                { material.material_translation.demo_coin, 2 },
                { material.material_translation.i_bracelet, 2000 },
                { material.material_translation.pur_cape, 2000 }
         },
         1000000,//gold required
         1,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29051,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------500 karma potion
        craftingRecipes_list.Add(new craftingRecipe(1006,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.zombie_blood, 25 },
                { material.material_translation.seaweed, 25 },
                { material.material_translation.soul, 25 },
                { material.material_translation.w_stone, 25 },
                { material.material_translation.crys_ball, 25 },
                { material.material_translation.red_wizard_banner, 25 },
         },
         100000,//gold required
         1,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29052,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------1000 karma potion
        craftingRecipes_list.Add(new craftingRecipe(1007,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.w_stone, 25 },
                { material.material_translation.b_necklace, 25 },
                { material.material_translation.rune, 25 },
                { material.material_translation.h_stone, 25 },
                { material.material_translation.mah_wood, 25 },
                { material.material_translation.bw_wood, 25 },
         },
         150000,//gold required
         50,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29055,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------2500 karma potion
        craftingRecipes_list.Add(new craftingRecipe(1008,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.sco_tail, 100 },
                { material.material_translation.e_stone, 100 },
                { material.material_translation.dus_tome, 25 },
                { material.material_translation.anc_axe, 25 },
         },
         250000,//gold required
         75,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29056,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //-------karma reset potion
        craftingRecipes_list.Add(new craftingRecipe(1009,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.f_bracelet, 100 },
                { material.material_translation.f_stone, 100 },
                { material.material_translation.f_shards, 2 },
                { material.material_translation.hell_core, 2 },
         },
         500000,//gold required
         100,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29053,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));

        //------- NEGATIVE 5000 karma potion
        craftingRecipes_list.Add(new craftingRecipe(1010,
         new Dictionary<material.material_translation, int>() {
                { material.material_translation.w_stone, 10 },
                { material.material_translation.b_necklace, 12 },
                { material.material_translation.rune, 5 },
                { material.material_translation.soul, 10 },
                { material.material_translation.crys_ball, 12 },
                { material.material_translation.red_wizard_banner, 5 },
         },
         25000,//gold required
         10,//playerlevel required
         craftingType.profession.none,//not in use
         0,//not in use
         29054,//item ID in this case "Large Experience Potion"
         1//category number used in client UI
         ));



        //**************CATEGORY 2 - JEWELS*********************
        //1 Failsafe tickets for 1 perfect jewel A
        craftingRecipes_list.Add(new craftingRecipe(2000,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 1 }},
            0, 0, craftingType.profession.jewelry, 0,

            3312,//this is the ItemID = result of the crafting
            2
            ));

        //2 silver tickets for 1 Failsafe jewel A
        craftingRecipes_list.Add(new craftingRecipe(2001,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 2 }},
            0, 0, craftingType.profession.jewelry, 0,

            3308,//this is the ItemID = result of the crafting
            2
            ));

        //2 Failsafe tickets for 1 perfect jewel B
        craftingRecipes_list.Add(new craftingRecipe(2002,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 2 }},
            0, 0, craftingType.profession.jewelry, 0,

            3313,//this is the ItemID = result of the crafting
            2
            ));

        //4 silver tickets for 1 Failsafe jewel B
        craftingRecipes_list.Add(new craftingRecipe(2003,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 4 }},
            0, 0, craftingType.profession.jewelry, 0,

            3309,//this is the ItemID = result of the crafting
            2
            ));

        //3 Failsafe tickets for 1 perfect jewel C
        craftingRecipes_list.Add(new craftingRecipe(2004,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 3 }},
            0, 0, craftingType.profession.jewelry, 0,

            3314,//this is the ItemID = result of the crafting
            2
            ));

        //6 silver tickets for 1 Failsafe jewel C
        craftingRecipes_list.Add(new craftingRecipe(2005,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 6 }},
            0, 0, craftingType.profession.jewelry, 0,

            3310,//this is the ItemID = result of the crafting
            2
            ));

        //4 Failsafe tickets for 1 perfect jewel D
        craftingRecipes_list.Add(new craftingRecipe(2006,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventA, 4 }},
            0, 0, craftingType.profession.jewelry, 0,

            3315,//this is the ItemID = result of the crafting
            2
            ));

        //10 silver tickets for 1 Failsafe jewel D
        craftingRecipes_list.Add(new craftingRecipe(2007,
            new Dictionary<material.material_translation, int>() {
                { material.material_translation.eventB, 10 }},
            0, 0, craftingType.profession.jewelry, 0,

            3311,//this is the ItemID = result of the crafting
            2
            ));

        //**************CATEGORY 3 - MISC*********************
        //+100k gold ingot
        craftingRecipes_list.Add(new craftingRecipe(3000,
            100000, //gold required
            0, craftingType.profession.jewelry, 0,
            29060,//this is the ItemID = result of the crafting
            3
            ));

        //+500k gold ingot
        craftingRecipes_list.Add(new craftingRecipe(3001,
            500000, //gold required
            0, craftingType.profession.jewelry, 0,
            29061,//this is the ItemID = result of the crafting
            3
            ));

        //+1M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(3002,
            1000000, //gold required
            0, craftingType.profession.jewelry, 0,
            29062,//this is the ItemID = result of the crafting
            3
            ));

        //+10M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(3004,
            10000000, //gold required
            0, craftingType.profession.jewelry, 0,
            29063,//this is the ItemID = result of the crafting
            3
            ));

        //+50M gold ingot
        craftingRecipes_list.Add(new craftingRecipe(3005,
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
