using System;
using System.Reflection;

namespace DDDIntro.UnitTests
{
    public static class EntityExtensions
    {
        public static TEntity WithId<TEntity>(this TEntity obj, int id) where TEntity : class
        {
            var fieldInfo = typeof(TEntity)
                .GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, id);
            }
            else
            {
                var propertyInfo = typeof (TEntity).GetProperty("Id");
                if (propertyInfo == null)
                {
                    throw new ArgumentException("No field named 'id' or property named 'Id' found on object of type: " + typeof(TEntity).FullName);
                }
                propertyInfo.SetValue(obj, id, null);
            }

            return obj;
        }
    }
}