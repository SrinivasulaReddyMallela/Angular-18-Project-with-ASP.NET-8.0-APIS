using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityStandaloneMfa.Entities
{
    [Table("SAML_Configuration")]
    public class SAMLConfiguration
    {
        public SAMLConfiguration() { }
        [Key]
        public int SAMLConfigurationID { get; set; }
        public string Recipient { get; set; }
        public string Target { get; set; }
        public string Subject { get; set; }
        public string CertFileLocation { get; set; }
        public string CertPassword { get; set; }
        public string CertStoreName { get; set; }
        public string CertStoreLocation { get; set; }
        public string CertFindKey { get; set; }
        public string CertFriendlyName { get; set; }
        public string CertFindMethod { get; set; }
        public string Issuer { get; set; }
        public string Domain { get; set; }
        public string Version { get; set; }
        public string APPKey { get; set; }
        public Boolean IsActive { get; set; }

        public int SAMLConfigurationAttributesID { get; set; }
    }
    [Table("SAML_ConfigurationAttributes")]
    public class SAMLAttributes
    {
        public SAMLAttributes() { }
        [Key]
        public int SAMLConfigurationAttributesID { get; set; }
        public int Sequence { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public Boolean IsActive { get; set; }

    }
}
