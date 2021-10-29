using UnityEngine;
using UnityEngine.UI;
using Public;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Chess;
#pragma warning disable 0649
public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text current_team;
    [SerializeField]
    private Text my_team;
    [SerializeField]
    private GameObject roll_back_popup;
    [SerializeField]
    private GameObject promotion_popup;
    [SerializeField]
    private GameObject promotion_popup_enemy;

    [SerializeField]
    private GameObject piece;

    [SerializeField]
    private PhotonView PV;

    [SerializeField]
    private GameObject UI;

    private int promotion_piece_index = 0;

    [SerializeField]
    private GameObject skip_button;
    [SerializeField]
    private GameObject my_team_text;
    [SerializeField]
    private GameObject roll_back_button;

    [SerializeField]
    private Toggle is_white_checked;
    [SerializeField]
    private Toggle is_black_checked;

    [SerializeField]
    private Text white_time;
    [SerializeField]
    private Text black_time;
    [SerializeField]
    private GameObject timer;

    [SerializeField]
    private AudioSource sound_source;
    [SerializeField]
    private AudioClip move_sound;
    [SerializeField]
    private AudioClip fail_sound;
    [SerializeField]
    private AudioClip select_sound;

    private GameInitializer game_initializer;

    private void Start()
    {
        roll_back_popup.SetActive(false);
        promotion_popup.SetActive(false);
        promotion_popup_enemy.SetActive(false);
        game_initializer = GameObject.FindGameObjectWithTag("GameInitializer").GetComponent<GameInitializer>();
        my_team_text.SetActive(PublicVarriable.is_multiplay);
        if (PublicVarriable.is_multiplay)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PublicVarriable.white_time = PublicVarriable.time;
                PublicVarriable.black_time = PublicVarriable.time;
            }
            UI.SetActive(PV.IsMine);
        }
        else
        {
            timer.SetActive(PublicVarriable.is_time);
            PublicVarriable.white_time = PublicVarriable.time;
            PublicVarriable.black_time = PublicVarriable.time;
        }
        white_time.text = get_string_time(PublicVarriable.white_time);
        black_time.text = get_string_time(PublicVarriable.black_time);
    }

    private void Update()
    {
        if(!PublicVarriable.is_game_initialized)
        {
            return;
        }
        init_board();
        if (PublicVarriable.is_multiplay)
        {
            timer.SetActive((PublicVarriable.is_time && PhotonNetwork.LocalPlayer.IsMasterClient) || (PublicVarriable.multiplay_is_time && !PhotonNetwork.LocalPlayer.IsMasterClient));
            if (!PV.IsMine)
            {
                return;
            }
        }
        if (PublicVarriable.is_game_started)
        {
            check_time();

            if (!PublicVarriable.is_multiplay || PublicVarriable.chess_board.Current_Team == PublicVarriable.my_team)
            {
                check_move();
            }
        }
    }

    private void update_time()
    {
        if (PublicVarriable.chess_board.Current_Team.Team_Index == Constants.WHITE_TEAM_INDEX)
        {
            PublicVarriable.white_time -= Time.deltaTime;
        }
        if (PublicVarriable.chess_board.Current_Team.Team_Index == Constants.BLACK_TEAM_INDEX)
        {
            PublicVarriable.black_time -= Time.deltaTime;
        }

        white_time.text = get_string_time(PublicVarriable.white_time);
        black_time.text = get_string_time(PublicVarriable.black_time);
    }

    private void check_time()
    {
        if (PublicVarriable.is_multiplay)
        {
            if ((PublicVarriable.is_time && PhotonNetwork.LocalPlayer.IsMasterClient) || (PublicVarriable.multiplay_is_time && !PhotonNetwork.LocalPlayer.IsMasterClient))
            {
                update_time();
            }
        }
        else
        {
            if (PublicVarriable.is_time)
            {
                update_time();
            }
        }
    }

    private string get_string_time(float _second)
    {
        int second = (int) _second;
        int minute = second / 60;
        second = second % 60;
        int hour = minute / 60;
        minute = minute % 60;

        return get_string_time_min(hour) + ":" + get_string_time_min(minute) + ":" + get_string_time_min(second);
    }

    private string get_string_time_min(int _time)
    {
        if (_time > 0)
        {
            if (_time >= 10)
            {

                return _time.ToString();
            }
            else
            {
                return "0" + _time.ToString();
            }
        }
        else
        {
            return "00";
        }
    }

    private void check_move()
    {
        if (Input.GetMouseButtonDown(Key.select_piece))
        {
            Vector2 position = get_clicked();
            if (position != new Vector2(-1, -1))
            {
                if (PublicVarriable.chess_board.select_piece(position))
                {
                    sound_source.PlayOneShot(select_sound);
                }
                if (PublicVarriable.is_multiplay)
                {
                    PV.RPC("select_piece_rpc", RpcTarget.Others, (int)position.x, (int)position.y);
                }
            }
        }
        if (Input.GetMouseButtonDown(Key.move_piece))
        {
            Vector2 position = get_clicked();
            if (position != new Vector2(-1, -1))
            {
                if (PublicVarriable.chess_board.move_piece_to(position, ref PublicVarriable.chess_board))
                {
                    sound_source.PlayOneShot(move_sound);
                    if (PublicVarriable.chess_board.Is_Promotion)
                    {
                        promotion_piece_index = PublicVarriable.chess_board.get_promotion_piece_index();
                        promotion_popup.SetActive(true);
                        if (PublicVarriable.is_multiplay)
                        {
                            PV.RPC("activate_promotion_popup", RpcTarget.Others);
                        }
                    }
                }
                else
                {
                    sound_source.PlayOneShot(fail_sound);
                }
                if (PublicVarriable.is_multiplay)
                {
                    PV.RPC("move_piece_rpc", RpcTarget.Others, (int)position.x, (int)position.y);
                }
            }
        }
    }

    private void init_board()
    {
        is_white_checked.isOn = PublicVarriable.chess_board.Is_White_Checked;
        is_black_checked.isOn = PublicVarriable.chess_board.Is_Black_Checked;
        my_team.text = "my team: " + PublicVarriable.my_team.to_string();
        skip_button.SetActive((!PublicVarriable.is_multiplay || PublicVarriable.chess_board.Current_Team == PublicVarriable.my_team) && PublicVarriable.is_game_started);
        roll_back_button.SetActive(PublicVarriable.is_game_started);
        current_team.text = "current team: " + PublicVarriable.chess_board.Current_Team.to_string();
        for (int i = 0; i < PublicVarriable.pieces.Count; i++)
        {
            if (!PublicVarriable.chess_board.Pieces[i].Is_Dead)
            {
                int x = (int)PublicVarriable.chess_board.Pieces[i].Position.x;
                int y = (int)PublicVarriable.chess_board.Pieces[i].Position.y;
                PublicVarriable.pieces[i].position = PublicVarriable.lines[y, x].position;
                PublicVarriable.pieces[i].gameObject.GetComponent<Piece>().mesh.SetActive(true);
            }
            else
            {
                PublicVarriable.pieces[i].gameObject.GetComponent<Piece>().mesh.SetActive(false);
            }
        }
    }

    private Vector2 get_clicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject.GetComponent<BoardPiece>().index;
        }
        return new Vector2(-1, -1);
    }

    public void on_skip()
    {
        if (PublicVarriable.is_game_started)
        {
            if (PublicVarriable.is_multiplay)
            {
                if (PublicVarriable.chess_board.Current_Team == PublicVarriable.my_team)
                {
                    PublicVarriable.chess_board.reverse_current_team();
                    if (PublicVarriable.is_multiplay)
                    {
                        PV.RPC("skip_rpc", RpcTarget.Others);
                    }
                }
            }
            else
            {
                PublicVarriable.chess_board.reverse_current_team();
            }
        }
    }

    public void on_roll_back()
    {
        if (PublicVarriable.is_game_started)
        {
            if (PublicVarriable.is_multiplay)
            {
                PV.RPC("roll_back_popup_rpc", RpcTarget.Others);
            }
            else
            {
                if (PublicVarriable.chess_board.roll_back())
                {
                    game_initializer.init_pieces();
                    sound_source.PlayOneShot(move_sound);
                }
            }
        }
    }

    public void on_exit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void on_yes()
    {
        if (PublicVarriable.chess_board.roll_back())
        {
            game_initializer.init_pieces();
            sound_source.PlayOneShot(move_sound);
        }
        if (PublicVarriable.is_multiplay)
        {
            PV.RPC("roll_back_rpc", RpcTarget.Others);
        }
        roll_back_popup.SetActive(false);
    }

    public void on_no()
    {
        roll_back_popup.SetActive(false);
    }

    public void on_promotion(string _piece_name)
    {
        promotion_popup.SetActive(false);
        if(PublicVarriable.chess_board.promote_at(promotion_piece_index, _piece_name))
        {
            sound_source.PlayOneShot(move_sound);
        }
        game_initializer.init_pieces();
        if (PublicVarriable.is_multiplay)
        {
            PV.RPC("promote_at_rpc", RpcTarget.Others, promotion_piece_index, _piece_name);
        }
    }

    [PunRPC]
    private void move_piece_rpc(int _x, int _y)
    {
        if(PublicVarriable.chess_board.move_piece_to(new Vector2(_x, _y), ref PublicVarriable.chess_board))
        {
            sound_source.PlayOneShot(move_sound);
        }
    }

    [PunRPC]
    private void select_piece_rpc(int _x, int _y)
    {
        PublicVarriable.chess_board.select_piece(new Vector2(_x, _y));
    }

    [PunRPC]
    private void skip_rpc()
    {
        PublicVarriable.chess_board.reverse_current_team();
    }

    [PunRPC]
    private void roll_back_rpc()
    {
        if (PublicVarriable.chess_board.roll_back())
        {
            game_initializer.init_pieces();
            sound_source.PlayOneShot(move_sound);
        }
    }

    [PunRPC]
    private void roll_back_popup_rpc()
    {
        roll_back_popup.SetActive(true);
    }

    [PunRPC]
    private void promote_at_rpc(int _index, string _piece_name)
    {
        if (PublicVarriable.chess_board.promote_at(_index, _piece_name))
        {
            sound_source.PlayOneShot(move_sound);
        }
        game_initializer.init_pieces();
        promotion_popup_enemy.SetActive(false);
    }

    [PunRPC]
    private void activate_promotion_popup()
    {
        promotion_popup_enemy.SetActive(true);
    }
}
