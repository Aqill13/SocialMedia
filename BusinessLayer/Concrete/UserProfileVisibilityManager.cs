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
    public class UserProfileVisibilityManager : IUserProfileVisibilityService
    {
        private readonly IUserProfileVisibilityRepository _userProfileVisibilityRepository;

        public UserProfileVisibilityManager(IUserProfileVisibilityRepository userProfileVisibilityRepository)
        {
            _userProfileVisibilityRepository = userProfileVisibilityRepository;
        }

        public async Task<Dictionary<ProfileField, VisibilityLevel>> GetUserProfileVisibilitySettingsAsync(string userId)
        {
            var settings = await _userProfileVisibilityRepository.GetAllAsync(upv => upv.UserId == userId);
            return settings.ToDictionary(s => s.Field, s => s.Visibility);
        }

        public async Task SetUserProfileVisibilityAsync(string userId, ProfileField field, VisibilityLevel visibility)
        {
            var existingSetting = await _userProfileVisibilityRepository.GetFirstAsync(upv => upv.UserId == userId && upv.Field == field);
            if (existingSetting == null)
            {
                var entity = new UserProfileVisibility
                {
                    UserId = userId,
                    Field = field,
                    Visibility = visibility
                };
                await _userProfileVisibilityRepository.AddAsync(entity);
            }
            else
            {
                existingSetting.Visibility = visibility;
                await _userProfileVisibilityRepository.UpdateAsync(existingSetting);
            }
        }
    }
}
