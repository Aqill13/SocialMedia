using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserSocialLinkService
    {
        Task<List<UserSocialLink>> GetUserSocialLinksAsync(string userId);
    }
}
