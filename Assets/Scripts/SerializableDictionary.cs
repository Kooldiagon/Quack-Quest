using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializable
{
    public SerializableDictionary() { }

    protected SerializableDictionary(SerializationInfo info, StreamingContext context)
    {
        int count = info.GetInt32("Count");
        for (int i = 0; i < count; i++)
        {
            TKey key = (TKey)info.GetValue($"Key{i}", typeof(TKey));
            TValue value = (TValue)info.GetValue($"Value{i}", typeof(TValue));
            this.Add(key, value);
        }
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Count", this.Count);
        int i = 0;
        foreach (var kvp in this)
        {
            info.AddValue($"Key{i}", kvp.Key);
            info.AddValue($"Value{i}", kvp.Value);
            i++;
        }
    }
}
