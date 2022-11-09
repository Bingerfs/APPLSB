using Assets.DataPersistence.Models;
using Assets.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.DataPersistence
{
    public class ModuleDataManager : MonoBehaviour
    {
        [SerializeField]
        private UserPreferences _userPreferences;

        public static ModuleDataManager Instance { get; set; }

        private static readonly string RESOURCES_DIR = Path.Combine("Assets", "Resources");

        private static readonly string CONFIG_DIR = Path.Combine("DataBase", "config");

        private static readonly string DATA_DIR = Path.Combine(RESOURCES_DIR, "DataBase", "data");

        private static readonly string MODULES_CONFIG_FILENAME = "data";

        private static readonly string CATEGORY_CONFIG_FILENAME = "categories";

        private static readonly string ALL_SIGN_CODES_FILENAME = "Codes";

        private IDictionary<int, IDictionary<string, CategoryData>> _moduleDictionary;

        private IDataHandler<List<ModuleDataModel>> _fileDataHandlerModules;

        private IDataHandler<List<CategoryCodeFileModel>> _fileDataHandlerCategories;

        private List<string> _allWordCodes;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There can only be one instance of the Module Data Manager.");
            }

            Instance = this;
        }

        void Start()
        {
            _fileDataHandlerModules = new AssetsDataHandler<List<ModuleDataModel>>(CONFIG_DIR, MODULES_CONFIG_FILENAME);
            _fileDataHandlerCategories = new AssetsDataHandler<List<CategoryCodeFileModel>>(CONFIG_DIR, CATEGORY_CONFIG_FILENAME);
            _moduleDictionary = new Dictionary<int, IDictionary<string, CategoryData>>();
            LoadAllData();
        }

        void Update()
        {

        }

        private void LoadAllData()
        {
            TextAsset readCodes = Resources.Load<TextAsset>(ALL_SIGN_CODES_FILENAME);
            char[] delimiters = new char[] { '\r', '\n' };
            _allWordCodes = readCodes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
            var modules = _fileDataHandlerModules.Load();
            var categories = _fileDataHandlerCategories.Load();
            foreach (var module in modules)
            {
                int moduleNumber = int.Parse(module.Name.Split(' ')[1]);
                var moduleCategories = categories.Where(category => module.Categories.Any(c => c.Name.Equals(category.Name)));
                foreach (var category in module.Categories)
                {
                    IDictionary<string, CategoryData> auxDict = null;
                    if (!_moduleDictionary.TryGetValue(moduleNumber, out auxDict))
                    {
                        auxDict = new Dictionary<string, CategoryData>();
                        _moduleDictionary.Add(moduleNumber, auxDict);
                    }

                    var categoryData = new CategoryData();
                    categoryData.Name = category.Name;
                    categoryData.SetCode = category.Set;
                    categoryData.Code = moduleCategories.First(c => c.Name.Equals(category.Name)).Code;
                    var categoryExpression = _allWordCodes.Where(c => c.Contains(categoryData.IdentifierCode));
                    categoryData.Expressions = categoryExpression.Select(ce => {
                        var splitCode = ce.Split('#');
                        var expressionCode = splitCode[1].Remove(0, ($"{categoryData.Code}{categoryData.SetCode}").Length);
                        var expressionData = new ExpressionData { Expression = splitCode[0], CategoryCode = $"{categoryData.Code}{categoryData.SetCode}", ExpressionCode = expressionCode };
                        return expressionData;
                    }).ToList();
                    auxDict.Add(categoryData.IdentifierCode, categoryData);
                }
            }
        }

        public List<ExpressionData> GetExpressionsByModule(int moduleNumber)
        {
            List<ExpressionData> moduleExpressions = new List<ExpressionData>();
            IDictionary<string, CategoryData> categories = null;
            if (_moduleDictionary.TryGetValue(moduleNumber, out categories))
            {
                foreach (var category in categories)
                {
                    moduleExpressions.AddRange(category.Value.Expressions);
                }
            }

            return moduleExpressions;
        }

        public IDictionary<int, IDictionary<string, CategoryData>> GetModules()
        {
            return _moduleDictionary;
        }

        public string GetWordByCode(string code)
        {
            var wordCode = _allWordCodes.FirstOrDefault(w => w.Contains(code));
            var splitWordCode = wordCode.Split('#');
            return splitWordCode[0];
        }
    }
}