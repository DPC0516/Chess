using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private int piece_index;

    public GameObject mesh;

    public int Piece_Index
    {
        get
        {
            return piece_index;
        }
        set
        {
            piece_index = value;
        }
    }

    public void init(GameObject _mesh, int _piece_index)
    {
        mesh = Instantiate(_mesh, transform);
        piece_index = _piece_index;
    }
}
