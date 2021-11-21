using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Arbetsprov.Models;
using System.Linq;

namespace Arbetsprov.Controllers
{
    public class PriceController : Controller
    {
        //Lista med priser
        private static readonly List<Price> pricelist = new List<Price>();
        private static readonly List<Price> sortedpricelist = new List<Price>();
        private static readonly List<Price> priceinfolist = new List<Price>();
        
        //Koppling till databas
        static readonly SqlConnection sqlconn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;");

        //SQL query från databasen
        static readonly SqlCommand sqlcmd = new SqlCommand("SELECT * FROM dbo.Price", sqlconn);

        //Metod som skickar värde till vy Index 
        public ActionResult Index()
        {
            //Öppnar connection till SQL
            sqlconn.Open();

            //Skapar en datareader
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();

            //Läser databas med hjälp av datareader
            while (sqlrdr.Read())
            {
                //Nytt objekt av klassen Price
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
                //Adderar objekten till listan 
                pricelist.Add(price);
            }
            //Stänger connection till sql efter att data är hämtat
            sqlconn.Close();

            //Returnerar data från listan pricelist till vy View
            return View(pricelist);
        }
        //Inmatning av SKU i adressfält
        [Route("{pricecode}")]

        //Metod som skickar värde till vy Info
        public ActionResult Info(string pricecode)
        {
            //Loopar igenom samtiga artiklar i prislistan
            foreach (var item in pricelist)
            {
                //Jämför datum och CatalogEntryCode som matades in i adressfält
                if (item.CatalogEntryCode == pricecode && DateTime.Now < item.ValidUntil && DateTime.Now > item.ValidFrom)
                {
                    //Adderar objekten till listan som gick igenom if-satsen
                    sortedpricelist.Add(item);                 
                }
                //Fortsätter loop från början
                else
                {
                    continue;
                }
            }
            //Minsta pris för jämföra priser i föregående lista
            decimal minprice = sortedpricelist.Min(p => p.UnitPrice);

            foreach (var item in sortedpricelist)
            {
                if (item.UnitPrice == minprice)
                {
                    priceinfolist.Add(item);
                }
            }

            //Returnerar data från listan priceinfolist till vy Info
            return View(priceinfolist);
        }
    }
}