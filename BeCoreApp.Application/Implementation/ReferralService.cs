using AutoMapper;
using AutoMapper.QueryableExtensions;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.RealEstate;
using BeCoreApp.Application.ViewModels.Product;
using BeCoreApp.Data.Entities;
using BeCoreApp.Data.Enums;
using BeCoreApp.Data.IRepositories;
using BeCoreApp.Infrastructure.Interfaces;
using BeCoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BeCoreApp.Utilities.Helpers;
using BeCoreApp.Application.ViewModels.Valuesshare;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BeCoreApp.Application.Implementation
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _referralRepository;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ReferralService(
            IBlogCategoryService blogCategoryService,
            IBlogRepository blogRepository,
            IReferralRepository referralRepository,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _blogRepository = blogRepository;
            _blogCategoryService = blogCategoryService;
            _referralRepository = referralRepository;
            _unitOfWork = unitOfWork;
        }

        public List<ReferralGroupViewModel> GetAll(string appUserId)
        {
            var referralGroups = new List<ReferralGroupViewModel>();

            var blogQuery = _blogRepository.FindAll();

            var referralQuery = _referralRepository.FindAll();

            var blogCategories = _blogCategoryService.GetMainItems();
            foreach (var blogcategory in blogCategories)
            {
                var model = new ReferralGroupViewModel();
                model.Name = blogcategory.Name;
                model.Description = blogcategory.Description;

                var blogCategoryIds = _blogCategoryService
                    .GetBlogCategoryIdsById(blogcategory.Id, MenuFrontEndType.BaiViet);

                var blogs = blogQuery.Where(x => blogCategoryIds.Contains(x.BlogCategoryId)
                                                    && !string.IsNullOrWhiteSpace(x.ReferralLink)).ToList();
                foreach (var blog in blogs)
                {
                    var referral = GetByBlogId(appUserId, blog.Id);
                    if (referral != null)
                    {
                        model.Referrals.Add(referral);
                    }
                    else
                    {
                        model.Referrals.Add(new ReferralViewModel
                        {
                            BlogId = blog.Id,
                            BlogName = blog.Name,
                            ReferralLinkRule = blog.ReferralLinkRule,
                            BlogImage = blog.Image,
                            BlogDescription = blog.Description
                        });
                    }
                }

                referralGroups.Add(model);
            }

            return referralGroups;
        }

        public async Task<ReferralViewModel> GetReferralLinkUpline(string appUserId, int blogId)
        {
            var model = new ReferralViewModel();

            var blog = _blogRepository.FindById(blogId);

            var referralQuery = _referralRepository.FindAll();

            var appUser = await _userManager.FindByIdAsync(appUserId);

            var appUserUplineF1 = await _userManager.FindByIdAsync(appUser.ReferralId.ToString());
            if (appUserUplineF1.IsSystem)
            {
                model.BlogName = blog.Name;
                model.ReferralLink = blog.ReferralLink;
                model.Description = "";
            }
            else
            {
                var referralUplineF1 = referralQuery
                    .FirstOrDefault(x => x.BlogId == blogId && x.AppUserId == appUserUplineF1.Id);
                if (referralUplineF1 != null)
                {
                    model.BlogName = blog.Name;
                    model.ReferralLink = referralUplineF1.ReferralLink;
                    model.Description = referralUplineF1.Description;
                }
                else
                {
                    var appUserUplineF2 = await _userManager.FindByIdAsync(appUserUplineF1.ReferralId.ToString());
                    if (appUserUplineF2.IsSystem)
                    {
                        model.BlogName = blog.Name;
                        model.ReferralLink = blog.ReferralLink;
                        model.Description = "";
                    }
                    else
                    {
                        var referralUplineF2 = referralQuery
                            .FirstOrDefault(x => x.BlogId == blogId && x.AppUserId == appUserUplineF2.Id);
                        if (referralUplineF2 != null)
                        {
                            model.BlogName = blog.Name;
                            model.ReferralLink = referralUplineF2.ReferralLink;
                            model.Description = referralUplineF2.Description;
                        }
                        else
                        {
                            var appUserUplineF3 = await _userManager.FindByIdAsync(appUserUplineF2.ReferralId.ToString());
                            if (appUserUplineF3.IsSystem)
                            {
                                model.BlogName = blog.Name;
                                model.ReferralLink = blog.ReferralLink;
                                model.Description = "";
                            }
                            else
                            {
                                var referralUplineF3 = referralQuery
                                    .FirstOrDefault(x => x.BlogId == blogId && x.AppUserId == appUserUplineF3.Id);
                                if (referralUplineF3 != null)
                                {
                                    model.BlogName = blog.Name;
                                    model.ReferralLink = referralUplineF3.ReferralLink;
                                    model.Description = referralUplineF3.Description;
                                }
                                else
                                {
                                    var appUserUplineF4 = await _userManager.FindByIdAsync(appUserUplineF3.ReferralId.ToString());
                                    if (appUserUplineF4.IsSystem)
                                    {
                                        model.BlogName = blog.Name;
                                        model.ReferralLink = blog.ReferralLink;
                                        model.Description = "";
                                    }
                                    else
                                    {
                                        var referralUplineF4 = referralQuery
                                            .FirstOrDefault(x => x.BlogId == blogId && x.AppUserId == appUserUplineF4.Id);
                                        if (referralUplineF4 != null)
                                        {
                                            model.BlogName = blog.Name;
                                            model.ReferralLink = referralUplineF4.ReferralLink;
                                            model.Description = referralUplineF4.Description;
                                        }
                                        else
                                        {
                                            var appUserUplineF5 = await _userManager.FindByIdAsync(appUserUplineF4.ReferralId.ToString());
                                            if (appUserUplineF5.IsSystem)
                                            {
                                                model.BlogName = blog.Name;
                                                model.ReferralLink = blog.ReferralLink;
                                                model.Description = "";
                                            }
                                            else
                                            {
                                                var referralUplineF5 = referralQuery
                                                    .FirstOrDefault(x => x.BlogId == blogId && x.AppUserId == appUserUplineF5.Id);
                                                if (referralUplineF5 != null)
                                                {
                                                    model.BlogName = blog.Name;
                                                    model.ReferralLink = referralUplineF5.ReferralLink;
                                                    model.Description = referralUplineF5.Description;
                                                }
                                                else
                                                {
                                                    model.BlogName = blog.Name;
                                                    model.ReferralLink = blog.ReferralLink;
                                                    model.Description = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return model;
        }

        public ReferralViewModel GetByBlogId(string appUserId, int blogId)
        {
            var userId = new Guid(appUserId);
            var referralQuery = _referralRepository.FindAll(x => x.Blog);
            var referral = referralQuery.FirstOrDefault(x => x.AppUserId == userId && x.BlogId == blogId);
            if (referral == null)
                return null;

            var model = new ReferralViewModel()
            {
                Id = referral.Id,
                Description = referral.Description,
                AppUserId = referral.AppUserId,
                BlogName = referral.Blog.Name,
                BlogId = referral.BlogId,
                BlogDescription = referral.Blog.Description,
                BlogImage = referral.Blog.Image,
                ReferralLink = referral.ReferralLink,
                ReferralLinkRule = referral.Blog.ReferralLinkRule
            };

            return model;
        }

        public ReferralViewModel GetById(int id)
        {
            var referral = _referralRepository.FindById(id);
            if (referral == null)
                return null;

            var model = new ReferralViewModel()
            {
                Id = referral.Id,
                Description = referral.Description,
                AppUserId = referral.AppUserId,
                BlogId = referral.BlogId,
                ReferralLink = referral.ReferralLink
            };

            return model;
        }

        public void AddOrUpdate(ReferralViewModel referralVm)
        {
            var referralQuery = _referralRepository.FindAll();
            Referral referral = referralQuery.FirstOrDefault(x => x.AppUserId == referralVm.AppUserId && x.BlogId == referralVm.BlogId);

            if (referral == null)
            {
                referral = new Referral();
                referral.AppUserId = referralVm.AppUserId;
                referral.BlogId = referralVm.BlogId;
                referral.Description = referralVm.Description;
                referral.ReferralLink = referralVm.ReferralLink;
                _referralRepository.Add(referral);
            }
            else
            {
                referral.Description = referralVm.Description;
                referral.ReferralLink = referralVm.ReferralLink;
                _referralRepository.Update(referral);
            }
        }

        public void Delete(int id) => _referralRepository.Remove(id);

        public void Save() => _unitOfWork.Commit();
    }
}
