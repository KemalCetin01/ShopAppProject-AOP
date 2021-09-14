using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.DataAccess;
using ShopApp.Core.Entities.Concrete;
using ShopApp.Entities.Concrete;

namespace ShopApp.DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
    }
}
