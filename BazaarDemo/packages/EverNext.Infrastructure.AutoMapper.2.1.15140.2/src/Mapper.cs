using System;
using System.Collections.Generic;
using System.Linq;

namespace EverNext.Infrastructure.AutoMapper
{
    public class Mapper : Domain.Contracts.Services.IObjectMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return global::AutoMapper.Mapper.Map<TSource, TTarget>(source, target);
        }

        public TTarget Map<TSource, TTarget>(TSource source)
        {
            return global::AutoMapper.Mapper.Map<TSource, TTarget>(source);
        }

        public object Map(object source, object target, Type sourceType, Type targetType)
        {
            return global::AutoMapper.Mapper.Map(source, target, sourceType, targetType);
        }

        public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source)
        {
            return source.Select(x => Map<TSource, TTarget>(x));
        }
    }
}
