using CorgECS.Entities;
using CorgECS.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Components
{
	public abstract class Component
	{

		public Entity? Parent { get; private set; }

		/// <summary>
		/// Create the component and attach it to the parent
		/// </summary>
		/// <param name="parent"></param>
		public void LinkComponent(Entity parent)
		{
			Parent = parent;
			Initialise();
		}

		/// <summary>
		/// Run initialisation method and register appropriate signals.
		/// </summary>
		/// <param name="parent"></param>
		public abstract void Initialise();

		/// <summary>
		/// Register a function to execute when an asynchronous signal is raised against the parent of this component. A
		/// reponse is not expected by the signal caller.
		/// </summary>
		/// <typeparam name="T">The type of the signal to be registered</typeparam>
		/// <param name="signalHandler">The action to be executed when a signal of type T is raised against the parent of this component.</param>
		public void RegisterSignal<T>(Action<T> signalHandler)
			where T : Signal
		{
			Parent?.RegisterSignal<T>(signalHandler);
		}

		public void RegisterSignal<TSignal, TResult>(Func<Signal<TResult>, SignalResult<TResult>> signalHandler)
			where TSignal : Signal<TResult>
		{
			Parent?.RegisterSignal<TSignal, TResult>(signalHandler);
		}

	}
}
