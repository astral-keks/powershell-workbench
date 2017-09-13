using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Sandbox
{
    public class Singleton
    {
        public static Controller Instance { get; set; }
    }

    public class Controller
    {
        public string Property { get; set; } = "AAAAAAA";
    }

    public class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Controller>();
            builder.RegisterType<Singleton>().PropertiesAutowired();

            var container = builder.Build();
            Console.WriteLine(container);
        }

        private static object Union(object list1, object list2, Type itemType)
        {
            var unionMethod = typeof(Enumerable)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "Union")
                .MakeGenericMethod(itemType);
            var union = unionMethod.Invoke(null, new[] { list1, list2 });

            var toListMethod = typeof(Enumerable)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "ToList")
                .MakeGenericMethod(itemType);
            var list = toListMethod.Invoke(null, new[] { union });
            return list;
        }
    }
}
