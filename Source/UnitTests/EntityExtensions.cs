using System;
using System.Reflection;
using DDDIntro.Domain.Abstract;

namespace DDDIntro.UnitTests
{
    public static class EntityExtensions
    {
        public static TEntity WithId<TEntity>(this TEntity obj, int id) where TEntity : EntityWithGeneratedId
        {
            var fieldInfo = typeof(TEntity)
                .GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, id);
            }
            else
            {
                var propertyInfo = typeof (EntityWithGeneratedId).GetProperty("Id");
                if (propertyInfo == null)
                {
                    throw new ArgumentException("No field named 'id' or property named 'Id' found on object of type: " + typeof (TEntity).FullName);
                }
                propertyInfo.SetValue(obj, id, null);
            }

            return obj;
        }
    }
}