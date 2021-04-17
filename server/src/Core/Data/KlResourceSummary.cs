using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Keyless]
    public class KlResourceSummary
    {
        /// <summary>
        ///     元素名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     ID, 主键。
        /// </summary>
        public long Id { get; set; }
        public string Uploader { get; set; }
        public int Size { get; set; }
    }
}
