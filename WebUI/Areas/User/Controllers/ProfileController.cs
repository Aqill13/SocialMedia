using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    //[Authorize]
    [Route("User/[controller]/[action]/{username?}")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPostService _postService;
        private readonly IUserSocialLinkService _userSocialLinkService;
        private readonly IUserProfileVisibilityService _userProfileVisibilityService;
        private readonly IFollowService _followService;
        private readonly IUserProfileInfoService _userProfileInfoService;
        private readonly IUserWorkExperienceService _userWorkExperienceService;
        private readonly IUserEducationService _userEducationService;

        public ProfileController(UserManager<AppUser> userManager, IPostService postService, IUserSocialLinkService userSocialLinkService,
            IUserProfileVisibilityService userProfileVisibilityService, IFollowService followService, IUserProfileInfoService userProfileInfoService,
            IUserWorkExperienceService userWorkExperienceService, IUserEducationService userEducationService)
        {
            _userManager = userManager;
            _postService = postService;
            _userSocialLinkService = userSocialLinkService;
            _userProfileVisibilityService = userProfileVisibilityService;
            _followService = followService;
            _userProfileInfoService = userProfileInfoService;
            _userWorkExperienceService = userWorkExperienceService;
            _userEducationService = userEducationService;
        }
        // Posts
        public async Task<IActionResult> Posts(string? username)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (string.IsNullOrEmpty(username))
                username = User.Identity.Name;
            var profileUser = await _userManager.Users
                .Include(u => u.UserProfileInfo)
                .FirstOrDefaultAsync(u => u.UserName == username);
            if (profileUser == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var posts = await _postService.GetProfilePostsAsync(profileUser.Id, currentUserId!);
            var socialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(profileUser.Id);
            var visibilitySettings = await _userProfileVisibilityService.GetUserProfileVisibilitySettingsAsync(profileUser.Id);
            var model = new ProfilePostsViewModel
            {
                ProfileUser = profileUser,
                SocialLinks = socialLinks,
                Posts = posts,
                ActiveTopTab = "post",
                IsOwner = profileUser.Id == currentUserId,
                VisibilitySettings = visibilitySettings
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProfilePostsContent", model);
            }
            return View(model);
        }

        // About
        public async Task<IActionResult> About(string? username, string section = "profile", string sub = "personal", string mode = "full")
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (string.IsNullOrEmpty(username))
                username = User.Identity.Name;
            var profileUser = await _userManager.Users
                .Include(u => u.UserProfileInfo)
                .Include(u => u.Educations)
                .Include(u => u.WorkExperiences)
                .FirstOrDefaultAsync(u => u.UserName == username);
            if (profileUser == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var socialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(profileUser.Id);
            var visibilitySettings = await _userProfileVisibilityService.GetUserProfileVisibilitySettingsAsync(profileUser.Id);
            var isFollowing = await _followService.IsFollowingAsync(currentUserId!, profileUser.Id);

            ProfileVisibilityViewModel? visibilityViewModel = null;
            if (section == "account" && sub == "visibility" && profileUser.Id == currentUserId)
            {
                VisibilityLevel Get(ProfileField f) =>
                    visibilitySettings.TryGetValue(f, out var v) ? v : VisibilityLevel.Everyone;
                visibilityViewModel = new ProfileVisibilityViewModel
                {
                    VisibilityItems = new List<ProfileVisibilityItemViewModel>
                    {
                        new() { Field = ProfileField.BirthDate, Group = "Personal Information", Label = "Birth Date", Visibility = Get(ProfileField.BirthDate) },
                        new() { Field = ProfileField.Gender, Group = "Personal Information", Label = "Gender", Visibility = Get(ProfileField.Gender) },
                        new() { Field = ProfileField.Location, Group = "Personal Information", Label = "Location", Visibility = Get(ProfileField.Location) },
                        new() { Field = ProfileField.Status, Group = "Personal Information", Label = "Status", Visibility = Get(ProfileField.Status) },
                        new() { Field = ProfileField.Bio, Group = "Personal Information", Label = "About Me", Visibility = Get(ProfileField.Bio) },
                        new() { Field = ProfileField.PhoneNumber, Group = "Personal Information", Label = "Phone number", Visibility = Get(ProfileField.PhoneNumber) },
                        new() { Field = ProfileField.Birthplace, Group = "Personal Information", Label = "Birthplace", Visibility = Get(ProfileField.Birthplace) },
                        new() { Field = ProfileField.LivesIn, Group = "Personal Information", Label = "Lives In", Visibility = Get(ProfileField.LivesIn) },
                        new() { Field = ProfileField.Hobbies, Group = "Hobbies And Interest", Label = "Hobbies", Visibility = Get(ProfileField.Hobbies) },
                        new() { Field = ProfileField.FavoriteMovies, Group = "Hobbies And Interest", Label = "Favorite Movies", Visibility = Get(ProfileField.FavoriteMovies) },
                        new() { Field = ProfileField.FavoriteGames, Group = "Hobbies And Interest", Label = "Favorite Games", Visibility = Get(ProfileField.FavoriteGames) },
                        new() { Field = ProfileField.FavoriteBooks, Group = "Hobbies And Interest", Label = "Favorite Books", Visibility = Get(ProfileField.FavoriteBooks) },
                        new() { Field = ProfileField.WorkExperience, Group = "Professional Information", Label = "Work Experience", Visibility = Get(ProfileField.WorkExperience) },
                        new() { Field = ProfileField.Education, Group = "Professional Information", Label = "Education", Visibility = Get(ProfileField.Education) },
                        new() { Field = ProfileField.SocialLinks, Group = "Professional Information", Label = "Social Links", Visibility = Get(ProfileField.SocialLinks) },
                    }
                };
            }

            var model = new ProfileAboutViewModel
            {
                ProfileUser = profileUser,
                SocialLinks = socialLinks,
                ActiveTopTab = "about",
                ActiveSection = section,
                ActiveSubSection = sub,
                IsOwner = profileUser.Id == currentUserId,
                IsFriend = isFollowing,
                VisibilitySettings = visibilitySettings,
                VisibilityViewModel = visibilityViewModel
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (mode == "main")
                    return PartialView("_ProfileAboutContent", model);
                else
                    return PartialView("_AboutRightContent", model);
            }
            return View(model);
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
                message = "Hobbies and interests updated successfully.",
                data = new
                {
                    hobbies = profileInfo.Hobbies,
                    favoriteBooks = profileInfo.FavoriteBooks,
                    favoriteMovies = profileInfo.FavoriteMovies,
                    favoriteGames = profileInfo.FavoriteGames
                }
            });
        }

        // Work Experience Add/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateWorkExperience(WorkExperienceEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            UserWorkExperience entity;
            if (model.Id > 0)
            {
                entity = await _userWorkExperienceService.GetByIdAsync(model.Id);
                if (entity == null || entity.UserId != currentUser.Id)
                    return Json(new { success = false, message = "Work experience not found" });
                entity.CompanyName = model.CompanyName;
                entity.Position = model.Position;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                await _userWorkExperienceService.UpdateAsync(entity);
            }
            else
            {
                entity = new UserWorkExperience
                {
                    UserId = currentUser.Id,
                    CompanyName = model.CompanyName,
                    Position = model.Position,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                await _userWorkExperienceService.AddAsync(entity);
            }
            return Json(new
            {
                success = true,
                message = "Work experience saved successfully",
                data = new
                {
                    id = entity.Id,
                    companyName = entity.CompanyName,
                    position = entity.Position,
                    startDateDisplay = entity.StartDate.ToString("MMMM yyyy"),
                    endDateDisplay = entity.EndDate?.ToString("MMMM yyyy") ?? "Present",
                    startDateValue = entity.StartDate.ToString("yyyy-MM-dd"),
                    endDateValue = entity.EndDate?.ToString("yyyy-MM-dd") ?? ""
                }
            });
        }

        // Work Experience Delete
        [HttpPost]
        public async Task<IActionResult> DeleteWorkExperience(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var entity = await _userWorkExperienceService.GetByIdAsync(id);
            if (entity == null || entity.UserId != currentUser.Id)
                return Json(new { success = false, message = "Work experience could not be deleted" });
            await _userWorkExperienceService.DeleteAsync(entity);
            return Json(new { success = true, message = "Work experience deleted successfully" });
        }

        // Education Add/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateEducation(EducationEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            UserEducation entity;
            if (model.Id > 0)
            {
                entity = await _userEducationService.GetByIdAsync(model.Id);
                if (entity == null || entity.UserId != currentUser.Id)
                    return Json(new { success = false, message = "Education not found" });
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.SchoolName = model.SchoolName;
                entity.Field = model.Field;
                entity.Degree = model.Degree;
                await _userEducationService.UpdateAsync(entity);
            }
            else
            {
                entity = new UserEducation
                {
                    Degree = model.Degree,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Field = model.Field,
                    SchoolName = model.SchoolName,
                    UserId = currentUser.Id
                };
                await _userEducationService.AddAsync(entity);
            }
            return Json(new
            {
                success = true,
                message = "Education saved successfully",
                data = new
                {
                    id = entity.Id,
                    schoolName = entity.SchoolName,
                    field = entity.Field,
                    degree = entity.Degree,
                    startDateDisplay = entity.StartDate.ToString("MMMM yyyy"),
                    endDateDisplay = entity.EndDate?.ToString("MMMM yyyy") ?? "Present",
                    startDateValue = entity.StartDate.ToString("yyyy-MM-dd"),
                    endDateValue = entity.EndDate?.ToString("yyyy-MM-dd") ?? ""
                }
            });
        }

        // Delete education
        [HttpPost]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var entity = await _userEducationService.GetByIdAsync(id);
            if (entity == null || entity.UserId != currentUser.Id)
                return Json(new { success = false, message = "Education could not be deleted" });
            await _userEducationService.DeleteAsync(entity);
            return Json(new { success = true, message = "Education deleted successfully" });
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

        // Add social link
        [HttpPost]
        public async Task<IActionResult> AddSocialLink(UserSocialLink.SocialPlatform platform, string url)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            var link = new UserSocialLink
            {
                UserId = user.Id,
                Platform = platform,
                Url = url,
                IsVisible = true
            };
            await _userSocialLinkService.AddAsync(link);
            var iconClass = platform switch
            {
                UserSocialLink.SocialPlatform.Instagram => "fa-brands fa-instagram",
                UserSocialLink.SocialPlatform.Facebook => "fa-brands fa-facebook-f",
                UserSocialLink.SocialPlatform.Github => "fa-brands fa-github",
                UserSocialLink.SocialPlatform.Linkedin => "fa-brands fa-linkedin-in",
                UserSocialLink.SocialPlatform.Twitter => "fa-brands fa-x-twitter",
                UserSocialLink.SocialPlatform.Youtube => "fa-brands fa-youtube",
                _ => "ph ph-globe"
            };

            return Json(new
            {
                success = true,
                message = "Added successfully",
                platformValue = (int)platform,
                iconClass = iconClass,
                url = url
            });
        }

        // Change Profile Image
        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (file != null && file.Length > 0)
            {
                var oldImageUrl = user.ImageUrl;
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/profilep", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                user.ImageUrl = "/uploads/profilep/" + newFileName;
                await _userManager.UpdateAsync(user);
                if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-profile-account"))
                {
                    var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                        System.IO.File.Delete(oldPhysicalPath);
                }
                return Json(new
                {
                    success = true,
                    message = "Profile image updated successfully",
                    imageUrl = user.ImageUrl
                });
            }
            return Json(new
            {
                success = false,
                message = "No file selected"
            });
        }

        // Remove Profile Image
        [HttpPost]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var oldImageUrl = user.ImageUrl;
            user.ImageUrl = "/uploads/profilep/default-profile-account.jpg";
            await _userManager.UpdateAsync(user);
            if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-profile-account"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            return Json(new
            {
                success = true,
                message = "Profile image removed successfully",
                imageUrl = user.ImageUrl
            });
        }

        // Change Cover Image
        [HttpPost]
        public async Task<IActionResult> ChangeCoverImage(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (file != null && file.Length > 0)
            {
                var oldImageUrl = user.CoverImageUrl;
                var newFileName = Guid.NewGuid() + ".png";
                var filePath = Path.Combine("wwwroot", "uploads/cover", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                user.CoverImageUrl = "/uploads/cover/" + newFileName;
                await _userManager.UpdateAsync(user);
                if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-cover"))
                {
                    var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                        System.IO.File.Delete(oldPhysicalPath);
                }
                return Json(new
                {
                    success = true,
                    message = "Cover image updated successfully",
                    imageUrl = user.CoverImageUrl
                });
            }
            return Json(new
            {
                success = false,
                message = "No file selected"
            });
        }

        // Remove Cover Image
        [HttpPost]
        public async Task<IActionResult> RemoveCoverImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var oldImageUrl = user.CoverImageUrl;
            user.CoverImageUrl = "/uploads/cover/default-cover-img.png";
            await _userManager.UpdateAsync(user);
            if (!string.IsNullOrWhiteSpace(oldImageUrl) && !oldImageUrl.Contains("default-cover"))
            {
                var oldPhysicalPath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/', '\\'));
                if (System.IO.File.Exists(oldPhysicalPath))
                    System.IO.File.Delete(oldPhysicalPath);
            }
            return Json(new
            {
                success = true,
                message = "Cover image removed successfully",
                imageUrl = user.CoverImageUrl
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
            profileInfo.Hobbies = model.Hobbies;
            profileInfo.FavoriteMovies = model.FavoriteMovies;
            profileInfo.FavoriteGames = model.FavoriteGames;
            profileInfo.FavoriteBooks = model.FavoriteBooks;
            await _userProfileInfoService.UpdateAsync(profileInfo);
            return View();
        }
    }
}
