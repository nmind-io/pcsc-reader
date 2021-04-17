using PCSC.Exceptions;
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
    interface IReaderMonitor {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void OnCardInserted(CardStatusEventArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void OnCardRemoved(CardStatusEventArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void OnInitialized(CardStatusEventArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void OnStatusChanged(StatusChangeEventArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        void OnMonitorException(PCSCException ex);
    }

}
