using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml.Serialization;
using System.IO;
using NLog;
namespace IdentityStandaloneMfa.SSO
{
    /// <summary>
    /// SamlSignedXml - Class is used to sign xml, basically the when the ID is retreived the correct ID is used.  
    /// without this, the id reference would not be valid.
    /// </summary>
    public class SamlSignedXml : SignedXml
    {
        private string _referenceAttributeId = "";
        public SamlSignedXml(XmlDocument document, string referenceAttributeId) : base(document)
        {
            _referenceAttributeId = referenceAttributeId;
        }
        public override XmlElement GetIdElement(
            XmlDocument document, string idValue)
        {
            return (XmlElement)
                document.SelectSingleNode(
                    string.Format("//*[@{0}='{1}']",
                    _referenceAttributeId, idValue));

        }
    }
    public static class SigningHelper
    {
        public enum SignatureType
        {
            Response,
            Assertion
        };
     
        /// <summary>
        /// Signs an XML Document for a Saml Response
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cert2"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public static XmlElement SignDoc(XmlDocument doc, X509Certificate2 cert2, string referenceId, string referenceValue)
        {
            SamlSignedXml sig = new SamlSignedXml(doc, referenceId);
            // Add the key to the SignedXml xmlDocument. 
            sig.SigningKey = cert2.PrivateKey;

            // Create a reference to be signed. 
            Reference reference = new Reference();

            reference.Uri = String.Empty;
            reference.Uri = "#" + referenceValue;

            // Add an enveloped transformation to the reference. 
            XmlDsigEnvelopedSignatureTransform env = new
                XmlDsigEnvelopedSignatureTransform();

            XmlDsigExcC14NTransform env2 = new XmlDsigExcC14NTransform();

            reference.AddTransform(env);
            reference.AddTransform(env2);

            // Add the reference to the SignedXml object. 
            sig.AddReference(reference);

            // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate). 
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyData = new KeyInfoX509Data(cert2);

            keyInfo.AddClause(keyData);

            sig.KeyInfo = keyInfo;

            // Compute the signature. 
            sig.ComputeSignature();

            // Get the XML representation of the signature and save it to an XmlElement object. 
            XmlElement xmlDigitalSignature = sig.GetXml();

            return xmlDigitalSignature;
        }
    }
    public static class EncryptionHelper
    {
        /// <summary>
        /// Signs an XML Document for a Saml Response
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cert2"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public static EncryptedData EncryptDoc(XmlDocument doc, X509Certificate2 cert2)
        {
            EncryptedXml xml = new EncryptedXml(doc);
            return xml.Encrypt(doc.DocumentElement, cert2);

        }
    }
    public static class SamlHelper
    {
       // private readonly ILogger<SamlHelper> Logger;
       // var Logger = NLog.LogManager.GetCurrentClassLogger();
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(typeof(SamlHelper));

        /// <summary>
        /// Creates a Version 2.0 Saml Assertion
        /// </summary>
        /// <param name="issuer">Issuer</param>
        /// <param name="subject">Subject</param>
        /// <param name="attributes">Attributes</param>
        /// <returns>returns a Version 1.1 Saml Assertion</returns>
        private static AssertionType CreateSamlAssertion(string issuer, string recipient, string domain, string subject, Dictionary<string, string> attributes)
        {
            // Here we create some SAML assertion with ID and Issuer name. 
            AssertionType assertion = new AssertionType();
            assertion.ID = "_" + Guid.NewGuid().ToString();

            NameIDType issuerForAssertion = new NameIDType();
            issuerForAssertion.Value = issuer.Trim();

            assertion.Issuer = issuerForAssertion;
            assertion.Version = "2.0";

            assertion.IssueInstant = System.DateTime.UtcNow;

            //Not before, not after conditions 
            ConditionsType conditions = new ConditionsType();
            conditions.NotBefore = DateTime.UtcNow;
            conditions.NotBeforeSpecified = true;
            conditions.NotOnOrAfter = DateTime.UtcNow.AddMinutes(5);
            conditions.NotOnOrAfterSpecified = true;

            AudienceRestrictionType audienceRestriction = new AudienceRestrictionType();
            audienceRestriction.Audience = new string[] { domain.Trim() };

            conditions.Items = new ConditionAbstractType[] { audienceRestriction };

            //Name Identifier to be used in Saml Subject
            NameIDType nameIdentifier = new NameIDType();
            nameIdentifier.NameQualifier = domain.Trim();
            nameIdentifier.Value = subject.Trim();

            SubjectConfirmationType subjectConfirmation = new SubjectConfirmationType();
            SubjectConfirmationDataType subjectConfirmationData = new SubjectConfirmationDataType();

            subjectConfirmation.Method = "urn:oasis:names:tc:SAML:2.0:cm:bearer";
            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;
            // 
            // Create some SAML subject. 
            SubjectType samlSubject = new SubjectType();

            AttributeStatementType attrStatement = new AttributeStatementType();
            AuthnStatementType authStatement = new AuthnStatementType();
            authStatement.AuthnInstant = DateTime.UtcNow;
            AuthnContextType context = new AuthnContextType();
            context.ItemsElementName = new ItemsChoiceType5[] { ItemsChoiceType5.AuthnContextClassRef };
            context.Items = new object[] { "AuthnContextClassRef" };
            authStatement.AuthnContext = context;

            samlSubject.Items = new object[] { nameIdentifier, subjectConfirmation };

            assertion.Subject = samlSubject;

            IPHostEntry ipEntry =
                Dns.GetHostEntry(System.Environment.MachineName);

            SubjectLocalityType subjectLocality = new SubjectLocalityType();
            subjectLocality.Address = ipEntry.AddressList[0].ToString();

            attrStatement.Items = new AttributeType[attributes.Count];
            int i = 0;
            // Create userName SAML attributes. 
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                AttributeType attr = new AttributeType();
                attr.Name = attribute.Key;
                attr.NameFormat = "urn:oasis:names:tc:SAML:2.0:attrname-format:basic";
                attr.AttributeValue = new object[] { attribute.Value };
                attrStatement.Items[i] = attr;
                i++;
            }
            assertion.Conditions = conditions;

            assertion.Items = new StatementAbstractType[] { authStatement, attrStatement };

            return assertion;

        }
        /// <summary>
        /// GetPostSamlResponse - Returns a Base64 Encoded String with the SamlResponse in it.
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="issuer">Issuer</param>
        /// <param name="domain">Domain</param>
        /// <param name="subject">Subject</param>
        /// <param name="storeLocation">Certificate Store Location</param>
        /// <param name="storeName">Certificate Store Name</param>
        /// <param name="findType">Certificate Find Type</param>
        /// <param name="certLocation">Certificate Location</param>
        /// <param name="findValue">Certificate Find Value</param>
        /// <param name="certFile">Certificate File (used instead of the above Certificate Parameters)</param>
        /// <param name="certPassword">Certificate Password (used instead of the above Certificate Parameters)</param>
        /// <param name="attributes">A list of attributes to pass</param>
        /// <param name="signatureType">Whether to sign Response or Assertion</param>
        /// <returns>A base64Encoded string with a SAML response.</returns>
        public static string GetPostSamlResponse(string recipient, string issuer, string domain, string subject,
            StoreLocation storeLocation, StoreName storeName, X509FindType findType, string certFile, string certPassword, object findValue,
            Dictionary<string, string> attributes, SigningHelper.SignatureType signatureType, string encCertFile)
        {
            ResponseType response = new ResponseType();
            // Response Main Area
            response.ID = "_" + Guid.NewGuid().ToString();
            response.Destination = recipient;
            response.Version = "2.0";
            response.IssueInstant = System.DateTime.UtcNow;

            NameIDType issuerForResponse = new NameIDType();
            issuerForResponse.Value = issuer.Trim();

            response.Issuer = issuerForResponse;

            StatusType status = new StatusType();

            status.StatusCode = new StatusCodeType();
            status.StatusCode.Value = "urn:oasis:names:tc:SAML:2.0:status:Success";

            response.Status = status;

            XmlSerializer responseSerializer =
                new XmlSerializer(response.GetType());

            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            XmlWriter responseWriter = XmlTextWriter.Create(stringWriter, settings);

            string samlString = string.Empty;

            AssertionType assertionType = SamlHelper.CreateSamlAssertion(
                issuer.Trim(), recipient.Trim(), domain.Trim(), subject.Trim(), attributes);


            if (encCertFile != String.Empty)
            {
                XmlSerializer assertionSerializer =
                    new XmlSerializer(assertionType.GetType());

                StringWriter assertionWriter = new StringWriter();
                XmlWriterSettings assertionSettings = new XmlWriterSettings();
                assertionSettings.OmitXmlDeclaration = true;
                assertionSettings.Indent = true;
                assertionSettings.Encoding = Encoding.UTF8;

                XmlWriter assertionXmlWriter = XmlTextWriter.Create(assertionWriter, assertionSettings);
                assertionSerializer.Serialize(assertionXmlWriter, assertionType);
                XmlDocument assertion = new XmlDocument();
                assertion.LoadXml(assertionWriter.ToString());
                if (encCertFile != null)
                {
                    EncryptedData enc = EncryptionHelper.EncryptDoc(assertion, new X509Certificate2(encCertFile));

                    EncryptedDataType encAssertion = new EncryptedDataType();
                    encAssertion.CipherData.Item = enc.CipherData.CipherValue.ToString();
                }
            }



            response.Items = new AssertionType[] { assertionType };

            responseSerializer.Serialize(responseWriter, response);
            responseWriter.Close();

            samlString = stringWriter.ToString();

            samlString = samlString.Replace("SubjectConfirmationData",
                string.Format("SubjectConfirmationData NotOnOrAfter=\"{0:o}\" Recipient=\"{1}\"",
                DateTime.UtcNow.AddMinutes(5), recipient));

            stringWriter.Close();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(samlString);
            X509Certificate2 cert = null;
            if (System.IO.File.Exists(certFile))
            {
                cert = new X509Certificate2(certFile, certPassword);
            }
            else
            {
                X509Store store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection coll = store.Certificates.Find(findType, findValue, false);
                if (coll.Count < 1)
                {
                    throw new ArgumentException("Unable to locate certificate");
                }
                cert = coll[0];
                store.Close();
            }

            XmlElement signature =
                SigningHelper.SignDoc(doc, cert, "ID",
                signatureType == SigningHelper.SignatureType.Response ? response.ID : assertionType.ID);

            if (signatureType == SigningHelper.SignatureType.Response)
            {
                doc.DocumentElement.InsertBefore(signature,
                    doc.DocumentElement.ChildNodes[1]);
            }
            else
            {
                doc.DocumentElement.ChildNodes[2].InsertBefore(signature,
                    doc.DocumentElement.ChildNodes[2].ChildNodes[1]);
            }

            if (SamlHelper.Logger.IsDebugEnabled)
            {
                SamlHelper.Logger.Info(
                    "Saml Assertion before encoding = {0}",
                    doc.OuterXml.ToString());
            }
            string responseStr = doc.OuterXml;

            byte[] base64EncodedBytes =
                Encoding.UTF8.GetBytes(responseStr);

            string returnValue = System.Convert.ToBase64String(
                base64EncodedBytes);

            return returnValue;
        }
        /// <summary>
        /// GetPostSamlResponse - Returns a Base64 Encoded String with the SamlResponse in it with a Default Signature type.
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="issuer">Issuer</param>
        /// <param name="domain">Domain</param>
        /// <param name="subject">Subject</param>
        /// <param name="storeLocation">Certificate Store Location</param>
        /// <param name="storeName">Certificate Store Name</param>
        /// <param name="findType">Certificate Find Type</param>
        /// <param name="certLocation">Certificate Location</param>
        /// <param name="findValue">Certificate Find Value</param>
        /// <param name="certFile">Certificate File (used instead of the above Certificate Parameters)</param>
        /// <param name="certPassword">Certificate Password (used instead of the above Certificate Parameters)</param>
        /// <param name="attributes">A list of attributes to pass</param>
        /// <returns>A base64Encoded string with a SAML response.</returns>
        public static string GetPostSamlResponse(string recipient, string issuer, string domain, string subject,
            StoreLocation storeLocation, StoreName storeName, X509FindType findType, string certFile, string certPassword, object findValue,
            Dictionary<string, string> attributes)
        {
            return GetPostSamlResponse(recipient, issuer, domain, subject, storeLocation, storeName, findType, certFile, certPassword, findValue, attributes,
                SigningHelper.SignatureType.Response, null);
        }
        /// <summary>
        /// GetPostSamlResponse - Returns a Base64 Encoded String with the SamlResponse in it with a Default Signature type.
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="issuer">Issuer</param>
        /// <param name="domain">Domain</param>
        /// <param name="subject">Subject</param>
        /// <param name="storeLocation">Certificate Store Location</param>
        /// <param name="storeName">Certificate Store Name</param>
        /// <param name="findType">Certificate Find Type</param>
        /// <param name="certLocation">Certificate Location</param>
        /// <param name="findValue">Certificate Find Value</param>
        /// <param name="certFile">Certificate File (used instead of the above Certificate Parameters)</param>
        /// <param name="certPassword">Certificate Password (used instead of the above Certificate Parameters)</param>
        /// <param name="attributes">A list of attributes to pass</param>
        /// <returns>A base64Encoded string with a SAML response.</returns>
        public static string GetPostSamlResponse(string recipient, string issuer, string domain, string subject,
            StoreLocation storeLocation, StoreName storeName, X509FindType findType, string certFile, string certPassword, object findValue,
            Dictionary<string, string> attributes, SigningHelper.SignatureType signatureType)
        {
            return GetPostSamlResponse(recipient, issuer, domain, subject, storeLocation, storeName, findType, certFile, certPassword, findValue, attributes,
                signatureType, null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="samlString"></param>
        /// <returns></returns>
        public static IdentityStandaloneMfa.SSO.ResponseType GetSamlResponse(String samlString)
        {
            XmlSerializer responseSerializer =
                new XmlSerializer(typeof(ResponseType));

            StringReader stringReader = new StringReader(samlString);

            ResponseType response = (ResponseType)responseSerializer.Deserialize(stringReader);
            stringReader.Close();

            return response;
        }
    }
}
