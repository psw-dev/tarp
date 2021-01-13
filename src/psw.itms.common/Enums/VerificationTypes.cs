namespace PSW.ITMS.Common.Enums
{
    public enum VerificationTypes
    {
        ALL_PENDING =            0,  // 00000000
        NTN_VERIFIED =           1,  // 00000001
        SECP_VERIFIED =          2,  // 00000010
        BIO_VERIFIED =           4,  // 00000100
        PAYMENT_VERIFIED =       8,  // 00001000
        OTP_VERIFIED =          16,  // 00010000
        USER_SUBSCRIBED =       32   // 00100000
    }


    public class StatusChanger
    {
        public bool CheckIfRequired(sbyte state, VerificationTypes checkState)
        {
            if((state&(sbyte)checkState)==(sbyte)checkState)
            {
                //checkState is ON
                return true;
            }
            else
            {
                //checkState is OFF
                return false;
            }

        }

        public sbyte ChangeState(sbyte state, VerificationTypes newstate)
        {
            if((state&(sbyte)newstate)==(sbyte)newstate)
            {
                //checkState is ON
                return state;
            }
            else
            {
                //checkState is OFF
                sbyte sbNewState = (sbyte)newstate;
                return (sbyte)(state | sbNewState);
            }

        }
    }
}