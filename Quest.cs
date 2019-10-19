using System.Collections.Generic;
[System.Serializable]
public class Quest
{
    [System.Serializable]
    public class QuestConditions
    {
        //used for triggering 
        public enum conditions_to_trigger
        {
            none,
            finish_before_in_minutes,
            fail_on_death,
            ds_dont_fail_any_ds_wave,
            ds_dont_contact_poison
        }

        public bool any_condition_used;
        public float finish_before_in_minutes;
        public bool fail_on_death;
        public bool ds_dont_fail_any_ds_wave;
        public bool ds_dont_contact_poison;

        public QuestConditions()
        {

        }
        public QuestConditions(float finish_before_in_minutes, bool fail_on_death, bool ds_dont_fail_any_ds_wave, bool ds_dont_contact_poison)
        {
            Finish_before_in_minutes = finish_before_in_minutes;
            Fail_on_death = fail_on_death;
            Ds_dont_fail_any_ds_wave = ds_dont_fail_any_ds_wave;
            Ds_dont_contact_poison = ds_dont_contact_poison;
        }

        public float Finish_before_in_minutes
        {
            get
            {
                return finish_before_in_minutes;
            }

            set
            {
                finish_before_in_minutes = value;
                if (finish_before_in_minutes != -1)
                {
                    any_condition_used = true;
                }
            }
        }
        public bool Fail_on_death
        {
            get
            {
                return fail_on_death;
            }

            set
            {
                fail_on_death = value;
                if (fail_on_death)
                {
                    any_condition_used = true;
                }
            }
        }
        public bool Ds_dont_fail_any_ds_wave
        {
            get
            {
                return ds_dont_fail_any_ds_wave;
            }

            set
            {
                ds_dont_fail_any_ds_wave = value;
                if (ds_dont_fail_any_ds_wave)
                {
                    any_condition_used = true;
                }
            }
        }
        public bool Ds_dont_contact_poison
        {
            get
            {
                return ds_dont_contact_poison;
            }

            set
            {
                ds_dont_contact_poison = value;
                if (ds_dont_contact_poison)
                {
                    any_condition_used = true;
                }
            }
        }
    }

    //V2.0
    public enum quest_types
    {
        alone_quest,
        party_allowed_quest
        //guild_party_only_quest//,
        //pk_party_quest
        //hero_party_quest
    }

    public int _quest_ID;
    //configurations
    public int given_by_this_NPC_ID;
    public quest_types quest_type;
    public bool allow_pet_quest;//can quest pet see this quest remotely?
    //--->back log scan required   
    public float repeat_every_in_minutes;
    public int quest_id_completed_required;
    //--->others    
    public QuestConditions _QuestConditions = new QuestConditions();
    //requisites
    public int min_level_required;
    public int max_level_allowed;
    #region other future requisists
    //karma requirements
    //guild level requirements
    //stats requirements
    //max deaths (for achievements) requirements.. etc
    #endregion
    //awards
    public int exp_awarded;
    public int gold_awarded;
    public List<int> crates_IDs_awarded;
    public int title_awarded;
    //tasks
    public List<Task> _tasks_to_complete;
    //meta - used to carry a bit more data like when the quest will be available (in case of repeatable quests) and others
    public int[] meta_data = new int[] { 0 };//[0] = 0 means available | [0] != 0 is the UTC unixTimestamp when the quest will be available

    public Quest()
    {

    }

    /// <summary>
    /// Fully customizablequest
    /// </summary>
    public Quest(int quest_ID, int given_by_this_NPC_ID, quest_types quest_type, bool allow_pet_quest, float repeat_every_in_minutes, int quest_id_completed_required, QuestConditions QuestConditions, int min_level_required, int max_level_allowed, int exp_awarded, int gold_awarded, List<int> crates_IDs_awarded, int title_awarded, List<Task> tasks_to_complete)
    {
        _quest_ID = quest_ID;
        this.given_by_this_NPC_ID = given_by_this_NPC_ID;
        this.quest_type = quest_type;
        this.allow_pet_quest = allow_pet_quest;
        this.repeat_every_in_minutes = repeat_every_in_minutes;
        this.quest_id_completed_required = quest_id_completed_required;
        _QuestConditions = QuestConditions;
        this.min_level_required = min_level_required;
        this.max_level_allowed = max_level_allowed;
        this.exp_awarded = exp_awarded;
        this.gold_awarded = gold_awarded;
        this.crates_IDs_awarded = crates_IDs_awarded;
        this.title_awarded = title_awarded;
        _tasks_to_complete = tasks_to_complete;
    }


}