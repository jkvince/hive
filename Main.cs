using Godot;
using System;
using Array = Godot.Collections.Array;

public class Main : Spatial
{
	private AudioStreamPlayer _audioStreamPlayer;
	private Control _currentMenu;

	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_audioStreamPlayer.VolumeDb = -80;
		var tween = CreateTween();
		tween.TweenProperty(_audioStreamPlayer, "volume_db", -15.0f, 3.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Expo);
	}


}
