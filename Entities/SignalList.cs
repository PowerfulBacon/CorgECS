using CorgECS.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Entities
{
	/// <summary>
	/// Internal list that stores lists of signals. The types of these intermerge with each other
	/// so this uses objects and casting. This is internal as incorrect modification is unchecked by
	/// the compiler.
	/// </summary>
	internal class SignalList
	{

		private List<object> signals = new List<object>();

		/// <summary>
		/// Add a signal to the list of signal handlers attached to this signal list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="action"></param>
		public void AddSignal<T>(Action<T> action)
		{
			signals.Add(action);
		}

		public void HandleSignal<T>(T signal)
			where T : Signal
		{
			signals.ForEach(x => ((Action<T>)x)(signal));
		}

		public void AddSignal<TSignal, TResult>(Func<TSignal, SignalResult<TResult>> action)
		{
			signals.Add(action);
		}

		public SignalResult<TResult> HandleSignal<TSignal, TResult>(TSignal signal)
			where TSignal : Signal<TResult>
		{
			SignalResult<TResult> result = new SignalResult<TResult>();
			signals.ForEach(x =>
			{
				result.AddResult(((Func<TSignal, SignalResult<TResult>>)x)(signal));
			});
			return result;
		}

	}
}
