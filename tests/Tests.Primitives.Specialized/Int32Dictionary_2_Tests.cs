namespace Arborescence.Primitives.Specialized;

using System.Collections.Generic;
using Xunit;

public sealed class Int32Dictionary_2_Tests
{
    [Fact]
    public void TryGetValue_AfterPutting_ShouldRetrieveSameValue()
    {
        Int32Dictionary<string, List<string>> dictionary = Int32DictionaryFactory<string>.Create(new List<string>());
        const int key = 0;
        const string originalValue = "Καλημέρα";

        dictionary[key] = originalValue;
        bool hasValue = dictionary.TryGetValue(key, out string? retrievedValue);

        Assert.True(hasValue);
        Assert.Equal(originalValue, retrievedValue);
    }
}
