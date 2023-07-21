using System;
using System.Collections.Generic;

namespace Astara_API.DataAccess.mytasks.Model;

public partial class User
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Apellidos { get; set; }

    public int Rol { get; set; }

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
