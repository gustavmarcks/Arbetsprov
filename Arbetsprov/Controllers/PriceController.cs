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
        readonly List<Price> priceinfolist = new List<Price>();

        public ActionResult Index()
        {
            //Databas
            using (SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;"))
            {
                var model = new List<Price>();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Price", conn))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
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
                            model.Add(price);
                            //priceinfolist.Add(price);
                            ViewBag.PriceList = model;
                        }
                    }
                }
            }
            return View();
        }

        [Route("{pricecode}")]
        [Route("Price/Info/{pricecode}")]
        public ActionResult Info(string pricecode)
        {
            //Databas
            using (SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;"))
            {
                var model = new List<Price>();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Price", conn))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
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
                                    var price2 = new Price
                                    {
                                        MarketId = item.MarketId,
                                        UnitPrice = item.UnitPrice,
                                        CurrencyCode = item.CurrencyCode,
                                        ValidFrom = item.ValidFrom,
                                        ValidUntil = item.ValidUntil
                                    };
                                    model.Add(item);
                                    ViewBag.pList = model;
                                    //priceinfolist.Add(price);
                                }
                                else
                                {
                                    continue;
                                }
                            }


                        }
                    }
                }
                return View();
            }

        }
    }
}