using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class Ram
{
    public string MaRam { get; set; } = null!;

    public string? DungLuong { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
