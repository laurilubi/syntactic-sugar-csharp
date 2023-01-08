using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Syntactic.Sugar.Core.Collections;
using Syntactic.Sugar.Core.SimpleValues;
using Syntactic.Sugar.Core.String;

namespace Syntactic.Sugar.Core.Enum;

public static class EnumTools
{
    private static readonly ConcurrentDictionary<Type, CacheItem> Cache = new();

    public static string GetName(this System.Enum value)
    {
        return value.ToString();
    }

    public static string GetDescription(this System.Enum value)
    {
        var enumType = GetEnumType(value);
        var cacheItem = GetCached(enumType);
        var simpleEnumValue = cacheItem.ByValue.SingleOrDefault(value);
        AssertTools.Assert(simpleEnumValue != null,
            () => new ArgumentException($"Enum value {value} has no description in enum {enumType}."));
        return simpleEnumValue.Description;
    }

    public static string GetDescriptionOrName(this System.Enum value)
    {
        var enumType = GetEnumType(value);
        var cacheItem = GetCached(enumType);
        var simpleEnumValue = cacheItem.ByValue.SingleOrDefault(value);
        return simpleEnumValue?.Description ?? GetName(value);
    }

    public static T FromName<T>(string name) where T : struct
    {
        var nullableEnum = FromNameNullable<T>(name);
        AssertTools.Assert(nullableEnum != null, () =>
        {
            var type = typeof(T);
            return new ArgumentException($"Missing name for enum {type}.");
        });
        return nullableEnum.Value;
    }

    public static T? FromNameNullable<T>(string name) where T : struct
    {
        var enumType = GetEnumType<T>();

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var cacheItem = GetCached(enumType);
        var simpleEnumValue = cacheItem.ByName.SingleOrDefault(name);
        if (simpleEnumValue != null)
        {
            return (T?)simpleEnumValue.Value;
        }

        var errorMessage = string.Format("Invalid name '{0}' for enum {1}. Valid names are {2}.",
            name,
            enumType,
            cacheItem.ByName.Keys.Join());
        throw new ArgumentException(errorMessage, nameof(name));
    }

    public static T FromDescription<T>(string description) where T : struct
    {
        var nullableEnum = FromDescriptionNullable<T>(description);
        AssertTools.Assert(nullableEnum != null, () =>
        {
            var type = typeof(T);
            return new ArgumentException($"Missing description for enum {type}.");
        });
        return nullableEnum.Value;
    }

    public static T? FromDescriptionNullable<T>(string description) where T : struct
    {
        var enumType = GetEnumType<T>();

        if (string.IsNullOrEmpty(description))
        {
            return null;
        }

        var cacheItem = GetCached(enumType);
        var simpleEnumValue = cacheItem.ByDescription.SingleOrDefault(description);
        if (simpleEnumValue != null)
        {
            return (T?)simpleEnumValue.Value;
        }

        var errorMessage = string.Format("Invalid description '{0}' for enum {1}. Valid descriptions are {2}.",
            description,
            enumType,
            cacheItem.ByDescription.Keys.Join());
        throw new ArgumentException(errorMessage, nameof(description));
    }

    public static List<SimpleEnumValue> GetValues<T>()
    {
        var enumType = GetEnumType<T>();
        var cacheItem = GetCached(enumType);
        return cacheItem.ByValue.Values.ToList();
    }

    public static List<T> GetValuesTyped<T>()
    {
        return GetValues<T>()
            .Select(v => v.Value)
            .Cast<T>()
            .ToList();
    }

    public static bool IsValidValue<T>(T value)
    {
        return value.IsOneOf(GetValuesTyped<T>());
    }

    private static Type GetEnumType(System.Enum value)
    {
        var type = value.GetType();
        AssertTools.Assert(type.IsEnum,
            () => throw new InvalidOperationException($"Type {type} is expected to be an enum."));
        return type;
    }

    private static Type GetEnumType<T>()
    {
        var type = typeof(T);
        AssertTools.Assert(type.IsEnum,
            () => throw new InvalidOperationException($"Type {type} is expected to be an enum."));
        return type;
    }

    private static CacheItem GetCached(Type type)
    {
        if (Cache.ContainsKey(type)) return Cache[type];

        var cacheItem = new CacheItem();
        foreach (var field in type.GetFields().Where(_ => _.IsStatic))
        {
            var value = (ValueType)field.GetValue(null);
            var name = field.Name;
            var descriptionAttribute = (DescriptionAttribute)
                Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            var description = descriptionAttribute?.Description;

            var simpleEnumValue = new SimpleEnumValue(value, name, description);
            cacheItem.ByValue.TryAdd(value, simpleEnumValue);
            cacheItem.ByName.TryAdd(name, simpleEnumValue);
            if (description != null)
                cacheItem.ByDescription.TryAdd(description, simpleEnumValue);
        }

        Cache.TryAdd(type, cacheItem);
        return cacheItem;
    }

    private class CacheItem
    {
        public ConcurrentDictionary<ValueType, SimpleEnumValue> ByValue { get; } = new();
        public ConcurrentDictionary<string, SimpleEnumValue> ByName { get; } = new();
        public ConcurrentDictionary<string, SimpleEnumValue> ByDescription { get; } = new();
    }
}