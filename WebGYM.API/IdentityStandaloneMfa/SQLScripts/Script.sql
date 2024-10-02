/*
INSERT INTO [dbo].[SAML_Configuration](Recipient,Target,Subject,CertFileLocation,CertPassword,CertStoreName,CertStoreLocation,CertFindKey,CertFriendlyName,
CertFindMethod,Issuer,Domain,Version,APPKey,IsActive)
SELECT 'http://desktop-pv0a2lp/GYMWeb/' Recipient,'http://desktop-pv0a2lp/GYMWeb/'Target,'localuserid' Subject,' ' CertFileLocation,' ' CertPassword,'Root'CertStoreName,'LocalMachine' CertStoreLocation,
' ' CertFindKey,'desktop-pv0a2lp'CertFriendlyName,
'FindBySubjectName' CertFindMethod,'http://desktop-pv0a2lp/IdentityStandaloneMfa/' Issuer,'desktop-pv0a2lp' Domain,'2.0' Version,'GYMAPP' APPKey,1 IsActive

INSERT INTO [dbo].[SAML_CONFIGURATIONATTRIBUTES]

SELECT  1 Sequence,'Email'AttributeName,' ' AttributeValue,1 IsActive,SAMLConfigurationID FROM [DBO].[SAML_CONFIGURATION] WHERE APPKEY='GYMAPP'
UPDATE [SAML_CONFIGURATION] SET TARGET ='HTTP://LOCALHOST:4200/PostSAMLResponse/'
*/

 