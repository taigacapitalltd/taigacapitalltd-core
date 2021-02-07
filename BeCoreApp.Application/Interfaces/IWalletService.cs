using BeCoreApp.Application.ViewModels.BlockChain;
using BeCoreApp.Utilities.Dtos;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3.Accounts;

namespace BeCoreApp.Application.Interfaces
{
    public interface IWalletService
    {
        Task<GenericResult> WithdrawUSDT(decimal amount, string customerId);
        Task<GenericResult> WithdrawVSCoin(decimal amount, string customerId);
    }
}
