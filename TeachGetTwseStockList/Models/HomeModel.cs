using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachGetTwseStockList.Models
{
    public class HomeModel
    {
        /// <summary>
        /// 參數
        /// </summary>
        public class GetListIn
        {
            public string Q_MARKET_TYPE { get; set; }
            public string Q_ASSETS_TYPE { get; set; }
        }

        /// <summary>
        /// 回傳
        /// </summary>
        public class GetListOut
        {
            public string ErrMsg { get; set; }
            public List<StockRow> gridList { get; set; }
            public string CName { get; set; }
            public int RowCnt { get; set; }
        }

        public class StockRow
        {
            public string SYMBOL_CODE { get; set; }
            public string SYMBOL_NAME { get; set; }
            public string MARKET_TYPE { get; set; }
            public string ASSETS_TYPE { get; set; }
            public string PUBLIC_DATE { get; set; }
            public string INDUSTRY { get; set; }
        }
    }
}