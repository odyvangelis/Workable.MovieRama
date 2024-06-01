namespace MovieRama.Tests;

using MovieRama.Tests.Fixtures;

[CollectionDefinition(nameof(Initialization))]
public class Initialization : ICollectionFixture<InitializationFixture>
{ }

[Collection(nameof(Initialization))]
public class MovieRamaTestBase : IClassFixture<ServiceResolver>
{ }
