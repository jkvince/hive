using Godot;
using System;

public class QuickMenu : MenuAbstract
{
	private PopupMenu _popupMenu;
	
	public override void _Ready()
	{
		base._Ready();
		_popupMenu = GetNode<PopupMenu>("PopupMenu");
	}
	
	private void _on_Button_pressed() {
		_popupMenu.Popup_();
	}
	
	private void _on_Return_pressed()
	{
		Main.SetMenu(Main.Menus.Main);
	}
	
}





