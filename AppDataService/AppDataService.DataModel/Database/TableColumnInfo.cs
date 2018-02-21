using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class TableColumnInfo
    {
        private static Dictionary<string, BasicType> BasicDataTypeLookupDictionary = new Dictionary<string, BasicType>();

        static TableColumnInfo()
        {
            BasicDataTypeLookupDictionary.Add(typeof(int).FullName, BasicType.Integer);
            BasicDataTypeLookupDictionary.Add(typeof(int?).FullName, BasicType.Integer);
            BasicDataTypeLookupDictionary.Add(typeof(double).FullName, BasicType.Integer);
            BasicDataTypeLookupDictionary.Add(typeof(double?).FullName, BasicType.Integer);
            BasicDataTypeLookupDictionary.Add(typeof(string).FullName, BasicType.String);
            BasicDataTypeLookupDictionary.Add(typeof(bool).FullName, BasicType.Boolean);
            BasicDataTypeLookupDictionary.Add(typeof(bool?).FullName, BasicType.Boolean);
            BasicDataTypeLookupDictionary.Add(typeof(float).FullName, BasicType.FloatingPoint);
            BasicDataTypeLookupDictionary.Add(typeof(float?).FullName, BasicType.FloatingPoint);
            BasicDataTypeLookupDictionary.Add(typeof(decimal).FullName, BasicType.FloatingPoint);
            BasicDataTypeLookupDictionary.Add(typeof(decimal?).FullName, BasicType.FloatingPoint);
            BasicDataTypeLookupDictionary.Add(typeof(DateTime).FullName, BasicType.DateTime);
            BasicDataTypeLookupDictionary.Add(typeof(DateTime?).FullName, BasicType.DateTime);
        }

        public TableColumnInfo(PropertyInfo property)
        {
            this.ColumnName = property.Name;
            this.ModelPropertyInfo = property;
            this.BasicType = BasicDataTypeLookupDictionary[property.PropertyType.FullName];
        }

        public string ColumnName { get; set; }

        public PropertyInfo ModelPropertyInfo { get; set; }

        public BasicType BasicType { get; set; }
    }
}
