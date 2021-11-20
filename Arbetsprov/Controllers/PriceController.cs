using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Arbetsprov.Models;

namespace Arbetsprov.Controllers
{
    public class PriceController : Controller
    {
        readonly List<Price> pricelist = new List<Price>();
        public List<Price> priceinfolist = new List<Price>();
        public List<Price> priceinfolist2 = new List<Price>();

        //Koppling till databas
        static readonly SqlConnection sqlconn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;");

        //SQL query från databasen
        static readonly SqlCommand sqlcmd = new SqlCommand("SELECT * FROM dbo.Price", sqlconn);

        public ActionResult Index()
        {
            sqlconn.Open();
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                    
            while (sqlrdr.Read())
            {
                var price = new Price
                {
                    PriceValueId = int.Parse(sqlrdr["PriceValueId"].ToString()),
                    Created = DateTime.Parse(sqlrdr["Created"].ToString()),
                    Modified = DateTime.Parse(sqlrdr["Modified"].ToString()),
                    CatalogEntryCode = sqlrdr["CatalogEntryCode"].ToString(),
                    MarketId = sqlrdr["MarketId"].ToString(),
                    CurrencyCode = sqlrdr["CurrencyCode"].ToString(),
                    ValidFrom = DateTime.Parse(sqlrdr["ValidFrom"].ToString()),
                    //NULL??
                    ValidUntil = DateTime.Parse(sqlrdr["ValidUntil"].ToString()),
                    UnitPrice = decimal.Parse(sqlrdr["UnitPrice"].ToString())
                };
                pricelist.Add(price);
                //priceinfolist.Add(price);
                ViewBag.PriceList = pricelist;
            }
            sqlconn.Close();
            return View();   
        }
    
        [Route("{pricecode}")]
        [Route("Price/Info/{pricecode}")]
        public ActionResult Info(string pricecode)
        {
            sqlconn.Open();
            SqlDataReader rdr = sqlcmd.ExecuteReader();
            {
                while (rdr.Read())
                {
                    var price = new Price
                    {
                        PriceValueId = int.Parse(rdr["PriceValueId"].ToString()),
                        Created = DateTime.Parse(rdr["Created"].ToString()),
                        Modified = DateTime.Parse(rdr["Modified"].ToString()),
                        CatalogEntryCode = rdr["CatalogEntryCode"].ToString(),
                        MarketId = rdr["MarketId"].ToString(),
                        CurrencyCode = rdr["CurrencyCode"].ToString(),
                        ValidFrom = DateTime.Parse(rdr["ValidFrom"].ToString()),
                         //NULL??
                        ValidUntil = DateTime.Parse(rdr["ValidUntil"].ToString()),
                        UnitPrice = decimal.Parse(rdr["UnitPrice"].ToString())
                    };
                    priceinfolist.Add(price);

                    foreach (var item in priceinfolist)
                    {
                        if (item.CatalogEntryCode == pricecode)
                        {
                            _ = new Price
                            {
                                MarketId = item.MarketId,
                                UnitPrice = item.UnitPrice,
                                CurrencyCode = item.CurrencyCode,
                                ValidFrom = item.ValidFrom,
                                ValidUntil = item.ValidUntil
                            };
                            priceinfolist2.Add(item);
                            ViewBag.plist = priceinfolist2;
                        }
                        else
                        {
                            continue;
                        }
                    }                
                }
                sqlconn.Close();
                return View();
            }
        }                      
    }   
}