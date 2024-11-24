using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class BoNhoTrong
{
    public string MaBoNhoTrong { get; set; } = null!;

    public string? DungLuong { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
