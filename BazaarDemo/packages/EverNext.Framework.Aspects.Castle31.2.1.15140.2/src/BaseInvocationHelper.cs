using System.Reflection;
using Castle.DynamicProxy;
using EverNext.Domain.Contracts.Aspects;
using EverNext.Domain.Model.Aspects;

namespace EverNext.Framework.Aspects.Castle
{
    /// <summary>
    /// BaseInvocationHelper
    /// </summary>
    public static class BaseInvocationHelper
    {
        /// <summary>
        /// CreateNewBaseInvocation
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static IBaseInvocation CreateNewBaseInvocation(IInvocation invocation)
        {
            BaseInvocation bInvocation = new BaseInvocation(invocation.Arguments,
                                                            invocation.Method,
                                                            invocation.TargetType,
                                                            invocation.Proxy,
                                                            invocation.InvocationTarget,
                                                            ((MemberInfo)invocation.Method),
                                                            invocation.InvocationTarget,
                                                            invocation.GenericArguments,
                                                            invocation.InvocationTarget,
                                                            invocation.MethodInvocationTarget);

            return bInvocation;
        }
    }
}
