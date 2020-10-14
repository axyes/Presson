using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    Board m_board = null;
    Coordinates m_coordinates = new Coordinates(0,0);

    public Coordinates Coordinates { get => m_coordinates; set => m_coordinates = value; }
    public Board Board { get => m_board; set => m_board = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitByBoard(Board p_board, Coordinates p_coordinates)
    {
        m_board = p_board;
        m_coordinates = p_coordinates;


        //SetPosition
        transform.position = m_board.GetSlotPosition(p_coordinates);
        //SetScale
        float scale = m_board.GetOptimalScale() / 10; //Todo remove this /10 with a plane with 1x1 scale
        transform.localScale = new Vector3(scale, 1, scale);

        //Todo remove this with the enable border
        if (m_board.IsBorder(p_coordinates))
        {
            ActivateBorder();
        }
    }


    public void ActivateBorder()
    {
        if (m_board == null) { return; }

        bool[] border = m_board.GetBorderToActivate(m_coordinates);

        for (int i = 0; i < border.Length; i++)
        {
            bool currentBorder = border[i];

            var meshes = transform.GetChild(i).GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = currentBorder;
            }
        }

    }
  
}
