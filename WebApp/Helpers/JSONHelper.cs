using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces.Helpers;

namespace WebApp.Helpers
{
    public class JSONHelper : IJSONHelper
    {
        public bool IsValidJSON(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) { return false; }
            data = data.Trim();
            if ((data.StartsWith("{") && data.EndsWith("}")) || //For object
                (data.StartsWith("[") && data.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(data);
                    return true;
                }
                catch (JsonReaderException)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
