using CorgECS.Entities;
using CorgECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Worlds
{
	public class World
	{

		private Dictionary<Type, EntitySystem> EntitySystems { get; } = new Dictionary<Type, EntitySystem>();

		/// <summary>
		/// Create a new entity attached to this world.
		/// </summary>
		/// <returns>The created entity</returns>
		public Entity CreateEntity()
		{
			return new Entity(this);
		}

		/// <summary>
		/// Get the desired entity system
		/// </summary>
		/// <typeparam name="T">The type of the entity system that we want to fetch</typeparam>
		/// <returns></returns>
		public T GetEntitySystem<T>()
			where T : EntitySystem, new()
		{
			lock (EntitySystems)
			{
				if (EntitySystems.TryGetValue(typeof(T), out EntitySystem? entitySystem))
				{
					return (T)entitySystem;
				}
				else
				{
					T createdSystem = new T();
					createdSystem.JoinWorld(this);
					EntitySystems.Add(typeof(T), createdSystem);
					return createdSystem;
				}
			}
		}

	}
}
