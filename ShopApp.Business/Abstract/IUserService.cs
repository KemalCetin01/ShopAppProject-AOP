using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.Entities.Concrete;
using ShopApp.Core.Utilities.Results;
using ShopApp.Entities.Concrete;

namespace ShopApp.Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        void Add(User user);
        User GetByMail(string email);

    }
}
