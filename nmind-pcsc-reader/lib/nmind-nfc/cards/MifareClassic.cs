﻿using PCSC;
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
    class MifareClassic : AbstractSmartCard {

        /// <summary>
        /// 
        /// </summary>
        public enum KeyType : byte {
            A = 0x60,
            B = 0x61
        }

        /// <summary>
        /// 
        /// </summary>
        public enum MemorySize : int {
            Classic1K = 15 * 3 * 16,
            Classic4K = 31 * 3 * 16,
            ClassicMini = 5 * 3 * 16
        }

        /// <summary>
        /// 
        /// </summary>
        private byte[] authentificationKey;

        /// <summary>
        /// 
        /// </summary>
        private KeyType authentificationKeyType;

        /// <summary>
        /// 
        /// </summary>
        private byte keyNumber = 0x00;

        // writable sectors * writable blocks per sector * bytes per block
        // maybe value = number of sectors?
        private MemorySize memorySize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memorySize"></param>
        public MifareClassic(MemorySize memorySize) : this(memorySize, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, KeyType.B) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memorySize"></param>
        /// <param name="authentificationKey"></param>
        /// <param name="authentificationKeyType"></param>
        public MifareClassic(MemorySize memorySize, byte[] authentificationKey, KeyType authentificationKeyType) {
            this.memorySize = memorySize;
            this.authentificationKey = authentificationKey;
            this.authentificationKeyType = authentificationKeyType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public override byte[] GetCardMemory(SCardReader reader)  {
            SCardError sc = reader.BeginTransaction();

            if (sc != SCardError.Success) {
                return new byte[0]; // TODO proper exception
            }

            if (!LoadAuthenticationKey(reader)) {
                return new byte[0]; // TODO proper exception
            }

            byte[] memory = new byte[(int)memorySize];

            int sectors;

            switch (memorySize) {

                case MemorySize.Classic1K:
                    sectors = 15;
                    break;

                case MemorySize.Classic4K:
                    sectors = 31;
                    break;

                case MemorySize.ClassicMini:
                    sectors = 5;
                    break;

                default:
                    return new byte[0]; // TODO proper exception
            }

            bool readMemory = true;
            int currentSector = 1; // ignore first (=0) sector, since it's not usable

            while (readMemory && currentSector <= sectors) {
                bool readSector = true;
                int sectorOffset = currentSector * 4;
                int currentBlock = 0;

                while (readSector && currentBlock < 3){
                    readSector = false;
                    byte[] block;

                    try{
                        block = ReadBinaryBlock((byte)(sectorOffset + currentBlock), reader);
                    } catch (Exception){
                        throw;
                    }

                    int i = 0;
                    while (!readSector && i < block.Length) { // check for data
                        if (block[i] != 0x00) readSector = true;
                        i++;
                    }

                    if (readSector) { // update only if data
                        Buffer.BlockCopy(block, 0, memory, 48 * (currentSector - 1) + (currentBlock * 16), 16);
                    }

                    currentBlock++;
                }

                readMemory = readSector;
                currentSector++;
            }

            reader.EndTransaction(SCardReaderDisposition.Leave);

            return memory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool LoadAuthenticationKey(SCardReader reader) {

            var apdu = new PCSC.Iso7816.CommandApdu(IsoCase.Case3Short, reader.ActiveProtocol) {
                CLA = 0xFF,
                Instruction = InstructionCode.ExternalAuthenticate,
                P1 = 0x00, // key structure
                P2 = keyNumber, // key number
                Data = authentificationKey
            };

            var responseApdu = SendAPDU(apdu, reader);

            if (responseApdu != null && responseApdu.SW1 == 0x90) {
                return true;
            } else {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool AuthenticateBlock(byte blockNumber, SCardReader reader){

            var apdu = new CommandApdu(IsoCase.Case3Short, reader.ActiveProtocol) {
                CLA = 0xFF,
                Instruction = InstructionCode.InternalAuthenticate,
                P1 = 0x00,
                P2 = 0x00,
                Data = new byte[]{
                    (byte)0x01, // version number
                    (byte)0x00,
                    blockNumber,
                    (byte)authentificationKeyType,
                    keyNumber}
            };

            var responseApdu = SendAPDU(apdu, reader);

            if (responseApdu != null && responseApdu.SW1 == 0x90) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private byte[] ReadBinaryBlock(byte blockNumber, SCardReader reader){

            if (!AuthenticateBlock(blockNumber, reader)) {
                return new byte[0]; // TODO throw exception
            }

            byte blockSize = 0x10; // all classic variants have 16 byte blocks

            var apdu = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol){
                CLA = 0xFF,
                Instruction = InstructionCode.ReadBinary,
                P1 = 0x00,
                P2 = blockNumber, // block number
                Le = blockSize // bytes to read
            };

            var responseApdu = SendAPDU(apdu, reader);

            if (responseApdu != null && responseApdu.SW1 == 0x90) {
                return responseApdu.GetData().Take(16).ToArray(); // todo check size in debugging
            } else {
                return new byte[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetCardType(){

            switch (memorySize)  {
                case MemorySize.Classic1K:
                    return "Mifare Classic 1K";

                case MemorySize.Classic4K:
                    return "Mifare Classic 4K";

                case MemorySize.ClassicMini:
                    return "Mifare Classic Mini";

                default:
                    return "Invalid Type";
            }
        }
    }
}
