using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Entities;
using BeCoreApp.Data.Enums;
using BeCoreApp.Data.IRepositories;
using BeCoreApp.Infrastructure.Interfaces;
using BeCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeCoreApp.Application.Implementation
{
    public class KYCService : IKYCService
    {
        private readonly IKYCRepository _kycRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public KYCService(UserManager<AppUser> userManager,
            IUserService userService,
            IKYCRepository kycRepository,
            IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userManager = userManager;
            _kycRepository = kycRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ApprovalAsync(KYCViewModel kycVm)
        {
            var kyc = _kycRepository.FindById(kycVm.Id);
            kyc.Type = KYCType.Approval;
            kyc.Reason = kycVm.Reason;
            _kycRepository.Update(kyc);
            Save();

            var user = await _userManager.FindByIdAsync(kycVm.AppUserId.ToString());

            bool reApproval = user.IsUpdatedKYC.HasValue ? user.IsUpdatedKYC.Value : false;

            user.IsUpdatedKYC = true;
            user.UpdateKYCDate = DateTime.Now;
            user.BankBillImage = kyc.BankBillImage;
            user.BankCardImage = kyc.BankCardImage;
            user.CMNDImage = kyc.CMNDImage;
            user.WithdrawPublishKey = kyc.WithdrawPublishKey;

            await _userManager.UpdateAsync(user);

            if (reApproval == false)
            {
                //user.WalletStart += 100;
                await _userService.UpdateCustomerReferral(user.Id.ToString());
            }
        }

        public async Task RevokeAsync(string appUserId)
        {
            var user = await _userManager.FindByIdAsync(appUserId);

            bool isApproved = user.IsUpdatedKYC.HasValue ? user.IsUpdatedKYC.Value : false;
            if (isApproved)
            {
                user.IsUpdatedKYC = false;
                user.UpdateKYCDate = null;
                user.BankBillImage = null;
                user.BankCardImage = null;
                user.CMNDImage = null;
                user.WithdrawPublishKey = null;
                await _userManager.UpdateAsync(user);

                var kyc = _kycRepository.FindAll()
                    .FirstOrDefault(x => x.AppUserId == user.Id && x.Type == KYCType.Approval);

                if (kyc != null)
                {
                    kyc.Type = KYCType.Lock;
                    kyc.Reason = "Bạn đã vi phạm dùng thông tin cá nhân của người khác, và sử dụng phần mềm cắt ghép. Cảm ơn";
                    _kycRepository.Update(kyc);
                    Save();
                }

                await _userService.RevokeCustomerReferral(user.Id.ToString());
            }
        }

        public void Reject(KYCViewModel kycVm)
        {
            var kyc = _kycRepository.FindById(kycVm.Id);
            kyc.Type = KYCType.Reject;
            kyc.Reason = kycVm.Reason;
            _kycRepository.Update(kyc);
            Save();
        }

        public void Lock(KYCViewModel kycVm)
        {
            var kyc = _kycRepository.FindById(kycVm.Id);
            kyc.Type = KYCType.Lock;
            kyc.Reason = kycVm.Reason;
            _kycRepository.Update(kyc);
            Save();
        }

        public void UnLock(KYCViewModel kycVm)
        {
            var kyc = _kycRepository.FindById(kycVm.Id);
            kyc.Type = KYCType.Pending;
            kyc.Reason = kycVm.Reason;
            _kycRepository.Update(kyc);
            Save();
        }

        public async Task AddAsync(KYCViewModel kycVm)
        {
            var user = await _userManager.FindByIdAsync(kycVm.AppUserId.ToString());
            var model = new KYC()
            {
                AppUserId = kycVm.AppUserId,
                BankBillImage = kycVm.BankBillImage,
                BankCardImage = kycVm.BankCardImage,
                CMNDImage = kycVm.CMNDImage,
                WithdrawPublishKey = kycVm.WithdrawPublishKey,
                Reason = kycVm.Reason,
                Type = KYCType.Pending,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Status = Status.Active,
                ByModified = user.UserName,
                ByCreated = user.UserName,
            };

            _kycRepository.Add(model);
        }

        public PagedResult<KYCViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _kycRepository.FindAll(x => x.AppUser);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.AppUser.FullName.Contains(keyword) || x.AppUser.UserName.Contains(keyword)
                || x.AppUser.Email.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderBy(x => x.Type).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new KYCViewModel()
                {
                    Id = x.Id,
                    UserName = x.AppUser.UserName,
                    PhoneNumber = x.AppUser.PhoneNumber,
                    BankBillImage = x.BankBillImage,
                    Email = x.AppUser.Email,
                    ID = $"VS{x.AppUser.SponsorNo}",
                    FullName = x.AppUser.FullName,
                    BankCardImage = x.BankCardImage,
                    WithdrawPublishKey = x.WithdrawPublishKey,
                    CMNDImage = x.CMNDImage,
                    Reason = x.Reason,
                    Type = x.Type,
                    Status = x.Status,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    ByCreated = x.ByCreated,
                    ByModified = x.ByModified,
                }).ToList();

            var paginationSet = new PagedResult<KYCViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
        public bool CheckShowFormRequestKYC(string userId)
        {
            var model = _kycRepository.FindAll(x => x.AppUserId == new Guid(userId));
            if (model.Where(x => x.Type == KYCType.Approval || x.Type == KYCType.Pending || x.Type == KYCType.Lock).Count() > 0)
            {
                return false;
            }

            return true;
        }
        public List<KYCViewModel> GetAllByUserId(string userId)
        {
            var model = _kycRepository.FindAll(x => x.AppUserId == new Guid(userId))
                .Select(x => new KYCViewModel()
                {
                    Id = x.Id,
                    UserName = x.AppUser.UserName,
                    PhoneNumber = x.AppUser.PhoneNumber,
                    BankBillImage = x.BankBillImage,
                    Email = x.AppUser.Email,
                    FullName = x.AppUser.FullName,
                    BankCardImage = x.BankCardImage,
                    WithdrawPublishKey = x.WithdrawPublishKey,
                    CMNDImage = x.CMNDImage,
                    Reason = x.Reason,
                    Type = x.Type,
                    Status = x.Status,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    ByCreated = x.ByCreated,
                    ByModified = x.ByModified,
                }).ToList();

            return model;
        }
        public KYCViewModel GetById(int id)
        {
            var model = _kycRepository.FindById(id, x => x.AppUser);
            if (model == null)
                return null;


            var kycVm = new KYCViewModel()
            {
                Id = model.Id,
                AppUserId = model.AppUser.Id,
                UserName = model.AppUser.UserName,
                PhoneNumber = model.AppUser.PhoneNumber,
                BankBillImage = model.BankBillImage,
                Email = model.AppUser.Email,
                FullName = model.AppUser.FullName,
                BankCardImage = model.BankCardImage,
                CMND = model.AppUser.CMND,
                WithdrawPublishKey = model.WithdrawPublishKey,
                CMNDImage = model.CMNDImage,
                Reason = model.Reason,
                Type = model.Type,
                Status = model.Status,
                DateCreated = model.DateCreated,
                DateModified = model.DateModified,
                ByCreated = model.ByCreated,
                ByModified = model.ByModified,
                ID = $"VS{model.AppUser.SponsorNo}"
            };

            return kycVm;
        }

        public void Save() => _unitOfWork.Commit();

        public void Delete(int id) => _kycRepository.Remove(id);
    }
}
