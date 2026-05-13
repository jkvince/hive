using Godot;
using System;

public class QuickMenu : Control
{
	private PopupMenu _popupMenu;
	
	public override void _Ready()
	{
		_popupMenu = GetNode<PopupMenu>("PopupMenu");
	}
	
	private void _on_Button_pressed() {
		_popupMenu.Popup_();
	}
	
	private void _on_Return_pressed()
	{
		Main.Instance.SetMenu(Main.Menus.Main);
	}
	
}





