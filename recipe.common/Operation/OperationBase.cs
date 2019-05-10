using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Threading.Tasks;
using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Helper;
using ch.thommenmedia.common.Interfaces;

namespace ch.thommenmedia.common.Operation
{
    public abstract class OperationBase<TInput, TResult> : IOperation<TInput, TResult>, IDisposable
    {
        protected Stopwatch DurationStopwatch = new Stopwatch();

        /// <summary>
        ///     the input parameter stored to have access from other methos eq. Authorize
        /// </summary>
        protected TInput Input;
        protected bool IsAsyncExecution = false;
        protected ISecurityAccessor SecurityAccessor { get; set; }


        protected OperationBase(ISecurityAccessor securityAccessor)
        {
            IsSecurityEnabled = true;
            SecurityAccessor = securityAccessor;
        }


        /// <summary>
        ///     Ist die Security aktiv?
        /// </summary>
        public bool IsSecurityEnabled { get; set; }

        void IDisposable.Dispose()
        {
            // Benötigte Ressourcen wieder freigeben. Wie z.B. ein Datenbank-Context
        }


        /// <summary>
        ///     Startet die Operation mit einem Input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Resultat</returns>
        public Task<TResult> StartAsync(TInput input)
        {
            return new AsyncHelper(SecurityAccessor).ExecuteAsync(() =>
            {
                IsAsyncExecution = true;
                OnStart?.Invoke(this, new OperationEventArgs
                {
                    Duration = 0
                });
                DurationStopwatch.Start();
                var r = StartInt(input);
                DurationStopwatch.Stop();
                OnFinished?.Invoke(this, new OperationEventArgs
                {
                    Duration = DurationStopwatch.ElapsedMilliseconds,
                    Input = Input
                });
                return r;
            });
        }


        /// <inheritdoc />
        /// <summary>
        ///     Startet die Operation mit einem Input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Resultat</returns>
        public TResult Start(TInput input)
        {
            OnStart?.Invoke(this, new OperationEventArgs
            {
                Duration = 0
            });
            DurationStopwatch.Start();
            var r = StartInt(input);
            DurationStopwatch.Stop();
            OnFinished?.Invoke(this, new OperationEventArgs
            {
                Duration = DurationStopwatch.ElapsedMilliseconds,
                Input = Input
            });
            return r;
        }

        object IOperation.StartUntyped(object input)
        {
            if (input != null)
            {
                var param = input is TInput input1 ? input1 : default(TInput);
                return StartInt(param);
            }

            throw new ArgumentNullException("input");
        }

        public void StartUntyped()
        {
            Start();
        }

        public Type InputType => typeof(TInput);
        public Type OutputType => typeof(TResult);

        public event EventHandler<OperationEventArgs> OnStart;
        public event EventHandler<OperationEventArgs> OnError;
        public event EventHandler<OperationEventArgs> OnAuthError;
        public event EventHandler<OperationEventArgs> OnFinished;

        /// <summary>
        ///     Startet die Operation ohne Parameter
        /// </summary>
        public TResult Start()
        {
            OnStart?.Invoke(this, new OperationEventArgs
            {
                Duration = 0
            });
            DurationStopwatch.Start();
            var r = StartInt();
            DurationStopwatch.Stop();
            OnFinished?.Invoke(this, new OperationEventArgs
            {
                Duration = DurationStopwatch.ElapsedMilliseconds,
                Input = Input
            });
            return r;
        }

        private TResult StartInt(TInput input)
        {
            Input = input;
#if DEBUG|| TEST
#else
            try
            {
#endif
            //else
            //security will not be cached
            if (IsSecurityEnabled && !Authorize())
            {
                throw new UnauthorizedAccessException(
                    "You don't have the privileges to run the {0} operation".Apply(GetType().Name));
            }

            return Execute(input);

#if DEBUG || TEST
#else
            }
            catch (AuthenticationException authEx)
            {
                OnAuthError?.Invoke(this, new OperationEventArgs()
                {
                    Exception = authEx,
                    Duration = this.DurationStopwatch.ElapsedMilliseconds,
                    Input = this.Input
                });
                throw new AuthenticationException(authEx.Message);
            }
            catch (UnauthorizedAccessException authEx)
            {
                OnAuthError?.Invoke(this, new OperationEventArgs()
                {
                    Exception = authEx,
                    Duration = this.DurationStopwatch.ElapsedMilliseconds,
                    Input = this.Input
                });
                throw new UnauthorizedAccessException(authEx.Message);
            }
            catch (Exception ex)
            {

                OnError?.Invoke(this, new OperationEventArgs()
                {
                    Exception = ex,
                    Duration = this.DurationStopwatch.ElapsedMilliseconds,
                    Input = this.Input
                });

                throw;
            }
#endif
        }

        private TResult StartInt()
        {
            try
            {
                if (IsSecurityEnabled && !Authorize())
                    throw new AuthenticationException("You don't have the privilleges to run this operation");

                return Input != null ? Execute(Input) : Execute();
            }
            catch (AuthenticationException authEx)
            {
                OnAuthError?.Invoke(this, new OperationEventArgs
                {
                    Exception = authEx,
                    Duration = DurationStopwatch.ElapsedMilliseconds,
                    Input = Input
                });
                throw new AuthenticationException(authEx.Message);
            }
            catch (UnauthorizedAccessException authEx)
            {
                OnAuthError?.Invoke(this, new OperationEventArgs
                {
                    Exception = authEx,
                    Duration = DurationStopwatch.ElapsedMilliseconds,
                    Input = Input
                });
                throw;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new OperationEventArgs
                {
                    Exception = ex,
                    Duration = DurationStopwatch.ElapsedMilliseconds,
                    Input = Input
                });

                throw;
            }
        }

        protected abstract TResult Execute(TInput input);
        protected abstract TResult Execute();

        protected abstract bool Authorize();

        public class OperationEventArgs : EventArgs
        {
            public long Duration { get; set; } //dauer der operation
            public Exception Exception { get; set; } //aktueller Fehler
            public object Input { get; set; }
            public object AdditionalLogData { get; set; }
        }
    }
}