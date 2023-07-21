using System;
using System.Collections.Generic;

namespace Astara_API.DataAccess.mytasks.Model;

public partial class Role
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public bool? Active { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
