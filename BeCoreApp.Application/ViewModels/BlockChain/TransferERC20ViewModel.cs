using System;
using System.Collections.Generic;
using System.Text;

namespace BeCoreApp.Application.ViewModels.BlockChain
{
    public class TransferERC20ViewModel
    {
        public decimal AmountTransfer { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string AddressReceiving { get; set; }
    }
}
