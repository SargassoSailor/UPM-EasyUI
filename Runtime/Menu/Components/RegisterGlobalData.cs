
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate List<string> PopulateOptionsAction();
public static class DataRegister
{
    public static Dictionary<string, PopulateOptionsAction> itemList = new Dictionary<string, PopulateOptionsAction>();
    public static Dictionary<string, string> itemListByCategory = new Dictionary<string, string>();

    public static Dictionary<string,List<string>> itemTypeList = new Dictionary<string, List<string>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void Init()
    {
        if(itemList == null)
        {
            itemList = new Dictionary<string, PopulateOptionsAction>();
        }
        

        Debug.Log("Init Register");
    }

    public static void AddData(string name, PopulateOptionsAction option)
    {
        bool result = itemList.TryAdd(name.ToLower(), option);
    }

    public static void AddTypeData(Type t, List<string> options)
    {
        bool result = itemTypeList.TryAdd(t.ToString(), options);
    }

    public static List<string> GetTypeOptions(string t)
    {
        bool result = itemTypeList.TryGetValue(t, out List<string> options);

        return options;
    }

    public static List<string> GetAllTypeOptionNames()
    {
        return itemList.Keys.ToList();
    }

    public static PopulateOptionsAction GetData(string name)
    {
        bool result = itemList.TryGetValue(name.ToLower(), out var option);
        return result ? option : null;
    }

    public static List<string> GetFullOptions(string name)
    {
        bool result = itemList.TryGetValue(name.ToLower(), out PopulateOptionsAction optionCall);
        List<string> fullOpts = optionCall.Invoke();

        return fullOpts;
    }

    public static void AddDataForCategory(string category, string optionName)
    {
        bool result = itemListByCategory.TryAdd(category.ToLower(), optionName.ToLower());

    }

    public static PopulateOptionsAction GetDataForCategory(string category)
    {
        bool result = itemListByCategory.TryGetValue(category.ToLower(), out string option);
        if (result)
        {
            PopulateOptionsAction optionCall = itemList[option];
            return optionCall;
        }
        return null;
    }

    public static List<string> GetAllOptionNames()
    {
        return itemList.Keys.ToList();
    }
}

