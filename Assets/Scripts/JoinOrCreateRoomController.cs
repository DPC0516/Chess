using System.Collections.Generic;
using UnityEngine;
using Public;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Chess;
#pragma warning disable 0649
public class JoinOrCreateRoomController : MonoBehaviour
{
    [SerializeField]
    private Toggle is_multiplay;
    [SerializeField]
    private Toggle is_join;
    [SerializeField]
    private InputField user_name;
    [SerializeField]
    private InputField room_name;
    [SerializeField]
    private InputField time;
    [SerializeField]
    private Toggle is_time;
    [SerializeField]
    private GameObject time_setting;

    [SerializeField]
    private GameObject multiplay_setting;

    void Start()
    {
        PublicVarriable.is_game_initialized = false;
        PublicVarriable.is_game_started = false;
        PublicVarriable.enemy_name = "none";
        PublicVarriable.my_team = new ChessTeam(Constants.WHITE_TEAM_INDEX);
        PublicVarriable.pieces = new List<Transform>();
        PublicVarriable.lines = new Transform[8, 8];
        PublicVarriable.chess_board = new ChessBoard();
        is_multiplay.isOn = PublicVarriable.is_multiplay;
        is_join.isOn = PublicVarriable.is_join;
        room_name.text = PublicVarriable.room_name;
        user_name.text = PublicVarriable.user_name;
        is_time.isOn = PublicVarriable.is_time;
        time.text = PublicVarriable.time.ToString();
    }

    private void Update()
    {
        multiplay_setting.SetActive(is_multiplay.isOn);
        if (is_multiplay.isOn)
        {
            if (is_join.isOn)
            {
                is_time.isOn = false;
            }
        }
        time_setting.SetActive(is_time.isOn);
    }

    public void on_start()
    {
        PublicVarriable.is_multiplay = is_multiplay.isOn;
        PublicVarriable.is_join = is_join.isOn;
        PublicVarriable.room_name = room_name.text;
        PublicVarriable.user_name = user_name.text;
        PublicVarriable.is_time = is_time.isOn;
        PublicVarriable.time = int.Parse(time.text);
        SceneManager.LoadScene("Game");
    }

    public void on_cancel()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
