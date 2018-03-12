﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public static class DataModelHelper
    {
        private static IReadOnlyCollection<string> dataModelInterfaceProperties;

        private static Dictionary<Type, IEnumerable<string>> modelProperties = new Dictionary<Type, IEnumerable<string>>();      

        static DataModelHelper()
        {
            var interfaceType = typeof(IDataModel);
            dataModelInterfaceProperties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Select(x => x.Name).ToList();
        }

        public static IEnumerable<string> GetProperties(Type type, bool ignoreDataModelInterface = true)
        {
            if(!modelProperties.Keys.Contains(type))
            {
                ReadProperties(type);
            }
            
            return ignoreDataModelInterface ? modelProperties[type].Where(x => !dataModelInterfaceProperties.Contains(x)) : modelProperties[type];
        }

        private static void ReadProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            modelProperties.Add(type, properties.Select(x => x.Name));
        }
        
    }
}
