using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Entities;
using BeCoreApp.Data.Enums;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeCoreApp.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlockChainService _blockChainService;
        public UserService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IBlockChainService blockChainService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _blockChainService = blockChainService;
        }

        #region Customer

        public PagedResult<AppUserViewModel> GetAllCustomerPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users.Where(x => x.IsSystem == false);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.CMND.Contains(keyword)
                || x.PhoneNumber.Contains(keyword)
                || x.WithdrawPublishKey.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            var data = query.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new AppUserViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    ID = $"VS{x.SponsorNo}",
                    Avatar = x.Avatar,
                    IsActivated = x.IsActivated,
                    IsUpdatedKYC = x.IsUpdatedKYC,
                    EmailConfirmed = x.EmailConfirmed,
                    BirthDay = x.BirthDay.ToString(),
                    Email = x.Email,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    CMND = x.CMND,
                    Status = x.Status,
                    ActivatedDate = x.ActivatedDate,
                    ReferralId = x.ReferralId,
                    DateCreated = x.DateCreated,
                    WalletStart = x.WalletStart,
                    WalletUsdt = x.WalletUsdt,
                    WalletValuesShare = x.WalletValuesShare,
                    WalletEthereum = x.WalletEthereum
                }).ToList();

            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public PagedResult<AppUserViewModel> GetCustomerReferralPagingAsync(string customerId, int refIndex, string keyword, int page, int pageSize)
        {
            IQueryable<AppUser> dataCustomers = null;
            var userList = _userManager.Users.Where(x => x.EmailConfirmed == true && x.IsSystem == false);

            if (!string.IsNullOrEmpty(keyword))
                userList = userList.Where(x => x.FullName.Contains(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));

            var f1Customers = userList.Where(x => x.ReferralId == new Guid(customerId));
            if (refIndex == 1)
            {
                dataCustomers = f1Customers;
            }
            else
            {
                var f1Ids = f1Customers.Select(x => x.Id).ToList();
                var f2Customers = userList.Where(x => f1Ids.Contains(x.ReferralId.Value));
                if (refIndex == 2)
                {
                    dataCustomers = f2Customers;
                }
                else
                {
                    var f2Ids = f2Customers.Select(x => x.Id).ToList();
                    var f3Customers = userList.Where(x => f2Ids.Contains(x.ReferralId.Value));
                    if (refIndex == 3)
                    {
                        dataCustomers = f3Customers;
                    }
                    else
                    {
                        var f3Ids = f3Customers.Select(x => x.Id).ToList();
                        var f4Customers = userList.Where(x => f3Ids.Contains(x.ReferralId.Value));
                        if (refIndex == 4)
                        {
                            dataCustomers = f4Customers;
                        }
                        else
                        {
                            var f4Ids = f4Customers.Select(x => x.Id).ToList();
                            var f5Customers = userList.Where(x => f4Ids.Contains(x.ReferralId.Value));
                            if (refIndex == 5)
                            {
                                dataCustomers = f5Customers;
                            }
                        }
                    }
                }
            }

            int totalRow = dataCustomers.Count();
            var data = dataCustomers.OrderBy(x => x.SponsorNo).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new AppUserViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    ID = $"VS{x.SponsorNo}",
                    Avatar = x.Avatar,
                    IsActivated = x.IsActivated,
                    IsUpdatedKYC = x.IsUpdatedKYC,
                    EmailConfirmed = x.EmailConfirmed,
                    BirthDay = x.BirthDay.ToString(),
                    Email = x.Email,
                    CMND = x.CMND,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    ActivatedDate = x.ActivatedDate,
                    ReferralId = x.ReferralId,
                    DateCreated = x.DateCreated
                }).ToList();

            return new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<NetworkViewModel> GetNetworkInfo(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            var customerReferal = await _userManager.FindByIdAsync(customer.ReferralId.ToString());

            var model = new NetworkViewModel();
            model.IsUpdatedKYC = customer.IsUpdatedKYC.HasValue ? customer.IsUpdatedKYC.Value : false;
            model.IsActivated = customer.IsActivated.HasValue ? customer.IsActivated.Value : false;
            model.FullName = customer.FullName;
            model.CMND = customer.CMND;
            model.Email = customer.Email;
            model.Member = customer.UserName;
            model.ID = $"VS{customer.SponsorNo}";
            model.Referal = customerReferal.UserName;
            model.IDMember = $"VS{customer.SponsorNo} ({customer.UserName})";
            model.PhoneNumber = customer.PhoneNumber;
            model.ReferalLink = $"https://Salanests.com/Admin/Account/Register?refId=VS{customer.SponsorNo}";

            var userList = _userManager.Users.Where(x => x.EmailConfirmed == true && x.IsSystem == false);

            var f1Customers = userList.Where(x => x.ReferralId == customer.Id);
            model.TotalF1 = f1Customers.Count();
            model.TotalMember += f1Customers.Count();
            model.TotalUpdatedKYC += f1Customers.Where(x => x.IsUpdatedKYC == true).Count();
            model.TotalActivated += f1Customers.Where(x => x.IsActivated == true).Count();

            foreach (var f1Customer in f1Customers)
            {
                var f2Customers = userList.Where(x => x.ReferralId == f1Customer.Id);
                model.TotalF2 += f2Customers.Count();
                model.TotalMember += f2Customers.Count();
                model.TotalUpdatedKYC += f2Customers.Where(x => x.IsUpdatedKYC == true).Count();
                model.TotalActivated += f2Customers.Where(x => x.IsActivated == true).Count();

                foreach (var f2Customer in f2Customers)
                {
                    var f3Customers = userList.Where(x => x.ReferralId == f2Customer.Id);
                    model.TotalF3 += f3Customers.Count();
                    model.TotalMember += f3Customers.Count();
                    model.TotalUpdatedKYC += f3Customers.Where(x => x.IsUpdatedKYC == true).Count();
                    model.TotalActivated += f3Customers.Where(x => x.IsActivated == true).Count();

                    foreach (var f3Customer in f3Customers)
                    {
                        var f4Customers = userList.Where(x => x.ReferralId == f3Customer.Id);
                        model.TotalF4 += f4Customers.Count();
                        model.TotalMember += f4Customers.Count();
                        model.TotalUpdatedKYC += f4Customers.Where(x => x.IsUpdatedKYC == true).Count();
                        model.TotalActivated += f4Customers.Where(x => x.IsActivated == true).Count();

                        foreach (var f4Customer in f4Customers)
                        {
                            var f5Customers = userList.Where(x => x.ReferralId == f4Customer.Id);
                            model.TotalF5 += f5Customers.Count();
                            model.TotalMember += f5Customers.Count();
                            model.TotalUpdatedKYC += f5Customers.Where(x => x.IsUpdatedKYC == true).Count();
                            model.TotalActivated += f5Customers.Where(x => x.IsActivated == true).Count();
                        }
                    }
                }
            }

            return model;
        }

        public async Task<GenericResult> UpdateCustomerActivated(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
                return new GenericResult(false, "Tài khoản không tồn tại.");

            if (customer.IsActivated == true)
                return new GenericResult(false, "Tài khoản đã kích hoạt.");

            decimal usdtDeposit = await _blockChainService.GetERC20Balance(customer.PublishKey,
                CommonConstants.USDTSmartContractAddress, CommonConstants.USDTDecimalPlaces);
            if (usdtDeposit < 100)
                return new GenericResult(false, "Tài khoản của bạn không đủ hạn mức 100 USDT để kích hoạt. Vui lòng nạp thêm và kích hoạt lại sau.");


            TransactionReceipt transactionReceipt = await _blockChainService.SendEthAsync(CommonConstants.PrivateKeyERC20VT, customer.PublishKey, 0.0046m);
            if (transactionReceipt.HasErrors() != true)
            {
                await _blockChainService.SendERC20Async(customer.PrivateKey, CommonConstants.PublishKeyERC20VT,
                    CommonConstants.USDTSmartContractAddress, usdtDeposit, CommonConstants.USDTDecimalPlaces);
            }


            customer.WalletUsdt = usdtDeposit - 100;
            customer.WalletStart += 100;
            customer.IsActivated = true;
            customer.ActivatedDate = DateTime.Now;

            var resultCustomer = await _userManager.UpdateAsync(customer);

            if (!resultCustomer.Succeeded)
            {
                return new GenericResult(false, resultCustomer.Errors);
            }

            var customerParentF1 = await _userManager.FindByIdAsync(customer.ReferralId.Value.ToString());
            if (customerParentF1 != null && customerParentF1.IsSystem == false)
            {
                int countChildrenF1 = CountCustomerChildren(customerParentF1.Id.ToString());
                if (customerParentF1.IsActivated == true)
                {
                    if (countChildrenF1 % 3 == 0)
                        customerParentF1.WalletUsdt += 60;
                    else
                        customerParentF1.WalletUsdt += 20;

                    customerParentF1.WalletStart += (20 * 0.7m);

                    var resultParentF1 = await _userManager.UpdateAsync(customerParentF1);
                    if (!resultParentF1.Succeeded)
                        return new GenericResult(false, resultParentF1.Errors);
                }

                var customerParentF2 = await _userManager.FindByIdAsync(customerParentF1.ReferralId.Value.ToString());
                if (customerParentF2 != null && customerParentF2.IsSystem == false)
                {
                    if (customerParentF2.IsActivated == true)
                    {
                        customerParentF2.WalletUsdt += 10;
                        customerParentF2.WalletStart += (10 * 0.7m);

                        var resultParentF2 = await _userManager.UpdateAsync(customerParentF2);
                        if (!resultParentF2.Succeeded)
                            return new GenericResult(false, resultParentF2.Errors);
                    }

                    var customerParentF3 = await _userManager.FindByIdAsync(customerParentF2.ReferralId.Value.ToString());
                    if (customerParentF3 != null && customerParentF3.IsSystem == false)
                    {
                        if (customerParentF3.IsActivated == true)
                        {
                            customerParentF3.WalletUsdt += 5;
                            customerParentF3.WalletStart += (5 * 0.7m);

                            var resultParentF3 = await _userManager.UpdateAsync(customerParentF3);
                            if (!resultParentF3.Succeeded)
                                return new GenericResult(false, resultParentF3.Errors);
                        }

                        var customerParentF4 = await _userManager.FindByIdAsync(customerParentF3.ReferralId.Value.ToString());
                        if (customerParentF4 != null && customerParentF4.IsSystem == false)
                        {
                            if (customerParentF4.IsActivated == true)
                            {
                                customerParentF4.WalletUsdt += 5;
                                customerParentF4.WalletStart += (5 * 0.7m);

                                var resultParentF4 = await _userManager.UpdateAsync(customerParentF4);
                                if (!resultParentF4.Succeeded)
                                    return new GenericResult(false, resultParentF4.Errors);
                            }

                            var customerParentF5 = await _userManager.FindByIdAsync(customerParentF4.ReferralId.Value.ToString());
                            if (customerParentF5 != null && customerParentF5.IsSystem == false)
                            {
                                if (customerParentF5.IsActivated == true)
                                {
                                    customerParentF5.WalletUsdt += 10;
                                    customerParentF5.WalletStart += (10 * 0.7m);

                                    var resultParentF5 = await _userManager.UpdateAsync(customerParentF5);
                                    if (!resultParentF5.Succeeded)
                                        return new GenericResult(false, resultParentF5.Errors);
                                }
                            }
                        }
                    }
                }
            }


            //var result = await UpdateCustomerReferral(id);

            //if (result.Success == true)
            //{
            return new GenericResult(true, "Kích hoạt tài khoản thành công.");
            //}


            //return result;
        }

        public async Task<GenericResult> UpdateCustomerReferral(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return new GenericResult(false, "Tài khoản không tồn tại.");
            }

            //DateTime rangeDate = new DateTime(2020, 7, 2);
            //if (customer.UpdateKYCDate.HasValue && customer.UpdateKYCDate.Value.Date > rangeDate)
            //{
            customer.WalletStart += 100;
            var resultCustomer = await _userManager.UpdateAsync(customer);

            if (!resultCustomer.Succeeded)
            {
                return new GenericResult(false, resultCustomer.Errors);
            }

            var customerParentF1 = await _userManager.FindByIdAsync(customer.ReferralId.Value.ToString());
            if (customerParentF1 != null && customerParentF1.IsSystem == false)
            {
                customerParentF1.WalletStart += (20 * 0.3m);

                var resultParentF1 = await _userManager.UpdateAsync(customerParentF1);
                if (!resultParentF1.Succeeded)
                {
                    return new GenericResult(false, resultParentF1.Errors);
                }

                var customerParentF2 = await _userManager.FindByIdAsync(customerParentF1.ReferralId.Value.ToString());
                if (customerParentF2 != null && customerParentF2.IsSystem == false)
                {
                    customerParentF2.WalletStart += (10 * 0.3m);

                    var resultParentF2 = await _userManager.UpdateAsync(customerParentF2);
                    if (!resultParentF2.Succeeded)
                    {
                        return new GenericResult(false, resultParentF2.Errors);
                    }

                    var customerParentF3 = await _userManager.FindByIdAsync(customerParentF2.ReferralId.Value.ToString());
                    if (customerParentF3 != null && customerParentF3.IsSystem == false)
                    {
                        customerParentF3.WalletStart += (5 * 0.3m);

                        var resultParentF3 = await _userManager.UpdateAsync(customerParentF3);
                        if (!resultParentF3.Succeeded)
                        {
                            return new GenericResult(false, resultParentF3.Errors);
                        }

                        var customerParentF4 = await _userManager.FindByIdAsync(customerParentF3.ReferralId.Value.ToString());
                        if (customerParentF4 != null && customerParentF4.IsSystem == false)
                        {
                            customerParentF4.WalletStart += (5 * 0.3m);

                            var resultParentF4 = await _userManager.UpdateAsync(customerParentF4);
                            if (!resultParentF4.Succeeded)
                            {
                                return new GenericResult(false, resultParentF4.Errors);
                            }

                            var customerParentF5 = await _userManager.FindByIdAsync(customerParentF4.ReferralId.Value.ToString());
                            if (customerParentF5 != null && customerParentF5.IsSystem == false)
                            {
                                customerParentF5.WalletStart += (10 * 0.3m);

                                var resultParentF5 = await _userManager.UpdateAsync(customerParentF5);
                                if (!resultParentF5.Succeeded)
                                {
                                    return new GenericResult(false, resultParentF5.Errors);
                                }
                            }
                        }
                    }
                }
            }
            //}

            return new GenericResult(true, "Cập nhật người giới thiệu thành công");
        }

        public async Task<GenericResult> RevokeCustomerReferral(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return new GenericResult(false, "Tài khoản không tồn tại.");
            }

            customer.WalletStart -= 100;

            var resultCustomer = await _userManager.UpdateAsync(customer);

            if (!resultCustomer.Succeeded)
            {
                return new GenericResult(false, resultCustomer.Errors);
            }

            //var customerParentF1 = await _userManager.FindByIdAsync(customer.ReferralId.Value.ToString());
            //if (customerParentF1 != null && customerParentF1.IsSystem == false)
            //{
            //    customerParentF1.WalletStart -= 20;

            //    var resultParentF1 = await _userManager.UpdateAsync(customerParentF1);
            //    if (!resultParentF1.Succeeded)
            //    {
            //        return new GenericResult(false, resultParentF1.Errors);
            //    }

            //    var customerParentF2 = await _userManager.FindByIdAsync(customerParentF1.ReferralId.Value.ToString());
            //    if (customerParentF2 != null && customerParentF2.IsSystem == false)
            //    {
            //        customerParentF2.WalletStart -= 10;

            //        var resultParentF2 = await _userManager.UpdateAsync(customerParentF2);
            //        if (!resultParentF2.Succeeded)
            //        {
            //            return new GenericResult(false, resultParentF2.Errors);
            //        }

            //        var customerParentF3 = await _userManager.FindByIdAsync(customerParentF2.ReferralId.Value.ToString());
            //        if (customerParentF3 != null && customerParentF3.IsSystem == false)
            //        {
            //            customerParentF3.WalletStart -= 5;

            //            var resultParentF3 = await _userManager.UpdateAsync(customerParentF3);
            //            if (!resultParentF3.Succeeded)
            //            {
            //                return new GenericResult(false, resultParentF3.Errors);
            //            }

            //            var customerParentF4 = await _userManager.FindByIdAsync(customerParentF3.ReferralId.Value.ToString());
            //            if (customerParentF4 != null && customerParentF4.IsSystem == false)
            //            {
            //                customerParentF4.WalletStart -= 5;

            //                var resultParentF4 = await _userManager.UpdateAsync(customerParentF4);
            //                if (!resultParentF4.Succeeded)
            //                {
            //                    return new GenericResult(false, resultParentF4.Errors);
            //                }

            //                var customerParentF5 = await _userManager.FindByIdAsync(customerParentF4.ReferralId.Value.ToString());
            //                if (customerParentF5 != null && customerParentF5.IsSystem == false)
            //                {
            //                    customerParentF5.WalletStart -= 10;

            //                    var resultParentF5 = await _userManager.UpdateAsync(customerParentF5);
            //                    if (!resultParentF5.Succeeded)
            //                    {
            //                        return new GenericResult(false, resultParentF5.Errors);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            return new GenericResult(true, "Thu hồi người giới thiệu thành công");
        }

        public async Task UpdateBalanceAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());

            user.WalletUsdt = userVm.WalletUsdt;
            user.DateModified = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateFeeEthereumAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());

            user.WalletEthereum = userVm.WalletEthereum;
            user.WalletValuesShare = userVm.WalletValuesShare;
            user.DateModified = DateTime.Now;

            await _userManager.UpdateAsync(user);
        }

        public async Task<decimal> UpdateExchangeBalanceAsync(string userId)
        {
            decimal amountExchange = 1;

            var user = await _userManager.FindByIdAsync(userId);
            if (user.WalletStart < 100)
            {
                user.WalletStart -= 1;
                user.WalletValuesShare += 1;
            }
            else
            {
                amountExchange = user.WalletStart.Value * 0.01m;
                amountExchange = Math.Round(amountExchange, 2);
                user.WalletStart -= amountExchange;
                user.WalletValuesShare += amountExchange;
            }
            await _userManager.UpdateAsync(user);

            return amountExchange;
        }

        int CountCustomerChildren(string id)
        {
            return _userManager.Users.Where(x => x.IsSystem == false && x.IsActivated == true
               && x.ReferralId.Value.ToString() == id).Count();
        }

        #endregion

        #region User System


        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users.Where(x => x.IsSystem);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));

            int totalRow = query.Count();
            var data = query.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new AppUserViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Avatar = x.Avatar,
                    BirthDay = x.BirthDay.ToString(),
                    Email = x.Email,
                    FullName = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    DateCreated = x.DateCreated
                }).ToList();

            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public List<AppUserTreeViewModel> GetTreeAll()
        {
            var listData = _userManager.Users.Where(x => x.EmailConfirmed == true)
                .Select(x => new AppUserTreeViewModel()
                {
                    id = x.Id,
                    text = x.UserName,
                    icon = "fa fa-folder",
                    state = new AppUserTreeState { opened = true },
                    data = new AppUserTreeData
                    {
                        rootId = x.Id,
                        referralId = x.ReferralId,
                        userName = x.UserName,
                        fullName = x.FullName,
                        activatedDate = x.ActivatedDate,
                        isActivated = x.IsActivated,
                        updateKYCDate = x.UpdateKYCDate,
                        isUpdatedKYC = x.IsUpdatedKYC,
                        avatar = x.Avatar,
                        dateCreated = x.DateCreated,
                        email = x.Email,
                        isSystem = x.IsSystem,
                        phoneNumber = x.PhoneNumber,
                        status = x.Status,
                        walletStart = x.WalletStart
                    }
                });

            if (listData.Count() == 0)
                return new List<AppUserTreeViewModel>();

            var groups = listData.GroupBy(i => i.data.referralId);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue)
                    .ToDictionary(g => g.Key.Value, g => g.ToList());

                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }
            return roots;
        }

        private void AddChildren(AppUserTreeViewModel root, IDictionary<Guid, List<AppUserTreeViewModel>> source)
        {
            if (source.ContainsKey(root.id))
            {
                root.children = source[root.id].ToList();
                for (int i = 0; i < root.children.Count; i++)
                    AddChildren(root.children[i], source);
            }
            else
            {
                root.icon = "fa fa-file m--font-warning";
                root.children = new List<AppUserTreeViewModel>();
            }
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var userVm = new AppUserViewModel()
            {
                Id = user.Id,
                PublishKey = user.PublishKey,
                BankBillImage = user.BankBillImage,
                ActivatedDate = user.ActivatedDate,
                BankCardImage = user.BankCardImage,
                ReferalLink = $"https://Salanests.com/Admin/Account/Register?refId=VS{user.SponsorNo}",
                CMND = user.CMND,
                CMNDImage = user.CMNDImage,
                WalletWin = user.WalletWin,
                EmailConfirmed = user.EmailConfirmed,
                IsSystem = user.IsSystem,
                IsUpdatedKYC = user.IsUpdatedKYC,
                PrivateKey = user.PrivateKey,
                ReferralId = user.ReferralId,
                WalletGame = user.WalletGame,
                WalletStart = user.WalletStart,
                WalletUsdt = user.WalletUsdt,
                UpdateKYCDate = user.UpdateKYCDate,
                WalletUsdtReferral = user.WalletUsdtReferral,
                WalletValuesShare = user.WalletValuesShare,
                WithdrawPublishKey = user.WithdrawPublishKey,
                WalletEthereum = user.WalletEthereum,
                ByCreated = user.ByCreated,
                ByModified = user.ByModified,
                DateModified = user.DateModified,
                IsActivated = user.IsActivated,
                UserName = user.UserName,
                Avatar = user.Avatar,
                BirthDay = user.BirthDay.ToString(),
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status,
                DateCreated = user.DateCreated,
                ID = $"VS{user.SponsorNo}",
                Roles = roles.ToList()
            };

            return userVm;
        }

        public async Task<bool> AddAsync(AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                IsSystem = true
            };

            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded && userVm.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                {
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles.ToArray());
                }
            }

            return result.Succeeded;
        }


        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());

            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, userVm.Roles.Except(currentRoles).ToArray());
            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                user.DateModified = DateTime.Now;

                await _userManager.UpdateAsync(user);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        #endregion
    }
}
