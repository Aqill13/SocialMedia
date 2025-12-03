using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class ProfileSettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserProfileVisibilityService _userProfileVisibilityService;
        private readonly IUserProfileInfoService _userProfileInfoService;
        private readonly IUserWorkExperienceService _userWorkExperienceService;
        private readonly IUserEducationService _userEducationService;
        private readonly IUserSocialLinkService _userSocialLinkService;

        public ProfileSettingsController(UserManager<AppUser> userManager, IUserWorkExperienceService userWorkExperienceService,
            IUserProfileVisibilityService userProfileVisibilityService, IUserProfileInfoService userProfileInfoService, IUserEducationService userEducationService,
            IUserSocialLinkService userSocialLinkService)
        {
            _userManager = userManager;
            _userProfileVisibilityService = userProfileVisibilityService;
            _userProfileInfoService = userProfileInfoService;
            _userWorkExperienceService = userWorkExperienceService;
            _userEducationService = userEducationService;
            _userSocialLinkService = userSocialLinkService;
        }
        // About Info Visibility Update
        [HttpPost]
        public async Task<IActionResult> UpdateVisibility(ProfileField field, VisibilityLevel visibility)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var currentUserId = _userManager.GetUserId(User);
            await _userProfileVisibilityService.SetUserProfileVisibilityAsync(currentUserId!, field, visibility);
            return Ok();
        }

        // User Account Public / Private 
        [HttpPost]
        public async Task<IActionResult> TogglePrivateAccount(bool isPrivate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            user.IsPrivate = isPrivate;
            await _userManager.UpdateAsync(user);
            return Json(new
            {
                success = true,
                message = isPrivate ? "Your account is now private" : "Your account is now public"
            });
        }

        // Profile Info Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileInfo(ProfileInfoEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var profileInfo = await _userProfileInfoService.GetFirstAsync(x => x.UserId == currentUser.Id);
            if (profileInfo == null)
            {
                profileInfo = new UserProfileInfo
                {
                    UserId = currentUser.Id
                };
                await _userProfileInfoService.AddAsync(profileInfo);
            }
            profileInfo.BirthDate = model.BirthDate;
            profileInfo.Location = model.Location;
            profileInfo.Birthplace = model.Birthplace;
            profileInfo.LivesIn = model.LivesIn;
            profileInfo.Gender = model.Gender;
            profileInfo.Status = model.Status;
            profileInfo.Bio = model.Bio;
            await _userProfileInfoService.UpdateAsync(profileInfo);
            return Json(new
            {
                success = true,
                message = "Profile information updated successfully.",
                data = new
                {
                    birthDate = profileInfo.BirthDate?.ToString("d"),
                    location = profileInfo.Location,
                    birthplace = profileInfo.Birthplace,
                    livesIn = profileInfo.LivesIn,
                    gender = profileInfo.Gender,
                    status = profileInfo.Status,
                    bio = profileInfo.Bio
                }
            });
        }

        // Hobbies And Interests Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHobbiesAndInterests(HobbiesAndInterestsUpdateViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var profileInfo = await _userProfileInfoService.GetFirstAsync(x => x.UserId == currentUser.Id);
            if (profileInfo == null)
            {
                profileInfo = new UserProfileInfo
                {
                    UserId = currentUser.Id
                };
                await _userProfileInfoService.AddAsync(profileInfo);
            }
            profileInfo.Hobbies = model.Hobbies;
            profileInfo.FavoriteBooks = model.FavoriteBooks;
            profileInfo.FavoriteMovies = model.FavoriteMovies;
            profileInfo.FavoriteGames = model.FavoriteGames;
            await _userProfileInfoService.UpdateAsync(profileInfo);
            return Json(new
            {
                success = true,
                message = "Hobbies and interests updated successfully",
                data = new
                {
                    hobbies = profileInfo.Hobbies,
                    favoriteBooks = profileInfo.FavoriteBooks,
                    favoriteMovies = profileInfo.FavoriteMovies,
                    favoriteGames = profileInfo.FavoriteGames
                }
            });
        }

        // Edit Profile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var profileInfo = await _userProfileInfoService.GetFirstAsync(x => x.UserId == currentUser.Id);
            var educations = await _userEducationService.GetAllAsync(x => x.UserId == currentUser.Id);
            var workExperiences = await _userWorkExperienceService.GetAllAsync(x => x.UserId == currentUser.Id);
            var socialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(currentUser.Id);
            var model = new ProfileEditViewModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                ImageUrl = currentUser.ImageUrl,
                CoverImageUrl = currentUser.CoverImageUrl,
                Bio = profileInfo?.Bio,
                Location = profileInfo?.Location,
                BirthDate = profileInfo?.BirthDate,
                Birthplace = profileInfo?.Birthplace,
                LivesIn = profileInfo?.LivesIn,
                Status = profileInfo?.Status,
                Gender = profileInfo?.Gender,
                Hobbies = profileInfo?.Hobbies,
                FavoriteMovies = profileInfo?.FavoriteMovies,
                FavoriteGames = profileInfo?.FavoriteGames,
                FavoriteBooks = profileInfo?.FavoriteBooks,
                SocialLinks = socialLinks,
                Educations = educations,
                WorkExperiences = workExperiences
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (!ModelState.IsValid)
            {
                model.SocialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(currentUser.Id);
                model.Educations = await _userEducationService.GetAllAsync(x => x.UserId == currentUser.Id);
                model.WorkExperiences = await _userWorkExperienceService.GetAllAsync(x => x.UserId == currentUser.Id);
                return View(model);
            }
            var oldImageUrl = currentUser.ImageUrl;
            var oldCoverImageUrl = currentUser.CoverImageUrl;
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/profilep", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImageFile.CopyToAsync(stream);
                }
                currentUser.ImageUrl = "/uploads/profilep/" + newFileName;
            }
            else if (model.DeleteProfileImage)
            {
                currentUser.ImageUrl = "/uploads/profilep/default-profile-account.jpg";
            }
            if (model.CoverImageFile != null && model.CoverImageFile.Length > 0)
            {
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/cover", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImageFile.CopyToAsync(stream);
                }
                currentUser.CoverImageUrl = "/uploads/cover/" + newFileName;
            }
            else if (model.DeleteCoverImage)
            {
                currentUser.CoverImageUrl = "/uploads/cover/default-cover-img.png";
            }
            await _userManager.UpdateAsync(currentUser);
            // Delete old images if changed
            if (currentUser.ImageUrl != oldImageUrl && !string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-profile-account"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            if (currentUser.CoverImageUrl != oldCoverImageUrl && !string.IsNullOrWhiteSpace(oldCoverImageUrl) && !oldCoverImageUrl.Contains("default-cover"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldCoverImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            var profileInfo = await _userProfileInfoService.GetFirstAsync(x => x.UserId == currentUser.Id);
            if (profileInfo == null)
            {
                profileInfo = new UserProfileInfo
                {
                    UserId = currentUser.Id
                };
                await _userProfileInfoService.AddAsync(profileInfo);
            }
            profileInfo.Bio = model.Bio;
            profileInfo.Location = model.Location;
            profileInfo.BirthDate = model.BirthDate;
            profileInfo.Birthplace = model.Birthplace;
            profileInfo.LivesIn = model.LivesIn;
            profileInfo.Status = model.Status;
            profileInfo.Gender = model.Gender;
            await _userProfileInfoService.UpdateAsync(profileInfo);
            return Json(new
            {
                success = true,
                message = "Profile updated successfully",
                data = new
                {
                    bio = profileInfo.Bio,
                    location = profileInfo.Location,
                    birthDate = profileInfo.BirthDate?.ToString("yyyy-MM-dd"),
                    birthplace = profileInfo.Birthplace,
                    livesIn = profileInfo.LivesIn,
                    status = profileInfo.Status,
                    gender = profileInfo.Gender,
                    firstName = currentUser.FirstName,
                    lastName = currentUser.LastName,
                    phoneNumber = currentUser.PhoneNumber,
                    imageUrl = currentUser.ImageUrl,
                    coverImageUrl = currentUser.CoverImageUrl
                }
            });
        }
    }
}
