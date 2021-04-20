using SpringCard.PCSC;
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
        protected SCardReader reader;

        /// <summary>
        /// 
        /// </summary>
        public NFCReader() {
            
        }

        /// <summary>
        /// 
        /// </summary>
        ~NFCReader() {
            if (reader != null) {
                reader.Release();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCardPresent() {
            return reader.CardPresent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCardAvailable() {
            return reader.CardAvailable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        /// <returns></returns>
        public bool Connect(string readerName) {
            reader = new SCardReader(readerName);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect() {
            reader.Release();
            reader = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetUID() {

            SCardChannel channel = new SCardChannel(reader);

            byte[] uid = new byte[0];

            if (!channel.Connect()) {
                return uid;
            }

            CAPDU command = NFCCommand.ReadCardUid();
            RAPDU response = channel.Transmit(command);

            if (response.SW != 0x9000) {
                return uid;
            }

            return response.data.GetBytes(); ;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetATR() {
            CardBuffer cardAtr = reader.CardAtr;
            Console.WriteLine(cardAtr.AsString(" "));
            return cardAtr.Bytes;
        }

    }

}
