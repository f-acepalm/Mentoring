using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ClassMapper
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            return new Mapper<TSource, TDestination>(CreateMapFunction<TSource, TDestination>());
        }

        private Func<TSource, TDestination> CreateMapFunction<TSource, TDestination>()
        {
            var source = Expression.Parameter(typeof(TSource), "source");
            var body = Expression.MemberInit(
                Expression.New(
                    typeof(TDestination)),
                    source.Type.GetProperties()
                    .Where(p => 
                        typeof(TDestination).GetProperty(p.Name) != null
                        && typeof(TDestination).GetProperty(p.Name).CanWrite)
                    .Select(
                        p => Expression.Bind(typeof(TDestination).GetProperty(p.Name),
                        Expression.Property(source, p))));
            var expr = Expression.Lambda<Func<TSource, TDestination>>(body, source);

            return expr.Compile();
        }
    }
}
