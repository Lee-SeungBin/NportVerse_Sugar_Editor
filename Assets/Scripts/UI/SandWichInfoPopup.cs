﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandWichInfoPopup : MonoBehaviour
{

    public Dropdown TasteStepDropdown;

    public Text boxlayer;

    private Box selectbox;

    public void Show(Box box)
    {
        gameObject.SetActive(true);
        TasteStepDropdown.value = 0;
        SetData(box);
    }

    public void SetData(Box box)
    {
        boxlayer.text = box.boxLayer.ToString();
        selectbox = box;
        MapManager.Instance.specialMode.ActiveAlphaLayerOfBox(box, true);
    }

    public void Hide()
    {
        if(selectbox != null && selectbox.boxLayer != 0)
            selectbox.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnCreateTaste()
    {
        MapManager.Instance.CrateTasteOfBox();
    }
    public void OnChangeTastePopup()
    {
        if (selectbox.boxLayer == 0)
        {
            UIManager.Instance.errorPopup.SetMessage("샌드위치가 없습니다.");
            return;
        }
        UIManager.Instance.mapEditManager.ShowSandWichChangePopup(selectbox);
        Hide();
    }
    public void OnDeleteTaste()
    {
        MapManager.Instance.DeleteTasteOfBox();
    }

    public void OnDestorySandWich()
    {
        MapManager.Instance.DestroyTasteOfBox();
        Hide();
    }
}