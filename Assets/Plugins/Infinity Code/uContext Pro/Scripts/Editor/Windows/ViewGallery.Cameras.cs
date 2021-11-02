/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class ViewGalleryCameras
    {
        static ViewGalleryCameras()
        {
            ViewGallery.OnCamerasMenuItem += OnCamerasMenuItem;
            ViewGallery.OnDrawCameras += OnDrawCameras;
            ViewGallery.OnGetCameras += OnGetCameras;
        }

        private static void OnCamerasMenuItem(ViewGallery gallery)
        {
            if (GUILayoutUtils.ToolbarButton("Cameras"))
            {
                GenericMenuEx menu = GenericMenuEx.Start();

                menu.Add("Remove All Temporary Cameras", RemoveAllTemporaryCameras, gallery);

                foreach (ViewGallery.CameraStateItem cam in gallery.cameras.Where(c => c.camera.GetComponentInParent<TemporaryContainer>() != null))
                {
                    menu.Add("Remove " + cam.camera.gameObject.name, RemoveTemporaryCamera, cam);
                }

                menu.ShowAsContext();
            }
        }

        private static void OnDrawCameras(ViewGallery gallery, float rowHeight, float maxLabelWidth, ref int offsetY, ref int row)
        {
            GUI.Label(new Rect(new Vector2(gallery.offsetX, offsetY), new Vector2(gallery.lastSize.x, 20)), "Cameras:", EditorStyles.boldLabel);

            offsetY += 20;
            ViewGallery.CameraStateItem[] cameras = gallery.cameras;
            for (int i = 0; i < cameras.Length; i++)
            {
                int col = i % gallery.countCols;

                float x = col * gallery.itemWidth + (col + 1) * gallery.offsetX;
                Rect rect = new Rect(x, row * rowHeight + offsetY, gallery.itemWidth, gallery.itemHeight);
                ViewGallery.CameraStateItem cameraState = cameras[i];

                if (cameraState.Draw(rect, maxLabelWidth)) cameraState.Set();

                if (i != cameras.Length - 1 && col == gallery.countCols - 1) row++;
            }

            row++;
            offsetY += 5;
        }

        private static ViewGallery.CameraStateItem[] OnGetCameras()
        {
            return Object.FindObjectsOfType<Camera>().OrderBy(c => c.name).Select(c => new ViewGallery.CameraStateItem(c)).ToArray();
        }

        private static void RemoveAllTemporaryCameras(object obj)
        {
            ViewGallery gallery = obj as ViewGallery;
            if (gallery == null) return;

            if (!EditorUtility.DisplayDialog(
                "Confirmation",
                "Are you sure you want to remove all temporary cameras?",
                "Remove", "Cancel")) return;

            Camera[] tempCameras = gallery.cameras.Select(c => c.camera).Where(c => c.GetComponentInParent<TemporaryContainer>() != null).ToArray();

            for (int i = 0; i < tempCameras.Length; i++) Object.DestroyImmediate(tempCameras[i].gameObject);
            ViewGallery.isDirty = true;
        }

        private static void RemoveTemporaryCamera(object userData)
        {
            GameObject go = (userData as Camera).gameObject;

            if (!EditorUtility.DisplayDialog(
                "Confirmation",
                "Are you sure you want to remove " + go.name + " camera?",
                "Remove", "Cancel")) return;

            Object.DestroyImmediate(go);
            ViewGallery.isDirty = true;
        }
    }
}