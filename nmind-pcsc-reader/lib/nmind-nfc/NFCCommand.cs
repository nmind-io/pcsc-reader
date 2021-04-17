using PCSC;
using PCSC.Iso7816;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static CommandApdu ReadCardUid(SCardProtocol protocol) {
            return new CommandApdu(IsoCase.Case2Short, protocol) {
                CLA = 0xFF,
                Instruction = InstructionCode.GetData,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0x00
            };
        }
    }

}
