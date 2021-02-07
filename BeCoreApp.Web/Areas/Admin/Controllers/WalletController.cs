using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeCoreApp.Application.Interfaces;
using BeCoreApp.Application.ViewModels.BlockChain;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Application.ViewModels.Valuesshare;
using BeCoreApp.Data.Enums;
using BeCoreApp.Extensions;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeCoreApp.Areas.Admin.Controllers
{
    public class WalletController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;
        private readonly IWalletService _walletService;
        private readonly IBlockChainService _blockChainService;
        private readonly IExchangeService _exchangeService;
        public WalletController(
            IUserService userService,
            IWalletService walletService,
            IConfiguration configuration,
            IRoleService roleService,
            IBlockChainService blockChainService,
            IExchangeService exchangeService,
            IAuthorizationService authorizationService)
        {
            _exchangeService = exchangeService;
            _blockChainService = blockChainService;
            _walletService = walletService;
            _userService = userService;
            _roleService = roleService;
            _configuration = configuration;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.GetSpecificClaim("UserId");
            var appUser = await _userService.GetById(userId);

            var model = new WalletViewModel()
            {
                PublishKeyVT = CommonConstants.PublishKeyERC20VT,
                WalletUsdtDeposit = 0,
                WalletEthereum = appUser.WalletEthereum,
                WalletGame = appUser.WalletGame,
                WalletStart = appUser.WalletStart,
                WalletUsdt = appUser.WalletUsdt,
                WalletValuesShare = appUser.WalletValuesShare,
                WalletWin = appUser.WalletWin,
                SystemFee = "5",
                PublishKey = appUser.PublishKey,
                AddressReceiving = appUser.WithdrawPublishKey,
                Balance = appUser.WalletUsdt.Value
            };

            try
            {
                model.WalletUsdtDeposit = await _blockChainService
                  .GetERC20Balance(appUser.PublishKey, CommonConstants.USDTSmartContractAddress,
                  CommonConstants.USDTDecimalPlaces);
            }
            catch (Exception ex)
            {
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawUSDT(string modelJson)
        {
            try
            {
                var transferUSDTVm = JsonConvert.DeserializeObject<TransferERC20ViewModel>(modelJson);
                var userId = User.GetSpecificClaim("UserId");
                var result = await _walletService.WithdrawUSDT(transferUSDTVm.Amount, userId);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawVSCoin(string modelJson)
        {
            try
            {
                var transferUSDTVm = JsonConvert.DeserializeObject<TransferERC20ViewModel>(modelJson);
                var userId = User.GetSpecificClaim("UserId");
                var result = await _walletService.WithdrawVSCoin(transferUSDTVm.AmountTransfer, userId);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEthereumDeposit(string transactionHash)
        {
            try
            {
                var userId = User.GetSpecificClaim("UserId");

                var result = await _blockChainService.CheckDepositEthereumByTransaction(transactionHash, userId);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Exchange()
        {
            try
            {
                var userId = User.GetSpecificClaim("UserId");
                var appUser = await _userService.GetById(userId);

                if (appUser.IsActivated != true)
                {
                   bool checkLimitExchange= _exchangeService.CheckLimitExchange(userId);
                    if (checkLimitExchange)
                    {
                        return new OkObjectResult(new GenericResult(false, "Bạn đã chuyển đổi hết 30% số lượng."));
                    }
                }

                if (appUser.WalletStart.HasValue && appUser.WalletStart.Value <= 0)
                {
                    return new OkObjectResult(new GenericResult(false, "VS TOKEN 1 số dư không đủ chuyển đổi."));
                }

                bool isExchanged = _exchangeService.CheckExchanged(userId);
                if (isExchanged)
                {
                    return new OkObjectResult(new GenericResult(false, "Bạn đã hết lượt trong ngày, vui lòng thử lại ngày mai."));
                }

                decimal amountExchange = await _userService.UpdateExchangeBalanceAsync(userId);

                ExchangeViewModel model = new ExchangeViewModel()
                {
                    Amount = amountExchange,
                    AppUserId = appUser.Id.Value,
                    ExchangeDate = DateTime.Now.Date,
                    WalletFrom = ExchangeType.VSTOKEN1,
                    WalletTo = ExchangeType.VSTOKEN2
                };
                _exchangeService.Add(model);
                _exchangeService.Save();

                return new OkObjectResult(new GenericResult(true, $"Chuyển đổi thành công {amountExchange} VS TOKEN từ VS TOKEN 1 đến VS TOKEN 2"));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new GenericResult(false, ex.Message));
            }
        }
    }
}