using PCSC;
using PCSC.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        protected List<IDevicesMonitor> devicesMonitors;

        /// <summary>
        /// 
        /// </summary>
        public NFCService() {
            devicesMonitors = new List<IDevicesMonitor>();
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
        /// <param name="monitor"></param>
        public void AddDevicesMonitor(IDevicesMonitor monitor) {
            if (!devicesMonitors.Contains(monitor)){
                devicesMonitors.Add(monitor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor"></param>
        public void RemoveDevicesMonitor(IDevicesMonitor monitor) {
            if (devicesMonitors.Contains(monitor)) {
                devicesMonitors.Remove(monitor);
            }
        }

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
            if(readerMonitor != null) {
                readerMonitor.Cancel();
                readerMonitor.Dispose();
                readerMonitor = null;
            }
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



        #region DevicesMonitor events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDevicesMonitorException(object sender, DeviceMonitorExceptionEventArgs args) {
            foreach(var m in devicesMonitors) {
                try {
                    m.OnMonitorException(args.Exception);
                } catch (Exception) {
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void OnDevicesStatusChanged(object sender, DeviceChangeEventArgs ev) {

            StopReaderMonitor();
            DisableDevicesMonitor();

            foreach (var readerName in ev.DetachedReaders) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnDetachedReader(readerName);
                    } catch (Exception e) {
                    }

                }
            }

            if(ev.DetachedReaders != null && ev.DetachedReaders.Count() > 0) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnDetachedReaders(ev.DetachedReaders);
                    } catch (Exception e) {
                    }

                }
            }

            foreach (var readerName in ev.AttachedReaders) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnAttachedReader(readerName);
                    } catch (Exception e) {
                    }

                }
            }

            if (ev.AttachedReaders != null && ev.AttachedReaders.Count() > 0) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnAttachedReaders(ev.AttachedReaders);
                    } catch (Exception e) {
                    }

                }
            }

            EnableDevicesMonitor();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void OnDevicesInitialized(object sender, DeviceChangeEventArgs ev) {

            foreach (var readerName in ev.AllReaders) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnInitialized(readerName);
                    } catch (Exception e) {
                    }

                }
            }

            if (ev.AttachedReaders != null && ev.AllReaders.Count() > 0) {
                foreach (var m in devicesMonitors) {

                    try {
                        m.OnInitialized(ev.AllReaders);
                    } catch (Exception e) {
                    }

                }
            }

        }

        #endregion
    }

}
