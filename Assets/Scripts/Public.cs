using UnityEngine;
using Chess;
using System.Collections.Generic;

namespace Public{

    public static class PublicVarriable
    {
        public static ChessBoard chess_board = new ChessBoard();
        public static List<Transform> pieces = new List<Transform>();
        public static Transform[,] lines = new Transform[8, 8];
        public static bool is_multiplay = false;
        public static bool is_game_initialized = false;
        public static bool is_game_started = false;
        public static bool is_spectator = false;
        public static ChessTeam my_team = new ChessTeam(Constants.WHITE_TEAM_INDEX);
        public static string room_name = "default_room";
        public static string user_name = "default_name";
        public static string enemy_name = "none";
        public static bool is_join = false;
        public static bool is_time = false;
        public static int time;
        public static float white_time;
        public static float black_time;
        public static bool multiplay_is_time = false;
    }

    public static class Key
    {
        public static int select_piece = 0;
        public static int move_piece = 1;
        public static KeyCode move_camera = KeyCode.LeftAlt;
        public static KeyCode promote_pawn = KeyCode.LeftControl;
    }

    public static class Path
    {
        public static string prefab_path = "Prefabs/";
        public static string piece_mesh_path = prefab_path + "PieceMesh/";
    }
}
