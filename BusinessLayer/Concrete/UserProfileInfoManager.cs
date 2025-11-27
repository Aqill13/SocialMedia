using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserProfileInfoManager : GenericManager<UserProfileInfo>, IUserProfileInfoService
    {
        private readonly IUserProfileInfoRepository _userProfileInfoRepository;

        public UserProfileInfoManager(IUserProfileInfoRepository userProfileInfoRepository) : base(userProfileInfoRepository)
        {
            _userProfileInfoRepository = userProfileInfoRepository;
        }
    }
}
