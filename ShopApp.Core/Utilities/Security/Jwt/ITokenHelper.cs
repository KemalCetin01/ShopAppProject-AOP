using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.Entities.Concrete;

namespace ShopApp.Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
