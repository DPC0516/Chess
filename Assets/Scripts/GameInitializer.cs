using UnityEngine;
using System.Collections.Generic;
using Public;
#pragma warning disable 0649
public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject white_board_piece_prefab;
    [SerializeField]
    private GameObject black_board_piece_prefab;

    [SerializeField]
    private Transform board;

    [SerializeField]
    private Transform board_camera;

    [SerializeField]
    private GameObject piece;

    //기물 세우기

    [SerializeField]
    private int space_width = 1;
    [SerializeField]
    private int space_height = 1;
    
    private const int BLACK = 0;
    private const int WHITE = 1;

    void Start()
    {
        Vector3 position;

        //보드 생성
        for (int i = 7; i >= 0; i--)
        {
            for (int j = 7; j >= 0; j--)
            {
                position = new Vector3(j + (space_width/2 * j), 0, i + (space_height/2 * i));
                int board_color = ((i + 1) + (j + 1)) % 2;
                Transform space = null;
                if (board_color == BLACK)
                {
                    space = Instantiate(black_board_piece_prefab, position, Quaternion.identity).transform;

                }
                if (board_color == WHITE)
                {
                    space = Instantiate(white_board_piece_prefab, position, Quaternion.identity).transform;
                }
                if(space != null)
                {
                    space.GetComponent<BoardPiece>().index = new Vector2(j, i);
                    space.parent = board;
                    PublicVarriable.lines[i, j] = space;
                }
            }
        }

        board_camera.position = board.transform.position + new Vector3(space_width * 3.5f, 0f, space_height * 3.5f);

        init_pieces();

        if (!PublicVarriable.is_multiplay)
        {
            PublicVarriable.is_game_initialized = true;
        }
    }

    public void init_pieces()
    {
        for(int i = 0; i < PublicVarriable.pieces.Count; i++)
        {
            Destroy(PublicVarriable.pieces[i].gameObject);
        }

        PublicVarriable.pieces = new List<Transform>();

        for (int i = 0; i < PublicVarriable.chess_board.Pieces.Count; i++)
        {
            GameObject piece_instantiate = Instantiate(piece, Vector2.zero, Quaternion.identity);
            piece_instantiate.GetComponent<Piece>().init(PublicVarriable.chess_board.Pieces[i].mesh, i);
            PublicVarriable.pieces.Add(piece_instantiate.transform);
        }
    }
}
