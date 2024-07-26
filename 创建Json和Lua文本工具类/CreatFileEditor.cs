using System.Text;//创建文本字符编码
using System.IO;//路径操作
using UnityEditor;
using UnityEngine;

/// <summary>
/// 创建文件工具类 Lua/txt
/// </summary>
namespace PIXELS
{
    public class CreatFileEditor : Editor
    {
        /// <summary>
        /// 静态方法来定义编辑器菜单项的回调方法
        /// </summary>
        //创建Lua
        [MenuItem("Assets/Create/Lua File", false, 1)]
        static void CreatLuaFile()
        {
            CreatFile("lua");
        }
        //创建txt
        [MenuItem("Assets/Create/Text File", false, 1)]
        static void CtreatTxtFile()
        {
            CreatFile("txt");
        }
        //创建json
        [MenuItem("Assets/Create/Json File", false, 1)]
        static void CtreatJsonFile()
        {
            CreatFile("json");
        }
        /// <summary>
        /// 创建具体实现
        /// </summary>
        /// <param name="fileName">创建的文件名</param>
        static void CreatFile(string fileName)
        {
            //获取要创建文件的目录 Selection.activeObject 点击对象
            var Creatpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            //文件名以及格式
            var newFileName = "New_" + fileName + "." + fileName;
            //文件路径
            var newFilePath = Creatpath + "/" + newFileName;

            //简单的重名处理
            //检查指定路径的文件是否存在
            if (File.Exists(newFilePath))
            {
                var newName = "new_" + fileName + "-" + UnityEngine.Random.Range(0, 100) + "." + fileName;
                newFilePath = Creatpath + "/" + newName;
                newFilePath = newFilePath.Replace(newFileName, newName);//更换名字
            }

            //创建文本并写入类容
            //如果是空白文件，编码并没有设成UTF-8
            File.WriteAllText(newFilePath, "", Encoding.UTF8);

            AssetDatabase.Refresh();//刷新资源

            //选中新创建的文件 以达到更名或者其他作用
            var asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);



            //直接进入重命名状态 待解决
            //选中具体文件会爆红但是无错 待解决
            //拓展 可以加一个输入什么类型文件就会多一个创建这个文件 在菜单栏里

        }

    }
}

