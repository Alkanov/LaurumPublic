using UnityEngine;
[System.Serializable]
public class Task
{   
    public enum task_types
    {
        pve_kill_x_y_times,
        area_explored,
        arena_pvp_kill,
        pvp_kill,
        ds_pve_kill,
        ds_pvp_kill,
        ds_enter,
        ds_wave,
        material_collection,//enum to int of the material.material_translation
        crafted_item,
        online_time
                           // ui_action//todo--> for tutorials
    }
    public int task_id;
    public task_types task_type;
    public int[] objectives_meta_data;
    public int required_amount;

    
    /// <summary>
    /// Full configurations
    /// </summary>
    public Task(int task_id, task_types task_type, int[] objectives_meta_data, int required_amount)
    {
        this.task_id = task_id;
        this.task_type = task_type;
        /// <summary>
        /// [0] for monster ID and future expansions
        /// </summary>
        this.objectives_meta_data = objectives_meta_data;
        this.required_amount = required_amount;
    }
}