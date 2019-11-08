using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ServerUniversalSettings : MonoBehaviour
{
    #region Managers
    x_ObjectHelper x_ObjectHelper;
    #endregion
    public enum var_names
    {
        none,
        f_a,
        f_b,
        f_c,
        f_d,
        f_x,
        f_m
    }

    //actual class used to store variables
    [Serializable]
    public class var_data
    {
        public var_names ID;
        public float value;
        public string description;

        public var_data(var_names var_name, float value, string description)
        {
            this.ID = var_name;
            this.value = value;
            this.description = description;
        }
    }   

    public Dictionary<var_names, var_data> dict_vars;
    //used to serialize/deserialize

    public List<var_data> global_vars;

    private void Awake()
    {
        x_ObjectHelper = GetComponent<x_ObjectHelper>();       
    }
    private void Start()
    {
        StartCoroutine(download_server_variables());
    }
    public void write_values(List<var_data> fake_list)
    {
        dict_vars = fake_list.ToDictionary(p => p.ID);
        foreach (KeyValuePair<var_names, var_data> kvp in dict_vars)
        {
            Debug.LogErrorFormat(
                "Key {0}: val:{1} desc:{2}",
                kvp.Key,
                kvp.Value.value,
                kvp.Value.description);
        }
    }

    IEnumerator download_server_variables()
    {
        UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(x_ObjectHelper.variables_repo);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError(uwr.error);
            Application.Quit();
        }
        else
        {
            try
            {
                write_values(JsonHelper.FromJson<var_data>(uwr.downloadHandler.text).ToList());
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
                Application.Quit();
                throw;
            }
        }
    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
