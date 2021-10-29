using UnityEngine;
using Photon.Pun;
using Public;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Chess;
#pragma warning disable 0649
public class NetworkInitializer : MonoBehaviour
{
    [SerializeField]
    PhotonView PV;

    [SerializeField]
    private GameObject start_button;
    [SerializeField]
    private GameObject reverse_team_button;

    [SerializeField]
    private Text white_team_name;
    [SerializeField]
    private Text black_team_name;

    [SerializeField]
    private GameObject team_name_text;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PublicVarriable.is_game_initialized = true;
            PV.RPC("init", RpcTarget.OthersBuffered, 
                PublicVarriable.my_team.reverse().Team_Index,
                PublicVarriable.time, 
                PublicVarriable.is_time);
        }
        team_name_text.SetActive(PublicVarriable.is_multiplay);
        PV.RPC("init_name_rpc", RpcTarget.OthersBuffered, PublicVarriable.user_name);
    }

    private void Update()
    {
        init_name();
        reverse_team_button.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length == 2 && !PublicVarriable.is_game_started);
        start_button.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length == 2 && !PublicVarriable.is_game_started);
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            PublicVarriable.enemy_name = "none";
        }
    }

    [PunRPC]
    private void init(int _team_index, int _time, bool _is_time)
    {
        PublicVarriable.my_team = new ChessTeam(_team_index);
        PublicVarriable.multiplay_is_time = _is_time;
        PublicVarriable.white_time = _time;
        PublicVarriable.black_time = _time;
        PublicVarriable.is_game_initialized = true;
    }

    private void init_name()
    {
        if (PublicVarriable.my_team.Team_Index == Constants.WHITE_TEAM_INDEX)
        {
            white_team_name.text = "WHITE: " + PublicVarriable.user_name;
            black_team_name.text = "BLACK: " + PublicVarriable.enemy_name;
        }
        if (PublicVarriable.my_team.Team_Index == Constants.BLACK_TEAM_INDEX)
        {
            white_team_name.text = "WHITE: " + PublicVarriable.enemy_name;
            black_team_name.text = "BLACK: " + PublicVarriable.user_name;
        }
    }

    [PunRPC]
    private void init_name_rpc(string _name)
    {
        PublicVarriable.enemy_name = _name;
    }

    [PunRPC]
    private void reverse_team_rpc()
    {
        PublicVarriable.my_team = PublicVarriable.my_team.reverse();
    }

    public void reverse_team()
    {
        PV.RPC("reverse_team_rpc", RpcTarget.AllBuffered);
    }

    public void start()
    {
        PV.RPC("start_rpc", RpcTarget.All);
    }

    [PunRPC]
    private void start_rpc()
    {
        PublicVarriable.is_game_started = true;
    }
}
