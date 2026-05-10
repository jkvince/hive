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
		LoadMusic(_musicIndex);
	}
	
	private void _on_Timer_timeout() {
		Play();
		// start downloading the next song
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
				return "lofi-jazz-cafe-327791.mp3";
			case 1:
				return "sergequadrado-moment-14023.mp3";
			case 2:
				return "u_ra02qyxlvr-rain-between-us-501934.mp3";
			case 3:
				return "adiiswanto-sunset-chill-jazzsmooth-jazz-465311.mp3";
			default:
				throw new ArgumentOutOfRangeException(nameof(index), index, null);
		}
	}

	private void LoadMusic(int fileindex) {
		string filename = IndexToFile(fileindex);
		if (OS.GetName() == "HTML5" || OS.GetName() == "Web") {
			HTTPRequest http = new 	HTTPRequest();
			AddChild(http);
			http.Connect("request_completed", this, nameof(RequestCompleted));

			Error err = http.Request("https://jkvince.github.io/hive/lazy-loaded/" + filename);
			if (err != Error.Ok) {
				GD.Print("Error");
			}
		} else {
			var stream = (AudioStream) GD.Load("build/" + filename);
			Stream = stream;
		}
	}

	private void RequestCompleted(int result, int responseCode, string[] headers, byte[] body) {
		if (responseCode != 200) {
			GD.Print("failed download");
		}

		var stream = new AudioStreamMP3();
		stream.Data = body;
		Stream = stream;

	}

}