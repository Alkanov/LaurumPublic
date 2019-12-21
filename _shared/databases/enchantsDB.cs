using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enchantsDB : MonoBehaviour
{

    public List<enchant> enchant_db = new List<enchant>();

    void Awake()
    {

        //Lazuli
        enchant_db.Add(new enchant(
            enchant.enchant_base.lazuli,
            new List<int> { 1, 2, 3, 4, 5 },
            new List<float[]> {
                new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
            },
            40f));
        
        //Lazuli - corrupted
        enchant_db.Add(new enchant(
            enchant.enchant_base.corrupted_lazuli,
       new List<int> { -1, -2, -3, -4, -5 },
       new List<float[]> {
                new float[3] {2f,3f,4f},
                new float[3] {3f,4f,5f},
                new float[3] {4f,5f,6f},
                 new float[3] {6f,7f,8f},
                new float[3] {9f,10f,11f}
       },
       0f));

        //Jade 
        enchant_db.Add(new enchant(
           enchant.enchant_base.jade,
           new List<int> { 6, 7, 8, 9, 10 },
           new List<float[]> {
                new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
           },
           40f));

        //Jade - corrupted
        enchant_db.Add(new enchant(
           enchant.enchant_base.corrupted_jade,
      new List<int> { -6, -7, -8, -9, -10 },
      new List<float[]> {
                new float[3] {4f,6f,8f},
                new float[3] {6f,8f,10f},
                new float[3] {8f,10f,12f},
                 new float[3] {12f,14f,16f},
                new float[3] {18f,20f,22f}
      },
      0f));

        //Sapphire
        enchant_db.Add(new enchant(
          enchant.enchant_base.saphire,
     new List<int> { 11, 12, 13, 14, 15 },
     new List<float[]> {
                new float[3] {4f,6f,8f},
                new float[3] {6f,8f,10f},
                new float[3] {8f,10f,12f},
                 new float[3] {12f,14f,16f},
                new float[3] {18f,20f,22f}
     },
     10f));

        //Sapphire - corrupted
        enchant_db.Add(new enchant(
         enchant.enchant_base.corrupted_saphire,
    new List<int> { -11, -12, -13, -14, -15 },
    new List<float[]> {
                new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
    },
    0f));



        //Siderite
        enchant_db.Add(new enchant(
         enchant.enchant_base.siderite,
    new List<int> { 16, 17, 18, 19, 20 },
    new List<float[]> {
            new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
    },
    10f));
        //Siderite - corrupted
        enchant_db.Add(new enchant(
        enchant.enchant_base.corrupted_siderite,
   new List<int> { -16, -17, -18, -19, -20 },
   new List<float[]> {
                new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
   },
   0f));

        //Gypsum
        enchant_db.Add(new enchant(
       enchant.enchant_base.gypsum,
  new List<int> { 21, 22, 23, 24, 25 },
  new List<float[]> {
              new float[3] {2f,3f,4f},
                new float[3] {3f,4f,5f},
                new float[3] {4f,5f,6f},
                 new float[3] {6f,7f,8f},
                new float[3] {9f,10f,11f}
  },
  5f));
        //Gypsum - corrupted
        enchant_db.Add(new enchant(
      enchant.enchant_base.corrupted_gypsum,
 new List<int> { -21, -22, -23, -24, -25 },
 new List<float[]> {
             new float[3] {8f,12f,16f},
                new float[3] {12f,16f,20f},
                new float[3] {16f,20f,24f},
                 new float[3] {24f,28f,32f},
                new float[3] {36f,40f,44f}
 },
 0f));


    }

    public enchant FetchEnchantBase(int id_to_search)
    {
        for (int i = 0; i < enchant_db.Count; i++)
        {
            if (enchant_db[i].IDs.Contains(id_to_search))
            {
                return enchant_db[i];
            }
        }
        return null;
    }

    public List<enchant> FetchEnchant_byChance(float max_drop_chance)
    {
        var to_return = new List<enchant>();
        for (int i = 0; i < enchant_db.Count; i++)
        {
            if (enchant_db[i].chance_to_drop >= max_drop_chance)
            {
                to_return.Add(enchant_db[i]);
            }
        }
        return to_return;

    }

}
