﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace Indigox.UUM.Application.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ZydcWebServiceImplServiceSoapBinding", Namespace="http://webservice.dhr/")]
    public partial class ZydcWebServiceImplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getBqInfoByJobnoOperationCompleted;
        
        private System.Threading.SendOrPostCallback updateProcessStateOperationCompleted;
        
        private System.Threading.SendOrPostCallback pushInfoByJobnoOperationCompleted;
        
        private System.Threading.SendOrPostCallback getPersonInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback testOperationCompleted;
        
        private System.Threading.SendOrPostCallback insertInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback getLeaveInfoByJobnoOperationCompleted;
        
        private System.Threading.SendOrPostCallback getOrgInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback getCodeInfoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ZydcWebServiceImplService() {
            this.Url = global::Indigox.UUM.Application.Properties.Settings.Default.Indigox_UUM_Application_WebReference_ZydcWebServiceImplService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event getBqInfoByJobnoCompletedEventHandler getBqInfoByJobnoCompleted;
        
        /// <remarks/>
        public event updateProcessStateCompletedEventHandler updateProcessStateCompleted;
        
        /// <remarks/>
        public event pushInfoByJobnoCompletedEventHandler pushInfoByJobnoCompleted;
        
        /// <remarks/>
        public event getPersonInfoCompletedEventHandler getPersonInfoCompleted;
        
        /// <remarks/>
        public event testCompletedEventHandler testCompleted;
        
        /// <remarks/>
        public event insertInfoCompletedEventHandler insertInfoCompleted;
        
        /// <remarks/>
        public event getLeaveInfoByJobnoCompletedEventHandler getLeaveInfoByJobnoCompleted;
        
        /// <remarks/>
        public event getOrgInfoCompletedEventHandler getOrgInfoCompleted;
        
        /// <remarks/>
        public event getCodeInfoCompletedEventHandler getCodeInfoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getBqInfoByJobno([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getBqInfoByJobno", new object[] {
                        arg0,
                        arg1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getBqInfoByJobnoAsync(string arg0, string arg1) {
            this.getBqInfoByJobnoAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getBqInfoByJobnoAsync(string arg0, string arg1, object userState) {
            if ((this.getBqInfoByJobnoOperationCompleted == null)) {
                this.getBqInfoByJobnoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetBqInfoByJobnoOperationCompleted);
            }
            this.InvokeAsync("getBqInfoByJobno", new object[] {
                        arg0,
                        arg1}, this.getBqInfoByJobnoOperationCompleted, userState);
        }
        
        private void OngetBqInfoByJobnoOperationCompleted(object arg) {
            if ((this.getBqInfoByJobnoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getBqInfoByJobnoCompleted(this, new getBqInfoByJobnoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string updateProcessState([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("updateProcessState", new object[] {
                        arg0,
                        arg1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void updateProcessStateAsync(string arg0, string arg1) {
            this.updateProcessStateAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void updateProcessStateAsync(string arg0, string arg1, object userState) {
            if ((this.updateProcessStateOperationCompleted == null)) {
                this.updateProcessStateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdateProcessStateOperationCompleted);
            }
            this.InvokeAsync("updateProcessState", new object[] {
                        arg0,
                        arg1}, this.updateProcessStateOperationCompleted, userState);
        }
        
        private void OnupdateProcessStateOperationCompleted(object arg) {
            if ((this.updateProcessStateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updateProcessStateCompleted(this, new updateProcessStateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pushInfoByJobno([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            object[] results = this.Invoke("pushInfoByJobno", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void pushInfoByJobnoAsync(string arg0, string arg1, string arg2, string arg3) {
            this.pushInfoByJobnoAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void pushInfoByJobnoAsync(string arg0, string arg1, string arg2, string arg3, object userState) {
            if ((this.pushInfoByJobnoOperationCompleted == null)) {
                this.pushInfoByJobnoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnpushInfoByJobnoOperationCompleted);
            }
            this.InvokeAsync("pushInfoByJobno", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.pushInfoByJobnoOperationCompleted, userState);
        }
        
        private void OnpushInfoByJobnoOperationCompleted(object arg) {
            if ((this.pushInfoByJobnoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.pushInfoByJobnoCompleted(this, new pushInfoByJobnoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getPersonInfo([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("getPersonInfo", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getPersonInfoAsync(string arg0) {
            this.getPersonInfoAsync(arg0, null);
        }
        
        /// <remarks/>
        public void getPersonInfoAsync(string arg0, object userState) {
            if ((this.getPersonInfoOperationCompleted == null)) {
                this.getPersonInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetPersonInfoOperationCompleted);
            }
            this.InvokeAsync("getPersonInfo", new object[] {
                        arg0}, this.getPersonInfoOperationCompleted, userState);
        }
        
        private void OngetPersonInfoOperationCompleted(object arg) {
            if ((this.getPersonInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getPersonInfoCompleted(this, new getPersonInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string test([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("test", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void testAsync(string arg0) {
            this.testAsync(arg0, null);
        }
        
        /// <remarks/>
        public void testAsync(string arg0, object userState) {
            if ((this.testOperationCompleted == null)) {
                this.testOperationCompleted = new System.Threading.SendOrPostCallback(this.OntestOperationCompleted);
            }
            this.InvokeAsync("test", new object[] {
                        arg0}, this.testOperationCompleted, userState);
        }
        
        private void OntestOperationCompleted(object arg) {
            if ((this.testCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.testCompleted(this, new testCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string insertInfo([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("insertInfo", new object[] {
                        arg0,
                        arg1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void insertInfoAsync(string arg0, string arg1) {
            this.insertInfoAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void insertInfoAsync(string arg0, string arg1, object userState) {
            if ((this.insertInfoOperationCompleted == null)) {
                this.insertInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OninsertInfoOperationCompleted);
            }
            this.InvokeAsync("insertInfo", new object[] {
                        arg0,
                        arg1}, this.insertInfoOperationCompleted, userState);
        }
        
        private void OninsertInfoOperationCompleted(object arg) {
            if ((this.insertInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.insertInfoCompleted(this, new insertInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getLeaveInfoByJobno([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getLeaveInfoByJobno", new object[] {
                        arg0,
                        arg1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getLeaveInfoByJobnoAsync(string arg0, string arg1) {
            this.getLeaveInfoByJobnoAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getLeaveInfoByJobnoAsync(string arg0, string arg1, object userState) {
            if ((this.getLeaveInfoByJobnoOperationCompleted == null)) {
                this.getLeaveInfoByJobnoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetLeaveInfoByJobnoOperationCompleted);
            }
            this.InvokeAsync("getLeaveInfoByJobno", new object[] {
                        arg0,
                        arg1}, this.getLeaveInfoByJobnoOperationCompleted, userState);
        }
        
        private void OngetLeaveInfoByJobnoOperationCompleted(object arg) {
            if ((this.getLeaveInfoByJobnoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getLeaveInfoByJobnoCompleted(this, new getLeaveInfoByJobnoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getOrgInfo([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getOrgInfo", new object[] {
                        arg0,
                        arg1});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getOrgInfoAsync(string arg0, string arg1) {
            this.getOrgInfoAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getOrgInfoAsync(string arg0, string arg1, object userState) {
            if ((this.getOrgInfoOperationCompleted == null)) {
                this.getOrgInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetOrgInfoOperationCompleted);
            }
            this.InvokeAsync("getOrgInfo", new object[] {
                        arg0,
                        arg1}, this.getOrgInfoOperationCompleted, userState);
        }
        
        private void OngetOrgInfoOperationCompleted(object arg) {
            if ((this.getOrgInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getOrgInfoCompleted(this, new getOrgInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.dhr/", ResponseNamespace="http://webservice.dhr/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getCodeInfo([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("getCodeInfo", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getCodeInfoAsync(string arg0) {
            this.getCodeInfoAsync(arg0, null);
        }
        
        /// <remarks/>
        public void getCodeInfoAsync(string arg0, object userState) {
            if ((this.getCodeInfoOperationCompleted == null)) {
                this.getCodeInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCodeInfoOperationCompleted);
            }
            this.InvokeAsync("getCodeInfo", new object[] {
                        arg0}, this.getCodeInfoOperationCompleted, userState);
        }
        
        private void OngetCodeInfoOperationCompleted(object arg) {
            if ((this.getCodeInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCodeInfoCompleted(this, new getCodeInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getBqInfoByJobnoCompletedEventHandler(object sender, getBqInfoByJobnoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getBqInfoByJobnoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getBqInfoByJobnoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void updateProcessStateCompletedEventHandler(object sender, updateProcessStateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class updateProcessStateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal updateProcessStateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void pushInfoByJobnoCompletedEventHandler(object sender, pushInfoByJobnoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class pushInfoByJobnoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal pushInfoByJobnoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getPersonInfoCompletedEventHandler(object sender, getPersonInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getPersonInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getPersonInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void testCompletedEventHandler(object sender, testCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class testCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal testCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void insertInfoCompletedEventHandler(object sender, insertInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class insertInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal insertInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getLeaveInfoByJobnoCompletedEventHandler(object sender, getLeaveInfoByJobnoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getLeaveInfoByJobnoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getLeaveInfoByJobnoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getOrgInfoCompletedEventHandler(object sender, getOrgInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getOrgInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getOrgInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getCodeInfoCompletedEventHandler(object sender, getCodeInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCodeInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCodeInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591