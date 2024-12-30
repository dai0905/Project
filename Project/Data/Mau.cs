using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class Mau
{
    public string MaMau { get; set; } = null!;

    public string? TenMau { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
