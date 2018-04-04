using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public static class DataModelHelper
    {
        private static IReadOnlyCollection<string> dataModelInterfaceProperties;

        private static IReadOnlyCollection<string> concurrencyInterfaceProperties;

        private static Dictionary<Type, IEnumerable<string>> modelProperties = new Dictionary<Type, IEnumerable<string>>();

        public static string RowVersionPropertyName
        {
            get
            {
                return concurrencyInterfaceProperties.First();
            }
        }

        static DataModelHelper()
        {
            var interfaceType = typeof(IDataModel);
            dataModelInterfaceProperties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Select(x => x.Name).ToList();

            var concurrencyInterfaceType = typeof(IRecordChangeInfo);
            concurrencyInterfaceProperties = concurrencyInterfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Select(x => x.Name).ToList();
        }

        public static IEnumerable<string> GetProperties(Type type, bool ignoreDataModelInterface = true, bool ignoreConcurrency = true)
        {
            if(!modelProperties.Keys.Contains(type))
            {
                ReadProperties(type);
            }
            
            var properties = ignoreDataModelInterface ? modelProperties[type].Where(x => !dataModelInterfaceProperties.Contains(x)) : modelProperties[type];
            properties = ignoreConcurrency ? properties.Where(x => !concurrencyInterfaceProperties.Contains(x)) : properties;
            return properties;
        }

        public static string GetPrimaryKeyProperty(Type type)
        {
            if (!modelProperties.Keys.Contains(type))
            {
                ReadProperties(type);
            }

            return modelProperties[type].Single(x => x == (type.Name + "Id"));
        }

        private static void ReadProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            modelProperties.Add(type, properties.Select(x => x.Name));
        }        
    }
}
