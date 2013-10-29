using UnityEngine;
using System;
using System.Collections;

public class Particle
{
	private Vector2 pos0;
	private Vector2 pos;
	private Color color0;
	private Color color;
	private float angle0;
	private float angle;
	private FLabel img;
	private int age;
	
	public delegate Vector2 posRuleDelegate(Vector2 pos0, int age);
	public delegate Color colorRuleDelegate(Color color0, int age);
	public delegate float angleRuleDelegate(float angle0, int age);
	public delegate bool shouldDieDelegate(int age);
	
	private posRuleDelegate posRule;
	private colorRuleDelegate colorRule;
	private angleRuleDelegate angleRule;
	private shouldDieDelegate shouldDie;
	
	public Particle (Vector2 iPos, Color iColor, float iAngle, FLabel i, posRuleDelegate pRule, colorRuleDelegate cRule, angleRuleDelegate aRule, shouldDieDelegate sDie,
		int a = 0)
	{
		pos0 = iPos;
		pos = new Vector2 (pos0.x, pos0.y);
		color0 = iColor;
		color = color0;
		angle0 = iAngle;
		angle = angle0;
		img = i;
		age = a;
		posRule = pRule;
		colorRule = cRule;
		angleRule = aRule;
		shouldDie = sDie;
	}
	
	public void update()
	{
		age++;
		pos = posRule(pos0, age);
		color = colorRule(color0, age);
		angle = angleRule(angle0, age);
		
		img.SetPosition(pos);
		img.color = color;
		img.rotation = angle;
	}
	
	public bool shouldRemove()
	{
		return shouldDie(age);
	}
	
	public FLabel getFSprite()
	{
		return img;	
	}
}

