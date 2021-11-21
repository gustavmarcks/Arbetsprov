using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Arbetsprov.Models;

namespace Arbetsprov.Controllers
{
    public class PriceController : Controller
    {
        //Lista med priser
        public static List<Price> pricelist = new List<Price>();
        public static List<Price> priceinfolist = new List<Price>();

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
                //!Hårdkodat värde för att endast ta ut svenska priser!
                if (item.CatalogEntryCode == pricecode && DateTime.Now < item.ValidUntil && DateTime.Now > item.ValidFrom && item.MarketId == "sv")
                {
                    //Sätter värden på nytt objekt om if-satsen gick igenom 
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
                    //Adderar objekten till listan som gick igenom if-satsen
                    priceinfolist.Add(item);
                }                   
            }
            //Returnerar data från listan priceinfolist till vy Info
            return View(priceinfolist);
        }                      
    }   
}