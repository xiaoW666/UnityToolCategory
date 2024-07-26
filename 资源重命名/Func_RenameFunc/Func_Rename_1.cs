using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace PHXH.PIXEL
{
#if UNITY_EDITOR
    public class Func_Rename_1 : EditorWindow
    {
        [Header("�����ļ���·��")]
        [HideInInspector] public string nameFolderPath;

        [Header("�������ļ���·��")]
        [HideInInspector] public string renameFolderPath;

        //��ȡ�����ļ���·��
        public void GetNameFolderPath(string _path)
        {
            nameFolderPath = _path;
        }

        //��ȡ�������ļ���·��
        public void GetRenameFolderPath(string _path)
        {
            renameFolderPath = _path;
        }

        public void Rename() 
        {
            string[] pathsName = Directory.GetFiles(nameFolderPath, "*.png");
            string[] pathsRename = Directory.GetFiles(renameFolderPath, "*.png");

            List<Texture2D> texturesName = GetAllTexture(pathsName);
            List<Texture2D> texturesRename = GetAllTexture(pathsRename);

            if (texturesName.Count >= texturesRename.Count)
            {
                for (int i = 0; i < texturesName.Count; i++)
                {
                    for (int j = 0; j < texturesRename.Count; j++)
                    {
                        if (CompareTexture(texturesName[i], texturesRename[j]) == true)
                        {         
                            string renameOldPath = pathsRename[j];

                            string renameNewPath = Path.Combine(Path.GetDirectoryName(pathsRename[j]), texturesName[i].name + ".png");
                            
                            File.Move(renameOldPath, renameNewPath);

                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < texturesRename.Count; i++)
                {
                    for (int j = 0; j < texturesName.Count; j++)
                    {
                        if (CompareTexture(texturesRename[i], texturesName[j]) == true)
                        {
                            string renameOldPath = pathsRename[i];

                            string renameNewPath = Path.Combine(Path.GetDirectoryName(pathsRename[i]), texturesName[j].name + ".png");

                            File.Move(renameOldPath, renameNewPath);

                            break;
                        }
                    }
                }
            }
        }

        //��ȡָ��·��������PNGͼƬ
        private List<Texture2D> GetAllTexture(string[] _paths) 
        {
            List<string> filePath = new List<string>();
            List<Texture2D> textures = new List<Texture2D>();

            for (int i = 0; i < _paths.Length; i++)
            {
                filePath.Add(_paths[i]);
            }

            foreach (string path in filePath)
            {
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(1, 1);

                string[] name = path.Split('\\');
                string[] name2 = name[1].Split('.');
                texture.name = name2[0];

                texture.LoadImage(fileData);
                textures.Add(texture);
            }

            return textures;
        }

        //���ضԱ�
        private bool CompareTexture(Texture2D _first, Texture2D _second)
        {
            Color[] firstPix = _first.GetPixels();
            Color[] secondPix = _second.GetPixels();
            if (firstPix.Length != secondPix.Length)
            {
                return false;
            }
            for (int i = 0; i < firstPix.Length; i++)
            {
                if (firstPix[i] != secondPix[i])
                {
                    return false;
                }
            }

            return true;
        }

        #region GUI
        [MenuItem("PIXEL/Func_Rename/Type_1_������")]
        private static void GetWindow()
        {
            Rect rect = new Rect(0, 0, 450, 550);
            GetWindowWithRect<Func_Rename_1>(rect, false, "������������", false);
        }

        private string finalTip = "";
        private Color finalColor = Color.white;

        public void OnGUI()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty nameFolderPath = serializedObject.FindProperty("nameFolderPath");
            SerializedProperty renameFolderPath = serializedObject.FindProperty("renameFolderPath");

            EditorGUILayout.PropertyField(nameFolderPath, true);

            if (nameFolderPath.stringValue == string.Empty)
            {
                EditorGUILayout.HelpBox("�����ļ��в���Ϊ��", MessageType.Error);
            }

            if (GUILayout.Button("ѡ�������ļ���"))
            {
                GetNameFolderPath(EditorUtility.OpenFolderPanel("�ļ���Դ������",
                                                                Application.dataPath,
                                                                "�����ļ���"));
            }

            EditorGUILayout.Space(19);
            EditorGUILayout.PropertyField(renameFolderPath, true);

            if (renameFolderPath.stringValue == string.Empty)
            {
                EditorGUILayout.HelpBox("�������ļ��в���Ϊ��", MessageType.Error);
            }

            if (GUILayout.Button("ѡ���������ļ���"))
            {
                GetRenameFolderPath(EditorUtility.OpenFolderPanel("�ļ���Դ������",
                                                                Application.dataPath,
                                                                "�������ļ���"));
            }

            EditorGUILayout.Space(19);
            EditorGUI.BeginDisabledGroup(nameFolderPath.stringValue == string.Empty || renameFolderPath.stringValue == string.Empty);

            List<string> temp = new List<string>();

            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontStyle = FontStyle.Bold;
            gUIStyle.fontSize = 11;
            gUIStyle.alignment = TextAnchor.UpperCenter;
            gUIStyle.normal.textColor = finalColor;

            GUILayout.Space(29);
            if (GUILayout.Button("---��ʼ������---"))
            {
                Rename();

                finalColor = Color.green;
                finalTip = "�����";
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.Space(9);
            EditorGUILayout.LabelField(finalTip, gUIStyle);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
#endif
}