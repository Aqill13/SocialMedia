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

        public ProfileController(UserManager<AppUser> userManager, IPostService postService, IUserSocialLinkService userSocialLinkService,
            IUserProfileVisibilityService userProfileVisibilityService, IFollowService followService)
        {
            _userManager = userManager;
            _postService = postService;
            _userSocialLinkService = userSocialLinkService;
            _userProfileVisibilityService = userProfileVisibilityService;
            _followService = followService;
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
            var model = new ProfilePostsViewModel
            {
                ProfileUser = profileUser,
                SocialLinks = socialLinks,
                Posts = posts,
                ActiveTopTab = "post",
                IsOwner = profileUser.Id == currentUserId
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
    }
}
