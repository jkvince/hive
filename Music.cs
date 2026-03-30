using Godot;
using System;
using System.Diagnostics;

public class Music : AudioStreamPlayer {
	private Timer _timer;
	private int _musicIndex;

	public override void _Ready() {
		_timer = GetNode<Timer>("Timer");
		_musicIndex = 0;
	}
	
	private void _on_Music_finished() {
		_timer.Start();
		Stop();
		NextSong();
		var stream = (AudioStream) GD.Load(IndexToFile(_musicIndex));
		Debug.Assert(stream != null);
		Stream = stream;
	}
	
	private void _on_Timer_timeout() {
		Play();
	}

	private void NextSong() {
		_musicIndex++;
		if (_musicIndex == 4) {
			_musicIndex = 0;
		}
	}

	private static string IndexToFile(int index) {
		switch (index) {
			case 0:
				return "audio/lofi-jazz-cafe-327791.mp3";
			case 1:
				return "audio/sergequadrado-moment-14023.mp3";
			case 2:
				return "audio/u_ra02qyxlvr-rain-between-us-501934.mp3";
			case 3:
				return "audio/adiiswanto-sunset-chill-jazzsmooth-jazz-465311.mp3";
			default:
				throw new ArgumentOutOfRangeException(nameof(index), index, null);
		}
	}


}