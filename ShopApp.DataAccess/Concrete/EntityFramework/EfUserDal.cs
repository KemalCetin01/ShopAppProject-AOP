using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.Core.Entities.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework.Contexts;
using ShopApp.Entities.Concrete;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfUserDal:EfEntityRepositoryBase<User, ShopAppContext>,IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new ShopAppContext())
            {
                var result = from operationClaim in context.OperationClaims
                    join userOperationClaim in context.UserOperationClaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim {Id = operationClaim.Id, Name = operationClaim.Name};
                return result.ToList();

            }
        }
    }
}
