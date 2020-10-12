using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Coordinates
{
    int x;
    int y;

    //Constructor
    public Coordinates(int p_x, int p_y)
    {
        x = p_x;
        y = p_y;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
}

/// <summary>
/// Game Board which contain a matrix of slots
/// </summary>
/// 
public class Board : MonoBehaviour
{
    //Contain all slots of the board, order by X and Y Coordinate
    Dictionary<Coordinates, Slot> slots = new Dictionary<Coordinates, Slot>();

    [Tooltip("Number of Playable columns")]
    [SerializeField]
    int m_playableColumns = 20;

    [Tooltip("Number of Playable lines")]
    [SerializeField]
    int m_playableLines = 12;

    [Tooltip("Number of additional Colomn for the safe zone")]
    [SerializeField]
    int m_marginColomns = 0;

    [Tooltip("Number of additional lines for the safe zone")]
    [SerializeField]
    int m_marginLines = 5;

    [Tooltip("Prefab of Playable Slot")]
    [SerializeField]
    GameObject m_prefabSlot = null;

    [Tooltip("Prefab of Safe Slot")]
    [SerializeField]
    GameObject m_prefabSlotSafe = null;

    [Tooltip("TopLeft Corner position of the board")]
    [SerializeField]
    Transform AnchorMin = null;

    [Tooltip("RightBottom Corner position of the board")]
    [SerializeField]
    Transform AnchorMax = null;


    public Dictionary<Coordinates, Slot> Slots { get => slots; set => slots = value; }
    public int PlayableColumns { get => m_playableColumns; set => m_playableColumns = value; }
    public int PlayableLines { get => m_playableLines; set => m_playableLines = value; }
    public int TotalColumns { get => m_playableColumns + m_marginColomns * 2;}
    public int TotalLines { get => m_playableLines + m_marginLines * 2; }
    public int FirstPlayableColumn { get => m_marginColomns; }
    public int FirstPlayableLine { get => m_marginLines; }
    public int LastPlayableColumn { get => TotalColumns-m_marginColomns-1; }
    public int LastPlayableLine { get => TotalLines - m_marginLines - 1; }

    //Initialisation du script
    void Start()
    {
        CreateDefaultAnchor();
        CreateSlots();
    }

    private void CreateDefaultAnchor()
    {
        if (AnchorMin == null)
        {
            AnchorMin = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            AnchorMin.position = new Vector3(-10, 5, 0);
        }

        if (AnchorMax == null)
        {
            AnchorMax = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            AnchorMin.position = new Vector3(10, -5, 0);
        }
    }

    private void CreateSlots()
    {
        for (int j = 0; j < TotalLines; j++)
        {
            for (int i = 0; i < TotalColumns; i++)
            {
                Coordinates slotCoor = new Coordinates(i, j);
                CreateSlot(slotCoor);         
            }
        }
    }

    private Slot CreateSlot(Coordinates p_Coordinates)
    {
        /// Security test
        if (slots.ContainsKey(p_Coordinates)) {
            Debug.LogWarning("Slot already exist");
            return null;
        }

        GameObject prefab = m_prefabSlot;
        GameObject slotObj;
        Slot slot;

        //Is this slot a safe zone 
        if (IsPlayableCoordinate(p_Coordinates))
        {
            prefab = m_prefabSlotSafe;
        }

        if (prefab != null)
        {
            slotObj = Instantiate(prefab, this.transform);
            slot = slotObj.GetComponent<Slot>();
            if (slot == null)
            {
                slot = slotObj.AddComponent<Slot>();
            }
        }
        else // Create default primitive
        {
            slotObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            slot = slotObj.AddComponent<Slot>();
        }

        slotObj.name = "X_" + p_Coordinates.X + "_Y_" + p_Coordinates.Y ;

        //Add to container
        slots.Add(p_Coordinates, slot);

        //SetPosition
        slot.transform.position = GetSlotPosition(p_Coordinates);
        float scale = GetOptimalScale() / 10; //Todo remove this /10 with a plane with 1x1 scale
        slot.transform.localScale = new Vector3(scale, 1, scale);

        return slot;

    }

    Vector3 GetSlotPosition(Coordinates p_Coordinates)
    {
        if (!IsCoordinatesValid(p_Coordinates)) { return Vector3.zero; }

        float x = Mathf.Lerp(AnchorMin.position.x, AnchorMax.position.x, (float) p_Coordinates.X / (TotalColumns-1));
        float y = Mathf.Lerp(AnchorMin.position.y, AnchorMax.position.y, (float) p_Coordinates.Y / (TotalLines-1));

        return new Vector3(x, y, 0);

    }

    bool IsPlayableCoordinate(Coordinates p_Coordinates)
    {
        int p_x = p_Coordinates.X;
        int p_y = p_Coordinates.Y;

        if(p_x<FirstPlayableColumn || p_y <FirstPlayableLine || p_x > LastPlayableColumn || p_y > LastPlayableLine)
        {
            //SafeZone
            return false;
        }
        return true;
    }

    private bool IsCoordinatesValid(Coordinates p_Coordinates)
    {
        //Invalid Index
        if (p_Coordinates.X < 0 || p_Coordinates.Y < 0) { return false; }
        if (p_Coordinates.X >= TotalColumns || p_Coordinates.Y >= TotalLines) { return false; }

        return true;
    }

    float GetOptimalScale()
    {

        //TODO Move Anchor min and anchor max in regard of the scale

        float scaleX = Mathf.Abs(AnchorMax.position.x - AnchorMin.position.x) / TotalColumns;
        float scaleY = Mathf.Abs(AnchorMax.position.y - AnchorMin.position.y) / TotalLines;

        return Mathf.Min(new float[] { scaleX, scaleY });
    }
}
