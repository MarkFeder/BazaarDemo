// -----------------------------------------------------------------------
// <copyright file="LoggerSubDependencyResolver.cs" company="everis">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Common.Logging;

namespace EverNext.Framework.Aspects.Castle
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoggerSubDependencyResolver:ISubDependencyResolver
    {
        public bool CanResolve(global::Castle.MicroKernel.Context.CreationContext context, ISubDependencyResolver contextHandlerResolver, global::Castle.Core.ComponentModel model, global::Castle.Core.DependencyModel dependency)
        {
            return dependency.TargetType == typeof(ILog);
        }

        public object Resolve(global::Castle.MicroKernel.Context.CreationContext context, ISubDependencyResolver contextHandlerResolver, global::Castle.Core.ComponentModel model, global::Castle.Core.DependencyModel dependency)
        {
            if (CanResolve(context, contextHandlerResolver, model, dependency))
            {
                if (dependency.TargetType == typeof(ILog))
                {
                    return LogManager.GetLogger(model.Implementation);
                }
            }
            return null;
        }
    }
}
