using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.Objects.Dynamic
{
    /// <summary>
    /// Helper class for creating new Types in process.
    /// </summary>
    public static class TypeBuilderHelper
    {

        private class BasicPropertyDefinition : IPropertyDefinition
        {
            string _as = string.Empty;

            public string Name { get; set; }

            public string Type { get; set; }

            public string As {
                get
                {
                    if (string.IsNullOrEmpty(_as))
                    {
                        return Name;
                    }

                    return _as;
                }
                set
                {
                    _as = value;
                }
            }

            public string Aggregate { get; set; }

            public Type AsType()
            {
                return System.Type.GetType(this.Type);
            }
        }

        /// <summary>
        /// Creates a new type with the specified properties, all will be created with type System.Object.
        /// </summary>
        /// <param name="typeName">The name of the new type.</param>
        /// <param name="propertyNames">The name of the properties to add.</param>
        /// <returns>The new Type.</returns>
        public static Type CreateType(string typeName, params string[] propertyNames)
        {
            return CreateType(typeName, propertyNames.Select(p => new BasicPropertyDefinition { Name = p, Type = "System.Object" }).ToArray());
        }

        /// <summary>
        /// Creates a new type containing the specified properties.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <param name="properties">The properties to add to the type.</param>
        /// <returns></returns>
        public static Type CreateType(string typeName, IEnumerable<IPropertyDefinition> properties)
        {
            return CreateType(typeName, properties, null, null);
        }

        /// <summary>
        /// Creates a new type containing the specified properties, based on the specified parent type, and implements the specified interfaces.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <param name="properties">The properties of the type to add.</param>
        /// <param name="baseType">The parent type the new type inherits from, can be null.</param>
        /// <param name="interfaces">The interfaces the new type implements, can be null.</param>
        /// <returns></returns>
        public static Type CreateType(string typeName, IEnumerable<IPropertyDefinition> properties, Type baseType, Type[] interfaces)
        {
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = typeName + "_" + Guid.NewGuid().ToString("N").Substring(24);
            AssemblyBuilder assBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assBuilder.DefineDynamicModule(assemblyName.Name + "_module");

            TypeBuilder typeBuilder = module.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class, baseType, interfaces);

            var fields = GenerateProperties(typeBuilder, properties);
            GenerateEquals(typeBuilder, fields);
            GenerateGetHashCode(typeBuilder, fields);

            Type t1 = typeBuilder.CreateType();
            return t1;
        }

        static List<FieldInfo> GenerateProperties(TypeBuilder typeBuilder, IEnumerable<IPropertyDefinition> properties)
        {
            List<FieldInfo> fields = new List<FieldInfo>();

            foreach (IPropertyDefinition key in properties)
            {
                string fieldName = key.As.CleanString();
                Type propertyType = key.AsType();

                FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + fieldName, propertyType, FieldAttributes.Private);
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(fieldName, PropertyAttributes.HasDefault, propertyType, new Type[0]);

                MethodBuilder getPropertyMethodBuilder = typeBuilder.DefineMethod("get_" + fieldName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                ILGenerator getIL = getPropertyMethodBuilder.GetILGenerator();
                getIL.Emit(OpCodes.Ldarg_0);
                getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getIL.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getPropertyMethodBuilder);

                MethodBuilder setPropertyMethodBuilder = typeBuilder.DefineMethod("set_" + fieldName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { propertyType });
                ILGenerator setIL = setPropertyMethodBuilder.GetILGenerator();
                setIL.Emit(OpCodes.Ldarg_0);
                setIL.Emit(OpCodes.Ldarg_1);
                setIL.Emit(OpCodes.Stfld, fieldBuilder);
                setIL.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setPropertyMethodBuilder);

                fields.Add(fieldBuilder);
            }

            return fields;
        }

        static void GenerateEquals(TypeBuilder typeBuilder, IEnumerable<FieldInfo> fields)
        {
            MethodBuilder mb = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.ReuseSlot |
                MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(bool), new Type[] { typeof(object) });
            ILGenerator gen = mb.GetILGenerator();
            LocalBuilder other = gen.DeclareLocal(typeBuilder);
            Label next = gen.DefineLabel();
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Isinst, typeBuilder);
            gen.Emit(OpCodes.Stloc, other);
            gen.Emit(OpCodes.Ldloc, other);
            gen.Emit(OpCodes.Brtrue_S, next);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ret);
            gen.MarkLabel(next);
            foreach (FieldInfo field in fields)
            {
                Type ft = field.FieldType;
                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                next = gen.DefineLabel();
                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Ldloc, other);
                gen.Emit(OpCodes.Ldfld, field);
                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("Equals", new Type[] { ft, ft }), null);
                gen.Emit(OpCodes.Brtrue_S, next);
                gen.Emit(OpCodes.Ldc_I4_0);
                gen.Emit(OpCodes.Ret);
                gen.MarkLabel(next);
            }
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Ret);
        }

        static void GenerateGetHashCode(TypeBuilder typeBuilder, IEnumerable<FieldInfo> fields)
        {
            MethodBuilder mb = typeBuilder.DefineMethod("GetHashCode",
                MethodAttributes.Public | MethodAttributes.ReuseSlot |
                MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(int), Type.EmptyTypes);
            ILGenerator gen = mb.GetILGenerator();
            gen.Emit(OpCodes.Ldc_I4_0);
            foreach (FieldInfo field in fields)
            {
                Type ft = field.FieldType;
                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("GetHashCode", new Type[] { ft }), null);
                gen.Emit(OpCodes.Xor);
            }
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates a dictionary mapping they specified properties to the actual System.Reflection.PropertyInfo for the property on the specified Type.
        /// </summary>
        /// <param name="type">The Type containing the properties to map.</param>
        /// <param name="propertyDefinitions">The property definitions to get the PropertyInfo for.</param>
        /// <remarks>The PropertyInfo from the Type is retrieved using the As property of the PropertyDefinition falling back to the Name property if not found in the Type, the key of the created dictionary is the Name property of the PropertyInfo.</remarks>
        /// <returns></returns>
        public static IDictionary<string, PropertyInfo> CreatePropertyInfoMap(Type type, IEnumerable<IPropertyDefinition> propertyDefinitions)
        {
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
            foreach (var key in propertyDefinitions)
            {
                PropertyInfo info = type.GetProperty(key.As.CleanString());

                if (info == null && !string.Equals(key.As, key.Name, StringComparison.OrdinalIgnoreCase))
                {
                    info = type.GetProperty(key.Name.CleanString());
                }

                if (info != null)
                    properties.Add(key.As.ToString(), info);
                
            }

            return properties;
        }

        /// <summary>
        /// Returns a collection of <paramref name="Lpp.Objects.Dynamic.IPropertyDefinition"/> for the specified type.
        /// </summary>
        /// <param name="type">The type to create the property definitions for.</param>
        /// <param name="skipProperties">The properties of the the type to skip.</param>
        /// <param name="prefix">Optional prefix to append to the property name.</param>
        /// <returns>IEnumerable of <paramref name="Lpp.Objects.Dynamic.IPropertyDefinition"/>.</returns>
        public static IEnumerable<IPropertyDefinition> GeneratePropertyDefinitionsFromType(Type type, string prefix = null, params string[] skipProperties)
        {
            var propertyDefinitions = type.GetProperties()
                .Where(p => skipProperties == null || !skipProperties.Any(s => string.Equals(s, p.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(p => new BasicPropertyDefinition {
                    Name = string.IsNullOrEmpty(prefix) ? p.Name : (prefix + p.Name),
                    Type = p.PropertyType.ToString()
                });

            return propertyDefinitions;
        }


        /// <summary>
        /// Converts a dictionary into an object of the specified type.
        /// </summary>
        /// <param name="type">The type of object to create and populate.</param>
        /// <param name="propertyBag">The dictionary containing the values.</param>
        /// <param name="propertyInfoMap">A mapping of the propetybag's key's to the PropertyInfo for the property on the specified type.</param>
        /// <returns></returns>
        public static object FlattenDictionaryToType(Type type, Dictionary<string, object> propertyBag, IDictionary<string, PropertyInfo> propertyInfoMap)
        {
            if (propertyInfoMap == null)
                throw new ArgumentNullException("The map between the keys and PropertyInfo is null.");

            var obj = Activator.CreateInstance(type);
            foreach (KeyValuePair<string, PropertyInfo> pair in propertyInfoMap)
            {
                if (!propertyBag.ContainsKey(pair.Key) || propertyBag[pair.Key] == null)
                    continue;

                PropertyInfo propInfo = pair.Value;

                try
                {
                    object value = Convert.ChangeType(propertyBag[pair.Key], propInfo.PropertyType);

                    propInfo.SetValue(obj, value, null);
                }
                catch (InvalidCastException ex)
                {
                    object pairValue = propertyBag[pair.Key];

                    if(pairValue != null)
                    {
                        var converter = System.ComponentModel.TypeDescriptor.GetConverter(propInfo.PropertyType);

                        if (converter.IsValid(pairValue.ToString()))
                        {
                            object value = converter.ConvertFromString(pairValue.ToString());

                            propInfo.SetValue(obj, value, null);
                        }
                    }
                    
                }
            }

            return obj;
        }

        /// <summary>
        /// Converts a collection of objects of type T to an collection of Dictionary&lt;string,object>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> ConvertToDictionary<T>(this IEnumerable<T> collection, IEnumerable<IPropertyDefinition> properties)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            if (!collection.Any())
                return list;

            Type sourceType = collection.First().GetType();
            Dictionary<IPropertyDefinition, PropertyInfo> propertyInfoLookup = new Dictionary<IPropertyDefinition, PropertyInfo>();
            foreach (var result in collection)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (IPropertyDefinition prop in properties)
                {
                    PropertyInfo pi;
                    if (!propertyInfoLookup.TryGetValue(prop, out pi))
                    {
                        pi = sourceType.GetProperty(CleanString(prop.As));
                        propertyInfoLookup.Add(prop, pi);
                    }
                    dic.Add(prop.As, pi.GetValue(result, null));
                }
                list.Add(dic);
            }

            return list;
        }

        static readonly System.Text.RegularExpressions.Regex replaceRegex = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9_@]", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);

        public static string CleanString(this string value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return replaceRegex.Replace(value.Trim(), string.Empty);
        }


    }
}
