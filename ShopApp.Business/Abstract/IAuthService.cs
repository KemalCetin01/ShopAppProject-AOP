using ShopApp.Core.Utilities.Results;
using ShopApp.Core.Entities.Concrete;
using ShopApp.Entities.Dtos;
using ShopApp.Core.Utilities.Security.Jwt;

namespace ShopApp.Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto,string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<string> getUserId(string token);

    }
}
