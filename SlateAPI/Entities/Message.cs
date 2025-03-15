using System;
using System.Collections.Generic;

namespace SlateAPI.Entities;

public partial class Message
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
