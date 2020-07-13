using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Linq.Search
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MakeSearchable : Attribute
    {
    }
    
    public static class ReflectionExtensions
    {
        public static IEnumerable<PropertyInfo> SearchableProperties(this IReflect obj)
        {
            return obj.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => Attribute.IsDefined(p, typeof(MakeSearchable)));
        }

        public static IEnumerable<PropertyInfo> SearchablePropertiesOfType<T>(this IReflect obj)
        {
            return obj.SearchableProperties().Where(p => p.PropertyType == typeof(T));
        }
    }
    
    public static class QueryableExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> source, 
                                              string search)
        {
            // Hämta alla string-properties som är markerade med vårt attribut
            var childProperties = typeof(T).SearchablePropertiesOfType<string>()
                                           .Select(prop => prop.Name);

            // Hämta alla string-properties som är markerade med vårt attribut på
            // de relaterade objekt som är markerade med vårt attribut
            var childproperties = typeof(T).SearchableProperties()
                                           .SelectMany(prop => 
                                               prop.PropertyType.SearchablePropertiesOfType<string>()
                                                   .Select(child => $"{prop.Name}.{child.Name}"));

            // Skapa en lista med alla properties samlade och
            // sen en sträng som gör .Contains på alla
            var properties = childProperties.Union(childproperties).Select(prop => $"{prop}.Contains(\"{search}\")").ToList();
            
            // Om det inte finns några properties att söka på
            // returnera då bara den ursprungliga listan
            if (!properties.Any()) return source;

            // Samla ihop alla söktermer med || för att då söka i alla fält            
            var query = string.Join("||", properties);
            return source.Where(query);
        }
    }
}