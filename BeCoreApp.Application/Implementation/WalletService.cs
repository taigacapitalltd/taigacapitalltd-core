using BeCoreApp.Application.Interfaces;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Nethereum.HdWallet;
using Microsoft.Extensions.Configuration;
using BeCoreApp.Application.ViewModels.BlockChain;
using Nethereum.Web3.Accounts;
using BeCoreApp.Utilities.Dtos;
using Nethereum.Util;
using BeCoreApp.Application.ViewModels.System;
using BeCoreApp.Data.IRepositories;
using System.Linq;
using Nethereum.Hex.HexTypes;
using BeCoreApp.Infrastructure.Interfaces;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Collections.Generic;
using System.Numerics;
using BeCoreApp.Utilities.Constants;
using BeCoreApp.Data.Enums;

namespace BeCoreApp.Application.Implementation
{
    public class WalletService : IWalletService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionService _transactionService;
        private readonly IInvestRepository _investRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddressUtil _addressUtil = new AddressUtil();
        private readonly IUserService _userService;
        private readonly IBlockChainService _blockChainService;

        public WalletService(
            IConfiguration configuration,
            ITransactionService transactionService,
            IInvestRepository investRepository,
            IUserService userService,
            IBlockChainService blockChainService,
            IUnitOfWork unitOfWork)
        {
            _investRepository = investRepository;
            _transactionService = transactionService;
            _userService = userService;
            _blockChainService = blockChainService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResult> WithdrawUSDT(decimal amount, string customerId)
        {
            try
            {
                var appUser = await _userService.GetById(customerId);
                if (appUser == null)
                    return new GenericResult(false, "Tài khoản không tồn tại trong hệ thống!");

                if (!_addressUtil.IsChecksumAddress(appUser.WithdrawPublishKey) || !_addressUtil.IsValidAddressLength(appUser.WithdrawPublishKey))
                    return new GenericResult(false, "Địa chỉ ví nhận USDT không đúng định dạng!");

                if (appUser.WalletUsdt <= 0)
                    return new GenericResult(false, "Số dư khả dụng không đủ!");

                if (amount <= 0)
                    return new GenericResult(false, "Vui lòng nhập số USDT cần rút");

                if (appUser.WalletUsdt < amount)
                    return new GenericResult(false, "Số USDT cần rút phải bé hơn hoặc bằng số dư khả dụng!");


                decimal systemFee = 0.05m;
                decimal amountTransfer = amount - (amount * systemFee);

                if (amountTransfer <= 0)
                    return new GenericResult(false, "Số USDT nhận được không đủ");

                if (appUser.WalletUsdt <= amountTransfer)
                    return new GenericResult(false, "Số USDT được rút không đủ");

                var transactionReceipt = await _blockChainService.SendERC20Async(CommonConstants.PrivateKeyERC20VT, appUser.WithdrawPublishKey, CommonConstants.USDTSmartContractAddress, amountTransfer, CommonConstants.USDTDecimalPlaces);

                if (!string.IsNullOrWhiteSpace(transactionReceipt))
                {
                    appUser.WalletUsdt -= amount;
                    await _userService.UpdateBalanceAsync(appUser);

                    _transactionService.Add(new TransactionViewModel()
                    {
                        AddressTo = appUser.WithdrawPublishKey,
                        ContractAddress = CommonConstants.USDTSmartContractAddress,
                        Amount = amount,
                        AmountTransfer = amountTransfer,
                        AppUserId = appUser.Id.Value,
                        TransactionHas = transactionReceipt,
                        DecimalPlaces = CommonConstants.USDTDecimalPlaces,
                        DateCreated = DateTime.Now,
                        Type=TransactionType.WithdrawUSDT,
                        AddressFrom= "Salanests"
                    });

                    _transactionService.Save();

                    return new GenericResult(true, $"Chuyển {amountTransfer} USDT tới địa chỉ {appUser.WithdrawPublishKey} thành công.");
                }
                else
                {
                    return new GenericResult(false, $"Chuyển {amountTransfer} USDT tới địa chỉ {appUser.WithdrawPublishKey} thất bại.");
                }
            }
            catch (Exception ex)
            {
                return new GenericResult(false, ex.Message);
            }
        }
        public async Task<GenericResult> WithdrawVSCoin(decimal amountTransfer, string customerId)
        {
            try
            {
                var appUser = await _userService.GetById(customerId);
                if (appUser == null)
                    return new GenericResult(false, "Tài khoản không tồn tại trong hệ thống!");

                if (appUser.WalletEthereum < 0.005m)
                    return new GenericResult(false, "Ethereum phí giao dịch không đủ. Tối thiểu 0.005 Ether");

                if (!_addressUtil.IsChecksumAddress(appUser.WithdrawPublishKey) || !_addressUtil.IsValidAddressLength(appUser.WithdrawPublishKey))
                    return new GenericResult(false, "Địa chỉ ví nhận VS Coin không đúng định dạng!");

                if (appUser.WalletValuesShare <= 0)
                    return new GenericResult(false, "Số dư VS Coin khả dụng không đủ!");

                if (amountTransfer <= 0)
                    return new GenericResult(false, "Vui lòng nhập số VS Coin rút");

                if (appUser.WalletValuesShare < amountTransfer)
                    return new GenericResult(false, "Số VS Coin cần rút phải bé hơn hoặc bằng số dư VS Coin khả dụng!");
                

                var transactionReceipt = await _blockChainService.SendERC20Async(CommonConstants.PrivateKeyERC20VT, 
                    appUser.WithdrawPublishKey, CommonConstants.VSTokenSmartContractAddress, amountTransfer, CommonConstants.VSTokenDecimalPlaces);

                if (!string.IsNullOrWhiteSpace(transactionReceipt))
                {
                    appUser.WalletEthereum -= 0.005m;
                    appUser.WalletValuesShare -= amountTransfer;
                    await _userService.UpdateFeeEthereumAsync(appUser);

                    _transactionService.Add(new TransactionViewModel()
                    {
                        AddressTo = appUser.WithdrawPublishKey,
                        ContractAddress = CommonConstants.VSTokenSmartContractAddress,
                        Amount = 0,
                        AmountTransfer = amountTransfer,
                        AppUserId = appUser.Id.Value,
                        TransactionHas = transactionReceipt,
                        DecimalPlaces = CommonConstants.VSTokenDecimalPlaces,
                        DateCreated = DateTime.Now,
                        Type = TransactionType.WithdrawVSToken,
                        AddressFrom = "Salanests"
                    });

                    _transactionService.Save();

                    return new GenericResult(true, $"Chuyển {amountTransfer} VS Coin tới địa chỉ {appUser.WithdrawPublishKey} thành công");
                }
                else
                {
                    return new GenericResult(false, $"Chuyển {amountTransfer} VS Coin tới địa chỉ {appUser.WithdrawPublishKey} thất bại");
                }
            }
            catch (Exception ex)
            {
                return new GenericResult(false, ex.Message);
            }
        }
    }
}
