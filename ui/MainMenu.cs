using Godot;
using System;

public class MainMenu : Control
{
	private void _on_StartButton_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.NewGame);
	}

	private void _on_OptionsButton_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.Options);
	}
	
	private void _on_RulesButton_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.Rules);
	}
	
	private void _on_CreditsButton_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.Credits);
	}
}

