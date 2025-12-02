using System;

namespace Core.Services;

/// <summary>
/// Marks a class that it is an internal service.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class ServiceAttribute : Attribute;

