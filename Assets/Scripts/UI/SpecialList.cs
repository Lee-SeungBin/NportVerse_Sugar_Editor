﻿using UnityEngine;
using UnityEngine.UI;

public class SpecialList : MonoBehaviour
{
    public Sprite[] specialSprites;

    [SerializeField]
    private GameObject[] BoxTypes;

    public int boxlayer;
    public int vinelayer;
    public int boxtype;

    public void SelectEmtpy()
    {
        UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.NONE, null);
    }

    public void SelectJelly()
    {
        UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.JELLY, specialSprites[(int)Enums.SPECIAL_TYPE.JELLY]);
    }

    public void SelectFrogSoup()
    {
        UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.FROG_SOUP, specialSprites[(int)Enums.SPECIAL_TYPE.FROG_SOUP]);
    }

    public void SelectBoxList(Toggle toggle)
    {
        for (int i = 0; i < BoxTypes.Length; i++)
        {
            if (!BoxTypes[i].activeSelf)
            {
                BoxTypes[i].SetActive(toggle.isOn);
                toggle.interactable = !toggle.isOn;
            }
            else
            {
                BoxTypes[i].SetActive(false);
                toggle.interactable = toggle.isOn;
                toggle.isOn = false;
            }
        }
    }
    public void SelectWoodenFence()
    {
        UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.WOODEN_FENCE, specialSprites[(int)Enums.SPECIAL_TYPE.WOODEN_FENCE]);
    }
    public void SelectBox(int boxLayer)
    {
        if (boxLayer == 1)
            UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.BOX, specialSprites[(int)Enums.SPECIAL_TYPE.BOX]);
        else if (boxLayer == 3)
            UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.BOX, specialSprites[4]);
        else if (boxLayer == 5)
            UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.BOX, specialSprites[5]); // 추후 수정 예정 specialSprites[]
        boxlayer = boxLayer;
    }
    public void SelectBoxtype(int boxType)
    {
        if (boxType == 1)
            UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.BOX, specialSprites[8]);
        else if (boxType == 2)
            UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.BOX, specialSprites[7]);
        boxtype = boxType;
    }

    public void SelectVine(int vineLayer)
    {
        UIManager.Instance.dragItem.SetSpecial(Enums.SPECIAL_TYPE.VINE, specialSprites[6]);
        vinelayer = vineLayer;
    }
}
