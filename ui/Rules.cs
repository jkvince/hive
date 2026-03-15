using Godot;
using System;

public class Rules : MenuAbstract
{
	private void _on_BackButton_pressed()
	{
		Main.SetMenu(Main.Menus.Main);
	}

}


