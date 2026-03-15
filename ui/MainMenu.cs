using Godot;
using System;

public class MainMenu : MenuAbstract
{
	private void _on_StartButton_pressed()
	{
		Main.SetMenu(Main.Menus.NewGame);
	}

	private void _on_OptionsButton_pressed()
	{
		Main.SetMenu(Main.Menus.Options);
	}
	
	private void _on_RulesButton_pressed()
	{
		Main.SetMenu(Main.Menus.Rules);
	}
}



