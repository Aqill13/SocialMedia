using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserSocialLinkService : IGenericService<UserSocialLink>
    {
        Task<List<UserSocialLink>> GetUserSocialLinksAsync(string userId);
        Task<List<UserSocialLink>> GetVisibleUserSocialLinksAsync(string userId);
    }
}
