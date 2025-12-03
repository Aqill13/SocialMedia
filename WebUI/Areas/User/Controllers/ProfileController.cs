using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.User.Models.Profile;
using static EntityLayer.Entities.UserSocialLink;

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
            var visibleSocialLinks = await _userSocialLinkService.GetVisibleUserSocialLinksAsync(profileUser.Id);
            var userSocialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(profileUser.Id);
            var visibilitySettings = await _userProfileVisibilityService.GetUserProfileVisibilitySettingsAsync(profileUser.Id);
            var model = new ProfilePostsViewModel
            {
                ProfileUser = profileUser,
                VisibleSocialLinks = visibleSocialLinks,
                UserSocialLinks = userSocialLinks,
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
        public async Task<IActionResult> About(string? username, string section = "profile", string sub = "personal", string mode = "main")
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
            var visibleSocialLinks = await _userSocialLinkService.GetVisibleUserSocialLinksAsync(profileUser.Id);
            var userSocialLinks = await _userSocialLinkService.GetUserSocialLinksAsync(profileUser.Id);
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
                VisibleSocialLinks = visibleSocialLinks,
                UserSocialLinks = userSocialLinks,
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

        public async Task<IActionResult> Friends(string? username, string section = "friends", string mode = "main")
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { area = "User" });
            if (string.IsNullOrEmpty(username))
                username = User.Identity.Name;
            var profileUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (profileUser == null)
                return NotFound();
            var currentUserId = _userManager.GetUserId(User);
            var followers = await _followService.GetFollowersAsync(profileUser.Id);
            var following = await _followService.GetFollowingsAsync(profileUser.Id);
            var isFriend = await _followService.IsFollowingAsync(currentUserId!, profileUser.Id);

            List<UserFollow> mutualUsers = new();
            if (currentUserId == profileUser.Id)
            {
                var followingIds = new HashSet<string>(following.Select(f => f.FollowingId));
                mutualUsers = followers.Where(f => followingIds.Contains(f.FollowerId)).ToList();
            }
            else
            {
                var myFollowing = await _followService.GetFollowingsAsync(currentUserId!);
                var myFollowingIds = new HashSet<string>(myFollowing.Select(f => f.FollowingId));
                mutualUsers = following.Where(f => myFollowingIds.Contains(f.FollowingId)).ToList();
            }

            var model = new ProfileFriendsViewModel
            {
                ActiveSection = section,
                ActiveTopTab = "friend",
                Followers = followers,
                Following = following,
                MutualUsers = mutualUsers,
                ProfileUser = profileUser,
                IsFriend = isFriend,
                IsOwner = profileUser.Id == currentUserId
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (mode == "main")
                    return PartialView("_ProfileFriendsContent", model);
                else
                    return PartialView("_FriendsTabBottomContent", model);
            }
            return View(model);
        }
    }
}
