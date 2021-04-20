using PCSC;
using PCSC.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nmind.pcsc {
   
    /// <summary>
    /// 
    /// </summary>
    class NFCService {

        /// <summary>
        /// 
        /// </summary>
        protected IDeviceMonitor deviceMonitor;

        /// <summary>
        /// 
        /// </summary>
        protected ISCardMonitor readerMonitor;

        /// <summary>
        /// 
        /// </summary>
        public NFCService() {
            EnableDevicesMonitor();
        }

        /// <summary>
        /// 
        /// </summary>
        ~NFCService() {
            StopReaderMonitor();
            DisableDevicesMonitor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ListReaders() {
            try {
                using (var context = ContextFactory.Instance.Establish(SCardScope.System)) {
                    return context.GetReaders();
                }
            } catch (Exception) {

                return new List<string>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasReaders() {
            return !Helper.IsEmpty(ListReaders());
        }


        #region CardMonitor Delegate and Event

        /*
        public delegate void CardInsertedEventHandler(CardStatusEventArgs args);

        public delegate void CardRemovedEventHandler(CardStatusEventArgs args);

        public delegate void CardInitializedEventHandler(CardStatusEventArgs args);

        public delegate void CardStatusChangedEventHandler(StatusChangeEventArgs args);

        public delegate void CardMonitorExceptionEventHandler(PCSCException ex);

        public event CardInsertedEventHandler OnCardInserted;

        public event CardRemovedEventHandler OnCardRemoved;

        public event CardInitializedEventHandler OnInitialized;

        public event CardStatusChangedEventHandler OnCardStatusChanged;

        public event CardMonitorExceptionEventHandler OnCardMonitorException;
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        public bool StartReaderMonitor(string readerName, IReaderMonitor monitor) {
            StopReaderMonitor();

            try {

                readerMonitor = MonitorFactory.Instance.Create(SCardScope.System);
                readerMonitor.CardInserted += (sender, args) => monitor.OnCardInserted(args);
                readerMonitor.CardRemoved += (sender, args) => monitor.OnCardRemoved(args);
                readerMonitor.StatusChanged += (sender, args) => monitor.OnStatusChanged(args);
                readerMonitor.Initialized += (sender, args) => monitor.OnInitialized(args);
                readerMonitor.MonitorException += (sender, args) => monitor.OnMonitorException(args);
                readerMonitor.Start(readerName);

                return true;
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopReaderMonitor() {
            if (readerMonitor != null) {
                readerMonitor.Cancel();
                readerMonitor.Dispose();
                readerMonitor = null;
            }
        }

        #endregion



        #region DevicesMonitor Delegate and Event

        public delegate void MonitorExceptionReadersEventHandler(Exception e);

        public delegate void DetachedReadersEventHandler(IEnumerable<string> readersName);

        public delegate void AttachedReadersEventHandler(IEnumerable<string> readersName);

        public delegate void InitializedReadersEventHandler(IEnumerable<string> readersName);

        public event MonitorExceptionReadersEventHandler OnReadersMonitorException;

        public event DetachedReadersEventHandler OnReadersDetached;

        public event AttachedReadersEventHandler OnReadersAttached;

        public event InitializedReadersEventHandler OnReadersInitialized;

        /// <summary>
        /// 
        /// </summary>
        protected void EnableDevicesMonitor() {
            deviceMonitor = DeviceMonitorFactory.Instance.Create(SCardScope.System);
            deviceMonitor.Initialized += OnDevicesInitialized;
            deviceMonitor.StatusChanged += OnDevicesStatusChanged;
            deviceMonitor.MonitorException += OnDevicesMonitorException;
            deviceMonitor.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DisableDevicesMonitor() {
            if (deviceMonitor != null) {
                deviceMonitor.Initialized -= OnDevicesInitialized;
                deviceMonitor.StatusChanged -= OnDevicesStatusChanged;
                deviceMonitor.MonitorException -= OnDevicesMonitorException;

                deviceMonitor.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDevicesMonitorException(object sender, DeviceMonitorExceptionEventArgs args) {
            OnReadersMonitorException?.Invoke(args.Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void OnDevicesStatusChanged(object sender, DeviceChangeEventArgs ev) {

            StopReaderMonitor();
            DisableDevicesMonitor();

            if(ev.DetachedReaders != null && ev.DetachedReaders.Count() > 0) {
                OnReadersDetached?.Invoke(ev.DetachedReaders);
            }

            if (ev.AttachedReaders != null && ev.AttachedReaders.Count() > 0) {
                OnReadersAttached?.Invoke(ev.AttachedReaders);
            }

            EnableDevicesMonitor();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void OnDevicesInitialized(object sender, DeviceChangeEventArgs ev) {

            if (ev.AttachedReaders != null && ev.AllReaders.Count() > 0) {
                OnReadersInitialized?.Invoke(ev.AllReaders);
            }

        }

        #endregion
    }

}
