using System;

namespace PSW.ITMS.Service.ModelValidators
{
    public static class Validator
    {
        public static string Validate(object DTO)
        {
            try
            {
                switch (DTO.GetType().Name)
                {
                    default:
                        break;
                }

                return "No matching model validator found.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}