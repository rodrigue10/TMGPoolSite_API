using System.Buffers.Binary;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using TMG_Site_API.NodeAPI.Models;

namespace TMG_Site_API.NodeAPI
{
    public static class ExtensionMethods
    {  

        public static int getEpochFromBlock(this GetBlock getBlock)
        {

            return getBlock.Timestamp + 1407722400;

        }
        public static double getPriceFromMachineData(this GetAT getAT)
        {
          


            var signaTotal = hexToDecimal(getAT.MachineData.Substring(16 * 16, 16));
            var assetTotal = hexToDecimal(getAT.MachineData.Substring(17 * 16, 16));




            if (assetTotal == 0)
            {
                return -1;
            }


            return (((double)signaTotal / 1000000.0) / (double)assetTotal);
        }

        public static double getPriceFromMachineData(this GetATDetails getAT)
        {
           
            var signaTotal = hexToDecimal(getAT.MachineData.Substring(16 * 16, 16));
            var assetTotal = hexToDecimal(getAT.MachineData.Substring(17 * 16, 16));

            if (assetTotal == 0) {
                return -1;
            }


            return (((double)signaTotal / 1000000.0) / (double)assetTotal) ;
        }

        public static long hexToDecimal(this string str) {
            return BinaryPrimitives.ReverseEndianness(Convert.ToInt64(str, 16));
        }

        public static double getCummulativeVolume(this GetAT getAT)
        {       

            var volume = hexToDecimal(getAT.MachineData.Substring(21 * 16, 16));

            return ((double)volume / 100000000.0);
        }

        public static double getCummulativeVolume(this GetATDetails getAT)
        {
           
            var volume = hexToDecimal(getAT.MachineData.Substring(21 * 16, 16));

            return ((double)volume / 100000000.0);
        }
          


    }
}
