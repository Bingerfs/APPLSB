using Assets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class EvaluationController : MonoBehaviour
{
    static string UNITY_RESOURCES_FOLDER = Path.Combine("Assets", "Resources");
    static string CATEGORY_PATH = Path.Combine(UNITY_RESOURCES_FOLDER, "DataBase", "config", "categories.json");
    static string CONFIG_PATH = Path.Combine(UNITY_RESOURCES_FOLDER, "DataBase", "config", "data.json");
    static string NAME_ANIMATOR_CONTROLLER = "main.controller";
    static string CODE_INDICATOR = "#";
    static string ANIMATIONS_FOLDER_PATH = @"Animations";
    static string CODE_SEPARATOR_CLIP = "_";
    static string NAME_CODES_FILE = "Codes.txt";
    public int SelectedModule { get; set; }

    private IEnumerable<string> _randomisedBaseSignCodes;

    void Start()
    {
        using (StreamReader reader = new StreamReader(CONFIG_PATH))
        {
            var readData = reader.ReadToEnd();
            var listOfModules = JsonConvert.DeserializeObject<List<Module>>(readData);
            var modulesDictionary = new Dictionary<Module, IEnumerable<string>>();
            foreach (var module in listOfModules)
            {
                modulesDictionary.Add(module, GetAllModuleCategories(module));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerable<string> GetAllModuleCategories(Module module)
    {
        IEnumerable<string> categories = new List<string>();
        using (StreamReader reader = new StreamReader(CATEGORY_PATH))
        {
            var readData = reader.ReadToEnd();
            var listOfCategories = JsonConvert.DeserializeObject<IEnumerable<Category>>(readData);
            categories = listOfCategories.Where(category => module.Categories.Any(category2 => category.Name.Equals(category2.Name))).Select(category => $"{category.Code}{1}");
            var size = categories.Count();
        }

        return categories;
    }
}
