using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Public;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (PublicVarriable.is_multiplay)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PublicVarriable.is_game_started = true;
            GameObject game_controller = Resources.Load(Path.prefab_path + "GameController") as GameObject;
            Instantiate(game_controller, Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = PublicVarriable.user_name;
        if (PublicVarriable.is_join)
        {
            PhotonNetwork.JoinRoom(PublicVarriable.room_name);
        }
        else
        {
            PhotonNetwork.CreateRoom(PublicVarriable.room_name, new RoomOptions { MaxPlayers = (byte)2}, null);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(Path.prefab_path + "GameController", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(Path.prefab_path + "NetworkInitializer", Vector3.zero, Quaternion.identity);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
