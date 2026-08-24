
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class TaskController : ControllerBase
{
    // Simular base de datos en memoria
    private static readonly List<TaskItem> _tasks = new()
   {
       new TaskItem { Id = 1, Title = "Aprender Git Flow", Completed = true},
       new TaskItem { Id = 2, Title = "Crear web API", Completed = false},
   };

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll()
    {
        return Ok(_tasks);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create([FromBody] TaskItem newTask)
    {
        // Asignamos un ID autoincrementable simple
        newTask.Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
        newTask.CreatedAt = DateTime.UtcNow;

        _tasks.Add(newTask);

        // Retonar HTTP 201 created y la tarea recien agregada
        return CreatedAtAction(nameof(GetAll), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
    {
        // Buscamos si la tarea existe en la lista
        var existingTask = _tasks.FirstOrDefault(t => t.Id == id);

        if (existingTask == null)
            return NotFound(new { message = $"No se encontró la tarea con Id {id}" });

        // Actualizamos propiedades 
        existingTask.Title = updatedTask.Title;
        existingTask.Completed = updatedTask.Completed;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Buscamos tarea
        var existingTask = _tasks.FirstOrDefault(t => t.Id == id);

        if (existingTask == null)
            return NotFound(new { message = $"No se encontró la tarea con id {id}" });

        // Eliminamos de la lista
        _tasks.Remove(existingTask);

        return NoContent();
    }

}