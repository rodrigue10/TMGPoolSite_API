using TMG_Site_API.Models;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMG_Site_API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TmgPoolApiContext context)
        {

            Random random = new ();
            Array values = Enum.GetValues<Summaries>();
                        

            if (context.TmgPrices.Any())
            {
                          
               


            }
            else 
            {
                //context.TmgPrices.Add(

                    //Seed DB with empty set ... maybe not necessary
                      //new TmgPrice
                      //{
                      //    Epoch = 0,
                      //    Price = 0,
                      //    BlockHeight = 0,
                      //    Volume = 0
                      //});

            }


            context.SaveChanges();
            return; 





        }
    }
}
