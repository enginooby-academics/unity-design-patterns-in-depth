#if GAIA_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Digger;

namespace Gaia.GX.ProceduralWorlds
{
    public class DiggerGaiaIntegration : MonoBehaviour
    {
        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Amandine Entertainment";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Digger PRO";
        }

        #endregion

        #region Methods exposed by Gaia as buttons must be prefixed with GX_

        /// <summary>
        /// Says what the product is about
        /// </summary>
        public static void GX_About()
        {
            EditorUtility.DisplayDialog("Digger PRO Integration", "Digger PRO is a simple yet powerful tool to create natural caves and overhangs in your Unity terrains directly from the Unity editor. Digger PRO supports runtime editing.", "OK");
        }

        /// <summary>
        /// Setup Terrain
        /// </summary>
        public static void GX_Terrain_SetupTerrain()
        {
            DiggerMasterEditor.SetupTerrains();
        }

        #region DiggerPRO

        /// <summary>
        /// setup Digger for runtime
        /// </summary>
        public static void GX_Terrain_SetupForRuntime()
        {
            DiggerMasterEditor.SetupRuntimeScripts();
        }

        #endregion

        /// <summary>
        /// Remove Digger from current scene
        /// </summary>
        public static void GX_Terrain_RemoveDiggerFromScene()
        {
            DiggerMasterEditor.RemoveDiggerFromTerrains();
        }

        #endregion
    }
}
#endif