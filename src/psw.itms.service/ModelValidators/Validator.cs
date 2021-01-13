using PSW.ITMS.Common;
using PSW.ITMS.Service.DTO;
using System.Text.Json;
using System;
using Microsoft.Extensions.Configuration;
using PSW.ITMS.Service.ModelValidators;

namespace PSW.ITMS.Service.ModelValidators 
{
    public static class Validator 
    {
        public static string Validate(object DTO)
        {
            try
            {
                switch(DTO.GetType().Name)
                {
                    default:
                    break;
                }

                return "No matching model validator found.";
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

    }

}