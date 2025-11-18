using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class FollowManager : IFollowService
    {
        private readonly IUserFollowRepository _userFollowRepository;
        private readonly IFollowRequestRepository _followRequestRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;

        public FollowManager(IUserFollowRepository userFollowRepository, IFollowRequestRepository followRequestRepository, 
            INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _userFollowRepository = userFollowRepository;
            _followRequestRepository = followRequestRepository;
            _notificationService = notificationService;
            _userManager = userManager;
        }


        // Public account follow/unfollow
        public async Task<bool> FollowUserAsync(string currentUserId, string targetUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var targetUser = await _userManager.FindByIdAsync(targetUserId);

            if (currentUser == null || targetUser == null) 
                return false;
            if (currentUserId == targetUserId)
                return false;
            if (targetUser.IsPrivate)
                return false;
            var existingFollow = await _userFollowRepository.IsFollowingAsync(currentUserId, targetUserId);
            if (existingFollow)
                return false;
            var userFollow = new UserFollow
            {
                FollowerId = currentUserId,
                FollowingId = targetUserId,
                FollowedAt = DateTime.UtcNow
            };
            await _userFollowRepository.AddAsync(userFollow);
            currentUser.FollowingCount += 1;
            targetUser.FollowersCount += 1;
            await _userManager.UpdateAsync(currentUser);
            await _userManager.UpdateAsync(targetUser);
            await _notificationService.CreateNotificationAsync(
                targetUserId,
                currentUserId,
                NotificationType.Follow,
                $"{currentUser.UserName} started following you.",
                null
            );
            return true;
        }

        public async Task<bool> UnfollowUserAsync(string currentUserId, string targetUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (currentUser == null || targetUser == null)
                return false;
            var existingFollow = await _userFollowRepository.GetFollowRecordAsync(currentUserId, targetUserId);
            if (existingFollow == null)
                return false;
            await _userFollowRepository.DeleteAsync(existingFollow);
            currentUser.FollowingCount -= 1;
            targetUser.FollowersCount -= 1;
            await _userManager.UpdateAsync(currentUser);
            await _userManager.UpdateAsync(targetUser);
            return true;
        }

        // Check follow status and lists
        public async Task<List<UserFollow>> GetFollowersAsync(string userId)
        {
            return await _userFollowRepository.GetFollowersAsync(userId);
        }

        public async Task<List<UserFollow>> GetFollowingsAsync(string userId)
        {
            return await _userFollowRepository.GetFollowingsAsync(userId);
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _userFollowRepository.IsFollowingAsync(followerId, followingId);
        }

        // Private account follow request
        public async Task<FollowRequest?> SendFollowRequestAsync(string currentUserId, string targetUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (currentUser == null || targetUser == null)
                return null;
            var existingRequest = await _followRequestRepository.GetPendingAsync(currentUserId, targetUserId);
            if (existingRequest != null)
                return null;
            var followRequest = new FollowRequest
            {
                FromUserId = currentUserId,
                ToUserId = targetUserId,
                RequestedAt = DateTime.UtcNow,
                Status = FollowRequestStatus.Pending
            };
            await _followRequestRepository.AddAsync(followRequest);
            await _notificationService.CreateNotificationAsync(
                targetUserId,
                currentUserId,
                NotificationType.FollowRequest,
                $"{currentUser.UserName} sent you a follow request.",
                null
            );
            return followRequest;
        }

        public async Task<bool> AcceptFollowRequestAsync(int requestId)
        {
            var request = await _followRequestRepository.GetByIdAsync(requestId);

            var fromUser = await _userManager.FindByIdAsync(request.FromUserId);
            var toUser = await _userManager.FindByIdAsync(request.ToUserId);
            if (fromUser == null || toUser == null) 
                return false;

            if (request == null || request.Status != FollowRequestStatus.Pending)
                return false;
            request.Status = FollowRequestStatus.Accepted;
            request.RespondedAt = DateTime.UtcNow;
            await _followRequestRepository.UpdateAsync(request);

            var userFollow = new UserFollow
            {
                FollowerId = request.FromUserId,
                FollowingId = request.ToUserId,
                FollowedAt = DateTime.UtcNow
            };
            await _userFollowRepository.AddAsync(userFollow);
            fromUser.FollowingCount += 1;
            toUser.FollowersCount += 1;
            await _userManager.UpdateAsync(toUser);
            await _userManager.UpdateAsync(fromUser);
            await _notificationService.CreateNotificationAsync(
                request.ToUserId,
                request.FromUserId,
                NotificationType.Follow,
                $"{(await _userManager.FindByIdAsync(request.ToUserId))?.UserName ?? "UndefinedUser"} accepted your follow request.",
                null
            );
            return true;
        }

        public async Task<bool> RejectFollowRequestAsync(int requestId)
        {
            var request = await _followRequestRepository.GetByIdAsync(requestId);
            if (request == null) 
                return false;
            request.Status = FollowRequestStatus.Rejected;
            request.RespondedAt = DateTime.UtcNow;
            await _followRequestRepository.UpdateAsync(request);
            return true;
        }
    }
}
