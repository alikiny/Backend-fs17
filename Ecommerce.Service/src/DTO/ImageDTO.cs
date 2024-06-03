using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Service.src
{
    public class ImageReadDto
    {
        public Guid Id { get; }
        public string Url { get; set; }

        public ImageReadDto(Guid id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}