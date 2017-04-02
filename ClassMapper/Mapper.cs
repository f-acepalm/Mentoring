using System;

namespace ClassMapper
{
    public class Mapper<TSource, TDestination>
    {
        Func<TSource, TDestination> _mapFunction;

        internal Mapper(Func<TSource, TDestination> mapFunction)
        {
            _mapFunction = mapFunction;
        }

        public TDestination Map(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return _mapFunction(source);
        }
    }
}
