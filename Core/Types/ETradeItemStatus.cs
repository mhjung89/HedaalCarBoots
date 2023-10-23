using System.ComponentModel;

namespace Core.Types
{
    public enum ETradeItemStatus
    {
        [Description("판매 중")]
        Active,
        [Description("판매 완료")]
        Sold,
        [Description("예약")]
        Reserved,
        [Description("삭제")]
        Removed
    }
}
