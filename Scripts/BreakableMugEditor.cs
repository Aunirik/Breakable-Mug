using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Aunirik.BreakableMug
{
    [CustomEditor(typeof(BreakableMug))]
    [CanEditMultipleObjects]
    public class BreakableMugEditor : Editor
    {
        private const int ImprintHeight = 366;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Separator();

            if (GUILayout.Button("Load Imprint"))
            {
                string loadPath = EditorUtility.OpenFilePanel("Breakable Mug Imprint", "", "");

                if (!File.Exists(loadPath))
                {
                    Debug.LogErrorFormat("BreakableMugEditor: Path doesn't exist\n{0}", loadPath);
                    return;
                }

                Renderer renderer = targets[0].GetComponent<Renderer>();

                if (renderer == null)
                {
                    Debug.LogErrorFormat("BreakableMugEditor: Missing renderer\n{0}", targets[0]);
                    return;
                }

                try
                {
                    Texture2D imprint = CalculateNewImprint(loadPath);

                    string savePath = AssetDatabase.GenerateUniqueAssetPath("Assets/BreakableMug_Custom_Texture.png");
                    File.WriteAllBytes(savePath, imprint.EncodeToPNG());
                    AssetDatabase.ImportAsset(savePath);

                    Undo.RecordObject(renderer.sharedMaterial, "Changing main texture of BreakableMug's material");
                    renderer.sharedMaterial.mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("BreakableMugEditor: Error\n{0}", e);
                }
            }
        }

        private Texture2D CalculateNewImprint(string path)
        {
            Texture2D texture = new Texture2D(1024, 1024);
            texture.LoadImage(File.ReadAllBytes(path));

            int scaledWidth = Mathf.Min(texture.width * ImprintHeight / texture.height, 1024);
            Color[] pixels = new Color[ImprintHeight * scaledWidth];

            for (int y = 0; y < ImprintHeight; y++)
            {
                for (int x = 0; x < scaledWidth; x++)
                {
                    pixels[y * scaledWidth + x] = texture.GetPixelBilinear(x / (scaledWidth - 1.0f), y / (ImprintHeight - 1.0f));
                }
            }

            Texture2D imprint = new Texture2D(1024, 1024);
            imprint.SetPixels(Mathf.Max(512 - scaledWidth / 2, 0), 657, scaledWidth, ImprintHeight, pixels);
            imprint.Apply();

            return imprint;
        }
    }
}
