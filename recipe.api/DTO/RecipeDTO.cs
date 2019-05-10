using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipe.api.DTO
{
    public class RecipeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
