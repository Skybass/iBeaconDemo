using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeaconDemo
{
	internal sealed class Timer : CancellationTokenSource
	{
		/// <summary>
		/// Timer Klasse. Muss seblbst implementiert werden da aktuell nicht unter Xamarin.Forms verfügbar.
		/// Ruft nach Ablauf Time X die mitgegebene Funktion auf
		/// </summary>
		/// <param name="callback">Callback.</param>
		/// <param name="state">State.</param>
		/// <param name="millisecondsDueTime">Milliseconds due time.</param>
		/// <param name="millisecondsPeriod">Milliseconds period.</param>
		/// <param name="waitForCallbackBeforeNextPeriod">If set to <c>true</c> wait for callback before next period.</param>
		internal Timer (Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
		{
			//Contract.Assert(period == -1, "This stub implementation only supports dueTime.");

			Task.Delay (millisecondsDueTime, Token).ContinueWith (async (t, s) => {
				var tuple = (Tuple<Action<object>, object>)s;

				while (!IsCancellationRequested) {
					if (waitForCallbackBeforeNextPeriod)
						tuple.Item1 (tuple.Item2);
					else
						Task.Run (() => tuple.Item1 (tuple.Item2));

					await Task.Delay (millisecondsPeriod, Token).ConfigureAwait (false);
				}
			}, Tuple.Create (callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing)
				Cancel ();
			base.Dispose (disposing);
		}
	}
}

