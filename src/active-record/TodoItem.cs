using Dapper;
using Microsoft.Data.Sqlite;

namespace active_record;

public class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool Completed { get; set; }

    private static SqliteConnection GetConnection() => new("Data Source=todo.db");

    public async Task SaveAsync()
    {
        await using var connection = GetConnection();
        connection.Open();

        if (Id == 0) // insert
        {
            Id = await connection.ExecuteScalarAsync<int>(
                "INSERT INTO TodoItems (Title, Completed) VALUES (@Title, @Completed); SELECT last_insert_rowid();",
                this
            );
        }
        else
        {
            await connection.ExecuteAsync(
                "UPDATE TodoItems SET Title = @Title, Completed = @Completed WHERE Id = @Id",
                this);
        }
    }

    public async Task DeleteAsync()
    {
        if (Id == 0) return;

        await using var connection = GetConnection();
        connection.Open();
        await connection.ExecuteAsync("DELETE FROM TodoItems WHERE Id = @Id;", this);
    }

    public static async Task<TodoItem?> FindAsync(int id)
    {
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QuerySingleOrDefaultAsync<TodoItem>("SELECT * FROM TodoItems WHERE Id = @Id",
            new { Id = id });
    }

    public static async Task<IEnumerable<TodoItem>> FindAllAsync()
    {
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<TodoItem>("SELECT * FROM TodoItems");
    }
}