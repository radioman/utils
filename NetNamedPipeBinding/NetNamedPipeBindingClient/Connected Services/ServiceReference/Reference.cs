﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetNamedPipeBindingClient.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IRemoteObject")]
    public interface IRemoteObject {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteObject/GetRBytes", ReplyAction="http://tempuri.org/IRemoteObject/GetRBytesResponse")]
        byte[] GetRBytes(int numBytes);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRemoteObject/ReceiveRBytes", ReplyAction="http://tempuri.org/IRemoteObject/ReceiveRBytesResponse")]
        bool ReceiveRBytes(byte[] bytes);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRemoteObjectChannel : NetNamedPipeBindingClient.ServiceReference.IRemoteObject, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RemoteObjectClient : System.ServiceModel.ClientBase<NetNamedPipeBindingClient.ServiceReference.IRemoteObject>, NetNamedPipeBindingClient.ServiceReference.IRemoteObject {
        
        public RemoteObjectClient() {
        }
        
        public RemoteObjectClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RemoteObjectClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RemoteObjectClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RemoteObjectClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public byte[] GetRBytes(int numBytes) {
            return base.Channel.GetRBytes(numBytes);
        }
        
        public bool ReceiveRBytes(byte[] bytes) {
            return base.Channel.ReceiveRBytes(bytes);
        }
    }
}
