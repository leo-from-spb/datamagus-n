using JetBrains.Annotations;
using Shouldly;

namespace Test.Appliance.Assertions;

public static class Verification
{

    public static void Verify([InstantHandle] params Action[] conditions) =>
        ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(null, (string?)null, conditions);

    public static void Verify(string? message, [InstantHandle] params Action[] conditions) =>
        ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(null, message, conditions);

    public static void Verify<T>(this T actual, [InstantHandle] params Action<T>[] conditions) =>
        actual.ShouldSatisfyAllConditions((string?)null, conditions);

    public static void Verify<T>(this T actual, string? message, [InstantHandle] params Action<T>[] conditions) =>
        actual.ShouldSatisfyAllConditions(message, conditions);

}
