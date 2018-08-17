using System;

namespace neogary
{
    public static class Extensions
    {
        public static T GetService<T>(this IServiceProvider sp)
        {
            return (T)sp.GetService(typeof(T)); 
        }
    }
}
