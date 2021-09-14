using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.Entities;

namespace ShopApp.Entities.Dtos
{
    public class UserForLoginDto:IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
