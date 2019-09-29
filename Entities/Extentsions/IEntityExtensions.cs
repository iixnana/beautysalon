using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extentsions
{
    public static class IEntityExtensions
    {
        public static bool IsObjectNull(this IEntity entity)
        {
            return entity == null;
        }

        public static bool IsEmptyObject(this IEntity entity, int id)
        {
            return !entity.Id.Equals(id);
        }

    }
}
