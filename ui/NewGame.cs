using Godot;
using System;

public class NewGame : MenuAbstract
{
	private HSlider _difficultySlider;
	public override void _Ready() {
		base._Ready();
		_difficultySlider = GetNode<HSlider>("HFlowContainer/DifficultySlider");
	}
	
	private void _on_StartButton_pressed()
	{
		Main.SetMenu(Main.Menus.InGame);
		
		var node = (Spatial) ((PackedScene) ResourceLoader.Load("res://board/Board.tscn")).Instance();
		Main.AddChild(node);
	}
	
	private void _on_DifficultySlider_drag_ended(bool value_changed)
	{
		if (value_changed) {
			var newValue = (int) _difficultySlider.Value;
			GetNode<GameLogic>("/root/Main/GameLogic").Difficulty = (GameLogic.Difficulties) newValue;
		}
	}

	private void _on_BackButton_pressed()
	{
		Main.SetMenu(Main.Menus.Main);
	}


}


