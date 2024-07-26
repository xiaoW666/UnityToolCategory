using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace PHXH.PIXEL
{
#if UNITY_EDITOR
    public class Func_Rename_0 : EditorWindow
    {
        [Header("文件夹路径")]
        [HideInInspector] public string folderPath;

        [Header("前缀")]
        [HideInInspector] public string preName;

        [Header("二级名称")]
        [HideInInspector] public bool haveSecondName;
        [HideInInspector] public string secondName;

        [Header("三级名称")]
        [HideInInspector] public bool haveThirdName;
        [HideInInspector] public string thirdName;

        [Header("序号")]
        [HideInInspector] public int serialNamuber;

        [Header("连接符号")]
        [HideInInspector] public bool needSetNewSymbol;
        [HideInInspector] public string symbol = "_";

        //重命名PNG文件
        public List<string> Name()
        {
            string[] pngFiles = Directory.GetFiles(folderPath, "*.png");
            List<string> wrongFiles = new List<string>();
            int order = serialNamuber;
            string temp = "";

            foreach (string oldPath in pngFiles)
            {
                string pngName = preName +
                                 (temp = haveSecondName ? symbol : null) +
                                 (secondName = haveSecondName ? secondName : null) +
                                 (temp = haveThirdName ? symbol : null) +
                                 (thirdName = haveThirdName ? thirdName : null) + symbol +
                                  order;

                string newName = pngName + Path.GetExtension(oldPath);
                string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);

                if (File.Exists(newPath))
                {
                    wrongFiles.Add(newName);
                    order += 1;
                    continue;
                }

                File.Move(oldPath, newPath);

                order += 1;
            }

            return wrongFiles;
        }

        //获取文件夹路径
        public void GetFolderPath(string _path)
        {
            folderPath = _path;
        }

        [MenuItem("PIXEL/Func_Rename/Type_0_新命名")]
        private static void GetWindow()
        {
            Rect rect = new Rect(0, 0, 450, 550);
            GetWindowWithRect<Func_Rename_0>(rect, false, "命名管理器", false);
        }

        private string finalTip = "";
        private Color finalColor = Color.white;

        public void OnGUI()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty folderPath = serializedObject.FindProperty("folderPath");

            SerializedProperty preName = serializedObject.FindProperty("preName");

            SerializedProperty haveSecondName = serializedObject.FindProperty("haveSecondName");
            SerializedProperty secondName = serializedObject.FindProperty("secondName");

            SerializedProperty haveThirdName = serializedObject.FindProperty("haveThirdName");
            SerializedProperty thirdName = serializedObject.FindProperty("thirdName");

            SerializedProperty serialNamuber = serializedObject.FindProperty("serialNamuber");

            SerializedProperty needSetNewSymbol = serializedObject.FindProperty("needSetNewSymbol");
            SerializedProperty symbol = serializedObject.FindProperty("symbol");

            EditorGUILayout.PropertyField(folderPath, true);

            if (folderPath.stringValue == string.Empty)
            {
                EditorGUILayout.HelpBox("目标文件夹不可为空", MessageType.Error);
            }

            if (GUILayout.Button("选择目标文件夹"))
            {
                GetFolderPath(EditorUtility.OpenFolderPanel("文件资源管理器",
                                                            Application.dataPath,
                                                            "目标文件夹"));
            }

            EditorGUI.BeginDisabledGroup(folderPath.stringValue == string.Empty);

            EditorGUILayout.PropertyField(preName, true);

            EditorGUILayout.PropertyField(haveSecondName, true);
            if (haveSecondName.boolValue)
            {
                EditorGUILayout.PropertyField(secondName, true);
            }

            EditorGUILayout.PropertyField(haveThirdName, true);
            if (haveThirdName.boolValue)
            {
                EditorGUILayout.PropertyField(thirdName, true);
            }

            EditorGUILayout.PropertyField(serialNamuber, true);

            EditorGUILayout.PropertyField(needSetNewSymbol, true);
            EditorGUI.BeginDisabledGroup(!needSetNewSymbol.boolValue);
            EditorGUILayout.PropertyField(symbol, true);
            EditorGUI.EndDisabledGroup();


            List<string> temp = new List<string>();

            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontStyle = FontStyle.Bold;
            gUIStyle.fontSize = 11;
            gUIStyle.alignment = TextAnchor.UpperCenter;
            gUIStyle.normal.textColor = finalColor;

            GUILayout.Space(29);
            if (GUILayout.Button("---开始命名---"))
            {
                temp = Name();

                if (temp.Count <= 0)
                {
                    finalColor = Color.green;
                    finalTip = "已完成";
                }
                else
                {
                    finalColor = Color.red;
                    finalTip = "";
                    for (int i = 0; i < temp.Count; i++)
                    {
                        finalTip += temp[i] + " || " + "该文件已存在或已重命名！" + "\n";
                    }
                }
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(9);
            EditorGUILayout.LabelField(finalTip, gUIStyle);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}