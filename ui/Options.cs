using Godot;
using System;

public class Options : MenuAbstract
{
	private int MusicIndex;
	private int SfxIndex;
	
	private HSlider _musicSlider;
	private HSlider _sfxSlider;
	private CheckButton _fullscreenCheck;
	
	public override void _Ready()
	{
		base._Ready();
		MusicIndex = AudioServer.GetBusIndex("music");
		SfxIndex = AudioServer.GetBusIndex("sfx");
		
		
		_musicSlider = GetNode<HSlider>("HFlowContainer/MusicSlide");
		_sfxSlider = GetNode<HSlider>("HFlowContainer/SfxSlide");
		_fullscreenCheck = GetNode<CheckButton>("HFlowContainer/FullscreenCheck");
		
		_musicSlider.Value = DbToRange(AudioServer.GetBusVolumeDb(MusicIndex));
		_sfxSlider.Value = DbToRange(AudioServer.GetBusVolumeDb(SfxIndex));
		_fullscreenCheck.Pressed = OS.WindowFullscreen;
	}
	
	
	private void _on_BackButton_pressed()
	{
		Main.SetMenu(Main.Menus.Main);
	}
	
	private void _on_FullscreenCheck_toggled(bool button_pressed)
	{
		OS.WindowFullscreen = button_pressed;
	}
	
	private void _on_MusicSlide_value_changed(float value)
	{
		AudioServer.SetBusVolumeDb(MusicIndex, RangeToDb(value));
	}
	
	private void _on_SfxSlide_value_changed(float value)
	{
		AudioServer.SetBusVolumeDb(SfxIndex, RangeToDb(value));
	}

	private static float RangeToDb(float range) {
		return - (float) Math.Pow(80 , (1 - range / 100));
	}

	private static float DbToRange(float db) {
		return (float) (1 - Math.Log(-db, 80)) * 100;
	}
}


