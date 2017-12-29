using System;
using System.Collections.Generic;
using System.Reflection;
namespace EcoDevice.AccessAPI
{
	public class ReflactUtil
	{
		public static System.Reflection.BindingFlags BindingAllFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
		public static System.Reflection.ConstructorInfo[] getConstructors(string assembltWithType)
		{
			System.Type type = ReflactUtil.typeFromName(assembltWithType);
			return type.GetConstructors(ReflactUtil.BindingAllFlags);
		}
		public static System.Type typeFromName(string assembltWithType)
		{
			if (assembltWithType == null || "".Equals(assembltWithType))
			{
				throw new System.ArgumentNullException("assembltWithType");
			}
			return System.Type.GetType(assembltWithType);
		}
		public static object newInstance(System.Type t, System.Collections.Generic.IDictionary<System.Type, object> paramArray)
		{
			if (t == null)
			{
				throw new System.ArgumentNullException("t");
			}
			int num = (paramArray == null) ? 0 : paramArray.Count;
			System.Type[] array = new System.Type[num];
			object[] array2 = new object[num];
			if (num > 0)
			{
				System.Collections.Generic.IEnumerator<System.Type> enumerator = paramArray.Keys.GetEnumerator();
				int num2 = 0;
				while (enumerator.MoveNext())
				{
					array[num2] = enumerator.Current;
					array2[num2] = paramArray[array[num2]];
					num2++;
				}
			}
			System.Reflection.ConstructorInfo constructor = t.GetConstructor(array);
			if (constructor == null)
			{
				throw new System.Exception();
			}
			return constructor.Invoke(array2);
		}
		public static System.Reflection.FieldInfo[] getFieldInfos(System.Type type)
		{
			return type.GetFields(ReflactUtil.BindingAllFlags);
		}
		public static System.Attribute Exists(System.Type attributeType, System.Reflection.FieldInfo fieldInfo)
		{
			System.Attribute[] array = (System.Attribute[])fieldInfo.GetCustomAttributes(attributeType, false);
			if (array != null && array.Length > 0)
			{
				return array[0];
			}
			return null;
		}
	}
}
