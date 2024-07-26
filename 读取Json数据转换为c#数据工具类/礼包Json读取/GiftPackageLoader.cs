using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 可以做成扩展插件使用
/// </summary>

[System.Serializable]
public class GiftPack
{
    public string name;//名字
    public string image;//图片
    public string description;//描述
    public int price;//价格
    public string currency;//货币
    public bool isPurchased = false; // 是否已购买
}

[System.Serializable]
public class GiftPackList
{
    public List<GiftPack> giftPacks;

}
public class GiftPackageLoader
{
    //接收储存值类 
    public GiftPack giftPack;
    //路径
    public string jsonText;

    public GiftPackList giftPackList;

    public List<GiftPack> LoadGiftPacks(string jsonFilePath)
    {
        //读取json 数据流
        jsonText = File.ReadAllText(jsonFilePath);

        //解析字符并转换c#对象
        //GiftPack[] giftPacks = JsonUtility.FromJson<GiftPack[]>(jsonText);直接用报 对象和数组 问题
        giftPackList = JsonUtility.FromJson<GiftPackList>(jsonText);

        //返回数据 判断
        if (giftPackList != null && giftPackList.giftPacks != null)
        {

            Debug.Log("success");//返回成功
            return giftPackList.giftPacks;
        }
        else
        {
            Debug.LogError("null");
            return new List<GiftPack>(); // 返回空列表以避免空引用异常
        }
    }

}


