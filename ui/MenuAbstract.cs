using Godot;
using System;

public abstract class MenuAbstract : Control {
	
	protected Main Main;
	
	public override void _Ready()
	{
		Main = GetNode<Main>("/root/Main");
	}

}
