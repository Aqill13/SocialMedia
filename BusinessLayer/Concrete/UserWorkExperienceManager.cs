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
    public class UserWorkExperienceManager : GenericManager<UserWorkExperience>, IUserWorkExperienceService
    {
        private readonly IUserWorkExperienceRepository _userWorkExperienceRepository;

        public UserWorkExperienceManager(IUserWorkExperienceRepository userWorkExperienceRepository) : base(userWorkExperienceRepository)
        {
            _userWorkExperienceRepository = userWorkExperienceRepository;
        }
    }
}
