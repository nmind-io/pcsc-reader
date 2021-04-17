using PCSC.Monitoring;
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
    interface IDevicesMonitor {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnMonitorException(Exception e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void OnDetachedReader(string readerName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void OnDetachedReaders(IEnumerable<string> readersName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void OnAttachedReader(string readerName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void OnAttachedReaders(IEnumerable<string> readersName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void OnInitialized(string readerName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void OnInitialized(IEnumerable<string> readersName);
    }

}
