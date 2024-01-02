namespace Arborescence.Primitives.Specialized;

using System;
using System.Collections.Generic;
using Xunit;

public sealed class Int32Dictionary_3_Tests
{
    private const string AbsenceMarkerLowerCase = "n/a";
    private const string AbsenceMarkerUpperCase = "N/A";

    [Fact]
    public void TryGetValue_ForAbsenceMarker_ShouldReturnFalse()
    {
        Int32Dictionary<string, List<string>, StringComparer> dictionary = CreateSparseDictionary();
        Assert.False(dictionary.TryGetValue(1, out _));
        Assert.False(dictionary.TryGetValue(3, out _));
    }

    [Fact]
    public void ContainsKey_ForAbsenceMarker_ShouldReturnFalse()
    {
        Int32Dictionary<string, List<string>, StringComparer> dictionary = CreateSparseDictionary();
        Assert.False(dictionary.ContainsKey(1));
        Assert.False(dictionary.ContainsKey(3));
    }

    [Fact]
    public void GetCount_ForSparseDictionary_ShouldBeLessThanMaxCount()
    {
        Int32Dictionary<string, List<string>, StringComparer> dictionary = CreateSparseDictionary();
        int count = dictionary.GetCount();
        int maxCount = dictionary.MaxCount;
        Assert.True(count <= maxCount);
        Assert.Equal(2, count);
        Assert.Equal(3, maxCount);
    }

    private static Int32Dictionary<string, List<string>, StringComparer> CreateSparseDictionary()
    {
        var dictionary = Int32Dictionary<string>.CreateWithAbsence(
            new List<string>(), StringComparer.InvariantCultureIgnoreCase, AbsenceMarkerUpperCase);
        dictionary[0] = "Καλημέρα";
        dictionary[1] = AbsenceMarkerLowerCase;
        dictionary[2] = "Mañana";
        return dictionary;
    }
}
