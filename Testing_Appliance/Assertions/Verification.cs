using System;
using JetBrains.Annotations;
using Shouldly;

namespace Testing.Appliance.Assertions;

/// <summary>
/// Handy methods for assertion aggregators from Shouldly.
/// </summary>
public static class Verification
{

    /// <summary>
    /// Verifies that all specified conditions hold, reporting all failures at once (not just the first one).
    /// Delegates to Shouldly's <c>ShouldSatisfyAllConditions</c>.
    /// </summary>
    /// <param name="conditions">assertion actions that each must pass.</param>
    public static void Verify([InstantHandle] params Action[] conditions) =>
        ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(null, (string?)null, conditions);

    /// <summary>
    /// Verifies that all specified conditions hold, reporting all failures at once with a custom message.
    /// Delegates to Shouldly's <c>ShouldSatisfyAllConditions</c>.
    /// </summary>
    /// <param name="message">custom message included in the failure output.</param>
    /// <param name="conditions">assertion actions that each must pass.</param>
    public static void Verify(string? message, [InstantHandle] params Action[] conditions) =>
        ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(null, message, conditions);

    /// <summary>
    /// Verifies that the actual value satisfies all specified conditions, reporting all failures at once.
    /// Delegates to Shouldly's <c>ShouldSatisfyAllConditions</c>.
    /// </summary>
    /// <param name="actual">the value to verify against all conditions.</param>
    /// <param name="conditions">assertion actions that each receive <paramref name="actual"/> and must pass.</param>
    /// <typeparam name="T">the type of the value being verified.</typeparam>
    public static void Verify<T>(this T actual, [InstantHandle] params Action<T>[] conditions) =>
        actual.ShouldSatisfyAllConditions((string?)null, conditions);

    /// <summary>
    /// Verifies that the actual value satisfies all specified conditions, reporting all failures at once with a custom message.
    /// Delegates to Shouldly's <c>ShouldSatisfyAllConditions</c>.
    /// </summary>
    /// <param name="actual">the value to verify against all conditions.</param>
    /// <param name="message">custom message included in the failure output.</param>
    /// <param name="conditions">assertion actions that each receive <paramref name="actual"/> and must pass.</param>
    /// <typeparam name="T">the type of the value being verified.</typeparam>
    public static void Verify<T>(this T actual, string? message, [InstantHandle] params Action<T>[] conditions) =>
        actual.ShouldSatisfyAllConditions(message, conditions);

}
