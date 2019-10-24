[System.Serializable]
public class material
{

    public enum material_translation //this is the table name (in the database), used for updating values and for better code readability
    {
        eventA,
        eventB,
        eventC,
        eventD,
        slime_core,
        mliquid,
        dirt,
        worm,
        stinger,
        honey,
        snake_venom,
        snake_egg,
        silk_web,
        spider_venom,
        fruit,
        bat_blood,
        blue_shroom,
        blue_spore,
        red_shroom,
        empty,
        red_spore,
        brain,
        zombie_blood,
        ectoplasm,
        mys_flyer,
        dk_ectoplasm,
        soul,
        tentacles,
        seaweed,
        s_water,
        w_stone,
        b_staff,
        crys_ball,
        r_staff,
        red_wizard_banner,
        wi_stone,
        shr_skull,
        b_necklace,
        r_mask,
        rune,
        grn_mush,
        h_stone,
        ring_pro,
        mah_wood,
        leaf,
        bw_wood,
        aut_leaf,
        sco_tail,
        scorpion,
        e_stone,
        enc_sand,
        dus_tome,
        g_tooth,
        anc_axe,
        g_horn,
        f_bracelet,
        fire_orb,
        f_stone,
        cha_bone,
        mag_eyeball,
        dw_leather,
        eyeball,
        ob_leather,
        i_bracelet,
        re_crown,
        pur_cape,
        H_of_ice_skeleton,
        holy_coin,
        demo_coin,
        f_shards,
        hell_core,


    }
    [UnityEngine.SerializeField]
    private int matID;//we get the ID from the enum
    [UnityEngine.SerializeField]
    private material_translation material_name;
    [UnityEngine.SerializeField]
    private int amount_held;
    public material_translation Material_name
    {
        get
        {
            return material_name;
        }

        set
        {
            material_name = value;
        }
    }
    public int Amount_held
    {
        get
        {
            return amount_held;
        }

        set
        {
            amount_held = value;
        }
    }


    #region Constructors
    public material()
    {

    }

    public material(material_translation material_name, int amount_held)
    {
        Material_name = material_name;
        Amount_held = amount_held;
        matID = (int)material_name;
    }

    #endregion
}
