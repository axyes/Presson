using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableContainer : MonoBehaviour
{
    Dictionary<Coordinates, GameObject> cells = new Dictionary<Coordinates, GameObject>();

    [SerializeField]
    GameObject m_prefabSpawnable = null;

    [Tooltip("Number of columns")]
    [Range(1,5)]
    [SerializeField]
    int m_columns = 2;

    [Tooltip("Number of lines")]
    [Range(1, 5)]
    [SerializeField]
    int m_lines = 2;

    [SerializeField]
    float unitVector = 1;


    public Dictionary<Coordinates, GameObject> Cells { get => cells; set => cells = value; }

    public int LastColumn { get => m_columns - 1; }
    public int LastLine { get => m_lines - 1; }
    public int Columns { get => m_columns; set => m_columns = value; }
    public int Lines { get => m_lines; set => m_lines = value; }

    Vector2 GetPivotCoordinates()
    {
        float pivotX = LastColumn / 2.0f;
        float pivotY = LastLine / 2.0f;

       return new Vector2(pivotX, pivotY);
    }


    void Start()
    {
        Spawnitems();
    }



    private void Spawnitems()
    {
        if (m_prefabSpawnable == null) { return; }

        for (int j = 0; j < Lines; j++)
        {
            for (int i = 0; i < Columns; i++)
            {
                if (Random.value > 0.5f) { continue; }


                Coordinates itemCoor = new Coordinates(i, j);
                GameObject item = Instantiate(m_prefabSpawnable, this.transform);

                Vector2 pos = GetRelativePositionToPivot(itemCoor) * unitVector;
                item.transform.localPosition = new Vector3(pos.x, pos.y, 0);

            }
        }
    }

    private Vector2 GetRelativePositionToPivot (Coordinates p_coordinates)
    {
        return p_coordinates.GetVector() - GetPivotCoordinates();
    }

    void Rotate()
    {

    }
}
