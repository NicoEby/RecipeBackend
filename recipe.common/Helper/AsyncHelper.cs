using System;
using System.Threading.Tasks;
using ch.thommenmedia.common.Interfaces;

namespace ch.thommenmedia.common.Helper
{
    public class AsyncHelper
    {
        public ISecurityAccessor SecurityAccessor { get; set; }

        public AsyncHelper(ISecurityAccessor securityAccessor)
        {
            SecurityAccessor = securityAccessor;
        }

        /// <summary>
        /// executes a task async safe with user context
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task ExecuteAsync(Action action)
        {
            if (action == null)
                return Task.CompletedTask;

            var userId = SecurityAccessor.CurrentUserid;
            return Task.Run(
                () =>
                {
                    SecurityAccessor.Impersonate(userId, "Execute Async Action");
                    action.Invoke();
                });
        }


        /// <summary>
        /// executes a task async safe with user context
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task<TResult> ExecuteAsync<TResult>(Func<TResult> action)
        {
            if (action == null)
                return null;

            var userId = SecurityAccessor.CurrentUserid;
            return Task.Run(
                () =>
                {
                    SecurityAccessor.Impersonate(userId, "Execute Async Action");

                    return action.Invoke();
                });
        }
    }
}
