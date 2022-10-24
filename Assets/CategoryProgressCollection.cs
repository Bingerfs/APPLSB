using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CategoryProgressCollection : MonoBehaviour
{
    [SerializeField]
    private Transform _objectCollectionTransform;

    [SerializeField]
    private List<UserCategoryProgress> _categoriesProgress;

    [SerializeField]
    private GameObject _categoryContainerPrefab;

    [SerializeField]
    private ScrollingObjectCollection scrollObject;

    private bool requireItemsUpdate;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var category in _categoriesProgress)
        {
            var categoryContainerInstance = Instantiate(_categoryContainerPrefab, _objectCollectionTransform);
            var categoryContScript = categoryContainerInstance.GetComponent<CategoryProgressContainer>();
            categoryContScript.ExpressionsProgress = category.expressionsProgress;
            categoryContScript.CategoryName = category.categoryName;
        }

        var objectCollectionScript = _objectCollectionTransform.GetComponent<GridObjectCollection>();
        objectCollectionScript.UpdateCollection();
        requireItemsUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (requireItemsUpdate)
        {
            requireItemsUpdate = false;
            scrollObject.UpdateContent();
        }
    }
}
