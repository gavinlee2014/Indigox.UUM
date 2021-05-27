using System;

namespace Indigox.UUM.Sync.Model
{
    public class SysKeyMapping
    {
        private SysConfiguration externalSystem;
        private string internalID;
        private string externalID;

        protected SysKeyMapping()
        {
        }

        public SysKeyMapping( string internalID, string externalID, SysConfiguration externalSystem )
        {
            this.externalSystem = externalSystem;
            this.internalID = internalID;
            this.externalID = externalID;
        }

        public object ID { get; set; }


        /// <summary>
        /// Extenal system info.
        /// </summary>
        public SysConfiguration ExternalSystem
        {
            get { return externalSystem; }
            set { externalSystem = value; }
        }

        /// <summary>
        /// UUM internal id.
        /// </summary>
        public string InternalID
        {
            get { return internalID; }
            set { internalID = value; }
        }

        /// <summary>
        /// Extenal id from extenal system.
        /// </summary>
        public string ExternalID
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}