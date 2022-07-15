using System;
using System.Linq;
using System.Reflection;
using BinarySerialization;

namespace UkTote
{
    public static class Size
    {
        public static int Of(ushort x)
        {
            return sizeof(ushort);
        }

        public static int Of(uint x)
        {
            return sizeof(uint);
        }

        public static int Of(ulong x)
        {
            return sizeof(ulong);
        }

        public static int Of(int x)
        {
            return sizeof(int);
        }

        public static int Of(Type type)
        {
            var name = type.Name;

            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    type = type.GetEnumUnderlyingType();
                }

                switch (type.Name)
                {
                    case "Byte":
                        return 1;
                    case "Int16":
                    case "UInt16":
                        return 2;
                    case "Int32":
                    case "UInt32":
                        return 4;
                    case "Int64":
                    case "UInt64":
                        return 8;
                    case "String":
                        return 1;
                }

                throw new ArgumentException();
            }
            else if (type.IsGenericType)
            {
                return Of(type.GenericTypeArguments[0]);
            }
            else
            {
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var ret = 0;
                foreach (var property in properties)
                {
                    if (property.CustomAttributes.Any(a => a.AttributeType == typeof(FieldOrderAttribute)))
                    {
                        if (property.GetCustomAttribute(typeof(FieldLengthAttribute)) is FieldLengthAttribute fieldLengthAttribute)
                        {
                            ret += (int)fieldLengthAttribute.ConstLength;
                        }
                        else if (property.GetCustomAttribute(typeof(FieldCountAttribute)) is FieldCountAttribute fieldCountAttribute)
                        {
                            ret += Of(property.PropertyType) * (int)fieldCountAttribute.ConstCount;
                        }
                        else
                        {
                            ret += Of(property.PropertyType);
                        }
                    }
                }
                if (type == typeof(Message.RequestMessage) || type.IsSubclassOf(typeof(Message.RequestMessage)))
                {
                    ret -= Message.MessageBase.HEADER_LENGTH - sizeof(ushort); // the derived length doesn't get located by the reflector without an explicit fieldcount attribute
                }
                else if (type.IsSubclassOf(typeof(Message.MessageBase)))
                {
                    ret -= Message.MessageBase.HEADER_LENGTH;
                }

                if (ret < 0) throw new ArgumentException();
                return ret;
            }
        }

        public static int Of<T>(T t) where T : Message.MessageBase
        {
            var type = t.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var ret = 0;
            foreach (var property in properties)
            {
                if (property.CustomAttributes.Any(a => a.AttributeType == typeof(FieldOrderAttribute)))
                {
                    var fieldCountAttribute = type.GetCustomAttribute(typeof(FieldCountAttribute));
                    if (fieldCountAttribute == null)
                    {
                        ret += Of(property.PropertyType);
                    }
                    else
                    {
                        var x = 1;
                    }
                }
            }
            return ret - Message.MessageBase.HEADER_LENGTH;
        }
    }
}
