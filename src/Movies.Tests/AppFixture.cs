using Movies.Application.Mappings;

namespace Movies.Tests;

/// <summary>
/// Applies the same Mapster configuration the application applies at startup.
/// <para>
/// Mapping is registered from <c>AddApplication()</c>. Tests that construct handlers directly
/// bypass the DI container, so they must configure mapping themselves — otherwise
/// <c>Adapt&lt;T&gt;()</c> silently produces objects with null members.
/// </para>
/// </summary>
public sealed class AppFixture
{
    public AppFixture()
    {
        MappingConfig.Configure();
    }
}

[CollectionDefinition(Name)]
public sealed class AppCollection : ICollectionFixture<AppFixture>
{
    public const string Name = "Application";
}
