using System;
using System.Collections;
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
        //Listor med priser
        public static List<Price> pricelist = new List<Price>();
        public static HashSet<Price> priceinfolist = new HashSet<Price>();

        //Koppling till databas
        static readonly SqlConnection sqlconn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;");

        //SQL query från databasen
        static readonly SqlCommand sqlcmd = new SqlCommand("SELECT * FROM dbo.Price", sqlconn);

        public ActionResult Index()
        {
            sqlconn.Open();
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                    
            //Sätter värden från databas till lista
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
                    ValidUntil = DateTime.Parse(sqlrdr["ValidUntil"].ToString()),
                    UnitPrice = decimal.Parse(sqlrdr["UnitPrice"].ToString())
                };
                pricelist.Add(price);
                ViewBag.PriceList = pricelist;
            }
            sqlconn.Close();
            return View();   
        }
        //Inmatning av SKU i adressfält
        [Route("{pricecode}")]
        public ActionResult Info(string pricecode)
        {
            foreach (var item in pricelist)
            {
                //Jämförelse
                if (item.CatalogEntryCode == pricecode && DateTime.Now < item.ValidUntil && DateTime.Now > item.ValidFrom)
                {
                    _ = new Price
                    {
                        PriceValueId = item.PriceValueId,
                        Created = item.Created,
                        Modified = item.Modified,
                        CatalogEntryCode = item.CatalogEntryCode,
                        MarketId = item.MarketId,
                        CurrencyCode = item.CurrencyCode, 
                        ValidFrom = item.ValidFrom,
                        ValidUntil = item.ValidUntil,
                        UnitPrice = item.UnitPrice
                    };
                    priceinfolist.Add(item);
                }
                else
                {
                   continue;
                }                       
            }                               
            ViewBag.PriceInfoList = priceinfolist;                       
            return View();
        }                      
    }   
}