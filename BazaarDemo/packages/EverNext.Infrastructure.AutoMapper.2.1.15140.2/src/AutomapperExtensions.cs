using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

public static class AutomapperExtensions
{
    public static IMappingExpression<TDestination, TSource> Bidirectional<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
    {
        return Mapper.CreateMap<TDestination, TSource>();
    }

    public static IMappingExpression<TSource, TDestination> ForAllMembersWithAnnotation<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Type attribute, Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
        var candidates = typeof(TSource).GetProperties().Where(c => c.IsDefined(attribute,true)).ToList();

        foreach (PropertyInfo pi in candidates)
        {
            expression = expression.ForMember(pi.Name, memberOptions);
        }

        return expression;
    }

    public static IMappingExpression<TSource, TDestination> ForAllMembersOfType<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Type type, Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
        var candidates = typeof(TSource).GetProperties().Where(c => type.IsAssignableFrom((c.PropertyType.IsGenericType && type.IsGenericType) ? c.PropertyType.GetGenericTypeDefinition() : c.PropertyType))
                                                        .Where(c => typeof(TDestination).GetProperties().Where(x => x.Name == c.Name).Any());

        foreach (PropertyInfo pi in candidates)
        {
            expression = expression.ForMember(pi.Name, memberOptions);
        }

        return expression;
    }

}