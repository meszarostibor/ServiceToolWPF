using System;
using System.Collections.Generic;

namespace ServiceToolWPF.Classes;

public partial class UserDTO
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public int Permission { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

}
