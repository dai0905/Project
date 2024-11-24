using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class ThuongHieu
{
    public string MaThuongHieu { get; set; } = null!;

    public string? TenThuongHieu { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
