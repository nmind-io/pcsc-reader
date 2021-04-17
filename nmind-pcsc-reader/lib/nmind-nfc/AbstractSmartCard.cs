using PCSC;
using PCSC.Iso7816;
using System;
using System.Linq;

/// <summary>
/// 
/// </summary>
namespace Nmind.pcsc {

    /// <summary>
    /// 
    /// </summary>
    abstract class AbstractSmartCard : ISmartCard {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public abstract byte[] GetCardMemory(SCardReader reader);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract String GetCardType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public byte[] GetUid(SCardReader reader){

            SCardError sc = reader.BeginTransaction();

            var responseApdu = SendAPDU(NFCCommand.ReadCardUid(reader.ActiveProtocol), reader);

            reader.EndTransaction(SCardReaderDisposition.Leave);

            if (responseApdu != null && responseApdu.HasData) {
                return responseApdu.GetData();
            } else {
                return new byte[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apdu"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected ResponseApdu SendAPDU(CommandApdu apdu, SCardReader reader) {
            var receivePci = new SCardPCI(); // IO returned protocol control information.
            var sendPci = SCardPCI.GetPci(reader.ActiveProtocol);

            var receiveBuffer = new byte[256];
            var command = apdu.ToArray();

            SCardError sc = reader.Transmit(
                sendPci,            // Protocol Control Information (T0, T1 or Raw)
                command,            // command APDU
                receivePci,         // returning Protocol Control Information
                ref receiveBuffer); // data buffer

            if (sc != SCardError.Success) {
                return null;
            }

            return new ResponseApdu(receiveBuffer, IsoCase.Case2Short, reader.ActiveProtocol);
        }
    }
}
