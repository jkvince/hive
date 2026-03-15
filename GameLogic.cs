using Godot;
using System;
using System.Collections.Generic;

public class GameLogic : Node
{
	public int TurnNumber;
	
	private int _currentPiece;
	private bool _currentPlayer;

	public enum Difficulties {
		Easy,
		Normal,
		Hard,
	}
	public Difficulties Difficulty;

	public int[] Player1Pieces = { };
	public int[] Player2Pieces = { };
	public readonly int[] StartingPieces = {
		1, // Queenbee
		3, // Ant
		3, // Grasshopper
		2, // Spider
		2, // Beetle
		1, // Ladybug
		1, // Mosquito
		1  // Pillbug
	};
	
	public Dictionary<int, Piece> BoardHash = new Dictionary<int, Piece>();
	public PieceCoordinate QueenPlayer1;
	public PieceCoordinate QueenPlayer2;
	
	public override void _Ready()
	{
		_currentPiece = 0;
		_currentPlayer = true;

		TurnNumber = 0;

		Player1Pieces = StartingPieces;
		Player2Pieces = StartingPieces;
		
	}

	//public List<PieceCoordinate> GetPossiblePositions(RigidBody node);
	
	private Piece GetPieceAt(PieceCoordinate coordinate) {
		var hash = coordinate.GetHashCode();
		if (BoardHash.ContainsKey(hash)) {
			return BoardHash[hash];
		}
		else {
			throw new KeyNotFoundException();
		}
	}

	public RigidBody GetPieceNodeAt(PieceCoordinate coordinate) {
		var piece = GetPieceAt(coordinate);
		return piece.NodeReference;
	}

	private bool IsTherePieceAt(PieceCoordinate coordinate) {
		var hash = coordinate.GetHashCode();
		return BoardHash.ContainsKey(hash);
	}

	private bool SetPiece(Piece piece) {
		var hash = piece.Coordinate.GetHashCode();
		if (!IsTherePieceAt(piece.Coordinate)) {
			BoardHash[hash] = piece;
			return true;
		}
		return false;
	}
	
	// --- Piece Structs
	
	public enum PieceType {
		QueenBee = 1,
		Ant,
		Grasshopper,
		Spider,
		Beetle,
		Ladybug,
		Mosquito,
		Pillbug,
	}
	
	public struct PieceCoordinate {
		public int Q;
		public int R;
		public int S;

		public PieceCoordinate(int q, int r, int s) {
			if (q + r + s != 0) throw new ArgumentException("not valid cube coordinate");
			Q = q;
			R = r;
			S = s;
		}

		public static bool operator ==(PieceCoordinate c1, PieceCoordinate c2) {
			return c1.GetHashCode() == c2.GetHashCode();
		}

		public static bool operator !=(PieceCoordinate c1, PieceCoordinate c2) {
			return c1.GetHashCode() != c2.GetHashCode();
		}

		public static PieceCoordinate operator +(PieceCoordinate c1, PieceCoordinate c2) {
			return new PieceCoordinate(
				c1.Q + c2.Q,
				c1.R + c2.R,
				c1.S + c2.S);
		}

		public override int GetHashCode() {
			return (Q * 31 + R) * 31 + S;
		}
	}
	
	public readonly PieceCoordinate East = new PieceCoordinate(1, 0, -1);
	public readonly PieceCoordinate NorthEast = new PieceCoordinate(1, -1, 0);
	public readonly PieceCoordinate NorthWest = new PieceCoordinate(0, -1, 1);
	public readonly PieceCoordinate West = new PieceCoordinate(-1, 0, 1);
	public readonly PieceCoordinate SouthWest = new PieceCoordinate(-1, 1, 0);
	public readonly PieceCoordinate SouthEast = new PieceCoordinate(0, 1, -1);
	
	public struct Piece {
		public PieceType Type;
		public PieceCoordinate Coordinate;
		public bool Player1;
		public int Height;
		public RigidBody NodeReference;

		public Piece(PieceType type, PieceCoordinate coordinate, bool player1, RigidBody node, int height) {
			Type = type;
			Coordinate = coordinate;
			Player1 = player1;
			Height = height;
			NodeReference = node;
		}

		public Piece(PieceType type, PieceCoordinate coordinate, bool player1, RigidBody node) {
			Type = type;
			Coordinate = coordinate;
			Player1 = player1;
			Height = 0;
			NodeReference = node;
		}
	}
}
