using System;
using System.Collections.Generic;
using System.Text;

namespace BeCoreApp.Application.ViewModels.BlockChain
{
    public class WalletViewModel
    {
        public decimal? WalletStart { get; set; }
        public decimal? WalletGame { get; set; }
        public decimal? WalletWin { get; set; }
        public decimal? WalletValuesShare { get; set; }
        public decimal? WalletUsdt { get; set; }
        public decimal? WalletEthereum { get; set; }
        public decimal? WalletUsdtDeposit { get; set; }

        public string PublishKeyVT { get; set; }

        public string SystemFee { get; set; }
        public decimal Balance { get; set; }
        public string PublishKey { get; set; }
        public string AddressReceiving { get; set; }
    }
}
