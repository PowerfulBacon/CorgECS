using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Signals
{
	public class SignalResult<T> : IEnumerable<T>
	{

		public bool HasResult => Results != null;

		public List<T>? Results { get; private set; } = null;

		/// <summary>
		/// Returns a signal result containing no value
		/// </summary>
		public static SignalResult<T> None { get; } = new SignalResult<T>();

		/// <summary>
		/// Creates a signal result with no value
		/// </summary>
		public SignalResult()
		{ }

		/// <summary>
		/// Creates a signal result that contains a value
		/// </summary>
		/// <param name="result"></param>
		public SignalResult(T result)
		{
			Results = new List<T>() { result };
		}

		/// <summary>
		/// Add a result to this result
		/// </summary>
		public void AddResult(SignalResult<T> result)
		{
			if (result.Results == null)
				return;
			if (Results == null)
			{
				Results = result.Results;
				return;
			}
			Results.AddRange(result.Results);
		}

		public void AddResult(T result)
		{
			if (Results == null)
			{
				Results = new List<T>() { result };
				return;
			}
			Results.Add(result);
		}

		/// <summary>
		/// Performs a function given that the result contains a value.
		/// </summary>
		/// <typeparam name="U">The output type of the converter function</typeparam>
		/// <param name="result">The function to run if the result exists</param>
		/// <returns>Returns a signal result containing either no value, or the transformed value</returns>
		public SignalResult<U> WithResults<U>(Func<T, U> resultActor)
		{
			if (Results == null)
				return SignalResult<U>.None;
			SignalResult<U> newResult = new SignalResult<U>();
			foreach (var result in Results)
			{
				newResult.AddResult(resultActor(result));
			}
			return newResult;
		}

		/// <summary>
		/// If a result exists, executes the given action with the result as the parameter.
		/// </summary>
		/// <param name="resultActor"></param>
		/// <returns></returns>
		public SignalResult<T> WithResults(Action<T> resultActor)
		{
			Results?.ForEach(x => resultActor(x));
			return this;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Results?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Results?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
		}
	}
}
