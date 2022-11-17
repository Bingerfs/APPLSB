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

        private List<CategoryCodeFileModel> _categories;

        private Dictionary<string, CategoryData> _categoryDictionary;

        private List<List<CategorySetData>> _modulesList;

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
            _categoryDictionary = new Dictionary<string, CategoryData>();
            _modulesList = new List<List<CategorySetData>>();
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
            var categorySets = modules.Aggregate(new List<CategoryFileModel> { }, (current, next) => { current.AddRange(next.Categories); return current; }).ToList();
            foreach (var category in categories)
            {
                var filteredCategorySets = categorySets.Where(cs => cs.Name.Equals(category.Name));
                var categoryData = new CategoryData(category.Name, category.Code);
                foreach (var set in filteredCategorySets)
                {
                    categoryData.AddSet(set.Set);
                    var categorySetExpressions = _allWordCodes.Where(c => c.Contains($"#{categoryData.Code}{set.Set}")).ToList();
                    var expressionsData = categorySetExpressions.Select(exp => {
                        var splitExpressionTexts = exp.Split('#');
                        var categorySetCode = $"{categoryData.Code}{set.Set}";
                        var expressionCode = exp.Substring(exp.IndexOf(categorySetCode) + categorySetCode.Length);
                        return new ExpressionData(categoryData.Code, set.Set, expressionCode, splitExpressionTexts[0]);
                    }).ToList();
                    categoryData.AddExpressionsToSet(expressionsData, set.Set);
                }

                _categoryDictionary.Add(categoryData.Code, categoryData);
            }

            foreach (var module in modules)
            {
                var listOfCategorySets = module.Categories.Select(c => new CategorySetData(GetCategoryCodeByName(c.Name), c.Set)).ToList();
                _modulesList.Add(listOfCategorySets);
            }

            /*foreach (var module in modules)
            {
                int moduleNumber = int.Parse(module.Name.Split(' ')[1]);
                var moduleCategories = _categories.Where(category => module.Categories.Any(c => c.Name.Equals(category.Name)));
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
            }*/
        }

        public string GetCategoryCodeByName(string categoryName)
        {
            string code = null;
            foreach (var categoryValuePair in _categoryDictionary)
            {
                if (categoryValuePair.Value.Name.Equals(categoryName))
                {
                    code = categoryValuePair.Key;
                    break;
                }
            }

            return code;
        }

        public List<ExpressionData> GetExpressionsByModule(int moduleNumber)
        {
            List<ExpressionData> expressions = new List<ExpressionData>();
            if (moduleNumber > 0)
            {
                var categorySets = _modulesList[moduleNumber - 1];
                foreach (var categorySet in categorySets)
                {
                    if (_categoryDictionary.ContainsKey(categorySet.CategoryCode))
                    {
                        var category = _categoryDictionary[categorySet.CategoryCode];
                        if (category.DictionaryOfSets.ContainsKey(categorySet.SetCode))
                        {
                            var foundExpressions = category.DictionaryOfSets[categorySet.SetCode];
                            expressions.AddRange(foundExpressions.Values);
                        }
                    }
                }
            }

            return expressions;
        }

        public CategoryData GetCategoryByCode(string categoryCode)
        {
            categoryCode = categoryCode.Contains('#') ? categoryCode : $"#{categoryCode}";
            CategoryData categoryData = null;
            if (_categoryDictionary.ContainsKey(categoryCode))
            {
                categoryData = _categoryDictionary[categoryCode];
            }

            return categoryData;
        } 
    }
}