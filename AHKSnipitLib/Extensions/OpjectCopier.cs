using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions
{
    

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ExtensionOperation
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Populates the retun value properties with the value from current instance
        /// </summary>
        /// <typeparam name="T">The Class type</typeparam>
        /// <param name="source">Current Instance of Class</param>
        /// <returns>
        /// New instance of <typeparamref name="T"/> Poplulated ti the values of source
        /// </returns>
        /// <remarks>
        /// If a property exsit with the Name Parent it will be Ignored
        /// </remarks>
        /// <example>
        /// itemType newItem = item.Poplulate(true);
        /// </example>
        public static T Poplulate<T>(this T source) where T : class, new()
        {
            return Poplulate(source, false);
        }

        /// <summary>
        /// Populates the retun value properties with the value from current instance
        /// </summary>
        /// <typeparam name="T">The Class type</typeparam>
        /// <param name="source">Current Instance of Class</param>
        /// <param name="IgnoreArray">If True any properties that are Arrays will be Ignored; Otherwise the they will be incldued</param>
        /// <returns>
        /// New instance of <typeparamref name="T"/> Poplulated ti the values of source
        /// </returns>
        /// <remarks>
        /// If a property exsit with the Name Parent it will be Ignored
        /// </remarks>
        /// <example>
        /// itemType newItem = item.Poplulate(true);
        /// </example>
        public static T Poplulate<T>(this T source, bool IgnoreArray) where T : class, new()
        {
            T newT = new T();
            PoplulateOther(source, newT, IgnoreArray);
            return newT;
        }

        /// <summary>
        /// Populates the dest properties with the value from current instance
        /// </summary>
        /// <typeparam name="T">The Class type</typeparam>
        /// <param name="source">Current Instance of Class</param>
        /// <param name="dest">Destination instance of Clase</param>
        /// <remarks>
        /// If a property exsit with the Name Parent it will be Ignored
        /// </remarks>
        /// <example>
        /// itemType newItem = new itemType();
        /// item.PoplulateOther(newItem);
        /// </example>
        public static void PoplulateOther<T>(this T source, T dest) where T : class
        {
            PoplulateOther(source, dest, false);
        }

        /// <summary>
        /// Populates the dest properties with the value from current instance
        /// </summary>
        /// <typeparam name="T">The Class type</typeparam>
        /// <param name="source">Current Instance of Class</param>
        /// <param name="dest">Destination instance of Clase</param>
        /// <param name="IgnoreArray">If True any properties that are Arrays will be Ignored; Otherwise the they will be incldued</param>
        /// <remarks>
        /// If a property exsit with the Name Parent it will be Ignored
        /// </remarks>
        /// <example>
        /// itemType newItem = new itemType();
        /// item.PoplulateOther(newItem, true);
        /// </example>
        public static void PoplulateOther<T>(this T source, T dest, bool IgnoreArray) where T : class
        {
            
            if (Object.ReferenceEquals(source, null))
            {
                return;
            }

            if (Object.ReferenceEquals(dest, null))
            {
                return;
            }


            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (sourcePropertyInfo.Name == @"Parent")
                {
                    continue;
                }
                if ((IgnoreArray == true) && (sourcePropertyInfo.PropertyType.IsArray == true))
                {
                    continue;
                }

                PropertyInfo destPropertyInfo = dest.GetType().GetProperty(sourcePropertyInfo.Name);

                if (destPropertyInfo.CanWrite == true)
                {
                    destPropertyInfo.SetValue(
                       dest,
                       sourcePropertyInfo.GetValue(source, null),
                       null);
                }
            }
            return;
        }
    }
}
