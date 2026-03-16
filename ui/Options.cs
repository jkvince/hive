using Godot;
using System;

public class Options : MenuAbstract
{
	private CheckButton _musicCheck;
	private CheckButton _sfxCheck;
	private CheckButton _fullscreenCheck;
	
	public override void _Ready()
	{
		base._Ready();
		_musicCheck = GetNode<CheckButton>("MusicCheck");
		_sfxCheck = GetNode<CheckButton>("SfxCheck");
		_fullscreenCheck = GetNode<CheckButton>("FullscreenCheck");
		
		_musicCheck.Pressed = !AudioServer.IsBusMute(AudioServer.GetBusIndex("music"));
		_sfxCheck.Pressed = !AudioServer.IsBusMute(AudioServer.GetBusIndex("sfx"));
		_fullscreenCheck.Pressed = OS.WindowFullscreen;
	}
	
	private void _on_CheckButton_toggled(bool button_pressed)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("sfx"), !button_pressed);
	}
	
	private void _on_MusicCheck_toggled(bool button_pressed)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("music"), !button_pressed);
	}
	
	private void _on_BackButton_pressed()
	{
		Main.SetMenu(Main.Menus.Main);
	}
	
	private void _on_FullscreenCheck_toggled(bool button_pressed)
	{
		OS.WindowFullscreen = button_pressed;
	}

}


