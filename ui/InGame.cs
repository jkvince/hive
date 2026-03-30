using Godot;
using System;

public class InGame : Control
{
	public override void _Ready()
	{
		
	}
	
	private void _on_Grasshopper_gui_input(object @event)
	{
		GD.Print("clicked");
	}
	
	private void _on_QueenBee_gui_input(object @event)
	{
		GD.Print("clicked");
	}


}

