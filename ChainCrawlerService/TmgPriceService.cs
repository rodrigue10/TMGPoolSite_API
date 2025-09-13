using System;
using System.Buffers.Binary;
using System.Configuration;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMG_Site_API.Data;
using TMG_Site_API.Models;
using TMG_Site_API.NodeAPI;
using TMG_Site_API.NodeAPI.Models;
using TMG_Site_API.NodeAPI.Services;



namespace TMG_Site_API.ChainCrawlerService
{

    public class TmgPriceService : BackgroundService
    {
        //Initialize some defaults:
        public int UpdateTime = 5000;
        private string ContractId = "";
        private string TokenId = "";
        public int FirstGoodBlock = 1052615;
        private  readonly DateTime UtcEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private  double OpenPrice = 200.0;
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        public int MaxBlockHeight = 0;
        //Used to help monitor and control service from web-page
        public bool IsRunning { get; set; }

        public bool IsSleep { get; set; } = false;


        private readonly ILogger<TmgPriceService> _logger;
        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory<TmgPoolApiContext> _contextFactoryTmgPoolApi;
        private readonly ISignumAPIService _signumApiService;


        public TmgPriceService(ILogger<TmgPriceService> logger, IConfiguration configuration, IDbContextFactory<TmgPoolApiContext> dbContextFactory, ISignumAPIService signumAPIService)
        {
            _logger = logger;
            _configuration = configuration;
            _contextFactoryTmgPoolApi = dbContextFactory;
            _signumApiService = signumAPIService;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            //Give the base hosted service web app some time to load first:
            await Task.Delay(5000, stoppingToken); 

            try
            {
                if (int.TryParse(_configuration["TmgPriceProcessor:UpdateInterval"], out int updateTime))
                {
                    UpdateTime = updateTime;
                }

                ContractId = _configuration["ContractDetails:ContractId"];
                TokenId = _configuration["ContractDetails:TokenId"];
                FirstGoodBlock = int.Parse(_configuration["ContractDetails:FirstTradeBlock"]);
                OpenPrice = double.Parse(_configuration["ContractDetails:FirstTradePrice"]);






                _logger.LogInformation($"{nameof(TmgPriceService)} starting {nameof(ExecuteAsync)}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    IsRunning = true;

                    using (var contextTmg = _contextFactoryTmgPoolApi.CreateDbContext() )                
                    {

                        //Testing and thought for future dynamic wiping out DB:
                           //if(!contextTmg.Database.CanConnect())
                           // { contextTmg.Database.EnsureCreated(); }

                        //Blockchain status test

                        BlockChainStatus blockChainStatus = _signumApiService.getBlockChainStatus().Result;
                        
                        int maxBlockHeight = blockChainStatus.LastBlockchainFeederHeight;
                        MaxBlockHeight = maxBlockHeight;

                        //Get Current AT:
                        GetAT getCurrentAt = _signumApiService.getAT(ContractId).Result;

                        int createdBlock = getCurrentAt.CreationBlock;

                        //Setup initial query to have the first one
                        int queryBlockHeight = getCurrentAt.CreationBlock;
                        double prevDayCumVolume = 0.00;
                        double cumalitiveVolume = 0.00;
                        double prevPrice = OpenPrice;


                        TmgPrice DbQuery = contextTmg.TmgPrices.OrderByDescending(x => x.BlockHeight).FirstOrDefaultAsync().Result;


                        //Case when db is empty and need to fill it with th first set of details
                        if (DbQuery is null)
                        {
                            //Fill in with the first known good price:

                            _logger.LogInformation("DB looks Empty....will need to start building the DB from AT Creation block");

                            GetATDetails initialDetails = await _signumApiService.getATDetails(ContractId, FirstGoodBlock.ToString());

                             GetBlock blockInit = await _signumApiService.getBlock(height:initialDetails.NextBlock.ToString());
                            //GetBlock blockInit = await _signumApiService.getBlock(height: queryBlockHeight.ToString());

                            int blocke = blockInit.getEpochFromBlock();
                            DateTime epochDate = UtcEpoch.AddSeconds(blocke).Date;
                            double JStimespan = epochDate.Subtract(UtcEpoch).TotalMilliseconds;
                            queryBlockHeight = initialDetails.NextBlock;
                            cumalitiveVolume = initialDetails.getCummulativeVolume();



                            TmgPrice tmgPriceInit = new TmgPrice();
                            {
                                tmgPriceInit.Epoch = blocke;
                                tmgPriceInit.Volume = cumalitiveVolume;
                                tmgPriceInit.Price = initialDetails.getPriceFromMachineData(); ;
                                tmgPriceInit.BlockHeight = queryBlockHeight;
                                tmgPriceInit.EpochDay = epochDate;
                                tmgPriceInit.JSTimespanDay = JStimespan;
                                tmgPriceInit.DayVolume = cumalitiveVolume - prevDayCumVolume;
                                tmgPriceInit.PrevPrice = prevPrice;


                            }
                            await contextTmg.TmgPrices.AddAsync(tmgPriceInit);
                            await contextTmg.SaveChangesAsync();



                            DbQuery = await contextTmg.TmgPrices.OrderByDescending(x => x.BlockHeight).FirstOrDefaultAsync();
                            queryBlockHeight = DbQuery.BlockHeight;
                            prevDayCumVolume = DbQuery.Volume;
                            prevPrice = DbQuery.Price;

                        }
                        else
                        {
                            _logger.LogInformation("DB has some stuff already.....see if i can keep things happy");

                            DbQuery = await contextTmg.TmgPrices.OrderByDescending(x => x.BlockHeight).FirstOrDefaultAsync();
                            queryBlockHeight = DbQuery.BlockHeight;
                            prevDayCumVolume = DbQuery.Volume;
                            prevPrice = DbQuery.Price;
                        
                        }

                        
                                                


                        while (maxBlockHeight > queryBlockHeight)
                        {
                            _logger.LogInformation($"Max Block Height: {maxBlockHeight} - Query Blockheight: {queryBlockHeight}");


                            queryBlockHeight++;

                            //Get AT Details at next block not in system:   
                            GetATDetails getATDetails = await _signumApiService.getATDetails(ContractId, queryBlockHeight.ToString());

                            var selectedVolume = getATDetails.getCummulativeVolume();

                            if (selectedVolume <= DbQuery.Volume)
                            {
                                //_logger.LogInformation($"Block: {queryBlockHeight} has no volume change");


                            }
                            else
                            {
                                //_logger.LogInformation($"Block: {queryBlockHeight} has more volume...should do something here....");

                                GetBlock blockInit = await _signumApiService.getBlock(height:getATDetails.NextBlock.ToString());
                                int blocke = blockInit.getEpochFromBlock();
                                DateTime epochDate = UtcEpoch.AddSeconds(blocke).Date;
                                double JStimespan = epochDate.Subtract(UtcEpoch).TotalMilliseconds;
                                cumalitiveVolume = getATDetails.getCummulativeVolume();



                                TmgPrice tmgPricelook = new TmgPrice();
                                {
                                    tmgPricelook.Epoch = blocke;
                                    tmgPricelook.Volume = cumalitiveVolume;
                                    tmgPricelook.Price = getATDetails.getPriceFromMachineData();
                                    tmgPricelook.BlockHeight = getATDetails.NextBlock;
                                    tmgPricelook.EpochDay = epochDate;
                                    tmgPricelook.JSTimespanDay = JStimespan;
                                    tmgPricelook.DayVolume = cumalitiveVolume - prevDayCumVolume;
                                    tmgPricelook.PrevPrice = prevPrice;



                                }
                              //  _logger.LogInformation(JsonConvert.SerializeObject(tmgPricelook));

                                await contextTmg.TmgPrices.AddAsync(tmgPricelook);

                                await contextTmg.SaveChangesAsync();
                              
                                

                                //_logger.LogInformation("Added Block data");
                                _logger.LogInformation($"Added new block Data: {JsonConvert.SerializeObject(tmgPricelook)}");


                                //Reselect from DB ?
                                DbQuery = contextTmg.TmgPrices.OrderByDescending(x => x.BlockHeight).FirstOrDefaultAsync().Result;
                                queryBlockHeight = DbQuery.BlockHeight;
                                prevDayCumVolume = DbQuery.Volume;
                                prevPrice = DbQuery.Price;
                            }

                        }

                        

                    }

                    //   _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);



                    //Waiting out to try finding more data:
                    IsSleep = true;
                    CurrentTime = DateTime.UtcNow;

                    await Task.Delay(UpdateTime, stoppingToken); // Example: delay for 1 second




                }

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
            }
            finally
            {
                IsRunning = false;
            }
         
        }
    }
}