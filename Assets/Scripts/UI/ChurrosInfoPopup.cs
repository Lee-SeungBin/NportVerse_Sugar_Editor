using UnityEngine;
using UnityEngine.UI;

public class ChurrosInfoPopup : MonoBehaviour
{
    public Toggle directionToggle;

    public BoxGroup boxGroup;

    private void OnEnable()
    {
        SetData(MapManager.Instance.specialMode?.selectTile);
        //MapManager.Instance.specialMode.selectTile.box.boxDirection
    }
    public void SetData(Tile tile)
    {
        directionToggle.isOn = (tile.box?.boxDirection != 0);
    }
    public void OnClickAddChurros()
    {
        MapManager.Instance.CreateChurros();
    }

    public void OnClickDeleteChurros()
    {
        MapManager.Instance.DeleteChurros();
    }

    public void OnChangeDirection()
    {
        MapManager.Instance.ChangeDirectionChurros(directionToggle.isOn);
    }
}
