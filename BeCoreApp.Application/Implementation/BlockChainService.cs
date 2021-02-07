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
using static Nethereum.Util.UnitConversion;
using Microsoft.AspNetCore.Identity;
using BeCoreApp.Data.Entities;

namespace BeCoreApp.Application.Implementation
{
    public class BlockChainService : IBlockChainService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionService _transactionService;
        private readonly IInvestRepository _investRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddressUtil _addressUtil = new AddressUtil();
        private readonly UserManager<AppUser> _userManager;

        public BlockChainService(
            IConfiguration configuration,
            ITransactionService transactionService,
            IInvestRepository investRepository,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _investRepository = investRepository;
            _transactionService = transactionService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public Account CreateEthereumAccount()
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Account(privateKey);
            return account;
        }

        public async Task<GenericResult> CheckDepositEthereumByTransaction(string transactionID, string customerId)
        {
            try
            {
                var invests = _investRepository.FindAll(x => x.AppUserId == new Guid(customerId));

                var investByTransaction = invests.FirstOrDefault(x => x.TransactionHash == transactionID);

                if (investByTransaction != null)
                {
                    return new GenericResult(false, "Mã giao dịch không hợp lệ.");
                }
                else
                {
                    var appUser = await _userManager.FindByIdAsync(customerId);
                    if (appUser == null)
                        return new GenericResult(false, "Tài khoản không tồn tại.");

                    if (!_addressUtil.IsChecksumAddress(appUser.WithdrawPublishKey) || !_addressUtil.IsValidAddressLength(appUser.WithdrawPublishKey))
                        return new GenericResult(false, "Địa chỉ ethereum không hợp lệ.");

                    Nethereum.RPC.Eth.DTOs.Transaction transactionInfo = await GetTransactionByTransactionID(transactionID);
                    if (transactionInfo == null)
                        return new GenericResult(false, "Mã giao dịch không đúng định dạng.");

                    if (transactionInfo.From.ToLower() != appUser.WithdrawPublishKey.ToLower())
                        return new GenericResult(false, "Địa chỉ ethereum gửi không chính xác.");

                    if (transactionInfo.To.ToLower() != CommonConstants.PublishKeyERC20VT.ToLower())
                        return new GenericResult(false, "Địa chỉ ethereum nhận không chính xác");

                    decimal depositValue = Web3.Convert.FromWei(transactionInfo.Value);

                    _investRepository.Add(new Invest
                    {
                        AppUserId = appUser.Id,
                        FromAddress = transactionInfo.From,
                        ToAddress = transactionInfo.To,
                        TransactionHash = transactionInfo.TransactionHash,
                        DateCreated = DateTime.Now,
                        EthereumAmount = depositValue
                    });
                    _unitOfWork.Commit();

                    appUser.WalletEthereum += depositValue;
                    appUser.DateModified = DateTime.Now;
                    await _userManager.UpdateAsync(appUser);

                    return new GenericResult(true, $"Nạp {depositValue} Ether từ địa chỉ {transactionInfo.From} đến địa chỉ {transactionInfo.To} thành công.");
                }
            }
            catch (Exception ex)
            {
                return new GenericResult(false, ex.Message);
            }
        }

        //private GenericResult CompareEthereum(decimal customerDepositEthValue)
        //{
        //    var coinMarKetCapInfo = GetListingLatest(1, 20, "USD");
        //    if (coinMarKetCapInfo == null)
        //        return new GenericResult(false, "CoinMarKetCap: The process of loading monetary value has a problem.");

        //    var dataEther = coinMarKetCapInfo.data.Find(x => x.symbol == "ETH");
        //    if (dataEther == null)
        //        return new GenericResult(false, "CoinMarKetCap..: The process of loading monetary value has a problem..");

        //    var priceEth = dataEther.quote.USD.price;

        //    var investUSD = 110;

        //    var expectDepositEth = (investUSD / priceEth);


        //    var customerDepositEth = customerDepositEthValue;

        //    if (customerDepositEth < expectDepositEth)
        //        return new GenericResult(false, "Your deposit is less than the required value of the system");

        //    return new GenericResult(true, "Your account is eligible for activation");
        //}

        //public CoinMarKetCapInfoViewModel GetListingLatest(int startIndex, int limitItem, string unitConvertTo)
        //{
        //    CoinMarKetCapInfoViewModel coinMarKetCapInfo = null;

        //    var queryString = HttpUtility.ParseQueryString(string.Empty);
        //    queryString["start"] = startIndex.ToString(); // "1"
        //    queryString["limit"] = limitItem.ToString();// "2"
        //    queryString["convert"] = unitConvertTo;// "USD";

        //    string urlCryptoCurrencyListingLatest = _configuration["BlockChain:CoinmarKetCapApi:CMC_PRO_API_URL:CryptoCurrency:Listings_Latest"];
        //    var URL = new UriBuilder(urlCryptoCurrencyListingLatest);
        //    URL.Query = queryString.ToString();

        //    var client = new WebClient();
        //    var cmcProApiKey = _configuration["BlockChain:CoinmarKetCapApi:CMC_PRO_API_KEY"];
        //    client.Headers.Add("X-CMC_PRO_API_KEY", cmcProApiKey);
        //    client.Headers.Add("Accepts", "application/json");
        //    var coinInfoString = client.DownloadString(URL.ToString());

        //    coinMarKetCapInfo = JsonConvert.DeserializeObject<CoinMarKetCapInfoViewModel>(coinInfoString);

        //    return coinMarKetCapInfo;
        //}

        public async Task<Nethereum.RPC.Eth.DTOs.Transaction> GetTransactionByTransactionID(string transactionID)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls
               | SecurityProtocolType.SystemDefault;

            var web3 = new Web3(CommonConstants.InfuraUrl);

            Nethereum.RPC.Eth.DTOs.Transaction transactionInfo = await web3.Eth.Transactions
                .GetTransactionByHash.SendRequestAsync(transactionID).ConfigureAwait(true);

            return transactionInfo;
        }

        public async Task<string> SendERC20Async(string privateKeyERC20VT, string receiverAddress, string contractAddress, decimal tokenAmount, int decimalPlaces)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls
               | SecurityProtocolType.SystemDefault;


            var account = new Account(privateKeyERC20VT);

            var web3 = new Web3(account, CommonConstants.InfuraUrl);

            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunctionViewModel>();
            var transferFunction = new TransferFunctionViewModel()
            {
                To = receiverAddress,
                FromAddress = account.Address,
                TokenAmount = Web3.Convert.ToWei(tokenAmount, decimalPlaces),
            };

            var transactionReceipt = await transferHandler
                .SendRequestAsync(contractAddress, transferFunction).ConfigureAwait(true);

            return transactionReceipt;
        }

        public async Task<TransactionReceipt> SendEthAsync(string privateKey, string receiverAddress, decimal tokenAmount)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.SystemDefault;

            var account = new Account(privateKey);

            var web3 = new Web3(account, CommonConstants.InfuraUrl);

            var transaction = await web3.Eth.GetEtherTransferService()
                    .TransferEtherAndWaitForReceiptAsync(receiverAddress, tokenAmount);

            return transaction;
        }

        public async Task<decimal> GetERC20Balance(string owner, string smartContractAddress, int decimalPlaces)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.SystemDefault;

            var web3 = new Web3(CommonConstants.InfuraUrl);

            var balanceOfMessage = new BalanceOfFunction() { Owner = owner };

            var queryHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();

            var balance = await queryHandler
                .QueryAsync<BigInteger>(smartContractAddress, balanceOfMessage)
                .ConfigureAwait(true);

            decimal balanceUsdt = Web3.Convert.FromWei(balance, decimalPlaces);

            return balanceUsdt;
        }
    }
}
