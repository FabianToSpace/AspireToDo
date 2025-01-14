using ToDoApp.Data.Models;

namespace ToDoApp.Web;

public class ToDoApiClient(HttpClient client)
{
    public async Task<IEnumerable<ToDoEntry>> GetToDos(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<ToDoEntry> toDoEntries = [];
        
        await foreach (var toDo in client.GetFromJsonAsAsyncEnumerable<ToDoEntry>("/todos", cancellationToken))
        {
            if (toDoEntries.Count >= maxItems)
            {
                break;
            }

            if (toDo is not null)
            {
                toDoEntries.Add(toDo);
            }
        }

        return toDoEntries;
    }

    public async Task<ToDoEntry?> GetToDo(int id)
    {
        var toDo = await client.GetFromJsonAsync<ToDoEntry>($"/todos/{id}");

        return toDo;
    }

    public async Task<HttpResponseMessage> AddToDo(ToDoEntry toDo)
    {
        return await client.PostAsJsonAsync("/todos", toDo);
    }

    public async Task<HttpResponseMessage> PatchToDo(ToDoEntry toDo)
    {
        return await client.PatchAsJsonAsync($"/todos/{toDo.Id}", toDo);
    }

    public async Task<HttpResponseMessage> DeleteToDo(int id)
    {
        return await client.DeleteAsync($"/todos/{id}");
    }
}