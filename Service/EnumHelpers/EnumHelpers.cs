﻿using Microsoft.EntityFrameworkCore;

namespace Service.EnumHelpers
{
    public static class Seeder
    {
        public static void SeedEnumData<TData, TEnum>(DbSet<TData> items)
            where TData : EnumBase<TEnum>
            where TEnum : struct
        {
            var enumType = EnsureEnum<TEnum>();

            foreach (TEnum evalue in Enum.GetValues(enumType))
            {
                var id = (int)Convert.ChangeType(evalue, typeof(int));

                if (id <= 0)
                    throw new Exception("Enum underlying value must start with 1");

                if (!items.Any(a => a.Id == id))
                {
                    var item = Activator.CreateInstance<TData>();
                    item.Id = id;
                    item.Name = Enum.GetName(enumType, evalue);
                    items.Add(item);
                }
            }
        }

        private static Type EnsureEnum<TEnum>()
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
                throw new Exception($"Type '{type.AssemblyQualifiedName}' must be enum");

            var underlyingType = Enum.GetUnderlyingType(type);

            if (underlyingType != typeof(int))
                throw new Exception("Enum underlyingType must be int");

            return type;
        }

    }
}
