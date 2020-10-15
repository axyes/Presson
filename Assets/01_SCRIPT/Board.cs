using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Coordinates
{
    int m_x;
    int m_y;

    public int X { get => m_x; set => m_x = value; }
    public int Y { get => m_y; set => m_y = value; }

    //Constructor
    public Coordinates(int p_x, int p_y)
    {
        m_x = p_x;
        m_y = p_y;
    }

    public Vector2 GetVector()
    {
        return new Vector2(m_x, m_y);
    }
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
    int m_columns = 20;

    [Tooltip("Number of Playable lines")]
    [SerializeField]
    int m_lines = 12;


    [Tooltip("Prefab of Playable Slot")]
    [SerializeField]
    GameObject m_prefabSlot = null;

    [Tooltip("TopLeft Corner position of the board")]
    [SerializeField]
    Transform AnchorMin = null;

    [Tooltip("RightBottom Corner position of the board")]
    [SerializeField]
    Transform AnchorMax = null;


    public Dictionary<Coordinates, Slot> Slots { get => slots; set => slots = value; }
    public int Columns { get => m_columns; set => m_columns = value; }
    public int LastColumn { get => m_columns - 1; }
    public int Lines { get => m_lines; set => m_lines = value; }
    public int LastLine { get => m_lines - 1; }

    //Initialisation du script
    void Start()
    {
        CreateDefaultAnchor();
        AdjustAnchorPosition();
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


    private void AdjustAnchorPosition()
    {
        float scale = GetOptimalScale();
        Vector3 defaultAnchorMaxPosition = AnchorMax.position;
        Vector3 ajustedAnchorMaxPosition = AnchorMin.position + new Vector3(scale * Columns, -scale * Lines, 0);
        Vector3 delta = defaultAnchorMaxPosition - ajustedAnchorMaxPosition;
        AnchorMax.position = ajustedAnchorMaxPosition + delta / 2;
        AnchorMin.position += delta / 2;
    }

    private void CreateSlots()
    {
        for (int j = 0; j < Lines; j++)
        {
            for (int i = 0; i < Columns; i++)
            {
                Coordinates slotCoor = new Coordinates(i, j);
                CreateSlot(slotCoor);         
            }
        }
    }

    private Slot CreateSlot(Coordinates p_coordinates)
    {
        /// Security test
        if (slots.ContainsKey(p_coordinates)) {
            Debug.LogWarning("Slot already exist");
            return null;
        }

        GameObject prefab = m_prefabSlot;
        GameObject slotObj;
        Slot slot;

        //Spawn
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

        slotObj.name = "X_" + p_coordinates.X + "_Y_" + p_coordinates.Y ;

        //Add to container
        slot.InitByBoard(this, p_coordinates);
        slots.Add(p_coordinates, slot);
      


        return slot;

    }

    public Vector3 GetSlotPosition(Coordinates p_coordinates)
    {
        if (!IsCoordinatesValid(p_coordinates)) { return Vector3.zero; }

        float x = Mathf.Lerp(AnchorMin.position.x, AnchorMax.position.x, (float) (p_coordinates.X) / (Columns-1));
        float y = Mathf.Lerp(AnchorMin.position.y, AnchorMax.position.y, (float) (p_coordinates.Y) / (Lines-1));

        return new Vector3(x, y, 0);

    }

    public bool IsCoordinatesValid(Coordinates p_Coordinates)
    {
        //Invalid Index
        if (p_Coordinates.X < 0 || p_Coordinates.Y < 0) { return false; }
        if (p_Coordinates.X >= Columns || p_Coordinates.Y >= Lines) { return false; }

        return true;
    }

    public bool IsBorder(Coordinates p_coordinates)
    {
        if(!IsCoordinatesValid(p_coordinates)){ return false; }

        if (p_coordinates.X == 0 || p_coordinates.Y == 0 || p_coordinates.X == Columns - 1 || p_coordinates.Y == Lines - 1)
        {
            return true;
        }

        return false;
    }

    public bool[] GetBorderToActivate(Coordinates p_coordinates)
    {
        bool[] border = new bool[8];

        if (!IsCoordinatesValid(p_coordinates)) { return border; }

        //TOP
        if(p_coordinates.Y==0) { border[0] = true; }
        //CORNER TOP RIGHT
        if(p_coordinates.Y==0 && p_coordinates.X == LastColumn) { border[1] = true; }
        // RIGHT
        if(p_coordinates.X == LastColumn) { border[2] = true; }
        //CORNER BOTTOM RIGHT
        if (p_coordinates.Y == LastLine && p_coordinates.X == LastColumn) { border[3] = true; }
        //BOTTOM
        if (p_coordinates.Y == LastLine) { border[4] = true; }
        //CORNER BOTTOM LEFT
        if (p_coordinates.Y == LastLine && p_coordinates.X == 0) { border[5] = true; }
        // LEFT
        if (p_coordinates.X == 0) { border[6] = true; }
        //CORNER TOP LEFT
        if (p_coordinates.Y == 0 && p_coordinates.X == 0) { border[7] = true; }


        return border;
    }

    public float GetOptimalScale()
    {

        //TODO Move Anchor min and anchor max in regard of the scale

        float scaleX = Mathf.Abs(AnchorMax.position.x - AnchorMin.position.x) / Columns;
        float scaleY = Mathf.Abs(AnchorMax.position.y - AnchorMin.position.y) / Lines;

        return Mathf.Min(new float[] { scaleX, scaleY });
    }
}
