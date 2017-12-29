using System;
using System.Reflection;
namespace CommonAPI
{
	public static class DeepCloneHelper
	{
		public static T DeepClone<T>(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("Object is null");
			}
			return (T)((object)DeepCloneHelper.CloneProcedure(obj));
		}
		private static object CloneProcedure(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			Type type = obj.GetType();
			if (type.IsPrimitive || type.IsEnum || type == typeof(string))
			{
				return obj;
			}
			if (type.IsArray)
			{
				Type type2 = Type.GetType(type.FullName.Replace("[]", string.Empty));
				Array array = obj as Array;
				Array array2 = Array.CreateInstance(type2, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					array2.SetValue(DeepCloneHelper.CloneProcedure(array.GetValue(i)), i);
				}
				return array2;
			}
			if (type.IsClass || type.IsValueType)
			{
				object obj2 = Activator.CreateInstance(obj.GetType());
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo[] array3 = fields;
				for (int j = 0; j < array3.Length; j++)
				{
					FieldInfo fieldInfo = array3[j];
					object value = fieldInfo.GetValue(obj);
					if (value != null)
					{
						fieldInfo.SetValue(obj2, DeepCloneHelper.CloneProcedure(value));
					}
				}
				return obj2;
			}
			throw new ArgumentException("The object is unknown type");
		}
	}
}
