using UnityEngine;
using System.Collections.Generic;
using MyIOSTREAM;
using Public;
#pragma warning disable 0660
#pragma warning disable 0661
namespace Chess
{
	//상수 정의(#define)
	public static class Constants
    {
		public const int TEAM_NULL_INDEX = -1;
		public const int WHITE_TEAM_INDEX = 0;
		public const int BLACK_TEAM_INDEX = 1;

		public const string TEAM_NULL_STRING = "NULL";
		public const string WHITE_TEAM_STRING = "WHITE";
		public const string BLACK_TEAM_STRING = "BLACK";

		public const string PAWN_NAME = "PAWN";
		public const string ROOK_NAME = "ROOK";
		public const string KNIGHT_NAME = "KNIGHT";
		public const string BISHOP_NAME = "BISHOP";
		public const string QUEEN_NAME = "QUEEN";
		public const string KING_NAME = "KING";

		public const int KING_SIDE_ROOK_INDEX = 1;
		public const int QUEEN_SIDE_ROOK_INDEX = 0;

		public const int NONE_PIECE = -1;
		public const int FIND_PIECE_FAILURE = -1;

		//Vector2 체스판 좌표에서의 에러 체크
		public static bool check_error(Vector2 _position)
		{
			return _position.x > 7 || _position.y > 7 || _position.x < 0 || _position.y < 0;
		}
	}

    //팀 정보
    public class ChessTeam
	{
		//팀 인덱스
		private int team_index;

		public int Team_Index 
		{
            get
            {
				return team_index;
            }
            set
            {
				team_index = value;
            }
		}

		public ChessTeam reverse()
        {
			if(team_index == Constants.WHITE_TEAM_INDEX)
            {
				return new ChessTeam(Constants.BLACK_TEAM_INDEX);
            }
			if(team_index == Constants.BLACK_TEAM_INDEX)
            {
				return new ChessTeam(Constants.WHITE_TEAM_INDEX);
			}
			return null;
        }

		//기본 생성자
		public ChessTeam()
        {
			team_index = Constants.TEAM_NULL_INDEX;
        }

		//생성자 오버로딩
		public ChessTeam(int _team_index)
        {
			team_index = _team_index;
        }

		//연산자 오버로딩
		public static bool operator ==(ChessTeam _a, ChessTeam _b)
        {
			return (_a.team_index == _b.team_index);
		}

		public static bool operator !=(ChessTeam _a, ChessTeam _b)
		{
			return !(_a.team_index == _b.team_index);
		}

		//현재 팀의 string을 리턴
		public string to_string()
		{
			if(team_index == Constants.WHITE_TEAM_INDEX)
            {
				return Constants.WHITE_TEAM_STRING;
			}
			if (team_index == Constants.BLACK_TEAM_INDEX)
			{
				return Constants.BLACK_TEAM_STRING;
			}

			return Constants.TEAM_NULL_STRING;
		}
    }

	//기물 이동 정보
	public class HistoryNode
	{
		//원래 위치
		private Vector2 last_position;

		public Vector2 Last_Position
        {
            get
            {
				return last_position;
            }

            set
            {
				last_position = value;
            }
        }

		//이동한 기물 인덱스
		private int piece_index;

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

		//원래 is_moved
		private bool last_is_moved;

		public bool Last_Is_Moved {
            get
            {
				return last_is_moved;
            }
            set
            {
				last_is_moved = value;
            }
		}

		//이 이동으로 인해 죽은 기물 인덱스
		private int dead_piece_index;

		public int Dead_Piece_Index
		{
			get
			{
				return dead_piece_index;
			}
			set
			{
				dead_piece_index = value;
			}
		}

		private ChessTeam last_current_team;
		public ChessTeam Last_Current_Team
        {
            get
            {
				return last_current_team;
            }
            set
            {
				last_current_team = value;
            }
        }

		private bool is_castling;
		public bool Is_Castling 
		{
			get
			{
				return is_castling;
			}
            set
            {
				is_castling = value;
            }
		}

		private bool is_promotion;
		public bool Is_Promotion
        {
            get
            {
				return is_promotion;
            }
            set
            {
				is_promotion = value;
            }
        }

		public HistoryNode(int _piece_index)
        {
            piece_index = _piece_index;
			is_castling = false;
			is_promotion = false;
			dead_piece_index = Constants.NONE_PIECE;
        }
	}

	//체스 기물들의 베이스 클래스
	public abstract class ChessPiece
	{
		//기물의 이름 ROOK, QUEEN, KING, BISHOP 등등
		protected string name;
		public string Name
        {
            get
            {
				return name;
            }
        }
		protected int index;
		public int Index
        {
            get
            {
				return index;
            }
        }
		//기물의 팀 정보
		protected ChessTeam team;
		public ChessTeam Team
        {
            get
            {
				return team;
            }
        }
		//현재 위치
		protected Vector2 position;
		public Vector2 Position
        {
            get
            {
				return position;
            }
            set
            {
				position = value;
            }
        }
		//움직였는지에 대한 여부
		protected bool is_moved;
		public bool Is_Moved
        {
            get
            {
				return is_moved;
            }
            set
            {
				is_moved = value;
            }
        }
		//죽었는지에 대한 여부
		protected bool is_dead;
		public bool Is_Dead
        {
            get
            {
				return is_dead;
            }
            set
            {
				is_dead = value;
            }
        }

		public GameObject mesh;

		public ChessPiece(string _name, int _index, ChessTeam _team)
        {
			name = _name;
			index = _index;
			team = _team;
			position = Vector2.zero;
			is_moved = false;
			is_dead = false;
			mesh = Resources.Load(Path.piece_mesh_path + team.to_string() + "_" + name + "Mesh") as GameObject;
		}

		public ChessPiece(string _name, int _index, ChessTeam _team, Vector2 _position)
        {
			name = _name;
			index = _index;
			team = _team;
			position = _position;
			is_moved = false;
			is_dead = false;
			mesh = Resources.Load(Path.piece_mesh_path + team.to_string() + "_" + name + "Mesh") as GameObject;
		}

		public ChessPiece(string _name, int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
        {
			name = _name;
			index = _index;
			team = _team;
			position = _position;
			is_moved = _is_moved;
			is_dead = false;
			mesh = Resources.Load(Path.piece_mesh_path + team.to_string() + "_" + name + "Mesh") as GameObject;
		}

		public ChessPiece(string _name, int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
		{
			name = _name;
			index = _index;
			team = _team;
			position = _position;
			is_moved = _is_moved;
			is_dead = _is_dead;
			mesh = Resources.Load(Path.piece_mesh_path + team.to_string() + "_" + name + "Mesh") as GameObject;
		}

		//이동 가능 좌표(공격 가능 좌표 표함)
		public abstract List<Vector2> get_available_position(ChessBoard _chess_board);

		//공격 가능 좌표
		public virtual List<Vector2> get_attackable_position(ChessBoard _chess_board)
		{
			return get_available_position(_chess_board);
        }

		//좌표 이동 가능 여부(공격 가능 여부 표함)
		public bool is_position_available(ChessBoard _chess_board, Vector2 _position)
        {
			List<Vector2> available_positions = get_available_position(_chess_board);
			for (int i = 0; i < available_positions.Count; i++)
			{
				if (available_positions[i] == _position)
				{
					return true;
				}
			}

			return false;
		}

		//공격 가능 여부
		public bool is_position_attackable(ChessBoard _chess_board, Vector2 _position)
        {
			List<Vector2> available_positions = get_attackable_position(_chess_board);
			for (int i = 0; i < available_positions.Count; i++)
			{
				if (available_positions[i] == _position)
				{
					return true;
				}
			}

			return false;
		}

		//이동
		public virtual bool move_to(Vector2 _position, ref ChessBoard _chess_board)
		{
			HistoryNode history_node = new HistoryNode(index);
			//목표 지점에 갈수 있는지
			if (is_position_available(_chess_board, _position))
			{
				//기물 이동 정보 생성
				history_node.Last_Position = position;
				history_node.Last_Is_Moved = is_moved;
				history_node.Last_Current_Team = new ChessTeam(_chess_board.Current_Team.Team_Index);

				//목표 지점에 적 기물이 있는지
				int to_go_piece_index = _chess_board.find_piece_at(_position);
				if (to_go_piece_index != Constants.FIND_PIECE_FAILURE)
				{
					//목표 지점의 기물이 같은 팀인지
					if (_chess_board.Pieces[to_go_piece_index].team == _chess_board.Current_Team)
					{
						return false;
					}
					_chess_board.Pieces[to_go_piece_index].Is_Dead = true;

					history_node.Dead_Piece_Index = _chess_board.Pieces[to_go_piece_index].index;
				}
				position = _position;
				is_moved = true;

				_chess_board.push_history_node(history_node);

				return true;
			}
			return false;
		}
	}

	//폰 클래스
	public class Pawn : ChessPiece
	{
		public Pawn(int _index, ChessTeam _team) 
			: base(Constants.PAWN_NAME, _index, _team)
        {
			
		}

		public Pawn(int _index, ChessTeam _team, Vector2 _position) 
			: base(Constants.PAWN_NAME, _index, _team, _position)
		{

        }

		public Pawn(int _index, ChessTeam _team, Vector2 _position, bool _is_moved) 
			: base(Constants.PAWN_NAME, _index, _team, _position, _is_moved)
		{

        }

		public Pawn(int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.PAWN_NAME, _index, _team, _position, _is_moved, _is_dead)
		{

        }

		//폰의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
		{
			int is_reverse = 1;
			if (team.Team_Index == Constants.BLACK_TEAM_INDEX)
			{
				is_reverse = -1;
			}
			List<Vector2> available_positions = new List<Vector2>();
			Vector2 _position = new Vector2(position.x, position.y + 1 * is_reverse);

			//앞이 막혀있지 않을시
			if (_chess_board.find_piece_at(_position) == Constants.FIND_PIECE_FAILURE)
			{
				available_positions.Add(_position);
				if (!is_moved)
				{
					_position = new Vector2(position.x, position.y + 2 * is_reverse);
					if (_chess_board.find_piece_at(_position) == Constants.FIND_PIECE_FAILURE)
					{
						available_positions.Add(_position);
					}
				}
			}

			int piece_right_index = _chess_board.find_piece_at(position + new Vector2(1, 1 * is_reverse));
			int piece_left_index = _chess_board.find_piece_at(position + new Vector2(-1, 1 * is_reverse));

			//대각선 앞에 적이 있을시 이동가능 위치에 포함
			if (piece_right_index != Constants.FIND_PIECE_FAILURE
				&& _chess_board.Pieces[piece_right_index].Team != team)
			{
				available_positions.Add(position + new Vector2(1, 1 * is_reverse));
			}
			if (piece_left_index != Constants.FIND_PIECE_FAILURE
				&& _chess_board.Pieces[piece_left_index].Team != team)
			{
				available_positions.Add(position + new Vector2(-1, 1 * is_reverse));
			}
			return available_positions;
		}

        public override List<Vector2> get_attackable_position(ChessBoard _chess_board)
        {
			int is_reverse = 1;
			if (team.Team_Index == Constants.BLACK_TEAM_INDEX)
			{
				is_reverse = -1;
			}
			List<Vector2> available_positions = new List<Vector2>();
            if (!Constants.check_error(position + new Vector2(1, 1 * is_reverse)))
            {
				available_positions.Add(position + new Vector2(1, 1 * is_reverse));
            }
            if (!Constants.check_error(position + new Vector2(-1, 1 * is_reverse)))
            {
				available_positions.Add(position + new Vector2(-1, 1 * is_reverse));
			}
			return available_positions;
		}

        public bool is_promotion_available()
        {
			if(team == new ChessTeam(Constants.WHITE_TEAM_INDEX))
            {
				if(position.y == 7)
                {
					return true;
                }
            }
			if(team == new ChessTeam(Constants.BLACK_TEAM_INDEX))
            {
				if(position.y == 0)
                {
					return true;
                }
            }
			return false;
        }
	}

	//룩 클래스
	public class Rook : ChessPiece
	{
		//킹사이드 퀸사이드 구분을 위한 인덱스
		private int rook_index;
		public int Rook_Index
        {
            get
            {
				return rook_index;
            }
        }

		public Rook(int _rook_index, int _index, ChessTeam _team) 
			: base(Constants.ROOK_NAME, _index, _team)
        {
			rook_index = _rook_index;
        }

		public Rook(int _rook_index, int _index, ChessTeam _team, Vector2 _position)
			: base(Constants.ROOK_NAME, _index, _team, _position)
		{
			rook_index = _rook_index;
		}

		public Rook(int _rook_index, int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
			: base(Constants.ROOK_NAME, _index, _team, _position, _is_moved)
		{
			rook_index = _rook_index;
		}

		public Rook(int _rook_index, int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.ROOK_NAME, _index, _team, _position, _is_moved, _is_dead)
		{
			rook_index = _rook_index;
		}

		public void castling(ref ChessBoard _chess_board)
		{
			//기물 이동 정보 생성
			HistoryNode history_node = new HistoryNode(index)
			{
				Last_Position = position,
				Last_Is_Moved = is_moved,
				Last_Current_Team = new ChessTeam(_chess_board.Current_Team.Team_Index)
			};

            if (rook_index == Constants.KING_SIDE_ROOK_INDEX)
			{
				position += new Vector2(-2, 0);
				is_moved = true;
			}
			if (rook_index == Constants.QUEEN_SIDE_ROOK_INDEX)
			{
				position += new Vector2(3, 0);
				is_moved = true;
			}

			_chess_board.push_history_node(history_node);
		}

		//룩의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
        {
			List<Vector2> available_positions = new List<Vector2>();
			//각 방향에 대한 2차원 벡터 값
			Vector2[] dirs = new Vector2[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
			for (int i = 0; i < dirs.Length; i++)
			{
				for (int j = 0; ; j++)
				{
					Vector2 _position = position + dirs[i] * (j + 1);
					int to_go_piece_index = _chess_board.find_piece_at(_position);
                    if (Constants.check_error(_position))
                    {
						break;
                    }
					if (to_go_piece_index == Constants.FIND_PIECE_FAILURE)
					{
						available_positions.Add(_position);
					}
					else
					{
						if (_chess_board.Pieces[to_go_piece_index].Team != team)
						{
							available_positions.Add(_position);
						}
						break;
					}
				}
			}
			return available_positions;
		}
	}

	//나이트 클래스
	public class Knight : ChessPiece
	{

		public Knight(int _index, ChessTeam _team)
			: base(Constants.KNIGHT_NAME, _index, _team)
        {

        }

		public Knight(int _index, ChessTeam _team, Vector2 _position)
			: base(Constants.KNIGHT_NAME, _index, _team, _position)
		{

		}

		public Knight(int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
			: base(Constants.KNIGHT_NAME, _index, _team, _position, _is_moved)
		{

		}

		public Knight(int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.KNIGHT_NAME, _index, _team, _position, _is_moved, _is_dead)
		{

		}

		//나이트의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
        {
			List<Vector2> available_positions = new List<Vector2>();
			//각 방향에 대한 2차원 벡터 값
			Vector2[] dirs = new Vector2[] { 
				new Vector2(2, 1),
				new Vector2(2, -1), 
				new Vector2(-2, 1),
				new Vector2(-2, -1), 
				new Vector2(1, 2), 
				new Vector2(-1, 2), 
				new Vector2(1, -2), 
				new Vector2(-1, -2) };
			for (int i = 0; i < dirs.Length; i++)
			{
				Vector2 _position = position + dirs[i];
				if (Constants.check_error(_position))
				{
					continue;
				}
				int to_go_piece_index = _chess_board.find_piece_at(_position);
				if (to_go_piece_index == Constants.FIND_PIECE_FAILURE)
				{
					available_positions.Add(_position);
				}
				else
				{
					if (_chess_board.Pieces[to_go_piece_index].Team != team)
					{
						available_positions.Add(_position);
					}
					continue;
				}
			}
			return available_positions;
		}
	}

	//비숍 클래스
	public class Bishop : ChessPiece
	{
		public Bishop(int _index, ChessTeam _team)
			: base(Constants.BISHOP_NAME, _index, _team)
		{

		}

		public Bishop(int _index, ChessTeam _team, Vector2 _position)
			: base(Constants.BISHOP_NAME, _index, _team, _position)
		{

		}

		public Bishop(int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
			: base(Constants.BISHOP_NAME, _index, _team, _position, _is_moved)
		{

		}

		public Bishop(int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.BISHOP_NAME, _index, _team, _position, _is_moved, _is_dead)
		{

		}

		//비숍의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
        {
			List<Vector2> available_positions = new List<Vector2>();
			//각 방향에 대한 2차원 벡터 값
			Vector2[] dirs = new Vector2[] { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };
			for (int i = 0; i < dirs.Length; i++)
			{
				for (int j = 0; ; j++)
				{
					Vector2 _position = position + dirs[i] * (j + 1);
					if (Constants.check_error(_position))
					{
						break;
					}
					int to_go_piece_index = _chess_board.find_piece_at(_position);
					if (to_go_piece_index == Constants.FIND_PIECE_FAILURE) 
					{ 
						available_positions.Add(_position);
					}
					else
					{
						if (_chess_board.Pieces[to_go_piece_index].Team != team)
						{
							available_positions.Add(_position);
						}
						break;
					}
				}
			}
			return available_positions;
		}
	}

	//퀸 클래스
	public class Queen : ChessPiece
	{
		public Queen(int _index, ChessTeam _team)
			: base(Constants.QUEEN_NAME, _index, _team)
		{

		}

		public Queen(int _index, ChessTeam _team, Vector2 _position)
			: base(Constants.QUEEN_NAME, _index, _team, _position)
		{

		}

		public Queen(int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
			: base(Constants.QUEEN_NAME, _index, _team, _position, _is_moved)
		{

		}

		public Queen(int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.QUEEN_NAME, _index, _team, _position, _is_moved, _is_dead)
		{

		}

		//퀸의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
        {
			List<Vector2> available_positions = new List<Vector2>();
			//각 방향에 대한 2차원 벡터 값
			Vector2[] dirs = new Vector2[] { 
				new Vector2(1, 1), 
				new Vector2(1, -1), 
				new Vector2(-1, 1), 
				new Vector2(-1, -1), 
				Vector2.left,
				Vector2.right,
				Vector2.up, 
				Vector2.down };
			for (int i = 0; i < dirs.Length; i++)
			{
				for (int j = 0; ; j++)
				{
					Vector2 _position = position + dirs[i] * (j + 1);
					if (Constants.check_error(_position))
					{
						break;
					}
					int to_go_piece_index = _chess_board.find_piece_at(_position);
					if (to_go_piece_index == Constants.FIND_PIECE_FAILURE)
					{
						available_positions.Add(_position);
					}
					else
					{
						if (_chess_board.Pieces[to_go_piece_index].Team != team)
						{
							available_positions.Add(_position);
						}
						break;
					}
				}
			}
			return available_positions;
		}
	}

	//킹 클래스
	public class King : ChessPiece
	{
		public King(int _index, ChessTeam _team)
			: base(Constants.KING_NAME, _index, _team)
        {

        }

		public King(int _index, ChessTeam _team, Vector2 _position)
			: base(Constants.KING_NAME, _index, _team, _position)
		{

		}

		public King(int _index, ChessTeam _team, Vector2 _position, bool _is_moved)
			: base(Constants.KING_NAME, _index, _team, _position, _is_moved)
		{

		}

		public King(int _index, ChessTeam _team, Vector2 _position, bool _is_moved, bool _is_dead)
			: base(Constants.KING_NAME, _index, _team, _position, _is_moved, _is_dead)
		{

		}

		//킹의 이동 가능 좌표
		public override List<Vector2> get_available_position(ChessBoard _chess_board)
        {
			List<Vector2> available_positions = new List<Vector2>();
			//각 방향에 대한 2차원 벡터 값
			Vector2[] dirs = new Vector2[] {
				new Vector2(1, 1),
				new Vector2(1, -1),
				new Vector2(-1, 1),
				new Vector2(-1, -1),
				Vector2.left,
				Vector2.right,
				Vector2.up,
				Vector2.down };
			for (int i = 0; i < dirs.Length; i++)
			{
				Vector2 _position = position + dirs[i];
				if (Constants.check_error(_position))
				{
					continue;
                }
				int to_go_piece_index = _chess_board.find_piece_at(_position);
				if (to_go_piece_index == Constants.FIND_PIECE_FAILURE)
				{
					available_positions.Add(_position);
				}
				else
				{
					if (_chess_board.Pieces[to_go_piece_index].Team != team)
					{
						available_positions.Add(_position);
					}
					continue;
				}
			}

			//캐슬링 체크

			Vector2[] short_castling = new Vector2[] {
				Vector2.right,
				new Vector2(2, 0),
			};

			Vector2 short_castling_position = new Vector2(2, 0);

			Vector2[] long_castling = new Vector2[] {
				Vector2.left,
				new Vector2(-2, 0),
				new Vector2(-3, 0),
			};

			Vector2 long_castling_position = new Vector2(-2, 0);

			if (is_moved)
			{
				return available_positions;
			}

			if (team.Team_Index == Constants.WHITE_TEAM_INDEX)
			{
				if (_chess_board.Is_White_Checked)
				{
					return available_positions;
				}
			}
			if (team.Team_Index == Constants.BLACK_TEAM_INDEX)
			{
				if (_chess_board.Is_Black_Checked)
				{
					return available_positions;
				}
			}

			bool is_short_castling = true;
			bool is_long_castling = true;

			//킹사이드 체크
			for (int i = 0; i < short_castling.Length; i++)
			{
				if (_chess_board.find_piece_at(position + short_castling[i]) != Constants.FIND_PIECE_FAILURE)
				{
					is_short_castling = false;
					break;
				}
			}

			if (is_short_castling)
			{
				int piece_index = _chess_board.get_rook(Constants.KING_SIDE_ROOK_INDEX, team);
				if (piece_index != Constants.FIND_PIECE_FAILURE)
				{
					if (!_chess_board.Pieces[piece_index].Is_Moved)
					{
						available_positions.Add(position + short_castling_position);
					}
				}
			}

			//퀸사이드 체크
			for (int i = 0; i < long_castling.Length; i++)
			{
				if (_chess_board.find_piece_at(position + long_castling[i]) != Constants.FIND_PIECE_FAILURE)
				{
					is_long_castling = false;
					break;
				}
			}

			//캐슬링 4번 규칙 적용

			if (is_long_castling)
			{
				int piece_index = _chess_board.get_rook(Constants.QUEEN_SIDE_ROOK_INDEX, team);
				if (piece_index != Constants.FIND_PIECE_FAILURE)
				{
					if (!_chess_board.Pieces[piece_index].Is_Moved)
					{
						available_positions.Add(position + long_castling_position);
					}
				}
			}

			return available_positions;
		}

		//킹의 전용 움직임 함수(캐슬링 지원)
		public override bool move_to(Vector2 _position, ref ChessBoard _chess_board)
        {
			HistoryNode history_node = new HistoryNode(index);
			//목표 지점에 갈수 있는지
			if (is_position_available(_chess_board, _position))
			{
				//기물 이동 정보 생성
				history_node.Last_Position = position;
				history_node.Last_Is_Moved = is_moved;
				history_node.Last_Current_Team = new ChessTeam(_chess_board.Current_Team.Team_Index);

				//목표 지점에 적 기물이 있는지
				int to_go_piece_index = _chess_board.find_piece_at(_position);
				if (to_go_piece_index != Constants.FIND_PIECE_FAILURE)
				{
					//목표 지점의 기물이 같은 팀인지
					if (_chess_board.Pieces[to_go_piece_index].Team == _chess_board.Current_Team)
					{
						return false;
					}
					_chess_board.Pieces[to_go_piece_index].Is_Dead = true;

					history_node.Dead_Piece_Index = _chess_board.Pieces[to_go_piece_index].Index;
				}

				//캐슬링을 했다면
				if (Vector2.Distance(position, _position) == 2)
				{
					int piece_index = Constants.FIND_PIECE_FAILURE;

					if (position.x - _position.x < 0)
					{
						piece_index = _chess_board.get_rook(Constants.KING_SIDE_ROOK_INDEX, team);
					}
					if (position.x - _position.x > 0)
					{
						piece_index = _chess_board.get_rook(Constants.QUEEN_SIDE_ROOK_INDEX, team);
					}

					if (piece_index != Constants.FIND_PIECE_FAILURE)
					{
						((Rook)_chess_board.Pieces[piece_index]).castling(ref _chess_board);
					}
					history_node.Is_Castling = true;
				}

				position = _position;
				is_moved = true;

				_chess_board.push_history_node(history_node);

				return true;
			}
			return false;
		}
	}

	//보드판
	public class ChessBoard
	{
		private bool is_promotion;
		public bool Is_Promotion
        {
            get
            {
				return is_promotion;
            }
        }

		//보드판의 기물들
		private List<ChessPiece> pieces;
		public List<ChessPiece> Pieces
        {
            get
            {
				return pieces;
            }
        }

		//현재 팀
		private ChessTeam current_team;
		public ChessTeam Current_Team
        {
            get
            {
				return current_team;
            }
            set
            {
				current_team = value;
            }
        }

		//현재 선택된 기물
		private int selected_piece_index;

		//화이트 팀 체크 상태
		private bool is_white_checked;
		public bool Is_White_Checked
        {
            get
            {
				return is_white_checked;
            }
            set
            {
				is_white_checked = value;
            }
        }

		//블랙 팀 체크 상태
		private bool is_black_checked;
		public bool Is_Black_Checked
		{
			get
			{
				return is_black_checked;
			}
			set
			{
				is_black_checked = value;
			}
		}

		//대국 기록 스택
		private Stack<HistoryNode> history;

		public ChessBoard()
        {
			reset();
        }

		//_position이 공격 받는중 인지 체크
		public bool is_attacked_at_by(Vector2 _position, ChessTeam _team)
		{
			for (int j = 0; j < pieces.Count; j++)
			{
				if (pieces[j].Team == _team && !pieces[j].Is_Dead)
				{
					if (pieces[j].is_position_attackable(this, _position))
                    {
						return true;
                    }
                }
            }
			return false;
        }

		//팀이 체크되어있는지 체크
		public bool is_checked(ChessTeam _team)
        {
			int piece_index = get_piece(Constants.KING_NAME, _team);
			if(piece_index == Constants.FIND_PIECE_FAILURE)
            {
				set_team_is_checked(_team, false);
				return false;
            }
			if(is_attacked_at_by(pieces[piece_index].Position, _team.reverse()))
			{
				set_team_is_checked(_team, true);
				return true;
			}
			set_team_is_checked(_team, false);
			return false;
        }

		//보드 리셋
		public void reset()
        {
			current_team = new ChessTeam(Constants.WHITE_TEAM_INDEX);

			is_white_checked = false;
			is_black_checked = false;

			pieces = new List<ChessPiece>();
			history = new Stack<HistoryNode>();

			selected_piece_index = Constants.NONE_PIECE;

			for (int is_black = 0; is_black < 2; is_black++)
			{
				//폰 초기화
				for (int i = 0; i < 8; i++)
				{
					pieces.Add(new Pawn(pieces.Count, new ChessTeam(is_black), new Vector2(i, 1 + is_black * 5)));
				}
				//룩 초기화
				for (int i = 0; i < 2; i++)
				{
					pieces.Add(new Rook(i, pieces.Count, new ChessTeam(is_black), new Vector2(i * 7, is_black * 7)));
				}
				//나이트 초기화
				for (int i = 0; i < 2; i++)
				{
					pieces.Add(new Knight(pieces.Count, new ChessTeam(is_black), new Vector2(1 + i * 5, is_black * 7)));
				}
				//비숍 초기화
				for (int i = 0; i < 2; i++)
				{
					pieces.Add(new Bishop(pieces.Count, new ChessTeam(is_black), new Vector2(2 + i * 3, is_black * 7)));
				}
				//퀸 초기화
				pieces.Add(new Queen(pieces.Count, new ChessTeam(is_black), new Vector2(3, is_black * 7)));
				//킹 초기화
				pieces.Add(new King(pieces.Count, new ChessTeam(is_black), new Vector2(4, is_black * 7)));
			}

			while ((history.Count > 0))
			{
				history.Pop();
			}
		}

		//체스 기물 선택
		public bool select_piece(Vector2 _position)
		{
			if (Constants.check_error(_position))
			{
				selected_piece_index = Constants.NONE_PIECE;
				return false;
			}
			selected_piece_index = find_piece_at(_position);
			if (selected_piece_index != Constants.FIND_PIECE_FAILURE)
			{
				if (pieces[selected_piece_index].Team != current_team)
				{
					selected_piece_index = Constants.NONE_PIECE;
					return false;
				}
            }
            else
            {
				return false;
            }
			return true;
		}

		//체스 기물 가져오기
		public int get_piece(string _name, ChessTeam _team)
        {
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].Name == _name && pieces[i].Team == _team && !pieces[i].Is_Dead)
				{
					return i;
				}
			}
			return Constants.FIND_PIECE_FAILURE;
        }

		//팀 변환
		public void reverse_current_team()
        {
			if (current_team.Team_Index == Constants.WHITE_TEAM_INDEX)
			{
				current_team.Team_Index = Constants.BLACK_TEAM_INDEX;
			}
			else if (current_team.Team_Index == Constants.BLACK_TEAM_INDEX)
			{
				current_team.Team_Index = Constants.WHITE_TEAM_INDEX;
			}
			selected_piece_index = Constants.NONE_PIECE;
		}

		//기물 가져오기
		public int get_rook(int _rook_index, ChessTeam _team)
        {
			for (int i = 0; i < pieces.Count; i++)
			{
				if(pieces[i].Name == Constants.ROOK_NAME)
                {
					Rook piece = (Rook)pieces[i];
					if(piece.Rook_Index == _rook_index && piece.Team == _team && !piece.Is_Dead)
                    {
						return i;
                    }
                }
			}
			return Constants.FIND_PIECE_FAILURE;
		}

		//좌표에 기물이 있는지 확인
		public int find_piece_at(Vector2 _position)
        {
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].Position == _position)
				{
					if (!pieces[i].Is_Dead)
					{
						return i;
					}
				}
			}
			return Constants.FIND_PIECE_FAILURE;
		}

		//체스 기물을 이동시킴
		public bool move_piece_to(Vector2 _to_go, ref ChessBoard _chess_board)
        {
            if (!is_promotion)
            {
				if (selected_piece_index == Constants.NONE_PIECE)
				{
					return false;
				}
				if (Constants.check_error(_to_go))
				{
					return false;
				}
				//선택된 기물이 현재 팀의 기물인지
				if (pieces[selected_piece_index].Team == current_team)
				{
					if (pieces[selected_piece_index].move_to(_to_go, ref _chess_board))
					{
						if (is_checked(current_team))
						{
							roll_back();
							return false;
						}
						is_checked(current_team.reverse());
						if (get_promotion_piece_index() != Constants.FIND_PIECE_FAILURE)
						{
							is_promotion = true;
						}
						else
						{
							reverse_current_team();
						}
						return true;
					}
				}
			}
			return false;
		}

		private void set_team_is_checked(ChessTeam _team, bool _is_checked)
        {
			if (_team.Team_Index == Constants.WHITE_TEAM_INDEX)
			{
				is_white_checked = _is_checked;
			}
			if (_team.Team_Index == Constants.BLACK_TEAM_INDEX)
			{
				is_black_checked = _is_checked;
			}
		}

		//기물 이동 정보 스택에 추가
		public void push_history_node(HistoryNode _history_node)
        {
			history.Push(_history_node);
        }

		//프로모션
		public bool promote_at(int _index, string _piece_name)
		{
			if (is_promotion_available_at(_index))
			{
				HistoryNode history_node = new HistoryNode(_index);
				history_node.Is_Promotion = true;
				switch (_piece_name)
                {
					case Constants.ROOK_NAME:
						pieces[_index] = new Rook(Constants.KING_SIDE_ROOK_INDEX, _index, pieces[_index].Team, pieces[_index].Position, true);
						break;
					case Constants.KNIGHT_NAME:
						pieces[_index] = new Knight(_index, pieces[_index].Team, pieces[_index].Position, true);
						break;
					case Constants.BISHOP_NAME:
						pieces[_index] = new Bishop(_index, pieces[_index].Team, pieces[_index].Position, true);
						break;
					case Constants.QUEEN_NAME:
						pieces[_index] = new Queen(_index, pieces[_index].Team, pieces[_index].Position, true);
						break;
				}
				is_promotion = false;
				history.Push(history_node);
				is_checked(current_team.reverse());
				reverse_current_team();
				return true;
            }
			return false;
        }

		//프로모션 가능 여부체크
		public bool is_promotion_available_at(int _index)
        {
			if (pieces[_index].Name == Constants.PAWN_NAME)
			{
				if (((Pawn)pieces[_index]).is_promotion_available())
				{
					return true;
				}
			}
			return false;
		}

		//프로모션 가능한 폰의 인덱스
		public int get_promotion_piece_index()
        {
			for(int i = 0; i < pieces.Count; i++)
            {
                if (is_promotion_available_at(i))
                {
					return i;
                }
            }
			return Constants.FIND_PIECE_FAILURE;
        }

		//롤백
		public bool roll_back()
        {
			if (history.Count == 0)
			{
				return false;
			}

			HistoryNode history_node = history.Pop();
			ChessPiece piece = pieces[history_node.Piece_Index];

            if (history_node.Is_Promotion)
            {
				pieces[history_node.Piece_Index] = 
					new Pawn(piece.Index, piece.Team, piece.Position, true);
				roll_back();
			}
            else
            {
				pieces[history_node.Piece_Index].Position = history_node.Last_Position;
				pieces[history_node.Piece_Index].Is_Moved = history_node.Last_Is_Moved;

				if (history_node.Dead_Piece_Index != Constants.NONE_PIECE)
				{
					pieces[history_node.Dead_Piece_Index].Is_Dead = false;
				}
				if (history_node.Is_Castling)
				{
					roll_back();
				}
				else
				{
					current_team = history_node.Last_Current_Team;
				}
			}
			is_checked(current_team);
			return true;
		}
	}
}