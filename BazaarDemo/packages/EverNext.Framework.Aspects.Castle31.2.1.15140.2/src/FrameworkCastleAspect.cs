using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using EverNext.Domain.Contracts.Aspects;
using EverNext.Domain.Model.Aspects;
using System.Diagnostics;

namespace EverNext.Framework.Aspects.Castle
{
    public class FrameworkCastleAspect<T> : IInterceptor where T : IBaseInterceptor
    {
        public virtual T aspect { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {
            IBaseInvocation bInvocation = BaseInvocationHelper.CreateNewBaseInvocation(invocation);
            InterceptionResult result = aspect.Preproceed(bInvocation);
            if (!result.Interrupt)
            {
                invocation.Proceed();
                result.Data = invocation.ReturnValue;
                aspect.Postproceed(bInvocation, invocation.ReturnValue);
            }
            else
            {
                invocation.ReturnValue = result.Data;
            }
        }
    }
}
