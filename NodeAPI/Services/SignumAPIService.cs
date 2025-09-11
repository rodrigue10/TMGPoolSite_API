using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NuGet.ContentModel;
using NuGet.Packaging.Signing;

using TMG_Site_API.NodeAPI.Models;

namespace TMG_Site_API.NodeAPI.Services
{
    public interface ISignumAPIService
    {

        //Interface for Exposed methods


       // public Task<UnconfirmedTransAPI?> getUnconfirmedTransactions(string accountID = "", string includeIndirect = "true");
        public Task<BlockChainStatus> getBlockChainStatus();
        public Task<GetAT> getAT(string id, string includeDetails = "true");
        public Task<GetATDetails> getATDetails(string id, string height = "");


        public Task<GetBlock> getBlock(string block = "", string height = "", string timestamp = "", string includeTransactions = "false");

        public Task<GetTrades> getTrades(string asset = "", string account = "", string firstIndex = "", string lastIndex = "",string includeAssetInfo = "true");

    //    public Task<GetPeers?> getPeers();
      //  public Task<GetPeer?> getPeer(string peerID = "");

      

    }

    public class SignumAPIService : ISignumAPIService
    {
        public HttpClient Client { get;  }
      
        //private readonly JsonSerializerOptions _options;

        public SignumAPIService(HttpClient client)
        {

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           
            //_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};

            Client = client;

        }


        public async Task<BlockChainStatus> getBlockChainStatus()
        {
                   

            string baseAPI = "/burst";
            string requestType = "?requestType=getBlockchainStatus";


            List<KeyValuePair<string, string>> allIputParams =
            [
                // Converting Request Params to Key Value Pair.  
               // new KeyValuePair<string, string>("&account=", accountID),
                //new KeyValuePair<string, string>("&includeIndirect=", includeIndirect),

            ];

            StringBuilder uri = new();

            uri.Append(baseAPI);
            uri.Append(requestType);

            foreach (var item in allIputParams)
            {
                uri.Append(item.Key);
                uri.Append(item.Value);

            }


            try
            {
                return await Client.GetFromJsonAsync<BlockChainStatus>(uri.ToString());
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;

        }

        public async Task<GetAT> getAT(string atId, string includeDetails = "true")
        {
           

            string baseAPI = "/burst";
            string requestType = "?requestType=getAT";


            List<KeyValuePair<string, string>> allIputParams =
            [
                // Converting Request Params to Key Value Pair.  
                 new KeyValuePair<string, string>("&at=", atId),
                new KeyValuePair<string, string>("&includeDetails=", includeDetails),

            ];

            StringBuilder uri = new();

            uri.Append(baseAPI);
            uri.Append(requestType);

            foreach (var item in allIputParams)
            {
                uri.Append(item.Key);
                uri.Append(item.Value);

            }


            try
            {
                return await Client.GetFromJsonAsync<GetAT>(uri.ToString());
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }

        public async Task<GetBlock> getBlock(string block = "", string height = "", string timestamp = "", string includeTransactions = "false")
        {


            string baseAPI = "/burst";
            string requestType = "?requestType=getBlock";


            List<KeyValuePair<string, string>> allIputParams =
            [
                // Converting Request Params to Key Value Pair.  
                 new KeyValuePair<string, string>("&block=", block),
                new KeyValuePair<string, string>("&height=", height),
                new KeyValuePair<string, string>("&timestamp=", timestamp),
                new KeyValuePair<string, string>("&includeTransactions=", includeTransactions)


            ];

            StringBuilder uri = new();

            uri.Append(baseAPI);
            uri.Append(requestType);

            foreach (var item in allIputParams)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    uri.Append(item.Key);
                    uri.Append(item.Value);
                }
            }


            try
            {
                return await Client.GetFromJsonAsync<GetBlock>(uri.ToString());
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }

        public async Task<GetTrades> getTrades(string asset = "", string account = "", string firstIndex = "", string lastIndex = "", string includeAssetInfo = "true")
        {
            

            string baseAPI = "/burst";
            string requestType = "?requestType=getTrades";


            List<KeyValuePair<string, string>> allIputParams =
            [
                // Converting Request Params to Key Value Pair.  
                 new KeyValuePair<string, string>("&asset=", asset),
                new KeyValuePair<string, string>("&account=", account),
                new KeyValuePair<string, string>("&firstIndex=", firstIndex),
                new KeyValuePair<string, string>("&lastIndex=", lastIndex),
                new KeyValuePair<string, string>("&includeAssetInfo=", includeAssetInfo)

            ];

            StringBuilder uri = new();

            uri.Append(baseAPI);
            uri.Append(requestType);

            foreach (var item in allIputParams)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    uri.Append(item.Key);
                    uri.Append(item.Value);
                }
            }


            try
            {
                return await Client.GetFromJsonAsync<GetTrades>(uri.ToString());
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }

        public async Task<GetATDetails> getATDetails(string at, string height = "")
        {
           

            string baseAPI = "/burst";
            string requestType = "?requestType=getATDetails";


            List<KeyValuePair<string, string>> allIputParams =
            [
                // Converting Request Params to Key Value Pair.  
                 new KeyValuePair<string, string>("&at=", at),
                new KeyValuePair<string, string>("&height=", height)             

            ];

            StringBuilder uri = new();

            uri.Append(baseAPI);
            uri.Append(requestType);

            foreach (var item in allIputParams)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    uri.Append(item.Key);
                    uri.Append(item.Value);
                }
            }


            try
            {
                return await Client.GetFromJsonAsync<GetATDetails>(uri.ToString());
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
    }
}
