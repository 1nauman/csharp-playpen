// See https://aka.ms/new-console-template for more information

using active_record;
using Dapper;
using Microsoft.Data.Sqlite;

await using var init = new SqliteConnection("Data Source=todo.db");
await init.OpenAsync();
await init.ExecuteAsync("DROP TABLE IF EXISTS TodoItems");
await init.ExecuteAsync("""
                        CREATE TABLE TodoItems (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Completed INTEGER NOT NULL
                        );
                        """);

var todo = new TodoItem { Title = "Learn Active Record", Completed = false };
await todo.SaveAsync();
Console.WriteLine($"Created TodoItem with Id {todo.Id} and Title: {todo.Title}");

// Update
todo.Completed = true;
await todo.SaveAsync();
Console.WriteLine($"Updated TodoItem {todo.Id}");

var loaded = await TodoItem.FindAsync(todo.Id);
Console.WriteLine($"Loaded: {loaded?.Title}, Completed={loaded?.Completed}");

// List all
var allTodoItems = await TodoItem.FindAllAsync();
foreach (var item in allTodoItems)
    Console.WriteLine($"{item.Id}: {item.Title} (Done: {item.Completed})");

// Delete
await todo.DeleteAsync();