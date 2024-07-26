using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace PHXH.PIXEL
{
#if UNITY_EDITOR
    public class Func_Rename_0 : EditorWindow
    {
        [Header("�ļ���·��")]
        [HideInInspector] public string folderPath;

        [Header("ǰ׺")]
        [HideInInspector] public string preName;

        [Header("��������")]
        [HideInInspector] public bool haveSecondName;
        [HideInInspector] public string secondName;

        [Header("��������")]
        [HideInInspector] public bool haveThirdName;
        [HideInInspector] public string thirdName;

        [Header("���")]
        [HideInInspector] public int serialNamuber;

        [Header("���ӷ���")]
        [HideInInspector] public bool needSetNewSymbol;
        [HideInInspector] public string symbol = "_";

        //������PNG�ļ�
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

        //��ȡ�ļ���·��
        public void GetFolderPath(string _path)
        {
            folderPath = _path;
        }

        [MenuItem("PIXEL/Func_Rename/Type_0_������")]
        private static void GetWindow()
        {
            Rect rect = new Rect(0, 0, 450, 550);
            GetWindowWithRect<Func_Rename_0>(rect, false, "����������", false);
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
                EditorGUILayout.HelpBox("Ŀ���ļ��в���Ϊ��", MessageType.Error);
            }

            if (GUILayout.Button("ѡ��Ŀ���ļ���"))
            {
                GetFolderPath(EditorUtility.OpenFolderPanel("�ļ���Դ������",
                                                            Application.dataPath,
                                                            "Ŀ���ļ���"));
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
            if (GUILayout.Button("---��ʼ����---"))
            {
                temp = Name();

                if (temp.Count <= 0)
                {
                    finalColor = Color.green;
                    finalTip = "�����";
                }
                else
                {
                    finalColor = Color.red;
                    finalTip = "";
                    for (int i = 0; i < temp.Count; i++)
                    {
                        finalTip += temp[i] + " || " + "���ļ��Ѵ��ڻ�����������" + "\n";
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