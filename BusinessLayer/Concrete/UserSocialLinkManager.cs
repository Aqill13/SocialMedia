using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserSocialLinkManager : IUserSocialLinkService
    {
        private readonly IUserSocialLinkRepository _userSocialLinkRepository;

        public UserSocialLinkManager(IUserSocialLinkRepository userSocialLinkRepository)
        {
            _userSocialLinkRepository = userSocialLinkRepository;
        }

        public async Task<List<UserSocialLink>> GetUserSocialLinksAsync(string userId)
        {
            return await _userSocialLinkRepository.GetAllAsync(x => x.UserId == userId && x.IsVisible);
        }
    }
}
