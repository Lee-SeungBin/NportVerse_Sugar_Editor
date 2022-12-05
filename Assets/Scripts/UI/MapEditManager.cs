﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditManager : MonoBehaviour
{
    public GameObject characterList;
    public GameObject specialList;

    public Button createMapButton;
    public GameObject createMapPopup;

    public Button modifyMapButton;
    public Dropdown modifyMapDropDown;
    public GameObject modifyMapPopup;

    public Button stageLoadButton;
    public Button jsonSaveButton;
    public JsonFileMaker jsonFileMakerPopup;
    public ObstacleOptionPopup obstacleOptionPopup;


    public CharacterInfoPopup characterInfoPopup;

    public TileSetVisiblePopup tileSetVisiblePopup;

    public Toggle centerSelectButton;


    public MapEditorPopups popups;



    private void Start()
    {
        HideCreateMapPopup();
        HideCharactorInfoPopup();
        HideModifyMapPopup();

        SetVisibleTileSetPopup(false);
        SetVisibleJsonFileSavePopup(false);
        SetVisibleObstacleOptionPopup(false);


        MapManager.Instance.onChangeMaps += OnChangeModifyMapDropDown;
    }


    public void OnChangeSelectModeDropDown(MapManager.SELECT_MODE selectMode)
    {
        if(selectMode == MapManager.SELECT_MODE.RAIL_SET)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        specialList.GetComponent<SpecialList>().SelectEmtpy();

        if (selectMode == MapManager.SELECT_MODE.MONSTER_SET)
        {
            characterList.SetActive(true);
            specialList.SetActive(false);
            stageLoadButton.gameObject.SetActive(false);
            jsonSaveButton.gameObject.SetActive(false);
            modifyMapButton.gameObject.SetActive(false);
            createMapButton.gameObject.SetActive(false);
        }
        else if (selectMode == MapManager.SELECT_MODE.SPECIAL_SET)
        {
            characterList.SetActive(false);
            specialList.SetActive(true);
            stageLoadButton.gameObject.SetActive(false);
            jsonSaveButton.gameObject.SetActive(false);
            modifyMapButton.gameObject.SetActive(false);
            createMapButton.gameObject.SetActive(false);
        }
        else
        {
            characterList.SetActive(false);
            specialList.SetActive(false);
            stageLoadButton.gameObject.SetActive(true);
            jsonSaveButton.gameObject.SetActive(true);
            modifyMapButton.gameObject.SetActive(true);
            createMapButton.gameObject.SetActive(true);
        }

        SetVisibleTileSetPopup(false);
        HideCharactorInfoPopup();
    }


    public void ShowCreateMapPopup()
    {
        createMapPopup.SetActive(true);

        createMapPopup.transform.Find("MapNumber").GetComponent<Text>().text = MapManager.Instance.Maps.Count.ToString();
    }

    public void HideCreateMapPopup()
    {
        createMapPopup.SetActive(false);
    }

    public void OnClickCreateMap()
    {
        int w = int.Parse(createMapPopup.transform.Find("Width").Find("Text").GetComponent<Text>().text);
        int h = int.Parse(createMapPopup.transform.Find("Height").Find("Text").GetComponent<Text>().text);

        MapManager.Instance.CreateMap(w, h);

        HideCreateMapPopup();
    }

    public void SetVisibleTileSetPopup(bool isActive)
    {
        tileSetVisiblePopup.gameObject.SetActive(isActive);

        if(isActive == false)
        {
            MapManager.Instance.SetNullToSelectTileSet();
        }
    }

    public void SetTileSetData(TileSet tileSet)
    {
        tileSetVisiblePopup.SetTileSetData(tileSet);
    }

         
    public void OnChangeSelectMapNumberOfModifyMapPopup()
    {
        MapManager.Instance.SelectMap(modifyMapDropDown.value);
    }

    public void ShowModifyMapPopup()
    {
        modifyMapPopup.SetActive(true);
    }

    public void DeleteMap()
    {
        MapManager.Instance.DeleteMap(modifyMapDropDown.value);
    }

    public void HideModifyMapPopup()
    {
        modifyMapPopup.SetActive(false);
    }

    private void OnChangeModifyMapDropDown()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        Dropdown.OptionData option;

        int len = MapManager.Instance.Maps.Count;
        for (int i = 0; i < len; ++i)
        {
            option = new Dropdown.OptionData();
            option.text = i.ToString();
            options.Add(option);
        }

        modifyMapDropDown.options = options;
    }

    public void OnClickModifyMap()
    {
        int w = int.Parse(modifyMapPopup.transform.Find("Width").Find("Text").GetComponent<Text>().text);
        int h = int.Parse(modifyMapPopup.transform.Find("Height").Find("Text").GetComponent<Text>().text);

        MapManager.Instance.ModifyMap(modifyMapDropDown.value, w, h);

        HideModifyMapPopup();
    }

    public void ShowCharactorInfoPopup(Charactor character)
    {
        characterInfoPopup.Show(character);
    }


    public void HideCharactorInfoPopup()
    {
        characterInfoPopup.Hide();
    }

    public void SetVisibleJsonFileSavePopup(bool isActive)
    {
        jsonFileMakerPopup.gameObject.SetActive(isActive);
    }
    

    public void SetVisibleObstacleOptionPopup(bool isActive)
    {
        obstacleOptionPopup.gameObject.SetActive(isActive);
    }
}