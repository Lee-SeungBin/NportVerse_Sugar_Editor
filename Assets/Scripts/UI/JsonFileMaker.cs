﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonFileMaker : MonoBehaviour
{
    public MapManager mapManager;
    public UIManager uIManager;
    public MissionPopup missionPopup;
    public Dropdown bgDropdown, bgmDropdown;

    public Toggle starGauge, moveBuff, fenceBuff, startingMove;

    public InputField starPercent, stageNumber, moveText, jumpText;

    public string SaveJsonForAndroid()
    {
        StageData data = new StageData();

        try
        {
            data.missions = missionPopup.GetUsingMissionDatas().ToArray();

            data.obstacles = new List<Obstacle>();

            data.bgNumber = bgDropdown.value;
            data.bgmNumber = bgmDropdown.value;
            data.moveCount = int.Parse(moveText.text);
            data.fenceCount = int.Parse(jumpText.text);
            data.showStarGauge = starGauge.isOn ? 1 : 0;
            data.starPercent = int.Parse(starPercent.text);
            data.isMoveAtStart = startingMove.isOn ? 1 : 0;
            data.usePossibleMoveBuff = moveBuff.isOn ? 1 : 0;
            data.usePossibleJumpBuff = fenceBuff.isOn ? 1 : 0;
            data.stageNumber = int.Parse(stageNumber.text);
        }
        catch (Exception)
        {
            UIManager.Instance.errorPopup.SetMessage("스테이지 정보를 확인해주세요.");
        }

        if (data.missions.Length == 0 ||
            data.moveCount == 0 ||
            data.fenceCount == 0 ||
            data.stageNumber == 0 ||
            string.IsNullOrEmpty(moveText.text) ||
            string.IsNullOrEmpty(jumpText.text) ||
            string.IsNullOrEmpty(starPercent.text) ||
            string.IsNullOrEmpty(stageNumber.text) ||
            mapManager.Maps.Count == 0)
        {
            UIManager.Instance.errorPopup.SetMessage("저장이 불가능합니다. 맵, 점프, 무브, 미션을 확인해주세요.");
            return null;
        }

        List<Map> maps = mapManager.Maps;
        int mapCount = maps.Count;

        MapData[] mapDatas = new MapData[mapCount];
        MapData mapData;

        TileSetData tileSet;
        List<TileSetData> tileSets;

        RailGroupData railGroupData;
        List<RailGroupData> railGroupDatas;
        List<RailGroup> railGroups;

        BoxGroupData boxGroupData;
        List<BoxGroupData> boxGroupDatas;
        List<BoxGroup> boxGroups;

        BoxRandomData boxRandomData;

        Charactor charactor;
        int tileCount;
        int i, w, h, r, ri;

        bool isRail = false;
        bool isJelly = false;
        bool isFrogSoup = false;
        bool isBox = false;
        bool isVine = false;

        for (i = 0; i < mapCount; ++i)
        {
            mapData = new MapData();
            mapData.x = maps[i].transform.position.x;
            mapData.y = maps[i].transform.position.y;
            mapData.centerX = maps[i].container.transform.localPosition.x;
            mapData.centerY = maps[i].container.transform.localPosition.y;
            mapData.width = maps[i].width;
            mapData.height = maps[i].height;

            tileCount = 0;
            tileSets = new List<TileSetData>();
            for (w = 0; w < maps[i].width; ++w)
            {
                for (h = 0; h < maps[i].height; ++h)
                {
                    charactor = maps[i].tileSets[w][h].character;

                    tileSet = new TileSetData();
                    tileSet.fenceVisibleState = maps[i].tileSets[w][h].isVisible == true ? 1 : 0;
                    if (charactor != null)
                    {
                        tileSet.code = charactor.name;//.Replace("0","100");
                        tileSet.startStar = charactor.isStar ? 1 : 0;
                        tileSet.position = charactor.tileIndex;
                        tileSet.iceStep = charactor.iceStep;
                        tileSet.heightDirection = charactor.isHeightDirection ? "1" : "0";
                        tileSet.userFence = charactor.isUser ? 1 : 0;
                    }
                    else
                    {
                        tileSet.code = "x";
                        tileSet.startStar = 0;
                        tileSet.position = 0;
                        tileSet.iceStep = 0;
                        tileSet.heightDirection = "0";
                        tileSet.userFence = 0;
                    }

                    tileSet.woodenFences = maps[i].tileSets[w][h].GetWoodenFencesForJsonSaving();
                    tileSet.isVisibleTiles = maps[i].tileSets[w][h].GetTileVisibleState();
                    bool isflavone = maps[i].tileSets[w][h].GetFlavoneBox();
                    int cnt = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (tileSet.isVisibleTiles[j] == false)
                            cnt++;
                    }
                    if ((tileSet.code == "x" && !isflavone) && cnt != 4)
                    {
                        UIManager.Instance.errorPopup.SetMessage("저장이 불가능합니다. 활성화된 타일에는 플라본이나 플라본 모자가 꼭 있어야합니다.");
                        return null;
                    }
                    tileSets.Add(tileSet);

                    ++tileCount;
                }
            }
            mapData.tileSetDatas = tileSets;


            List<BoxRandomData> randomBoxDataList = new List<BoxRandomData>();
            for (r = 0; r < maps[i].boxs.Count; ++r)
            {
                if (maps[i].boxs[r].boxTypes == 3)
                {
                    RandomBox randomBox = maps[i].boxs[r].GetComponent<RandomBox>();

                    boxRandomData = new BoxRandomData();
                    boxRandomData.isflavone = randomBox.isflavone;
                    boxRandomData.fenceindex = randomBox.fenceIndex;
                    boxRandomData.tileindex = randomBox.tileIndex;
                    boxRandomData.DataSet = new List<BoxRandomData.RandomData>();

                    for (int k = 0; k < randomBox.DataSet.Count; k++)
                    {
                        BoxRandomData.RandomData randomData = new BoxRandomData.RandomData();
                        randomData.percentage = new List<int>();
                        randomData.itemlist = new List<string>();
                        for (int q = 0; q < randomBox.DataSet[k].percentage.Count; q++)
                        {
                            if (randomBox.DataSet[k].percentage[q] != 0)
                            {
                                randomData.percentage.Add(randomBox.DataSet[k].percentage[q]);
                                randomData.itemlist.Add(randomBox.DataSet[k].itemlist[q]);
                            }
                        }
                        boxRandomData.DataSet.Add(randomData);

                    }
                    //foreach (var datas in randomBox.DataSet)
                    //{
                    //    BoxRandomData.RandomData randomData = new BoxRandomData.RandomData();
                    //    randomData.percentage = new List<int>();
                    //    randomData.itemlist = new List<string>();
                    //    boxRandomData.DataSet.Add(randomData);
                    //}

                    randomBoxDataList.Add(boxRandomData);
                }
            }
            //foreach (var boxData in randomBoxDataList)
            //{
            //    Debug.Log("isflavone: " + boxData.isflavone);
            //    foreach (var dataa in boxData.DataSet)
            //    {
            //        Debug.Log("DataSet - percentage: " + string.Join(", ", dataa.percentage));
            //        Debug.Log("DataSet - itemlist: " + string.Join(", ", dataa.itemlist));
            //    }
            //}
            railGroupDatas = new List<RailGroupData>();
            railGroups = maps[i].railManager.railGroups;
            List<int> rails;

            int visibleFenceCount;

            try
            {
                for (r = 0; r < railGroups.Count; ++r)
                {
                    visibleFenceCount = 0;

                    railGroupData = new RailGroupData();
                    railGroupData.straightMode = railGroups[r].railType == Enums.RAIL_TYPE.STRIGHT ? "1" : "0";

                    rails = new List<int>();
                    for (ri = 0; ri < railGroups[r].tileSets.Count; ++ri)
                    {
                        if (railGroups[r].tileSets[ri].isVisible)
                        {
                            ++visibleFenceCount;
                        }
                        rails.Add(railGroups[r].tileSets[ri].tileSetIndex);
                    }

                    if (railGroupData.straightMode == "1") // 직선 레일일 경우
                    {
                        if (railGroups[r].GetLastTileSet().isVisible || rails.Count <= 1)
                        {
                            throw new Exception("직선 레일은 한 개 일수 없으며 맨 끝 타일은 무조건 비어 있어야 합니다. 레일 그룹 번호 " + r + "의 타일의 끝을 없애주세요.");
                        }
                        if (rails.Count == visibleFenceCount)
                        {
                            throw new Exception("레일 그룹 번호 " + r + "의 타일이 보이는 상태입니다. 레일 그룹안의 타일 중 1개 이상을 없애주세요.");
                        }
                    }
                    if (railGroupData.straightMode == "0") // 회전 레일일 경우
                    {
                        TileSet lastTile = railGroups[r].GetLastTileSet();

                        int a = lastTile.tileSetIndex / lastTile.map.height;
                        int b = lastTile.tileSetIndex % lastTile.map.height;
                        // 마지막 레일의 인덱스를 통해 4방향에서 첫번째 타일과 인접한지 체크 및 2개 이하 인지 체크
                        if (!((a > 0 && lastTile.map.tileSets[a - 1][b].tileSetIndex == railGroups[r].tileSets[0].tileSetIndex) ||
                              (a < lastTile.map.width - 1 && lastTile.map.tileSets[a + 1][b].tileSetIndex == railGroups[r].tileSets[0].tileSetIndex) ||
                              (b > 0 && lastTile.map.tileSets[a][b - 1].tileSetIndex == railGroups[r].tileSets[0].tileSetIndex) ||
                              (b < lastTile.map.height - 1 && lastTile.map.tileSets[a][b + 1].tileSetIndex == railGroups[r].tileSets[0].tileSetIndex)) ||
                              (rails.Count <= 2))
                        {
                            throw new Exception("회전 레일은 두 개 일수 없으며 맨 끝과 맨 처음의 타일이 맞닿아 있어야 합니다. 레일 그룹 번호 " + r + "의 타일을 조정해주세요.");
                        }
                    }

                    railGroupData.rails = rails;
                    railGroupDatas.Add(railGroupData);
                }
            }
            catch (Exception e)
            {
                UIManager.Instance.errorPopup.SetMessage(e.Message);
                return null;
            }

            boxGroupDatas = new List<BoxGroupData>();
            boxGroups = maps[i].boxManager.boxGroups;
            List<int> FenceIndex;
            List<int> TileIndex;

            try
            {
                for (r = 0; r < boxGroups.Count; ++r)
                {
                    if (boxGroups[r].fenceindex.Count < 3)
                    {
                        throw new Exception("츄러스의 길이는 최소 3 이상이여야 합니다.");
                    }
                    boxGroupData = new BoxGroupData();
                    boxGroupData.direction = boxGroups[r].Direction;

                    FenceIndex = new List<int>();
                    TileIndex = new List<int>();

                    for (ri = 0; ri < boxGroups[r].fenceindex.Count; ++ri)
                    {
                        FenceIndex.Add(boxGroups[r].fenceindex[ri]);
                        TileIndex.Add(boxGroups[r].tileindex[ri]);
                    }

                    boxGroupData.fenceIndex = FenceIndex;
                    boxGroupData.tileIndex = TileIndex;
                    boxGroupDatas.Add(boxGroupData);
                }
            }
            catch (Exception e)
            {
                UIManager.Instance.errorPopup.SetMessage(e.Message);
                return null;
            }



            mapData.railGroupDatas = railGroupDatas;
            mapData.boxGroupDatas = boxGroupDatas;
            mapData.boxRandomDatas = randomBoxDataList;
            mapData.jellyDatas = GetJellyDatas(maps[i]);
            mapData.frogSoupDatas = GetFrogSoupDatas(maps[i]);
            mapData.boxDatas = GetBoxData(maps[i]);
            mapData.vineDatas = GetVineData(maps[i]);
            mapData.nextStageDatas = maps[i].nextStageDatas;

            isRail = mapData.railGroupDatas.Count > 0;
            isJelly = mapData.jellyDatas.Count > 0;
            isFrogSoup = mapData.frogSoupDatas.Count > 0;
            isBox = mapData.boxDatas.Count > 0;
            isVine = mapData.vineDatas.Count > 0;

            mapDatas[i] = mapData;
        }

        string jellyTerm = uIManager.mapEditManager.obstacleOptionPopup.jellyTerm.text;
        string jellyCount = uIManager.mapEditManager.obstacleOptionPopup.jellyCount.text;
        if (isJelly)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.JELLY,
                options = new string[] { }
            });
        }
        if (int.Parse(jellyTerm) > 0 && int.Parse(jellyCount) > 0)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.JELLY,
                options = new string[] { jellyTerm, jellyCount }
            });
        }

        // 클라이언트에서 각 장애물 컨트롤러를 작동 시키려면 반드시 필요한 부분
        if (isRail)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.RAIL,
                options = new string[] { }
            });
        }

        if (isFrogSoup)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.FROG_SOUP,
                options = new string[] { }
            });
        }

        if (isBox)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.BOX,
                options = new string[] { }
            });
        }

        if (isVine)
        {
            data.obstacles.Add(new Obstacle
            {
                type = (int)Enums.OBSTACLE_TYPE.VINE,
                options = new string[] { }
            });
        }

        data.mapDatas = mapDatas;

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        Debug.Log(jsonData);

        string AES256json = UIManager.Instance.cryptoMNG.EncrStage(jsonData);

        return AES256json;
    }

    private List<JellyData> GetJellyDatas(Map map)
    {
        JellyData jellyData;
        List<JellyData> jellyDatas = new List<JellyData>();

        List<Jelly> jellysOfMap = map.jellys;

        int jellyCount = jellysOfMap.Count;

        for (int i = 0; i < jellyCount; ++i)
        {
            jellyData = new JellyData();
            jellyData.fenceIndex = jellysOfMap[i].fenceIndex;
            jellyData.tileIndex = jellysOfMap[i].tileIndex.ToString();
            jellyDatas.Add(jellyData);
        }

        return jellyDatas;
    }

    private List<FrogSoupData> GetFrogSoupDatas(Map map)
    {
        FrogSoupData frogSoupData;
        List<FrogSoupData> frogSoupDatas = new List<FrogSoupData>();

        List<FrogSoup> frogSoupOfMap = map.frogSoups;

        int count = frogSoupOfMap.Count;

        for (int i = 0; i < count; ++i)
        {
            frogSoupData = new FrogSoupData();
            frogSoupData.fenceIndex = frogSoupOfMap[i].fenceIndex;
            frogSoupData.tileIndex = frogSoupOfMap[i].tileIndex.ToString();
            frogSoupDatas.Add(frogSoupData);
        }

        return frogSoupDatas;
    }

    private List<BoxData> GetBoxData(Map map)
    {
        BoxData boxData;
        List<BoxData> boxDatas = new List<BoxData>();

        List<Box> boxOfMap = map.boxs;

        int count = boxOfMap.Count;

        for (int i = 0; i < count; ++i)
        {
            boxData = new BoxData();
            boxData.fenceIndex = boxOfMap[i].fenceIndex.ToString();
            boxData.tileIndex = boxOfMap[i].tileIndex.ToString();
            boxData.boxLayer = boxOfMap[i].boxLayer.ToString();
            boxData.boxTypes = boxOfMap[i].boxTypes.ToString();
            if (boxOfMap[i].boxTier != null)
            {
                boxData.boxTier = boxOfMap[i].boxTier.ConvertAll(i => i.ToString());
            }
            boxDatas.Add(boxData);
        }

        return boxDatas;
    }

    private List<VineData> GetVineData(Map map)
    {
        VineData vineData;
        List<VineData> vineDatas = new List<VineData>();

        List<Vine> vineOfMap = map.vines;

        int count = vineOfMap.Count;

        for (int i = 0; i < count; ++i)
        {
            vineData = new VineData();
            vineData.fenceIndex = vineOfMap[i].fenceIndex.ToString();
            vineData.layer = vineOfMap[i].layer.ToString();
            vineDatas.Add(vineData);
        }

        return vineDatas;
    }

    public void OnClickCancel()
    {
        gameObject.SetActive(false);
    }

}

[Serializable]
public static class StageInfo
{
    public static StageData data;
}

[Serializable]
public class StageData
{
    public Mission[] missions;
    public List<Obstacle> obstacles;

    public int stageNumber;
    public int bgNumber;
    public int bgmNumber;

    public int moveCount;
    public int fenceCount;

    public int showStarGauge;
    public float starPercent;
    public int isMoveAtStart;

    public int usePossibleMoveBuff;
    public int usePossibleJumpBuff;

    public MapData[] mapDatas;
}


[Serializable]
public class Mission
{
    public int type;
    public int qty;
}

[Serializable]
public class Obstacle
{
    public int type;
    public string[] options;
}


[Serializable]
public class MapData
{
    public float x;
    public float y;
    public int width;
    public int height;
    public float centerX;
    public float centerY;
    public List<TileSetData> tileSetDatas;
    public List<RailGroupData> railGroupDatas;
    public List<BoxGroupData> boxGroupDatas;
    public List<BoxRandomData> boxRandomDatas;
    public List<JellyData> jellyDatas;
    public List<FrogSoupData> frogSoupDatas;
    public List<BoxData> boxDatas;
    public List<VineData> vineDatas;
    public List<NextStageData> nextStageDatas;
}

[Serializable]
public class TileSetData
{
    public int fenceVisibleState;
    public string code;
    public int startStar;
    public int position;
    public int iceStep;
    public string heightDirection;
    public int userFence;
    public bool[] woodenFences;
    public bool[] isVisibleTiles;
}

[Serializable]
public class RailGroupData
{
    public string straightMode;
    public List<int> rails;
}

[Serializable]
public class JellyData
{
    public int fenceIndex;
    public string tileIndex;
}
[Serializable]
public class FrogSoupData
{
    public int fenceIndex;
    public string tileIndex;
}
[Serializable]
public class BoxData
{
    public string fenceIndex;
    public string tileIndex;
    public string boxLayer;
    public string boxTypes;
    public List<string> boxTier;
}
[Serializable]
public class VineData
{
    public string fenceIndex;
    public string layer;
}
[Serializable]
public class NextStageData
{
    public int type;
    public int qty;
}
[Serializable]
public class BoxGroupData
{
    public int direction;
    public List<int> fenceIndex;
    public List<int> tileIndex;
}
[Serializable]
public class BoxRandomData
{
    public bool isflavone;
    public int fenceindex;
    public int tileindex;
    public struct RandomData
    {
        public List<string> itemlist;
        public List<int> percentage;
    }
    public List<RandomData> DataSet;
}