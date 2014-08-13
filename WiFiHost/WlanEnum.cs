using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiFiHost
{   
    class WlanEnum
    {
        public enum WlanHostedNetworkOpcode
        {
            /// <summary>  
            /// The opcode used to query or set the wireless Hosted Network connection settings.  
            /// </summary>  
            /// <remarks>  
            /// A pointer to a WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS structure is returned.  
            /// </remarks>  
            wlan_hosted_network_opcode_connection_settings = 0,
            /// <summary>  
            /// The opcode used to query the wireless Hosted Network security settings.  
            /// </summary>  
            /// <remarks>  
            /// A pointer to a WLAN_HOSTED_NETWORK_SECURITY_SETTINGS structure is returned.  
            /// </remarks>  
            wlan_hosted_network_opcode_security_settings,
            /// <summary>  
            /// The opcode used to query the wireless Hosted Network station profile.  
            /// </summary>  
            /// <remarks>  
            /// A PWSTR to contains an XML WLAN profile for connecting to the wireless Hosted Network is returned.  
            /// </remarks>  
            wlan_hosted_network_opcode_station_profile,
            /// <summary>  
            /// The opcode used to query or set the wireless Hosted Network enabled flag.  
            /// </summary>  
            /// <remarks>  
            /// A PBOOL that indicates if wireless Hosted Network is enabled is returned.  
            /// </remarks>  
            wlan_hosted_network_opcode_enable
        };

        /// <summary>  
        /// Specifies the origin of automatic configuration (auto config) settings.  
        /// </summary>  
        /// <remarks>  
        /// Corresponds to the native <c>WLAN_OPCODE_VALUE_TYPE</c> type.  
        /// </remarks>  
        public enum WlanOpcodeValueType
        {
            /// <summary>  
            /// The auto config settings were queried, but the origin of the settings was not determined.  
            /// </summary>  
            QueryOnly = 0,
            /// <summary>  
            /// The auto config settings were set by group policy.  
            /// </summary>  
            SetByGroupPolicy = 1,
            /// <summary>  
            /// The auto config settings were set by the user.  
            /// </summary>  
            SetByUser = 2,
            /// <summary>  
            /// The auto config settings are invalid.  
            /// </summary>  
            Invalid = 3
        }  

       public enum WlanHostedNetworkReason  
       {  
           /// <summary>  
           /// The operation was successful.  
           /// </summary>  
           wlan_hosted_network_reason_success = 0,  
           /// <summary>  
           /// Unknown error.  
           /// </summary>  
           wlan_hosted_network_reason_unspecified,  
           /// <summary>  
           /// Bad parameters.  
           /// </summary>  
           wlan_hosted_network_reason_bad_parameters,  
           /// <summary>  
           /// Service is shutting down.  
           /// </summary>  
           wlan_hosted_network_reason_service_shutting_down,  
           /// <summary>  
           /// Service is out of resources.  
           /// </summary>  
           wlan_hosted_network_reason_insufficient_resources,  
           /// <summary>  
           /// This operation requires elevation.  
           /// </summary>  
           wlan_hosted_network_reason_elevation_required,  
           /// <summary>  
           /// An attempt was made to write read-only data.  
           /// </summary>  
           wlan_hosted_network_reason_read_only,  
           /// <summary>  
           /// Data persistence failed.  
           /// </summary>  
           wlan_hosted_network_reason_persistence_failed,  
           /// <summary>  
           /// A cryptographic error occurred.  
           /// </summary>  
           wlan_hosted_network_reason_crypt_error,  
           /// <summary>  
           /// User impersonation failed.  
           /// </summary>  
           wlan_hosted_network_reason_impersonation,  
           /// <summary>  
           /// An incorrect function call sequence was made.  
           /// </summary>  
           wlan_hosted_network_reason_stop_before_start,  
           /// <summary>  
           /// A wireless interface has become available.  
           /// </summary>  
           wlan_hosted_network_reason_interface_available,  
           /// <summary>  
           /// A wireless interface has become unavailable.  
           /// </summary>  
           wlan_hosted_network_reason_interface_unavailable,  
           /// <summary>  
           /// The wireless miniport driver stopped the Hosted Network.  
           /// </summary>  
           wlan_hosted_network_reason_miniport_stopped,  
           /// <summary>  
           /// The wireless miniport driver status changed.  
           /// </summary>  
           wlan_hosted_network_reason_miniport_started,  
           /// <summary>  
           /// An incompatible connection started.  
           /// </summary>  
           wlan_hosted_network_reason_incompatible_connection_started,  
           /// <summary>  
           /// An incompatible connection stopped.  
           /// </summary>  
           wlan_hosted_network_reason_incompatible_connection_stopped,  
           /// <summary>  
           /// A state change occurred that was caused by explicit user action.  
           /// </summary>  
           wlan_hosted_network_reason_user_action,  
           /// <summary>  
           /// A state change occurred that was caused by client abort.  
           /// </summary>  
           wlan_hosted_network_reason_client_abort,  
           /// <summary>  
           /// The driver for the wireless Hosted Network failed to start.  
           /// </summary>  
           wlan_hosted_network_reason_ap_start_failed,  
           /// <summary>  
           /// A peer connected to the wireless Hosted Network.  
           /// </summary>  
           wlan_hosted_network_reason_peer_arrived,  
           /// <summary>  
           /// A peer disconnected from the wireless Hosted Network.  
            /// </summary>  
            wlan_hosted_network_reason_peer_departed,  
            /// <summary>  
            /// A peer timed out.  
            /// </summary>  
            wlan_hosted_network_reason_peer_timeout,  
            /// <summary>  
            /// The operation was denied by group policy.  
            /// </summary>  
            wlan_hosted_network_reason_gp_denied,  
            /// <summary>  
            /// The Wireless LAN service is not running.  
            /// </summary>  
            wlan_hosted_network_reason_service_unavailable,  
            /// <summary>  
            /// The wireless adapter used by the wireless Hosted Network changed.  
            /// </summary>  
            wlan_hosted_network_reason_device_change,  
            /// <summary>  
            /// The properties of the wireless Hosted Network changed.  
            /// </summary>  
            wlan_hosted_network_reason_properties_change,  
            /// <summary>  
            /// A virtual station is active and blocking operation.  
            /// </summary>  
            wlan_hosted_network_reason_virtual_station_blocking_use,  
            /// <summary>  
            /// An identical service is available on a virtual station.  
            /// </summary>  
            wlan_hosted_network_reason_service_available_on_virtual_station  
        };  

    }
}
