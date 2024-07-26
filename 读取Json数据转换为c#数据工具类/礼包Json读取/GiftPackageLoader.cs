using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// ����������չ���ʹ��
/// </summary>

[System.Serializable]
public class GiftPack
{
    public string name;//����
    public string image;//ͼƬ
    public string description;//����
    public int price;//�۸�
    public string currency;//����
    public bool isPurchased = false; // �Ƿ��ѹ���
}

[System.Serializable]
public class GiftPackList
{
    public List<GiftPack> giftPacks;

}
public class GiftPackageLoader
{
    //���մ���ֵ�� 
    public GiftPack giftPack;
    //·��
    public string jsonText;

    public GiftPackList giftPackList;

    public List<GiftPack> LoadGiftPacks(string jsonFilePath)
    {
        //��ȡjson ������
        jsonText = File.ReadAllText(jsonFilePath);

        //�����ַ���ת��c#����
        //GiftPack[] giftPacks = JsonUtility.FromJson<GiftPack[]>(jsonText);ֱ���ñ� ��������� ����
        giftPackList = JsonUtility.FromJson<GiftPackList>(jsonText);

        //�������� �ж�
        if (giftPackList != null && giftPackList.giftPacks != null)
        {

            Debug.Log("success");//���سɹ�
            return giftPackList.giftPacks;
        }
        else
        {
            Debug.LogError("null");
            return new List<GiftPack>(); // ���ؿ��б��Ա���������쳣
        }
    }

}


