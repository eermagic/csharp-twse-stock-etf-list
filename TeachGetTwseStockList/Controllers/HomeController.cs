using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static TeachGetTwseStockList.Models.HomeModel;

namespace TeachGetTwseStockList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 取得證交所上市及上櫃的股票及ETF清單
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public ActionResult GetList(GetListIn inModel)
        {
            GetListOut outModel = new GetListOut();

            // 檢查輸入來源
            if (string.IsNullOrEmpty(inModel.Q_MARKET_TYPE))
            {
                outModel.ErrMsg = "請輸入市場別";
                return Json(outModel);
            }
            if (string.IsNullOrEmpty(inModel.Q_ASSETS_TYPE))
            {
                outModel.ErrMsg = "請輸入資產別";
                return Json(outModel);
            }

            string url = "";
            string codeName = "";
            string stockCode = "";
            string stockName = "";
            string marketType = "";
            string industry = "";
            string publicDate = "";
            string assetType = "";
            if (inModel.Q_MARKET_TYPE == "TWSE")
            {
                outModel.CName = "證交所";
                // 來源網址
                url = "https://isin.twse.com.tw/isin/C_public.jsp?strMode=2";
            }
            else if (inModel.Q_MARKET_TYPE == "OTC")
            {
                outModel.CName = "櫃買中心";
                // 來源網址
                url = "https://isin.twse.com.tw/isin/C_public.jsp?strMode=4";
            }

            WebClient webClient = new WebClient();
            MemoryStream ms = new MemoryStream(webClient.DownloadData(url));

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(ms, Encoding.Default);
            // 取得 HTML 標籤
            HtmlNodeCollection trNode = doc.DocumentNode.SelectNodes("//table[2]/tr");
            if (trNode != null)
            {
                // 建立輸出table
                DataTable dtTwseStockList = new DataTable();
                dtTwseStockList.Columns.Add("SYMBOL_CODE");
                dtTwseStockList.Columns.Add("SYMBOL_NAME");
                dtTwseStockList.Columns.Add("MARKET_TYPE");
                dtTwseStockList.Columns.Add("ASSETS_TYPE");
                dtTwseStockList.Columns.Add("PUBLIC_DATE");
                dtTwseStockList.Columns.Add("INDUSTRY");

                foreach (HtmlNode tr in trNode)
                {
                    stockCode = "";
                    HtmlNodeCollection tdNode = tr.SelectNodes("td");
                    if (tdNode.Count == 7)
                    {
                        string stockType = tdNode[5].InnerText;
                        if (inModel.Q_MARKET_TYPE == "TWSE" && inModel.Q_ASSETS_TYPE == "STOCK")
                        {
                            // 取得證交所股票
                            if (stockType == "ESVUFR")
                            {
                                codeName = tdNode[0].InnerText;
                                stockCode = codeName.Split('　')[0];
                                stockName = codeName.Split('　')[1];
                                publicDate = tdNode[2].InnerText;
                                marketType = tdNode[3].InnerText;
                                industry = tdNode[4].InnerText;

                                assetType = "股票";
                            }
                        }
                        else if (inModel.Q_MARKET_TYPE == "TWSE" && inModel.Q_ASSETS_TYPE == "ETF")
                        {
                            // 取得證交所ETF
                            if (stockType == "CEOGEU" || stockType == "CEOGDU" || stockType == "CEOGMU" || stockType == "CEOJEU" || stockType == "CEOIBU" || stockType == "CEOJLU" || stockType == "CEOGBU" || stockType == "CEOIEU" || stockType == "CEOGCU" || stockType == "CEOIRU")
                            {
                                codeName = tdNode[0].InnerText;
                                stockCode = codeName.Split('　')[0];
                                stockName = codeName.Split('　')[1];
                                publicDate = tdNode[2].InnerText;
                                marketType = tdNode[3].InnerText;
                                industry = tdNode[4].InnerText;
                                assetType = "ETF";
                            }
                        }
                        else if (inModel.Q_MARKET_TYPE == "OTC" && inModel.Q_ASSETS_TYPE == "STOCK")
                        {
                            // 取得櫃買中心股票
                            if (stockType == "ESVUFR")
                            {
                                codeName = tdNode[0].InnerText;
                                stockCode = codeName.Split('　')[0];
                                stockName = codeName.Split('　')[1];
                                publicDate = tdNode[2].InnerText;
                                marketType = tdNode[3].InnerText;
                                industry = tdNode[4].InnerText;

                                assetType = "股票";
                            }

                        }
                        else if (inModel.Q_MARKET_TYPE == "OTC" && inModel.Q_ASSETS_TYPE == "ETF")
                        {
                            // 取得櫃買中心ETF
                            if (stockType == "CEOGBU" || stockType == "CEOGEU" || stockType == "CEOIBU" || stockType == "CEOIEU" || stockType == "CEOIRU" || stockType == "CEOJBU")
                            {
                                codeName = tdNode[0].InnerText;
                                stockCode = codeName.Split('　')[0];
                                stockName = codeName.Split('　')[1];
                                publicDate = tdNode[2].InnerText;
                                marketType = tdNode[3].InnerText;
                                industry = tdNode[4].InnerText;
                                assetType = "ETF";
                            }
                        }

                        if (stockCode != "")
                        {
                            // 加入 datatable
                            DataRow drNew = dtTwseStockList.NewRow();
                            drNew["SYMBOL_CODE"] = stockCode;
                            drNew["SYMBOL_NAME"] = stockName;
                            drNew["MARKET_TYPE"] = marketType;
                            drNew["ASSETS_TYPE"] = assetType;
                            drNew["PUBLIC_DATE"] = publicDate;
                            drNew["INDUSTRY"] = industry;
                            dtTwseStockList.Rows.Add(drNew);
                        }

                    }
                }

                // 輸出資料
                outModel.gridList = new List<StockRow>();
                foreach (DataRow dr in dtTwseStockList.Rows)
                {
                    StockRow row = new StockRow();
                    row.SYMBOL_CODE = dr["SYMBOL_CODE"].ToString();
                    row.SYMBOL_NAME = dr["SYMBOL_NAME"].ToString();
                    row.MARKET_TYPE = dr["MARKET_TYPE"].ToString();
                    row.ASSETS_TYPE = dr["ASSETS_TYPE"].ToString();
                    row.PUBLIC_DATE = dr["PUBLIC_DATE"].ToString();
                    row.INDUSTRY = dr["INDUSTRY"].ToString();
                    outModel.gridList.Add(row);
                }
                outModel.RowCnt = dtTwseStockList.Rows.Count;
            }

            // 回傳 Json 給前端
            return Json(outModel);
        }
    }
}