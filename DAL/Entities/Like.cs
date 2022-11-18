﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Like
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;

    }
}
