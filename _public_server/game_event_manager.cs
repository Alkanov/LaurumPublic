using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class game_event_manager : MonoBehaviour
{
    [System.Serializable]
    public class event_data_class
    {
        public game_event game_event;
        public float event_multiplier;
        public DateTime event_expire;
        public bool active;

        public event_data_class(game_event game_event, float event_multiplier, DateTime event_expire)
        {
            this.game_event = game_event;
            this.event_multiplier = event_multiplier;
            this.event_expire = event_expire;
        }
        public event_data_class()
        {

        }
    }

    public enum game_event
    {
        none,
        grind_exp,//multiplier
        quest_exp,//multiplier
        grind_gold,//multiplier
        quest_gold,//multiplier
        ds_exp,//multiplier
        ds_gold,//multiplier
        extra_hp,//multiplier
        extra_mp,//multiplier
        gems_sale,//multiplier
        ticket_gold,//1=on 0 =off
        ticket_silver,//1=on 0 =off
        guild_wars,//1=on 0=off 2=only used to turn on event on load_events as it has to be >0 to be added to active_events
        cheap_aug_reroll//price of re-roll is always multiplied by this variable
    }
    #region Managers
    public x_ObjectHelper x_ObjectHelper;
    #endregion

    #region events
    public List<event_data_class> active_events = new List<event_data_class>();
    #endregion

    #region data
    public List<event_data_class> temp_event_values = new List<event_data_class>();//keep the temp to compare it later and know if clients need update
    List<event_data_class> temp_values = new List<event_data_class>();
    #endregion

    private void Awake()
    {
        x_ObjectHelper = GetComponent<x_ObjectHelper>();
    }
    private void Start()
    {
        load_events_now();
        StartCoroutine(event_change_detection());
    }

    IEnumerator load_events(int times)
    {
        yield return new WaitForSeconds(10f);
        string url = x_ObjectHelper.ServerDBHandler.load_all_events();
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (times <= 5)
        {
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                yield return new WaitForSeconds(5f);
                int now = times + 1;
                StartCoroutine(load_events(now));
            }
            else
            {
                active_events = new List<event_data_class>();
                //comes like this
                //event_string@float@date,event_string@float@date,event_string@float@date
                string[] events = uwr.downloadHandler.text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                bool crash = false;
                for (int i = 0; i < events.Length; i++)
                {
                    //event_string@float@date
                    string[] event_data = events[i].Split('@');
                    game_event event_type;
                    if (Enum.TryParse(event_data[0], out event_type))
                    {
                        float this_event_multiplier;
                        if (float.TryParse(event_data[1], out this_event_multiplier))
                        {
                            if (this_event_multiplier > 0f)
                            {
                                DateTime this_event_expiration_date;
                                if (DateTime.TryParse(event_data[2], out this_event_expiration_date))
                                {
                                    active_events.Add(new event_data_class(event_type, this_event_multiplier, this_event_expiration_date));
                                    if (DateTime.UtcNow <= this_event_expiration_date)
                                    {
                                        if (event_type == game_event.guild_wars)
                                        {
                                            x_ObjectHelper.IRC_demo.submitData(string.Format("Event:{0} Mult:{1}", event_type.ToString(), this_event_multiplier));
                                        }
                                        else
                                        {
                                            x_ObjectHelper.IRC_demo.submitData(string.Format("Event:{0} Mult:{1} Expires:{2}", event_type.ToString(), this_event_multiplier, this_event_expiration_date.ToString()));
                                        }
                                    }
                                }
                                else
                                {
                                    crash = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            crash = true;
                            break;
                        }
                    }
                    else
                    {
                        crash = true;
                        break;
                    }
                }
                if (crash)
                {
                    x_ObjectHelper.IRC_demo.submitData(string.Format("Shutting down - error parsing event data:{0}", uwr.downloadHandler.text));
                    x_ObjectHelper.PlayersConnected.shutdown(false);
                }
            }
        }
        else
        {
            x_ObjectHelper.IRC_demo.submitData(string.Format("Shutting down - cant get event data"));
            x_ObjectHelper.PlayersConnected.shutdown(false);
        }
        //start event timers
        StartCoroutine(event_timer());
    }

    private void update_events_on_all_clients()
    {
        //FIX: there is still something fucked up here. Server is sending this every second instead of only when there is a change. Re-work it.
        temp_values.Clear();
        for (int i = 0; i < active_events.Count; i++)
        {
            temp_values.Add(active_events[i]);
        }
        //if something changed send it to all clients
        if (temp_event_values != temp_values)
        {
            //update the temp list
            temp_event_values.Clear();
            for (int i = 0; i < active_events.Count; i++)
            {
                temp_event_values.Add(active_events[i]);
            }
            //update events on all connected clients
            for (int i = 0; i < x_ObjectHelper.PlayersConnected.allPlayers.Count; i++)
            {
                x_ObjectHelper.PlayersConnected.allPlayers[i].GetComponent<PlayerGeneral>().send_server_events_to_client();
            }
        }

    }

    #region event timers
    IEnumerator event_timer()
    {
        if (gvg_time(DateTime.UtcNow.Hour) == x_ObjectHelper.use_even_time)
        {
            if (is_event_on(game_event.guild_wars, true) == 0f || is_event_on(game_event.guild_wars, true) == 2f)//was off or this is the first start
            {
                //activate event
                for (int i = 0; i < active_events.Count; i++)
                {
                    if (active_events[i].game_event == game_event.guild_wars)
                    {
                        //change ending time
                        active_events[i].event_multiplier = 1f;
                        active_events[i].event_expire = DateTime.UtcNow.AddHours(1);
                        //announce
                        x_ObjectHelper.IRC_demo.submitChat(string.Format("A War is brewing for control of PvP Territories. GvG is now Karma Free for 1 hour"));
                    }
                }
            }
        }
        else
        {
            if (is_event_on(game_event.guild_wars, true) != 0f)//anything but off (we can use any other number to turn event off here
            {
                //deactivate event
                for (int i = 0; i < active_events.Count; i++)
                {
                    if (active_events[i].game_event == game_event.guild_wars)
                    {
                        //announce it - only if we came from active event and not from initial (2=on >2=disabled) state
                        if (active_events[i].event_multiplier == 1f)
                        {
                            x_ObjectHelper.IRC_demo.submitChat(string.Format("The war has settled and the fighting has cooled. PvP areas are no longer contested. Karma is now active again"));
                        }
                        //change ending time
                        active_events[i].event_multiplier = 0;

                    }
                }
            }
        }
        yield return new WaitForSeconds(30f);
        StartCoroutine(event_timer());
    }
    IEnumerator event_change_detection()
    {
        yield return new WaitForSeconds(1f);
        if (active_events.Count > 0)//if events are loaded
        {
            update_events_on_all_clients();
        }
        StartCoroutine(event_change_detection());
    }
    #endregion

    #region Utility
    bool gvg_time(int value)
    {
        //even
        return value % 2 == 0;
    }
    public void load_events_now()
    {
        try
        {
            StartCoroutine(load_events(0));
        }
        catch (Exception ex)
        {
            x_ObjectHelper.IRC_demo.submitData(string.Format("Shutting down - exception getting event data"));
            x_ObjectHelper.IRC_demo.submitData(ex.ToString());
            x_ObjectHelper.PlayersConnected.shutdown(false);
        }

    }
    public float is_event_on(game_event game_event)
    {
        return is_event_on(game_event, false);
    }
    public float is_event_on(game_event game_event, bool ignore_date)
    {
        for (int i = 0; i < active_events.Count; i++)
        {
            if (active_events[i].game_event == game_event)
            {
                if (ignore_date)
                {
                    return active_events[i].event_multiplier;
                }
                else
                {
                    if (DateTime.UtcNow < active_events[i].event_expire)
                    {
                        active_events[i].active = true;
                        return active_events[i].event_multiplier;
                    }
                    else
                    {
                        active_events[i].active = false;
                    }
                }
            }
        }
        if (game_event == game_event.ticket_gold || game_event == game_event.ticket_silver || game_event == game_event.guild_wars)
        {
            //return 0 because 0 = off, so we cant return 1 as always
            //return 0 on guild_wars in cause its not found in the list
            return 0f;
        }
        return 1f;
    }
    #endregion

}
