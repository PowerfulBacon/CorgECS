using CorgECS.Components;
using CorgECS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Systems
{

	public abstract class EntitySystem
	{

		internal EntitySystem()
		{ }

		/// <summary>
		/// The world that this entity system belongs to
		/// </summary>
		internal World? AttachedWorld { get; private set; }

		public void JoinWorld(World world)
		{
			AttachedWorld = world;
		}


	}

	public abstract class EntitySystem<TComponent> : EntitySystem
		where TComponent : Component
	{

		/// <summary>
		/// List of components that belong to this entity system
		/// </summary>
		private HashSet<TComponent> components = new HashSet<TComponent>();

		/// <summary>
		/// Get the components as an enumerable set
		/// </summary>
		public IEnumerable<TComponent> Components => components;

		/// <summary>
		/// Have a 
		/// </summary>
		/// <param name="component"></param>
		public void JoinSystem(TComponent component)
		{
			components.Add(component);
		}

	}
}
