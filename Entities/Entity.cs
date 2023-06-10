using CorgECS.Components;
using CorgECS.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Entities
{
	public class Entity
	{

		#region Components

		public List<Component> Components { get; } = new List<Component>();

		public void AddComponent(Component component)
		{
			if (component.Parent != null)
				throw new Exception($"Cannot add {component.GetType()} to entity due to it already being attached to an entity.");
			Components.Add(component);
			component.LinkComponent(this);
		}

		#endregion

		#region Signal Handling

		/// <summary>
		/// List of signals registered to this object by type.
		/// </summary>
		internal Dictionary<Type, SignalList> SignalList { get; } = new Dictionary<Type, SignalList>();

		public void Raise<T>(T signal)
			where T : Signal
		{
			HandleSignal(signal);
		}

		public SignalResult<TResult> Raise<TSignal, TResult>(TSignal signal)
			where TSignal : Signal<TResult>
		{
			return HandleSignal<TSignal, TResult>(signal);
		}

		/// <summary>
		/// Register a function to execute when an asynchronous signal is raised against this entity. A
		/// reponse is not expected by the signal caller.
		/// </summary>
		/// <typeparam name="T">The type of the signal to be registered</typeparam>
		/// <param name="signalHandler">The action to be executed when a signal of type T is raised against this entity.</param>
		internal void RegisterSignal<T>(Action<T> signalHandler)
			where T : Signal
		{
			lock (SignalList)
			{
				if (SignalList.TryGetValue(typeof(T), out SignalList? signalist))
				{
					signalist.AddSignal(signalHandler);
				}
				else
				{
					SignalList createdSignalList = new SignalList();
					createdSignalList.AddSignal(signalHandler);
					SignalList.Add(typeof(T), createdSignalList);
				}
			}
		}

		/// <summary>
		/// Handle a signal that does not need a response
		/// </summary>
		/// <typeparam name="T">The type of the signal raised</typeparam>
		/// <param name="signal">The signal being raised against this entity</param>
		internal void HandleSignal<T>(T signal)
			where T : Signal
		{
			lock (SignalList)
			{
				if (SignalList.TryGetValue(typeof(T), out SignalList? signalist))
				{
					signalist.HandleSignal(signal);
				}
			}
		}

		/// <summary>
		/// Register a function to execute when a typed signal is raised against this entity. Typed signals
		/// return a result.
		/// </summary>
		/// <typeparam name="TSignal"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="signal"></param>
		/// <param name="signalHandler"></param>
		internal void RegisterSignal<TSignal, TResult>(Func<TSignal, SignalResult<TResult>> signalHandler)
			where TSignal : Signal<TResult>
		{
			lock (SignalList)
			{
				if (SignalList.TryGetValue(typeof(TSignal), out SignalList? signalist))
				{
					signalist.AddSignal<TSignal, TResult>(signalHandler);
				}
				else
				{
					SignalList createdSignalList = new SignalList();
					createdSignalList.AddSignal<TSignal, TResult>(signalHandler);
					SignalList.Add(typeof(TSignal), createdSignalList);
				}
			}
		}

		/// <summary>
		/// Handle a signal that returns a response
		/// </summary>
		/// <typeparam name="T">The type of the signal raised</typeparam>
		/// <param name="signal">The signal being raised against this entity</param>
		internal SignalResult<TResult> HandleSignal<TSignal, TResult>(TSignal signal)
			where TSignal : Signal<TResult>
		{
			lock (SignalList)
			{
				if (SignalList.TryGetValue(typeof(TSignal), out SignalList? signalist))
				{
					return signalist.HandleSignal<TSignal, TResult>(signal);
				}
			}
			// No signals registered
			return SignalResult<TResult>.None;
		}

		#endregion

	}
}
