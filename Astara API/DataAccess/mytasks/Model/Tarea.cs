using System;
using System.Collections.Generic;

namespace Astara_API.DataAccess.mytasks.Model;

public partial class Tarea
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public int UsuarioId { get; set; }

    public virtual User Usuario { get; set; } = null!;
}
