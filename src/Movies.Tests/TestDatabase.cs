using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Movies.Infrastructure;

namespace Movies.Tests;

/// <summary>
/// A throwaway SQLite database for a single test.
/// <para>
/// SQLite in-memory databases live only as long as the connection that owns them, so this
/// type holds the connection open and must be disposed (use <c>using</c>) to release it.
/// </para>
/// </summary>
public sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public TestDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<MoviesDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new MoviesDbContext(options);
        Context.Database.EnsureCreated();
    }

    public MoviesDbContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
