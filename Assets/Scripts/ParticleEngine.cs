using UnityEngine;
using System;
using System.Collections.Generic;

public class ParticleEngine
{
	List<Particle> particles;
	
	public ParticleEngine ()
	{
		particles = new List<Particle>();
	}
	
	public void addParticle(Particle p)
	{
		particles.Add(p);	
		Futile.stage.AddChild(p.getFSprite());
	}
	
	public void update()
	{
		for(int i = 0; i < particles.Count; i++)
		{
			particles[i].update();
			if(particles[i].shouldRemove())
			{
				Futile.stage.RemoveChild(particles[i].getFSprite());
				particles.Remove(particles[i]);
			}
		}
	}
}
