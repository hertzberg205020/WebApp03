using System;
using System.Collections.Generic;

namespace WebApp03.Models
{
    /// <summary>
    /// Page是分頁物件
    /// </summary>
    /// <typeparam name="P">是具體的Entity</typeparam>
    public class Page<P>
    {
        private int _pageNo;
        public const int PAGE_SIZE = 5;
        // 當前頁碼
        public int PageNo {
            get => _pageNo;
            set
            {
                // 頁數邊界值檢查(數據邊界的有效檢查)
                if (value < 1)
                {
                    _pageNo = 1;
                    return;
                } 
                
                if (value > PageTotal)
                {
                    _pageNo = PageTotal;
                    return;
                }
                
                _pageNo = value;
            } 
        }
        // 總頁碼
        public int PageTotal { get; set; }
        // 總記錄筆數
        public int TotalCounts { get; set; }
        // 當前頁展示數據
        public IEnumerable<P> Items { get; set; }

        public override string ToString()
        {
            return this.ReportAllProperties();
        }
    }
}