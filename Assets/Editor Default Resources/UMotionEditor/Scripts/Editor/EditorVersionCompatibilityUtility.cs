using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Compilation;
#if UNITY_2018_3_OR_NEWER

#endif

namespace UMotionEditor
{
	public static class EditorVersionCompatibilityUtility
	{
		//********************************************************************************
		// Public Properties
		//********************************************************************************

		//********************************************************************************
		// Private Properties
		//********************************************************************************
		
		//----------------------
		// Inspector
		//----------------------
		
		//----------------------
		// Internal
		//----------------------

		//********************************************************************************
		// Public Methods
		//********************************************************************************

        public static bool IsModelPrefab(GameObject gameObject)
        {
            #if UNITY_2018_3_OR_NEWER
            return (PrefabUtility.GetPrefabAssetType(gameObject) == PrefabAssetType.Model);
            #else
            return (PrefabUtility.GetPrefabType(gameObject) == PrefabType.ModelPrefab);
            #endif
        }

        public static bool IsPrefab(GameObject gameObject)
        {
            #if UNITY_2018_3_OR_NEWER
            return (PrefabUtility.GetPrefabAssetType(gameObject) != PrefabAssetType.NotAPrefab);
            #else
            return (PrefabUtility.GetPrefabType(gameObject) != PrefabType.None);
            #endif
        }

		public static bool IsInPrefabStage()
		{
			#if UNITY_2018_3_OR_NEWER
			return (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null);
			#else
			return false;
			#endif
		}

		//********************************************************************************
		// Private Methods
		//********************************************************************************
		
	}
}
