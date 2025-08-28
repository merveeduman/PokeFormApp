using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonReviewApp.Dto
{
    public class BulkDeleteRequestDto
    {
        public List<int> Ids { get; set; } = new List<int>();
    }
}

