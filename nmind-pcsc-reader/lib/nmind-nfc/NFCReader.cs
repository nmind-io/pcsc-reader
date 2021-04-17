using PCSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
namespace Nmind.pcsc {

    /// <summary>
    /// 
    /// </summary>
    public class NFCReader {

        /// <summary>
        /// 
        /// </summary>
        protected ISCardContext context;

        /// <summary>
        /// 
        /// </summary>
        protected SCardReader reader;

        /// <summary>
        /// 
        /// </summary>
        protected ISmartCard card;

        /// <summary>
        /// 
        /// </summary>
        public NFCReader() {
            context = ContextFactory.Instance.Establish(SCardScope.System);
            reader = new SCardReader(context);
        }

        /// <summary>
        /// 
        /// </summary>
        ~NFCReader() {
            if (context != null) {
                context.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        /// <returns></returns>
        public bool Connect(string readerName) {
            SCardError sc = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);

            if (sc != SCardError.Success) {
                return false;
            }

            SCardReaderState state = context.GetReaderStatus(readerName);
            byte[] atr = state.Atr ?? new byte[0];

            if (atr.Length != 20) { // is non-ISO14443A-3 card?
                Disconnect();
                return false;
            }

            switch (atr[14]) {

                case 0x01:
                    card = new MifareClassic(MifareClassic.MemorySize.Classic1K);
                    break;

                case 0x02:
                    card = new MifareClassic(MifareClassic.MemorySize.Classic4K);
                    break;

                case 0x03:
                    card = new MifareUltralight();
                    break;

                case 0x26:
                    card = new MifareClassic(MifareClassic.MemorySize.ClassicMini);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect() {
            card = null;
            reader.Disconnect(SCardReaderDisposition.Reset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetUID() {
            byte[] uid = new byte[0];

            try {
                uid = card.GetUid(reader);
            } finally {

            }

            return uid;
        }
    }

}
