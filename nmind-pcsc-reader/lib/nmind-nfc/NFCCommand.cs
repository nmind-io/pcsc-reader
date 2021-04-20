using SpringCard.PCSC;

/// <summary>
/// 
/// </summary>
namespace Nmind.pcsc {

    /// <summary>
    /// 
    /// </summary>
    class NFCCommand {

        /// <summary>
        /// 
        /// </summary>
        private NFCCommand() {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CAPDU ReadCardUid() {

            return new CAPDU() {
                CLA = 0xFF,
                INS = 0xCA,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0x00
            };
        }
    }

}
