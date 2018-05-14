using EnsembleFX.Core.DataFrame.Adapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EnsembleFX.Core.DataFrame.Helpers
{
    public class ExtractHelper
    {
        public DataSet Data { get; set; }

        #region Configuration properties

        #region Adapters
        #region AzureTableConfiguration
        private AzureTableAdapterConfiguration _AzureTableConfiguration;
        public AzureTableAdapterConfiguration AzureTableConfiguration
        {
            get
            {
                if (_AzureTableConfiguration == null)
                    _AzureTableConfiguration = new AzureTableAdapterConfiguration();
                return _AzureTableConfiguration;
            }
        }
        #endregion
        #endregion
        #endregion

        #region Public methods
        public void ClearDataset()
        {
            Data = new DataSet();
        }

        #region Write methods
        private void WriteToAdapter(IAdapter adapter)
        {
            adapter.Write(Data);
        }

        public void WriteToAzureTable()
        {
            WriteToAdapter(new AzureTableAdapter(_AzureTableConfiguration));
        }
        #endregion
        #endregion
    }
}
