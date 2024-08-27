
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate List<string> PopulateOptionsAction();
public static class DataRegister
{
    public static Dictionary<string, PopulateOptionsAction> optionsList = new Dictionary<string, PopulateOptionsAction>();
    public static Dictionary<string, string> optionsListByCategory = new Dictionary<string, string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void Init()
    {
        if(optionsList == null)
        {
            optionsList = new Dictionary<string, PopulateOptionsAction>();
        }
        

        Debug.Log("Init Register");
    }

    public static void AddOption(string name, PopulateOptionsAction option)
    {
        bool result = optionsList.TryAdd(name.ToLower(), option);
    }

    public static PopulateOptionsAction GetOption(string name)
    {
        bool result = optionsList.TryGetValue(name.ToLower(), out var option);
        return result ? option : null;
    }

    public static List<string> GetFullOptions(string name)
    {
        bool result = optionsList.TryGetValue(name.ToLower(), out PopulateOptionsAction optionCall);
        List<string> fullOpts = optionCall.Invoke();

        return fullOpts;
    }

    public static void AddOptionForCategory(string category, string optionName)
    {
        bool result = optionsListByCategory.TryAdd(category.ToLower(), optionName.ToLower());

    }

    public static PopulateOptionsAction GetOptionForCategory(string category)
    {
        bool result = optionsListByCategory.TryGetValue(category.ToLower(), out string option);
        if (result)
        {
            PopulateOptionsAction optionCall = optionsList[option];
            return optionCall;
        }
        return null;
    }

    public static List<string> GetAllOptionNames()
    {
        return optionsList.Keys.ToList();
    }
}

