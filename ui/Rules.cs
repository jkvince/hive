using Godot;

public class Rules : Control
{
	private void _on_BackButton_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.Main);
	}

}


