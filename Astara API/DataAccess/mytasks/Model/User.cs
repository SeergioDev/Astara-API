using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Astara_API.DataAccess.mytasks.Model;

public partial class User
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Apellidos { get; set; }

    public int Rol { get; set; }

    [JsonIgnore]
    public virtual Role RolNavigation { get; set; } = null!;
}
