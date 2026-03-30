using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public class Board : Spatial {
	
	private Camera _camera;
	private bool _isPanning;
	private Spatial _wireframeSelection;
	private Node _gameLogic;
	private Spatial _selection;
	
	public override void _Ready() {
		_camera = GetNode<Camera>("/root/Main/Camera");
		_wireframeSelection = GetNode<Spatial>("Selection");
		_gameLogic = GetNode<Node>("/root/Main/GameLogic");
		
		_state = States.NoSelected;
		_isPanning = false;

		_selection = GetNode<Spatial>("Selection");
		//_selection.Visible = false;
	}
	
	enum States {
		NoSelected,
		HandSelected,
		BoardSelected,
		MovingPiece,
		WaitingForOpponent
	}
	private States _state;
	
	/*
	public override void _Process(float delta) {
		switch (_state) {
			case States.NoSelected:
				NoSelected();
				return;
			case States.HandSelected:
				HandSelected();
				return;
			case States.BoardSelected:
				BoardSelected();
				return;
			case States.MovingPiece:
				MovingPiece();
				return;
			case States.WaitingForOpponent:
				return;
		}
		
		var mousePos = GetViewport().GetMousePosition();
		
		var boardPlane = new Plane(Vector3.Up, 0);

		var rayFrom = _camera.ProjectRayOrigin(mousePos);
		var rayDir = _camera.ProjectRayNormal(mousePos);
		var hitPos = boardPlane.IntersectRay(rayFrom, rayDir);
		if (hitPos != null) {
			var actualPos = (Vector3) hitPos;
			var snap = SnapCoordinatesToGrid(new Vector2(actualPos.x, actualPos.z));
			_wireframeSelection.Position = new Vector3(snap.x, 0, snap.y);
		}
		
	}
	*/
	
	public override void _Input(InputEvent @event) {
		switch (_state) {
			case States.NoSelected:
				NoSelected(@event);
				break;
		}
	}
	
	private void NoSelected(InputEvent @event) {
		if (@event is InputEventMouseButton mouseEvent) {
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == (int) ButtonList.Left) ShootRay();
			if (mouseEvent.ButtonIndex == (int)ButtonList.WheelUp) ZoomIn();
			if (mouseEvent.ButtonIndex == (int)ButtonList.WheelDown) ZoomOut();
			if (mouseEvent.ButtonIndex == (int)ButtonList.Right) _isPanning = mouseEvent.Pressed;
		}

		if (@event is InputEventMouseMotion mouseMotion && _isPanning) {
			var relative = mouseMotion.Relative;
			var movement = new Vector3(-relative.x, 0, -relative.y);
			_camera.Translation += movement * 0.01f;
		}
	}

	private void HandSelected() {
		
	}

	private void BoardSelected() {
		
	}

	private void MovingPiece() {
		
	}


	private void ShootRay() {
		var mousePos = GetViewport().GetMousePosition();
		var boardPlane = new Plane(Vector3.Up, 0);

		var rayFrom = _camera.ProjectRayOrigin(mousePos);
		var rayDir = _camera.ProjectRayNormal(mousePos);
		var hitPos = boardPlane.IntersectRay(rayFrom, rayDir);
		if (hitPos != null) {
			var actualPos = (Vector3) hitPos;
			//SpawnTileAt(actualPos, (PieceType) _currentPiece, _currentPlayer);
		}
	}

	public RigidBody SpawnTileAt(Vector3 position, string materialString, bool whitePiece) {
		var pos = SnapCoordinatesToGrid(new Vector2(position.x, position.z));
		var cube_pos = RealCoordinatesToCube(pos);
		
		// have the check in logic
		var node = (RigidBody)((PackedScene)ResourceLoader.Load("res://Pieces/HexTile.tscn")).Instance();
		var mesh = node.GetNode<MeshInstance>("hex");
			
		var material = new SpatialMaterial();
		material.DetailEnabled = true;
		var texture = (Texture) GD.Load(materialString);
		material.DetailAlbedo = texture;
		material.Uv1Offset = new Vector3(0.5f, 0, 0.5f);
		material.Uv1Scale = new Vector3(0.5f, 0.5f, 0.5f);
		material.Uv1Triplanar = true;
		material.Uv1TriplanarSharpness = 150f;
		if (whitePiece) {
			material.AlbedoColor = new Color(255, 255, 255);
		}
		else {
			material.AlbedoColor = new Color(0, 0, 0);
		}
		
		mesh.SetSurfaceMaterial(0, material);
			
		AddChild(node);
		node.Translation = new Vector3(pos.x, 2, pos.y);
		
		return node;
		
	}

	private const float CameraHeightMax = 15;
	private const float CameraHeightMin = 5;

	private void ZoomIn() {
		var y = _camera.Position.y - 2f;
		if (y < CameraHeightMin) y = CameraHeightMin;

		var tween = CreateTween();
		tween.TweenProperty(_camera, "translation:y", y, 0.1f)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Sine);
	}

	private void ZoomOut() {
		var y = _camera.Position.y + 2f;
		if (y > CameraHeightMax) y = CameraHeightMax;
		var tween = CreateTween();
		tween.TweenProperty(_camera, "translation:y", y, 0.1f)
			.SetEase(Tween.EaseType.InOut)
			.SetTrans(Tween.TransitionType.Sine);
	}


	private const float PieceSize = 1.15f;

	private static Vector2 CubeCoordinatesTo3D(Vector3 pos) {
		// (q, r, s)
		return new Vector2(PieceSize * 3 / 2 * pos.x, PieceSize * (pos.y + pos.x / 2) * (float)Math.Sqrt(3));
	}

	private static Vector3 RealCoordinatesToCube(Vector2 pos) {
		var q = (int)(pos.x / PieceSize * 2 / 3);
		var r = (int)((pos.y / PieceSize * 2 / (float)Math.Sqrt(3)) - q) / 2;
		var s = -q - r;
		return new Vector3(q, r, s);
	}

	private static Vector2 SnapCoordinatesToGrid(Vector2 position) {
		var pos = RealCoordinatesToCube(new Vector2(position.x, position.y));
		var pos2 = CubeCoordinatesTo3D(pos);
		return pos2;
	}
	
	public string PieceTypeString(GameLogic.PieceType type) {
		switch (type) {
			case GameLogic.PieceType.QueenBee:
				return "res://Pieces/queenbee.svg";
			case GameLogic.PieceType.Ant:
				return "res://Pieces/ant.svg";
			case GameLogic.PieceType.Grasshopper:
				return "res://Pieces/grasshopper.svg";
			case GameLogic.PieceType.Spider:
				return "res://Pieces/spider.svg";
			case GameLogic.PieceType.Beetle:
				return "res://Pieces/beetle.svg";
			case GameLogic.PieceType.Ladybug:
				return "res://Pieces/ladybug.svg";
			case GameLogic.PieceType.Mosquito:
				return "res://Pieces/mosquito.svg";
			case GameLogic.PieceType.Pillbug:
				return "res://Pieces/pillbug.svg";
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}
	}
}
