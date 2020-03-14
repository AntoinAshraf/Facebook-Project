using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Actions
    {
        public Actions()
        {
            RoleActions = new HashSet<RoleAction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<RoleAction> RoleActions { get; set; }
    }
}
