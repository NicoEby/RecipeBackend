using System;
using System.Collections.Generic;
using System.Text;

namespace recipe.data.Models
{
   public partial class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Recipe()
        {
            Id = Guid.NewGuid();
        }
    }
}
