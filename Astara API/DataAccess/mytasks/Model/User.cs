using System;
using System.Collections.Generic;

namespace Astara_API.DataAccess.mytasks.Model;

public partial class User
{
    public int Id { get; set; }

    public string Usuario { get; set; }

    public string Password { get; set; }
}
