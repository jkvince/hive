using Godot;
using System;
using Array = Godot.Collections.Array;

public class Main : Spatial
{
	private AudioStreamPlayer _audioStreamPlayer;
	private Control _currentMenu;

	private bool _fistClick;

	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("Music");
		_audioStreamPlayer.VolumeDb = -80;
		_fistClick = false;
		
		var node = (Control)((PackedScene)ResourceLoader.Load(MenuToString(Menus.Main))).Instance();
		AddChild(node);
		MoveInMenu(node);
		_currentMenu = node;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == (int)ButtonList.Left)
		{
			if (!_fistClick) {
				var tween = CreateTween();
				tween.TweenProperty(_audioStreamPlayer, "volume_db", -15.0f, 3.5f)
					.SetEase(Tween.EaseType.Out)
					.SetTrans(Tween.TransitionType.Expo);
				_fistClick = true;
				_audioStreamPlayer.Play();
			}
		}
	}
	

	public enum Menus {
		Main,
		NewGame,
		Options,
		Rules,
		Credits,
		InGame
	}
	
	public void SetMenu(Menus menu) {
		var node = (Control)((PackedScene)ResourceLoader.Load(MenuToString(menu))).Instance();
		AddChild(node);
		MoveInMenu(node);
		MoveOutMenu(_currentMenu);
		_currentMenu = node;
	}
	
	private static string MenuToString(Menus menu) {
		switch (menu) {
			case Menus.Main:
				return "res://ui/MainMenu.tscn";
			case Menus.NewGame:
				return "res://ui/NewGame.tscn";
			case Menus.Options:
				return "res://ui/Options.tscn";
			case Menus.Rules:
				return "res://ui/Rules.tscn";
			case Menus.Credits:
				return "res://ui/Credits.tscn";
			case Menus.InGame:
				return "res://ui/InGame.tscn";
			default:
				throw new ArgumentOutOfRangeException(nameof(menu), menu, null);
		}
	}
	
	private void MoveInMenu(Control node) {
		var res = OS.WindowSize.y;
		node.SetPosition(new Vector2(0, -res));
		var tween = CreateTween();
		tween.TweenProperty(node, "rect_position", new Vector2(0, 0), 1.3f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Circ);
	}
	
	private void MoveOutMenu(Control node) {
		var res = OS.WindowSize.y;
		var tween = CreateTween();
		tween.TweenProperty(node, "rect_position", new Vector2(0, res * 2), 0.4f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Sine);

		tween.TweenCallback(this, nameof(RemoveMenu), new Array(node));
	}
	
	private void RemoveMenu(Control node) {
		node.QueueFree();
	}

}
