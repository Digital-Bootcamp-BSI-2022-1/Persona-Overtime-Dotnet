using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_2.Infrastructure.Data.Models;

namespace dotnet_2.Infrastructure.Dto
{
    public class UserOTDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public UserOTDto() { }
        public UserOTDto(User userItem) =>
        (id, name) = (userItem.id, userItem.name);
    }
}