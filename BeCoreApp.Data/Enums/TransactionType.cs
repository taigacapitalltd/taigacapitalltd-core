using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BeCoreApp.Data.Enums
{
    public enum TransactionType
    {
        [Description("Cập Nhật KYC")]
        UpdateKYC = 1,
        [Description("Kích Hoạt VS Token")]
        ActivateVSToken = 2,
        [Description("Kích Hoạt USDT")]
        ActivateUSDT = 3,
        [Description("Nạp ETH")]
        DepositEth = 4,
        [Description("Rút USDT")]
        WithdrawUSDT = 5,
        [Description("Nạp USDT")]
        DepositUSDT = 6,
        [Description("Rút VS Token")]
        WithdrawVSToken = 7,
    }
}
