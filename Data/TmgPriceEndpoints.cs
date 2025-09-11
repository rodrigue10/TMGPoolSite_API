using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using TMG_Site_API.Models;
using TMG_Site_API.Pages;



namespace TMG_Site_API.Data;

public static class TmgPriceEndpoints
{
    public static void MapTmgPriceEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api").WithTags(nameof(TmgPrice));

        group.MapGet("/getAllPrices", async (TmgPoolApiContext db) =>
        {
            try
            {
                var tmgPrices =  await db.TmgPrices.AsNoTracking().OrderByDescending(p => p.Epoch).ToListAsync();

                return tmgPrices is List<TmgPrice> model ? Results.Ok(model) : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });
            }
            catch
            {
                return Results.Ok(new ErrorData() { errorCode = -1, errorDescription = "Internal API error." });
            }
            
        })
        .WithName("getAllPrices")
        .WithOpenApi();



        group.MapGet("/getLatestPrice", async  (TmgPoolApiContext db) =>
        {
            try
            {
                TmgPrice tmgPrice = await db.TmgPrices.AsNoTracking().OrderByDescending(p => p.Epoch).FirstOrDefaultAsync();

                return tmgPrice is TmgPrice model
                        ? Results.Ok(model)
                         : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });
            }
            catch
            {
                return Results.Ok(new ErrorData() { errorCode = -1, errorDescription = "Internal API error." });
            }
        })
        .WithName("getLatestPrice")
        .WithOpenApi();

        group.MapGet("/getPrices", async (TmgPoolApiContext db, int start, int? end = null) =>
        {

            try
            {
                if(start <=0 )
                {
                    return Results.Ok(new ErrorData() { errorCode = 2, errorDescription = "You must specify the `start` period." });
                    
                }
                else if (end == 0 || end is null)
                {


                    var tmgPrices = await db.TmgPrices.AsNoTracking().Where(p => p.Epoch >= start).OrderByDescending(p => p.Epoch).ToListAsync();
                                       

                    return tmgPrices is (List<TmgPrice> model ) ? Results.Ok(model) : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });

                }
                else if (end != null && end > start)
                {
                            var tmgPrices = await db.TmgPrices.AsNoTracking().Where(p => p.Epoch >= start && p.Epoch <= end).OrderByDescending(p => p.Epoch).ToListAsync();
                                       

                    return tmgPrices is (List<TmgPrice> model ) ? Results.Ok(model) : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });               


                }
                else
                {

                    var tmgPrices =  await db.TmgPrices.AsNoTracking().Where(p => p.Epoch > 0).OrderByDescending(o => o.Epoch).ToListAsync();

                    return tmgPrices is (List<TmgPrice> model) ? Results.Ok(model) : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });

                }
            }
            catch
            {
                return Results.Ok(new ErrorData() { errorCode = -1, errorDescription = "Internal API error." });
            }     
        })
       .WithName("getPrices")
       .WithOpenApi();


        group.MapGet("/getPriceAt", async (TmgPoolApiContext db, int epoch = 0, int blockheight = 0) =>
        {


            //both variables are empty/not supplied
            if (epoch == 0 && blockheight == 0 || epoch != 0 && blockheight != 0)
            {
                    ErrorData error = new() { errorCode = 3, errorDescription = "Only one of `epoch` or `blockheight` must be supplied." };
                                

                    return Results.Ok(error);


             }         

            else if (epoch != 0) 
            {
                return await db.TmgPrices.AsNoTracking().Where(model => (model.Epoch <= epoch)).OrderByDescending(o => o.Epoch)
                .FirstOrDefaultAsync()
                is TmgPrice model
                ? Results.Ok(model)
                : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });

       
                        
            }
            /*(blockheight != 0)*/
            else
            {
                return await db.TmgPrices.AsNoTracking().Where(model => (model.BlockHeight <= blockheight)).OrderByDescending(o => o.BlockHeight)
                   .FirstOrDefaultAsync()
                   is TmgPrice model
               ? Results.Ok(model)
               : Results.Ok(new ErrorData() { errorCode = 1, errorDescription = "No records of TMG price." });


            }
        })
        .WithName("getPriceAt")
        .WithOpenApi();



        group.MapGet("/getDailyOHLC", async (TmgPoolApiContext db, int start = 0) =>
        {
            //var sqlString = @"SELECT 
            //                    DATE_FORMAT(FROM_UNIXTIME(epoch), '%Y-%m-%d') AS date,
            //                    MIN(price) AS low,
            //                    MAX(price) AS high,
            //                    MAX(volume) as volume,
            //                    SUBSTRING_INDEX(GROUP_CONCAT(price ORDER BY epoch DESC), ',', 1) AS close
            //                FROM
            //                    tmg_prices
            //                WHERE
            //                    epoch >= UNIX_TIMESTAMP(DATE_FORMAT(FROM_UNIXTIME(), '%Y-%m-%d'))
            //                GROUP BY
            //                    date";


            var values = await db.TmgPrices.GroupBy(p => p.JSTimespanDay)
                                        .Select(g => new
                                        {
                                            date = g.Key, //JS Timespan date
                                            
                                            open = db.TmgPrices.AsNoTracking().Where(x => x.JSTimespanDay == g.Key).OrderBy(o => o.Epoch).FirstOrDefault().PrevPrice,     //Open needs to be on DB
                                            high = g.Max(x => x.Price), //High 
                                            low = g.Min(s => s.Price),  //Low 
                                            close = db.TmgPrices.AsNoTracking().Where(f => f.JSTimespanDay == g.Key).OrderByDescending(d => d.Epoch).FirstOrDefault().Price, //Close
                                            volume = g.Sum(v => v.DayVolume) //Volume

                                        }).ToListAsync();

            List<List<double>> finalOutput = [];
            


            foreach(var item in values)
            {
                List<double> temp = 
                [
                    
                    item.date,
                    Math.Round(item.open,4),
                    Math.Round(item.high,4),
                    Math.Round(item.low,4),
                    Math.Round(item.close,4),
                    Math.Round(item.volume,2)
                ];
                finalOutput.Add(temp);
            }



            return JsonSerializer.Serialize(finalOutput);




        })
        .WithName("getDailyOHLC")
        .WithOpenApi();

    }
}
